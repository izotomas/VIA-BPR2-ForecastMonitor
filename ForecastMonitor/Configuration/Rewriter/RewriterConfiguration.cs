using System.Net;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Rewrite.Internal;

namespace ForecastMonitor.Service.Configuration.Rewriter
{
    public static class RewriterConfiguration
    {
        public static RewriteOptions RewriteOptions => new RewriteOptions
        {
            Rules = { RootUrlToHelpPageRedirectRule }
        };

        private static RedirectRule RootUrlToHelpPageRedirectRule => new RedirectRule("^$", "help", (int) HttpStatusCode.Redirect);
    }
}
