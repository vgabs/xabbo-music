using System.Windows.Media;
using xabbo_music.Enum;

namespace xabbo_music.Extensions
{
    public static class EffectExtensions
    {
        public static Brush ToBrush(this Effect effect)
        {
            return effect switch
            {
                Effect.Xylophone => new SolidColorBrush(Color.FromArgb(0xFF, 0x85, 0xC4, 0x0F)),
                Effect.XylophoneHigh => new SolidColorBrush(Color.FromArgb(0xFF, 0xD5, 0x6F, 0x3E)),
                Effect.FunkyHorn => new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0xB5, 0x06)),
                Effect.Pad => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x8C, 0x00)),          // Orange
                Effect.Duck => new SolidColorBrush(Color.FromArgb(0xFF, 0x04, 0xD0, 0xD7)),
                Effect.DoubleBass => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x80)),    // Hot Pink
                Effect.Bass => new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x99, 0x00)),         // Dark Green
                Effect.Pad2 => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00)),         // Red
                Effect.Pad3 => new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0xFF)),         // Blue
                Effect.SquarePad => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x99, 0x99)),    // Coral
                Effect.Glass => new SolidColorBrush(Color.FromArgb(0xFF, 0x2E, 0xC0, 0xFF)),        // Sky Blue
                Effect.Whistle => new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xCC, 0xFF)),      // Light Blue
                Effect.XylophonePattern => new SolidColorBrush(Color.FromArgb(0xFF, 0x80, 0x00, 0xFF)), // Purple
                Effect.XylophoneHighPattern => new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF)), // Fuchsia
                _ => throw new System.NotImplementedException(),
            };
        }

        public static string ToMiniature(this Effect effect)
        {
            return effect switch
            {
                Effect.Xylophone => "/Images/sfx_xylo_miniature.png",
                Effect.XylophoneHigh => "/Images/sfx_xyloHigh_miniature.png",
                Effect.FunkyHorn => "/Images/sfx_funkyHorn_miniature.png",
                Effect.Pad => "/Images/sfx_pad_miniature.png",
                Effect.Duck => "/Images/sfx_duck_miniature.png",
                Effect.DoubleBass => "/Images/sfx_doubleBass_miniature.png",
                Effect.Bass => "/Images/sfx_bass_miniature.png",
                Effect.Pad2 => "/Images/sfx_pad2_miniature.png",
                Effect.Pad3 => "/Images/sfx_pad3_miniature.png",
                Effect.SquarePad => "/Images/sfx_squarePad_miniature.png",
                Effect.Glass => "/Images/sfx_glass_miniature.png",
                Effect.Whistle => "/Images/sfx_whistle_miniature.png",
                Effect.XylophonePattern => "/Images/sfx_xylophonePattern_miniature.png",
                Effect.XylophoneHighPattern => "/Images/sfx_xylophoneHighPattern_miniature.png",
                _ => throw new System.NotImplementedException(),
            };
        }

        public static string ToText(this Effect effect)
        {
            return effect switch
            {
                Effect.Xylophone => "Xylophone",
                Effect.XylophoneHigh => "Xylo High",
                Effect.FunkyHorn => "Funky Horn",
                Effect.Pad => "Pad",
                Effect.Duck => "Duck",
                Effect.DoubleBass => "Double Bass",
                Effect.Bass => "Bass",
                Effect.Pad2 => "Pad 2",
                Effect.Pad3 => "Pad 3",
                Effect.SquarePad => "Square Pad",
                Effect.Glass => "Glass",
                Effect.Whistle => "Whistle",
                Effect.XylophonePattern => "Xylo Pattern",
                Effect.XylophoneHighPattern => "X High Pattern",
                _ => throw new System.NotImplementedException(),
            };
        }

        public static string ToIdentifier(this Effect effect)
        {
            return effect switch
            {
                Effect.Xylophone => "sfx_xylo",
                Effect.XylophoneHigh => "sfx_xylo_high",
                Effect.FunkyHorn => "sfx_funkhorn",
                Effect.Pad => "sfx_pad1",
                Effect.Duck => "sfx_duck",
                Effect.DoubleBass => "sfx_bass_dbl",
                Effect.Bass => "sfx_bass1",
                Effect.Pad2 => "sfx_pad2",
                Effect.Pad3 => "sfx_pad3",
                Effect.SquarePad => "sfx_sqrpad_dbldotted",
                Effect.Glass => "sfx_glass",
                Effect.Whistle => "sfx_whistle",
                Effect.XylophonePattern => "sfx_xylo2",
                Effect.XylophoneHighPattern => "sfx_xylopattern",
                _ => throw new System.NotImplementedException(),
            };
        }

        public static Effect ToEffect(this string effectString)
        {
            return effectString switch
            {
                "Xylophone" => Effect.Xylophone,
                "Xylo_High" => Effect.XylophoneHigh,
                "Funky_Horn" => Effect.FunkyHorn,
                "Pad" => Effect.Pad,
                "Duck" => Effect.Duck,
                "Double_Bass" => Effect.DoubleBass,
                "Bass" => Effect.Bass,
                "Pad_2" => Effect.Pad2,
                "Pad_3" => Effect.Pad3,
                "Square_Pad" => Effect.SquarePad,
                "Glass" => Effect.Glass,
                "Whistle" => Effect.Whistle,
                "Xylo_Pattern" => Effect.XylophonePattern,
                "X_High_Pattern" => Effect.XylophoneHighPattern,
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
