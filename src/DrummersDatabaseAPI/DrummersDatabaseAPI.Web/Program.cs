using DrummersDatabaseAPI.Data.DbContexts;
using DrummersDatabaseAPI.Data.Repositories;
using DrummersDatabaseAPI.Service;
using DrummersDatabaseAPI.Web.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

//--LOGGING--//
Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.MSSqlServer
                    (
                        connectionString: builder.Configuration["ConnectionStrings:drummersDbConnectionString"],
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            TableName = "Logs",
                            SchemaName = "dbo",
                            AutoCreateSqlTable = true
                        },
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        formatProvider: null,
                        columnOptions: null,
                        logEventFormatter: null
                    )
                    .WriteTo.Console()
                    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


//--SERVICES--//

// Media Types
builder.Services.AddControllers(options =>
{
    //  media types: don't blindly return json regardless of what they asked for.
    options.ReturnHttpNotAcceptable = true;
})
.AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();

// OpenAPI & Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((setupAction) =>
{
    // since multiple projects will have xml documentation, we will need to loop thru all
    // the files and include  all xml docs.
    // TO DO: see if this works on Azure.
    DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
    foreach (var fileInfo in baseDirectoryInfo.EnumerateFiles("DrummersDatabaseAPI*.xml"))
    {
        setupAction.IncludeXmlComments(fileInfo.FullName);
    };

    // adds security scheme to api documentation and auth tool/button in Swashbuckle UI
    setupAction.AddSecurityDefinition("DrummersDatabaseAPIAuth", new OpenApiSecurityScheme()
    {
        // basic authentication, not OAuth2 or OpenAPI
        Type = SecuritySchemeType.Http,

        // also: apiKey, oauth2, openIdConnect
        Scheme = "Bearer",

        Description = "Input a valid token to access this API"
    });

    // to ensure Swashbuckle UI sends bearer token
    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "DrummersDatabaseAPIAuth"
                },
            },
            new List<string>()
        }
    });
});

// custom services: inject interfaceX, provide an implementation of concrete type Y
builder.Services.AddTransient<IMailService, LocalMailService>();
builder.Services.AddDbContext<DrummersDatabaseDbContext>(dbContextOptions => dbContextOptions.UseSqlServer(builder.Configuration["ConnectionStrings:drummersDbConnectionString"]));
builder.Services.AddScoped<IDrummersDatabaseRepository, DrummersDatabaseRepository>();
builder.Services.AddScoped<ICategoryProcessor, CategoryProcessor>();
builder.Services.AddScoped<ISubCategoryProcessor, SubCategoryProcessor>();
builder.Services.AddScoped<IEntryProcessor, EntryProcessor>();
// sets content type to return based on file extension of file.
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

// auto-mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// authentication token services. bearer token authentication middleware
builder.Services.AddAuthentication("Bearer").AddJwtBearer((options) =>
{
    // how we validate the token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // the expiration time of the token is automatically validated - nothing to do here.

        // items we want to validate
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,

        // who can issue the token
        ValidIssuer = builder.Configuration["Authentication:Issuer"],

        // who can use this token?
        ValidAudience = builder.Configuration["Authentication:Audience"],

        // this signing key matches the values we used for creating the token (in controller).
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
    };
});

// policies
builder.Services.AddAuthorization((options) =>
{
    // https://app.pluralsight.com/course-player?clipId=a2ec8948-6104-4cbd-9688-6ab6d312d90a
    options.AddPolicy("EntriesRequireClaim", policy =>
    {
        policy.RequireAuthenticatedUser();

        // requires the resources in the token provided to be "all"
        policy.RequireClaim("resources", "entries");
    });
});


//--APPLICATION--//
var app = builder.Build();

// configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // might now want this to go to prod //
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// required for endpoint routing. 'where an endpoint is SELECTED.'
// marks the position in the middleware pipeline where a routing decision is made.
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

// required for endpoint routing. 'where an endpoint is EXECUTED'.
// EndPoint routing: preferred way for .NET 6
// mapping the URI of the request to a controller & action.
app.UseEndpoints(endpoints =>
{
    // adds/maps endpoints for our controller actions (with no conventions)
    endpoints.MapControllers();
});


app.Run();
