using System;
using System.Threading.Tasks;

using Xabbo;
using Xabbo.Core;
using Xabbo.Interceptor;
using Xabbo.Interceptor.Tasks;
using Xabbo.Messages;

namespace xabbo_music.Game;

/// <summary>
/// Gets a catalog page by its ID.
/// </summary>
public sealed class GetCatalogPageTask : InterceptorTask<ICatalogPage>
{
    private readonly int _pageId;
    private readonly string _catalogType;

    public GetCatalogPageTask(IInterceptor interceptor, int pageId, string catalogType)
        : base(interceptor)
    {
        if (pageId <= 0)
            throw new ArgumentException($"Invalid catalog page ID: {pageId}.");

        _pageId = pageId;
        _catalogType = catalogType;
    }

    protected override ValueTask OnExecuteAsync() => Interceptor.SendAsync(Out.GetCatalogPage, _pageId, -1, _catalogType);

    [InterceptIn(nameof(Incoming.CatalogPage))]
    internal void HandleCatalogPage(InterceptArgs e)
    {
        try
        {
            var catalogPage = CatalogPage.Parse(e.Packet);
            if (catalogPage.Id == _pageId &&
                catalogPage.CatalogType == _catalogType)
            {
                if (SetResult(catalogPage))
                    e.Block();
            }
        }
        catch (Exception ex) { SetException(ex); }
    }
}
