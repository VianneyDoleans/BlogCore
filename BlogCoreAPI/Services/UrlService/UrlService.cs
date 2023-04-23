using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BlogCoreAPI.Services.UrlService;

public class UrlService : IUrlService
{
    private HttpClient Client { get; }
    
    public UrlService(HttpClient client)
    {
        Client = client;
    }

    public bool IsUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var hypotheticalUrl) &&
               (hypotheticalUrl.Scheme == Uri.UriSchemeHttp ||
                hypotheticalUrl.Scheme == Uri.UriSchemeHttps);
    }
    
    public async Task<bool> IsUrlPicture(string url, CancellationToken token)
    {
        try
        {
            var response = await Client.GetAsync(url, token);

            if (response.Content.Headers.ContentType?.MediaType != null)
            {
                if (response.Content.Headers.ContentType.MediaType.StartsWith("image/"))
                    return true;
            }
        }
        catch (HttpRequestException) { }
        catch (InvalidOperationException) { }
        return false;
    }
}