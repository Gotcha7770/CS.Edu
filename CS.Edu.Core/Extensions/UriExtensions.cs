using System;
using System.Linq;

namespace CS.Edu.Core.Extensions;

public static class UriExtensions
{
    public static string AppendToURL(this string baseURL, params string[] segments)
    {
        var values = new[] { baseURL.TrimEnd('/') }.Concat(segments.Select(s => s.Trim('/')));
        return string.Join("/", values);
    }

    public static Uri Append(this Uri baseUrl, params string[] segments)
    {
        return new Uri(baseUrl.AbsoluteUri.AppendToURL(segments));
    }
}