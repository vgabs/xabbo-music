using System.Collections.Generic;

using Xabbo;
using Xabbo.Core;
using Xabbo.Messages;

namespace xabbo_music.Game;

public class CatalogPage : ICatalogPage
{
    public int Id { get; set; }
    public string CatalogType { get; set; }
    public string LayoutCode { get; set; }
    public List<string> Images { get; set; }
    IReadOnlyList<string> ICatalogPage.Images => Images;
    public List<string> Texts { get; set; }
    IReadOnlyList<string> ICatalogPage.Texts => Texts;
    public List<CatalogOffer> Offers { get; set; }
    IReadOnlyList<ICatalogOffer> ICatalogPage.Offers => Offers;
    public int OfferId { get; set; }
    public bool AcceptSeasonCurrencyAsCredits { get; set; }
    public List<CatalogPageItem> FrontPageItems { get; set; }
    IReadOnlyList<ICatalogPageItem> ICatalogPage.Data => FrontPageItems;

    public CatalogPage()
    {
        CatalogType =
        LayoutCode = string.Empty;

        Images = new();
        Texts = new();
        Offers = new();
        FrontPageItems = new();
    }

    protected CatalogPage(IReadOnlyPacket packet)
    {
        Id = packet.ReadInt();
        CatalogType = packet.ReadString();
        LayoutCode = packet.ReadString();

        Images = packet.ReadList<string>();
        Texts = packet.ReadList<string>();

        Offers = new();
        int n = packet.ReadLegacyShort();
        for (int i = 0; i < n; i++)
        {
            CatalogOffer offer = CatalogOffer.Parse(packet);
            offer.Page = this;
            Offers.Add(offer);
        }

        OfferId = packet.ReadInt();
        AcceptSeasonCurrencyAsCredits = packet.ReadBool();

        FrontPageItems = new();
        if (packet.Available > 0)
        {
            n = packet.ReadLegacyShort();
            for (int i = 0; i < n; i++)
                FrontPageItems.Add(CatalogPageItem.Parse(packet));
        }
    }

    public void Compose(IPacket packet)
    {
        packet
            .WriteInt(Id)
            .WriteString(CatalogType)
            .WriteString(LayoutCode)
            .WriteCollection(Images)
            .WriteCollection(Texts)
            .WriteCollection(Offers)
            .WriteInt(OfferId)
            .WriteBool(AcceptSeasonCurrencyAsCredits)
            .WriteCollection(FrontPageItems);
    }

    public static CatalogPage Parse(IReadOnlyPacket packet) => new CatalogPage(packet);
}
