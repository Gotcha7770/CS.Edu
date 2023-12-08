using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using RichardSzalay.MockHttp;

namespace CS.Edu.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(DefaultConfig))]
public class HttpCompletionOptionBench
{
    private const string PathToHTML = "CS.Edu.Benchmarks.Resources.document.html";
    private const string Link = "http://localhost/api/documents/1";

    private readonly MockHttpMessageHandler _mockHttp = new MockHttpMessageHandler();
    private readonly HttpClient _client;

    public HttpCompletionOptionBench()
    {
        string content;
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(PathToHTML))
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            content = reader.ReadToEnd();
        }

        _mockHttp.When(Link)
            .Respond("text/html", content);
        _client = _mockHttp.ToHttpClient();
    }

    [Benchmark]
    public Task<HttpResponseMessage> ResponseContentRead()
    {
        return _client.GetAsync(Link);
    }

    [Benchmark]
    public Task<HttpResponseMessage> ResponseHeadersRead()
    {
        return _client.GetAsync(Link, HttpCompletionOption.ResponseHeadersRead);
    }
}