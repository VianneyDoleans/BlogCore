using System.Threading;
using System.Threading.Tasks;

namespace BlogCoreAPI.Services.UrlService;

public interface IUrlService
{
    public bool IsUrl(string url);

    public Task<bool> IsUrlPicture(string url, CancellationToken token);
}