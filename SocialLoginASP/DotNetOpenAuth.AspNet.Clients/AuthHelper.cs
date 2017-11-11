using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetOpenAuth.AspNet.Clients
{
    public static class AuthHelper
    {
        public static string GetSocialImage(AuthenticationResult result)
        {
            string imgDefault = "~/Content/img/default_profile.png";
            string image = String.Empty;
            switch (result.Provider)
            {
                case "facebook":
                case "google":
                    image = result.ExtraData["picture"];
                    break;
                case "twitter":
                    image = string.Format("https://twitter.com/{0}/profile_image?size=original", result.UserName);
                    break;
                case "instagram":
                    image = result.ExtraData["profile_picture"];
                    break;
                default:
                    image = string.Empty;
                    break;
            }

            return string.IsNullOrEmpty(image) ? imgDefault : image;
        }    
    }
}