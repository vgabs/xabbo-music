using xabbo_music.Enum;

namespace xabbo_music.Extensions
{
    public static class HotelExtensions
    {
        public static HHotel HostToHotel(this string host)
        {
            return host switch
            {
                "game-br.habbo.com" => HHotel.ComBr,
                "game-tr.habbo.com" => HHotel.ComTr,
                "game-us.habbo.com" => HHotel.Com,
                "game-de.habbo.com" => HHotel.De,
                "game-es.habbo.com" => HHotel.Es,
                "game-fi.habbo.com" => HHotel.Fi,
                "game-fr.habbo.com" => HHotel.Fr,
                "game-it.habbo.com" => HHotel.It,
                "game-nl.habbo.com" => HHotel.Nl,
                _ => HHotel.Unknown
            };
        }

        public static string HotelToUrl(this HHotel hotel)
        {
            return hotel switch
            {
                HHotel.ComBr => "https://www.habbo.com.br/",
                HHotel.ComTr => "https://www.habbo.com.tr/",
                HHotel.Com => "https://www.habbo.com/",
                HHotel.De => "https://www.habbo.de/",
                HHotel.Es => "https://www.habbo.es/",
                HHotel.Fi => "https://www.habbo.fi/",
                HHotel.Fr => "https://www.habbo.fr/",
                HHotel.It => "https://www.habbo.it/",
                HHotel.Nl => "https://www.habbo.nl/",
                _ => ""
            };
        }
    }
}
