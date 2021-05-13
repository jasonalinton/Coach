using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Coach.Data.DataAccess.Logging;
using Coach.Model.Logging;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Logging;
using Google.Apis.Tasks.v1;

namespace Google.Apis.Auth.OAuth2
{
    /// <summary>
    /// OAuth 2.0 verification code receiver that runs a local server on a free port and waits for a call with the 
    /// authorization verification code.
    /// </summary>
    public class LocalServerCodeReceiver2 : ICodeReceiver
    {
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<LocalServerCodeReceiver>();

        /// <summary>The call back format. Expects one port parameter.</summary>
        private const string LoopbackCallback = "http://localhost:{0}/authorize/";

        /// <summary>Close HTML tag to return the browser so it will close itself.</summary>
        private const string ClosePageResponse =
            @"<html>
              <head><title>OAuth 2.0 Authentication Token Received</title></head>
              <body>
                Received verification code. You may now close this window.
                <script type='text/javascript'>
                  // This doesn't work on every browser.
                  window.setTimeout(function() {
                      window.open('', '_self', ''); 
                      window.close(); 
                    }, 1000);
                  if (window.opener) { window.opener.checkToken(); }
                </script>
              </body>
            </html>";

        private string redirectUri;
        public string RedirectUri
        {
            get
            {
                if (!string.IsNullOrEmpty(redirectUri))
                {
                    return redirectUri;
                }

                return redirectUri = string.Format(LoopbackCallback, GetRandomUnusedPort());
            }
        }
        private static string redirectUriStatic;
        public static string RedirectUriStatic
        {
            get
            {
                if (!string.IsNullOrEmpty(redirectUriStatic))
                {
                    return redirectUriStatic;
                }

                return redirectUriStatic = string.Format(LoopbackCallback, GetRandomUnusedPort());
            }
        }

        public async Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl url,
            CancellationToken taskCancellationToken)
        {
            var authorizationUrl = url.Build().ToString();
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(RedirectUri);
                try
                {
                    LogDAO.LogInfo($"Listener Start", -1, "Google Task", "", "API");
                    listener.Start();

                    Logger.Debug("Open a browser with \"{0}\" URL", authorizationUrl);
                    LogDAO.LogInfo($"Open a browser with URL", -1, "Google Task", "", "API");

                    LogDAO.LogInfo($"Process Started", -1, "Google Task", "", "API");
                    Process.Start(authorizationUrl);

                    // Wait to get the authorization code response.
                    LogDAO.LogInfo($"Awaiting listener", -1, "Google Task", "", "API");
                    var context = await listener.GetContextAsync().ConfigureAwait(false);

                    LogDAO.LogInfo($"Listener finished", -1, "Google Task", "", "API");
                    NameValueCollection coll = context.Request.QueryString;
                    LogDAO.LogInfo($"coll (whatever that is)", -1, "Google Task", "", "API");

                    coll.ToString();
                    // Write a "close" response.
                    using (var writer = new StreamWriter(context.Response.OutputStream))
                    {
                        writer.WriteLine(ClosePageResponse);
                        writer.Flush();
                    }
                    context.Response.OutputStream.Close();
                    var hi = coll.AllKeys.ToDictionary(k => k, k => coll[k]);
                    // Create a new response URL with a dictionary that contains all the response query parameters.
                    return new AuthorizationCodeResponseUrl(coll.AllKeys.ToDictionary(k => k, k => coll[k]));
                }
                finally
                {
                    listener.Close();
                }
            }
        }


        /// <summary>Returns a random, unused port.</summary>
        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            try
            {
                listener.Start();
                return ((IPEndPoint)listener.LocalEndpoint).Port;
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}