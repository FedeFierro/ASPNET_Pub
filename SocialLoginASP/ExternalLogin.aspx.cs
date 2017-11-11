using Microsoft.AspNet.Membership.OpenAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.GoogleOAuth2;

namespace SocialLoginASP
{
    public partial class ExternalLogin : System.Web.UI.Page
    {
        protected string ProviderName
        {
            get { return (string)ViewState["ProviderName"] ?? String.Empty; }
            private set { ViewState["ProviderName"] = value; }
        }

        protected string ProviderDisplayName
        {
            get { return (string)ViewState["ProviderDisplayName"] ?? String.Empty; }
            private set { ViewState["ProviderDisplayName"] = value; }
        }

        protected string ProviderUserId
        {
            get { return (string)ViewState["ProviderUserId"] ?? String.Empty; }
            private set { ViewState["ProviderUserId"] = value; }
        }

        protected string ProviderUserName
        {
            get { return (string)ViewState["ProviderUserName"] ?? String.Empty; }
            private set { ViewState["ProviderUserName"] = value; }
        }

        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                ProcessProviderResult();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        private void ProcessProviderResult()
        {
            ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();
            FedeFacebookClient.RewriteRequest();
            InstagramClient.RewriteRequest();
            GoogleOAuth2Client.RewriteRequest();

            var redirectUrl = "~/ExternalLogin.aspx";
            var returnUrl = Request.QueryString["ReturnUrl"];
            if (!String.IsNullOrEmpty(returnUrl))
            {
                redirectUrl += "?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl);
            }


            var authResult = OpenAuth.VerifyAuthentication(redirectUrl);
            if (!authResult.IsSuccessful)
            {
                Title = "External login failed";
                userNameForm.Visible = false;
                ModelState.AddModelError("Provider", String.Format("External login {0} failed.", ProviderDisplayName));

                Trace.Warn("OpenAuth", String.Format("There was an error verifying authentication with {0})", ProviderName), authResult.Error);
                return;
            }
            ProviderName = authResult.Provider;
            ProviderUserId = authResult.ProviderUserId;
            ProviderUserName = authResult.UserName;
            Form.Action = ResolveUrl(redirectUrl);
            imgSocial.ImageUrl= AuthHelper.GetSocialImage(authResult);
        }
    }
}