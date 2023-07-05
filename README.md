# DrummersDatabaseAPI

Restful API for DrummersDatabase UI.

## Foundation

 - C# 10
 - EF 7.0
 - .NET 6

## NuGet Packages
 - AutoMapper 12.0
 - Microsoft.AspNetCore.Authentication.JwtBearer 6.0
 - Microsoft.AspNetCore.Mvc.NewtonsoftJson 6.0
 - Microsoft.EntityFrameworkCore 7.0
 - Microsoft.AspNetCore.JsonPatch 7.0
 - Microsoft.AspNetCore.Mvc.NewtonsoftJson 6.0   
*replaces default formatters with more robust features using JSON.NET (Newtonsoft)*
 - MSTest.TestFramework 2.2
 - Moq 4.1
 - Serilog.AspNetCore 6.1
 - Serilog.Sinks.MSSqlServer 6.3


## Authorization

All requests from API request a token to be provided in the request header.
A token can be requested as a POST to `{DrummersDatabaseAPIBaseUrl}api/Authentication/GetToken`.

The body of the post must contain: 
{
    "UserName" : "{string}",
    "Password" : "{string}",
    "Resources": "{string}"
}

If these values are correct, a token will be returned as a response. The token is valid for 30 minutes and bust be provided with all requests as part of the header.

`Authorization: Bearer {token here}`


