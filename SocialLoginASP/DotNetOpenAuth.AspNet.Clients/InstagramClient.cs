using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace DotNetOpenAuth.AspNet.Clients
{
    public class InstagramClient : OAuth2Client
    {
        #region Constants and Fields

        private const string AuthorizationEndpoint = "https://api.instagram.com/oauth/authorize/";
        private const string TokenEndpoint = "https://api.instagram.com/oauth/access_token";
        private const string UserInfoEndpoint = "https://api.instagram.com/v1/users/self/";
        private readonly string _appId;
        private readonly string _appSecret;
        private readonly string[] _requestedScopes;

        #endregion
        public InstagramClient(string appId, string appSecret)
            : this(appId, appSecret, new[] { "code" }) { }

        public InstagramClient(string appId, string appSecret, params string[] requestedScopes)
            : base("instagram")
        {
            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentNullException("appId");

            if (string.IsNullOrWhiteSpace(appSecret))
                throw new ArgumentNullException("appSecret");

            if (requestedScopes == null)
                throw new ArgumentNullException("requestedScopes");

            if (requestedScopes.Length == 0)
                throw new ArgumentException("One or more scopes must be requested.", "requestedScopes");

            _appId = appId;
            _appSecret = appSecret;
            _requestedScopes = requestedScopes;
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            var state = string.IsNullOrEmpty(returnUrl.Query) ? string.Empty : returnUrl.Query.Substring(1);

            return BuildUri(AuthorizationEndpoint, new NameValueCollection
                {
                    { "client_id", _appId },
                    { "redirect_uri", returnUrl.GetLeftPart(UriPartial.Path) },
                    { "response_type", string.Join(" ", _requestedScopes)},
                    { "state", state },
                });
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            Uri requestUri = BuildUri(UserInfoEndpoint, new NameValueCollection
			{
				{
					"access_token",
					accessToken
				}
			});
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
			IDictionary<string, string> result;
			using (WebResponse response = httpWebRequest.GetResponse())
			{
				using (Stream responseStream = response.GetResponseStream())
				{
					if (responseStream == null)
					{
						result = null;
					}
					else
					{
						using (StreamReader streamReader = new StreamReader(responseStream))
						{
							string text = streamReader.ReadToEnd();
                            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var jsonObject = serializer.DeserializeObject(text);
                            var jConvert = JsonConvert.DeserializeObject<Dictionary<string,Object>>(JsonConvert.SerializeObject(jsonObject));
                            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, Object>>(JsonConvert.SerializeObject(jConvert["data"]));
                            result = dictionary.ToDictionary(x => x.Key, x => x.Value.ToString());
                            return result;

                        }
					}
				}
			}
			return result;
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
			nameValueCollection.Add(new NameValueCollection
			{
				{
					"grant_type",
					"authorization_code"
				},
				{
					"code",
					authorizationCode
				},
				{
					"client_id",
					this._appId
				},
				{
					"client_secret",
					this._appSecret
				},
				{
					"redirect_uri",
					returnUrl.GetLeftPart(UriPartial.Path)
				}
			});
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(TokenEndpoint);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				using (StreamWriter streamWriter = new StreamWriter(requestStream))
				{
					streamWriter.Write(nameValueCollection.ToString());
				}
			}
			string result;
			using (WebResponse response = httpWebRequest.GetResponse())
			{
				Stream responseStream = response.GetResponseStream();
				if (responseStream == null)
				{
					result = null;
				}
				else
				{
					using (StreamReader streamReader = new StreamReader(responseStream))
					{
						string text = streamReader.ReadToEnd();
						JObject jObject = JObject.Parse(text);
						string text2 = jObject.Value<string>("access_token");
						result = text2;
					}
				}
			}
			return result;
		
        }

        private static Uri BuildUri(string baseUri, NameValueCollection queryParameters)
        {
            var keyValuePairs = queryParameters.AllKeys.Select(k => HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(queryParameters[k]));
            var qs = String.Join("&", keyValuePairs);

            var builder = new UriBuilder(baseUri) { Query = qs };
            return builder.Uri;
        }

        /// <summary>
        /// Facebook works best when return data be packed into a "state" parameter.
        /// This should be called before verifying the request, so that the url is rewritten to support this.
        /// </summary>
        public static void RewriteRequest()
        {
            var ctx = HttpContext.Current;

            var stateString = HttpUtility.UrlDecode(ctx.Request.QueryString["state"]);
            if (stateString == null || !stateString.Contains("__provider__=instagram"))
                return;

            var q = HttpUtility.ParseQueryString(stateString);
            q.Add(ctx.Request.QueryString);
            q.Remove("state");

            ctx.RewritePath(ctx.Request.Path + "?" + q);
        }
    }
}