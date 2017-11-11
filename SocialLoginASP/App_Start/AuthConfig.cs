using Microsoft.AspNet.Membership.OpenAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.GoogleOAuth2;

namespace SocialLoginASP
{
    internal static class AuthConfig
    {
        public static void RegisterOpenAuth()
        {
            //OpenAuth.AuthenticationClients.Add("facebook",()=>(new FedeFacebookClient("appID","appSecret")));
            //OpenAuth.AuthenticationClients.Add("instagram", () => (new InstagramClient("ClientId", "ClientSecret")));
            //OpenAuth.AuthenticationClients.Add("google", () => (new GoogleOAuth2Client("ClientId", "ClientSecret")));
            //OpenAuth.AuthenticationClients.AddTwitter("APIKey", "APISecret");

        }
    }
}
