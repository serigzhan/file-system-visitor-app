using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitorApp
{
    public class FileSystemVisitorEventArgs(string path) : EventArgs
    {
        public string Path { get; } = path;
        public bool Abort { get; set; }
        public bool Exlude { get; set; }
    }

    public class FileSystemVisitor
    {

        private readonly string _rootPath;
        private readonly Func<string, bool>? _fileFilter;
        private readonly Func<string, bool>? _dirFilter;

        public event EventHandler? Start;
        public event EventHandler? Finish;
        public event EventHandler<FileSystemVisitorEventArgs>? FileFound;
        public event EventHandler<FileSystemVisitorEventArgs>? DirectoryFound;
        public event EventHandler<FileSystemVisitorEventArgs>? FilteredFileFound;
        public event EventHandler<FileSystemVisitorEventArgs>? FilteredDirectoryFound;

        public FileSystemVisitor(
            string rootPath,
            Func<string, bool>? fileFilter,
            Func<string, bool>? folderFilter
        )
        {

            _rootPath = rootPath;
            _fileFilter = fileFilter;
            _dirFilter = folderFilter;

        }

        public void Visit()
        {

            Start?.Invoke(this, EventArgs.Empty);

            foreach (var item in Traverse(_rootPath))
            {
                Console.WriteLine($"-> {item}");
            }

            Finish?.Invoke(this, EventArgs.Empty);

        }

        private IEnumerable<string> Traverse (string path)
        {
            foreach (var file in FileSystemVisitorHelpers.SafeGetFiles(path))
            {

                var args = new FileSystemVisitorEventArgs(file);
                FileFound?.Invoke(this, args);

                if (_fileFilter != null && _fileFilter(file))
                {

                    FilteredFileFound?.Invoke(this, args);

                }

                yield return file;

            }

            foreach (var dir in FileSystemVisitorHelpers.SafeGetDirectories(path))
            {

                var args = new FileSystemVisitorEventArgs(dir);
                DirectoryFound?.Invoke(this, args);

                if (args.Abort) yield break;

                if (!args.Exlude)
                {

                    if (_dirFilter != null && _dirFilter(dir))
                    {

                        FilteredDirectoryFound?.Invoke(this, args);

                    }

                    yield return dir;

                    foreach (var subItem in Traverse(dir))
                    {
                        yield return subItem;
                    }

                }

            }
        }
    }
}
