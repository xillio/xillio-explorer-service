using System;
using XillioEngineSDK.responses;

namespace XillioEngineSDK
{
    public class Authentication
    {
        protected AuthenticationInfo info;
        protected DateTime validity;
        private XillioApi api;
        private string username, password, clientId, clientSecret;
        
        public Authentication()
        {
            
        }

        public Authentication(XillioApi api)
        {
            this.api = api;
        }

        public string GetToken()
        {
            if (info == null)
            {
                throw new NotAuthenticatedException("The SDK never got authenticated.");
            }
            if (Expired() && api == null)
            {
                throw new AuthenticationExpiredException("The token has expired.");
            }

            if (Expired())
            {
                return api.Authenticate(username, password, clientId, clientSecret).AccessToken;
            }

            return info.AccessToken;
        }

        private bool Expired()
        {
            return DateTime.Compare(DateTime.Now, validity) >= 0;
        }

        public AuthenticationInfo RegisterAuthentication(AuthenticationInfo info)
        {
            this.info = info;
            validity = DateTime.Now.AddMilliseconds(info.ExpiresIn);
            return info;
        }
        
        public AuthenticationInfo RegisterAuthentication(String username, String password, String clientId, String clientSecret, AuthenticationInfo info)
        {
            this.username = username;
            this.password = password;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.info = info;
            validity = DateTime.Now.AddMilliseconds(info.ExpiresIn);
            return info;
        }

        /// <summary>
        /// Does the authenticator auto refresh
        /// </summary>
        /// <returns></returns>
        public bool IsAutoRefresh()
        {
            // api is only set when refreshing is enabled.
            return api != null;
        }
    }
}