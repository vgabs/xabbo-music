using System.Collections.Generic;
using System.Linq;
using System;

namespace xabbo_music
{
    public static class Effects
    {
        public static string[] ConvertedNotes = new string[] { "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B", "C", "C#", "D" };
        public static string[] XyloNotes = new string[] { "z", "s", "x", "d", "c", "v", "g", "b", "h", "n", "j", "m", "," };
        public static string[] XyloHighNotes = new string[] { "q", "2", "w", "3", "e", "r", "5", "t", "6", "y", "7", "u", "i" };
        public static string[] FunkHornNotes = new string[] { "←", "∟", "↔", "▲", "▼", "!", "#", "$", "%", "&", "(", ")", "*" };
        public static string[] PadNotes = new string[] { "☺", "☻", "♥", "♦", "♣", "♠", "•", "◘", "○", "◙", "♂", "♀", "♪" };
        public static string[] DuckNotes = new string[] { "♫", "☼", "►", "◄", "↕", "‼", "¶", "§", "▬", "↨", "↑", "↓", "→" };
        public static string[] DoubleBassNotes = new string[] { "♔", "♕", "♖", "♗", "♘", "♙", "♚", "♛", "♜", "♝", "♞", "♟", "⛖" };
        public static string[] BassNotes = new string[] { "♨", "♩", "♧", "Ɨ", "Ɗ", "♭", "♮", "♯", "♰", "♱", "♲", "♳", "⛗" };
        public static string[] Pad2Notes = new string[] { "Ƌ", "♡", "♢", "ƌ", "♤", "ƍ", "Ǝ", "Ə", "⛛", "Ɛ", "Ƒ", "ƒ", "⛘" };
        public static string[] Pad3Notes = new string[] { "⚀", "⚁", "⚂", "⚃", "⚄", "⚅", "Ɠ", "Ɣ", "⚈", "⚉", "⚆", "⚇", "⛙" };
        public static string[] SquarePadNotes = new string[] { "⚙", "⚚", "⚛", "⚜", "⚝", "⚞", "⚟", "⚠", "⚡", "⚢", "⚣", "⚤", "⛚" };
        public static string[] GlassNotes = new string[] { "⚦", "⚧", "⚨", "⚩", "⚪", "⚫", "⚬", "⚭", "⚮", "⚯", "⚰", "⚱", "ƕ" };
        public static string[] WhistleNotes = new string[] { "⚲", "⚳", "⚴", "⚵", "⚶", "⚷", "⚸", "⚹", "⚺", "⚻", "⚼", "⚽", "⛜" };
        public static string[] XyloPatternNotes = new string[] { "⚾", "⚿", "⛀", "⛁", "⛂", "⛃", "⛄", "⛅", "⛆", "⛇", "⛈", "⛉", "⛝" };
        public static string[] XHighPatternNotes = new string[] { "⛊", "⛋", "⛌", "⛍", "⛎", "⛏", "⛐", "⛑", "⛒", "⛓", "⛔", "⛕", "⛞" };

        public static List<(string, string[])> IdentifiersAndNotes = new List<(string, string[])>()
        {
            ("sfx_xylo", XyloNotes),
            ("sfx_xylo_high", XyloHighNotes),
            ("sfx_funkhorn", FunkHornNotes),
            ("sfx_pad1", PadNotes),
            ("sfx_duck", DuckNotes),
            ("sfx_bass_dbl", DoubleBassNotes),
            ("sfx_bass1", BassNotes),
            ("sfx_pad2", Pad2Notes),
            ("sfx_pad3", Pad3Notes),
            ("sfx_sqrpad_dbldotted", SquarePadNotes),
            ("sfx_glass", GlassNotes),
            ("sfx_whistle", WhistleNotes),
            ("sfx_xylo2", XyloPatternNotes),
            ("sfx_xylopattern", XHighPatternNotes),
        };

        public static List<(Enum.Effect, string[])> EffectsAndNotes = new List<(Enum.Effect, string[])>()
        {
            (Enum.Effect.Xylophone, XyloNotes),
            (Enum.Effect.XylophoneHigh, XyloHighNotes),
            (Enum.Effect.FunkyHorn, FunkHornNotes),
            (Enum.Effect.Pad, PadNotes),
            (Enum.Effect.Duck, DuckNotes),
            (Enum.Effect.DoubleBass, DoubleBassNotes),
            (Enum.Effect.Bass, BassNotes),
            (Enum.Effect.Pad2, Pad2Notes),
            (Enum.Effect.Pad3, Pad3Notes),
            (Enum.Effect.SquarePad, SquarePadNotes),
            (Enum.Effect.Glass, GlassNotes),
            (Enum.Effect.Whistle, WhistleNotes),
            (Enum.Effect.XylophonePattern, XyloPatternNotes),
            (Enum.Effect.XylophoneHighPattern, XHighPatternNotes)
        };

        public static List<Enum.Effect> List = new()
        {
            Enum.Effect.Xylophone,
            Enum.Effect.XylophoneHigh,
            Enum.Effect.FunkyHorn,
            Enum.Effect.Pad,
            Enum.Effect.Duck,
            Enum.Effect.DoubleBass,
            Enum.Effect.Bass,
            Enum.Effect.Pad2,
            Enum.Effect.Pad3,
            Enum.Effect.SquarePad,
            Enum.Effect.Glass,
            Enum.Effect.Whistle,
            Enum.Effect.XylophonePattern,
            Enum.Effect.XylophoneHighPattern,
        };

        public static List<string[]> Notes() => IdentifiersAndNotes.Select(x => x.Item2).ToList();

        public static Enum.Effect GetEffect(string effectNote)
        {
            foreach (var effectAndNotes in EffectsAndNotes)
                if (effectAndNotes.Item2.Contains(effectNote))
                    return effectAndNotes.Item1;

            return Enum.Effect.Unknown;
        }
    }
}
