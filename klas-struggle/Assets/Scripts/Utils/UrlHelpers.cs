using System;

namespace Assets.Scripts.Utils
{
    public static class UrlHelpers
    {
        public static bool CheckURLValid(this string source) 
            => Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
