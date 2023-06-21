namespace DrummersDatabaseAPI.Web.Authentication
{
    public class AuthenticationRequestBody
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Resources { get; set; }
    }
}
