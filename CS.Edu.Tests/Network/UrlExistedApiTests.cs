using System;
using FluentAssertions;
using Flurl;
using Xunit;

namespace CS.Edu.Tests.Network;

public class UrlExistedApiTests
{
    // https://developer.mozilla.org/en-US/docs/Learn/Common_questions/Web_mechanics/What_is_a_URL
    // https://www.iana.org/assignments/uri-schemes/uri-schemes.xhtml
    // https://www.tutorialspoint.com/the-url-schemes
    // https://learn.microsoft.com/en-us/dotnet/api/system.uri?view=net-8.0
    // https://jsonplaceholder.typicode.com/

    [Theory]
    [InlineData("mailto:test@mail.com", "mailto")]
    [InlineData("javascript:print();", "javascript")]
    [InlineData("http://test.com/v1/items", "http")]
    [InlineData("https://test.com/v1/items", "https")]
    [InlineData("//test.com/v1/items", "")]
    [InlineData("1", "")]
    [InlineData("../cart", "")]
    [InlineData("/v1/cart", "")]
    public void SystemUri_Construction(string value, string scheme)
    {
        var url = new Uri(value, UriKind.RelativeOrAbsolute);
        url.Scheme.Should()
            .Be(scheme);
    }

    [Theory]
    [InlineData("1", "http://test.com/v1/items/1")]
    [InlineData("../cart", "http://test.com/v1/cart")]
    [InlineData("/v2/cart", "http://test.com/v2/cart")]
    public void SystemUri_Combining(string relative, string expected)
    {
        const string page = "http://test.com/v1/items";
        var url = new Uri(new Uri(page), relative);
        url.AbsoluteUri.Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("http://www.contoso.com/", "http://www.contoso.com/test/test.htm", "test/test.htm")]
    [InlineData("http://www.contoso.com/test1/", "http://www.contoso.com/", "../")]
    [InlineData("http://www.contoso.com:8000/", "http://www.contoso.com/test/test.htm", "http://www.contoso.com/test/test.htm")]
    [InlineData("http://username@www.contoso.com/", "http://www.contoso.com/test1/test1.txt", "test1/test1.txt")]
    public void SystemUri_MakeRelativeUri(string from, string to, string expected)
    {
        new Uri(from).MakeRelativeUri(new Uri(to))
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("https://test.com/v1/items", "https://test.com")]
    [InlineData("https://test.com:80/v1/items", "https://test.com:80")]
    public void GetUriRoot(string value, string expected)
    {
        var uri = new Uri(value);
        uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)
            .Should()
            .Be(expected);
    }

    [Theory]
    [InlineData("mailto:test@mail.com", "mailto")]
    [InlineData("javascript:print();", "javascript")]
    [InlineData("http://test.com/v1/items", "http")]
    [InlineData("https://test.com/v1/items", "https")]
    [InlineData("//test.com/v1/items", "")]
    [InlineData("item", "")]
    [InlineData("../cart", "")]
    [InlineData("/v1/cart", "")]
    public void Flurl_Construction(string value, string scheme)
    {
        var uri = new Url(value);
        uri.Scheme.Should()
            .Be(scheme);
    }

    [Theory]
    [InlineData("1", "http://test.com/v1/items/1")]
    [InlineData("../cart", "http://test.com/v1/cart")]
    [InlineData("/v2/cart", "http://test.com/v2/cart")]
    public void Flurl_Combining(string relative, string expected)
    {
        const string page = "http://test.com/v1/items";
        Url.Combine(page, relative)
            .Should()
            .Be(expected);
    }
}