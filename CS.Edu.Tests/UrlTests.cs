using System;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests.Utils;

[TestFixture]
public class UrlTests
{
    [Test]
    public void CreateUrl()
    {
        string baseUrl = "https://some.service/";
        string api = "/api/v1/";
        string route = "/importantThings/42";

        var url = new Uri(baseUrl + api + route);
    }

    [Test]
    public void CombineUrl()
    {
        string baseUrl = "https://some.service";
        string api = "/api/v1";
        string route = "/importantThings/42";

        var url = new Uri("https://some.service")
            .Append("/api/v1")
            .Append("/importantThings/42");
    }
}