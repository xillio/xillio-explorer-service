namespace XillioEngineSDK.responses
{
    public class AuthenticationInfo
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string Scope { get; set; }
        public string Jti { get; set; }

        public AuthenticationInfo()
        {
        }
    }
}