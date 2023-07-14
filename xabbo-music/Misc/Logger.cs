using xabbo_music.Enum;

using System.IO;

namespace xabbo_music.Misc
{
    public static class Logger
    {
        private static string magicTileFilePath = "files/magicTile.txt";
        private static string songsFolderPath = "files/songs/";

        public static void Initiate()
        {
            if (!Directory.Exists("files/"))
                Directory.CreateDirectory("files/");

            if (!Directory.Exists("files/songs/"))
                Directory.CreateDirectory("files/songs/");

            if (!File.Exists(magicTileFilePath))
                File.Create(magicTileFilePath).Dispose();
        }

        public static string ReadAllText(FileType fileType, string name = "")
        {
            Initiate();

            using var streamReader = new StreamReader(fileType == FileType.Song ? $"{songsFolderPath}{name}.txt" : magicTileFilePath);
            var result = streamReader.ReadToEnd();
            streamReader.Dispose();
            return result;
        }

        public static void WriteAllText(FileType fileType, string input, string name = "")
        {
            Initiate();
            File.WriteAllText(fileType == FileType.Song ? $"{songsFolderPath}{name}.txt" : magicTileFilePath, input);
        }
    }
}
