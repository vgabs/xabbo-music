using System.Collections.Generic;

using Xabbo;
using Xabbo.Core;
using Xabbo.Messages;

namespace xabbo_music.Game;

public class CatalogOffer : ICatalogOffer
{
    public int Id { get; set; }
    public CatalogPage? Page { get; set; }
    ICatalogPage? ICatalogOffer.Page => Page;
    public string FurniLine { get; set; } = string.Empty;
    public bool IsRentable { get; set; }
    public int PriceInCredits { get; set; }
    public int PriceInActivityPoints { get; set; }
    public ActivityPointType ActivityPointType { get; set; }
    public int PriceInSilver { get; set; }
    public bool CanPurchaseAsGift { get; set; }
    public List<CatalogProduct> Products { get; set; } = new();
    IReadOnlyList<ICatalogProduct> ICatalogOffer.Products => Products;
    public int ClubLevel { get; set; }
    public bool CanPurchaseMultiple { get; set; }
    public bool IsPet { get; set; }
    public string PreviewImage { get; set; } = string.Empty;

    public CatalogOffer()
    {
        FurniLine = string.Empty;
        Products = new List<CatalogProduct>();
        PreviewImage = string.Empty;
    }

    protected CatalogOffer(IReadOnlyPacket packet, bool fromCatalog)
    {
        Id = packet.ReadInt();
        FurniLine = packet.ReadString();
        IsRentable = packet.ReadBool();
        PriceInCredits = packet.ReadInt();
        PriceInActivityPoints = packet.ReadInt();
        ActivityPointType = (ActivityPointType)packet.ReadInt();
        PriceInSilver = packet.ReadInt();
        CanPurchaseAsGift = packet.ReadBool();

        short n = packet.ReadLegacyShort();
        for (int i = 0; i < n; i++)
            Products.Add(CatalogProduct.Parse(packet));

        ClubLevel = packet.ReadInt();
        CanPurchaseMultiple = packet.ReadBool();

        if (fromCatalog)
        {
            IsPet = packet.ReadBool();
            PreviewImage = packet.ReadString();
        }
    }

    public void Compose(IPacket packet, bool fromCatalog = true)
    {
        packet
            .WriteInt(Id)
            .WriteString(FurniLine)
            .WriteBool(IsRentable)
            .WriteInt(PriceInCredits)
            .WriteInt(PriceInActivityPoints)
            .WriteInt((int)ActivityPointType)
            .WriteInt(PriceInSilver)
            .WriteBool(CanPurchaseAsGift)
            .WriteCollection(Products)
            .WriteInt(ClubLevel)
            .WriteBool(CanPurchaseMultiple);

        if (fromCatalog)
        {
            packet
                .WriteBool(IsPet)
                .WriteString(PreviewImage);
        }
    }

    void IComposable.Compose(IPacket packet) => Compose(packet, true);

    public static CatalogOffer Parse(IReadOnlyPacket packet, bool fromCatalog = true)
        => new(packet, fromCatalog);
}
