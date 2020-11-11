using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace MeetingPlanner.Others.Localization
{
    public class LocalizationCookieProvider : RequestCultureProvider
    {
        public static readonly string DefaultCookieName = "language";

        public string CookieName { get; set; } = DefaultCookieName;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentException(nameof(httpContext));

            var cookie = httpContext.Request.Cookies[CookieName];

            if (string.IsNullOrEmpty(cookie))
                return NullProviderCultureResult;

            var providerResultCulture = ParseCookieValue(cookie);

            return Task.FromResult(providerResultCulture);
        }

        public static ProviderCultureResult ParseCookieValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : new ProviderCultureResult(value, value);
        }
    }
}
