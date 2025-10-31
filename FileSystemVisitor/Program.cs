using System;

namespace FileSystemVisitorApp
{

    class Program
    {

        static void Main()
        {
            string path = string.Empty;

            while (true)
            {
                Console.Write("Enter path to visit: ");
                path = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.WriteLine("Path cannot be empty.");
                    continue;
                }

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Directory does not exist. Try again.\n");
                    continue;
                }

                break;
            }

            var visitor = new FileSystemVisitor(
                path,
                fileName => fileName.EndsWith(".txt"),
                dirName => Path.GetFileName(dirName).StartsWith("test", StringComparison.CurrentCultureIgnoreCase)
            );

            visitor.Start += (sender, e) => Console.WriteLine("Search started");

            visitor.Finish += (sender, e) => Console.WriteLine("Search finished");

            visitor.FileFound += (sender, e) => Console.WriteLine($"File found: {Path.GetFileName(e.Path)}");

            visitor.FilteredFileFound += (sender, e) => Console.WriteLine($"Filterd file: {Path.GetFileName(e.Path)}");

            visitor.DirectoryFound += (sender, e) =>
            {
                Console.WriteLine($"Directory Found: {Path.GetFileName(e.Path)}");
                e.Abort = e.Path.Contains("Abort");
            };


            visitor.FilteredDirectoryFound += (sender, e) => Console.WriteLine($"Filtered Directory: {Path.GetFileName(e.Path)}");

            try
            {
                visitor.Visit();
            } catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }

}