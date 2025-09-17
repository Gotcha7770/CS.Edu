using System;
using Xunit;

namespace CS.Edu.Tests;

public class UrlValidationTests
{
    [Theory]
    [InlineData("http://localhost:5087/Hotel/List/?page=2&count=10")]
    [InlineData("http://localhost:5087/Hotel/List/ ?page=2&count=10")]
    [InlineData("http://localhost:5087/Hotel/List/%20?page=2&count=10")]
    public void ValidateUrl1(string value)
    {
        var success = Uri.TryCreate(value, UriKind.Absolute, out var url);
    }

    [Theory]
    [InlineData("http://localhost:5087/Hotel/List/?page=2&count=10")]
    [InlineData("http://localhost:5087/Hotel/List/ ?page=2&count=10")]
    [InlineData("http://localhost:5087/Hotel/List/%20?page=2&count=10")]
    public void ValidateUrl2(string value)
    {
        var success = Uri.IsWellFormedUriString(value, UriKind.Absolute);
    }
}