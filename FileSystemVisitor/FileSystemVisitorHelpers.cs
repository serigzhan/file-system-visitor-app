namespace FileSystemVisitorApp
{
    internal static class FileSystemVisitorHelpers
    {
        public static IEnumerable<string> SafeGetDirectories(string path)
        {

            try
            {

                return Directory.GetDirectories(path);

            }
            catch
            {

                return [];

            }

        }


        public static IEnumerable<string> SafeGetFiles(string path)
        {

            try
            {

                return Directory.GetFiles(path);

            }
            catch
            {

                return [];

            }

        }

        public static string GetCurrentDirName(string path)
        {

            return Path.GetFileName(path);

        }
    }
}