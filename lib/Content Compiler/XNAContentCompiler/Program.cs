using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace XNAContentCompiler
{
    static class Program
    {
        private const string OutputDirectory = "./content";
        private const string InputDirectory = "./source";

        private static readonly string[] HelpOptions = {"help", "h"};

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool fileWatch = "watch".Equals(args.FirstOrDefault());
            bool displayHelp = args.Any(HelpOptions.Contains);

            if (displayHelp)
            {
                Console.WriteLine(@"first argument: input directory");
                Console.WriteLine(@"second argument: output directory");
                Console.WriteLine(@"Third argument is optionally...");
                Console.WriteLine(@"	 watch: watches the input directory for file changes.");
                Console.WriteLine(@"      A space separated list of relative files located in the input directory");
            }
            if (args.Length <= 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else 
            {

                if (displayHelp)
                {
                    Console.WriteLine("");
                    Console.ReadLine();
                }
                else if (fileWatch)
                {
                    bool running = true;

                    var thread = new Thread(() =>
                    {
                        Console.WriteLine(@"Watching {0}....", InputDirectory);

                        var watcher = new FileSystemWatcher(InputDirectory);
                        watcher.BeginInit();
                        watcher.Changed += (sender, eventArgs) =>
                        {
                            if (eventArgs.ChangeType == WatcherChangeTypes.Changed)
                            {
                                Console.Write(@"{0} changed...", eventArgs.Name);
                                CompileFiles(eventArgs.FullPath);
                            }
                        };
                        watcher.Filter = "*.*";
                        watcher.EndInit();

                        while (running)
                        {
                            Thread.Sleep(1000);

                            if (Console.KeyAvailable && Console.ReadKey().KeyChar == 'q')
                                running = false;
                        }
                    })
                    {
                        IsBackground = false
                    };

                    thread.Start();


                    Thread.CurrentThread.Join();
                    Console.WriteLine(@"Thread watching terminated..");
                }
                else if (DirectoryExists(InputDirectory))
                {
                    CompileFiles(args);
                }
                else
                {
                    Console.WriteLine(@"Put source files into a {0} folder along side this tool.", InputDirectory);
                }
            }
        }

        private static void CompileFiles(params string[] files)
        {
            // create output directory
            if (!DirectoryExists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);

            using (var contentBuilder = new ContentBuilder())
            {
                var fileArguments = files.Where(IsValidFile).ToList();

                if (fileArguments.Any())
                    fileArguments.ForEach(file => contentBuilder.Add(file, Path.GetFileName(file)));

                contentBuilder.Build();

                var tempDir = contentBuilder.OutputDirectory;

                CopyOutputFilesToContentDirectory(tempDir, OutputDirectory);
            }
        }

        private static void CopyOutputFilesToContentDirectory(string source, string destination)
        {
            var filesToCopy = Directory.GetFiles(source, "*.xnb").ToList();

            Console.ForegroundColor = ConsoleColor.Green;
                filesToCopy.ForEach(file =>
                {
                    var fileName = Path.GetFileName(file);
                    var filePath = Path.Combine(destination, fileName);

                    Console.Write(@"   {0}...", fileName);
                    File.Copy(file, filePath, true);
                    Console.WriteLine(@"....Copied");
                });

            Console.ForegroundColor = ConsoleColor.White;
            Directory.Delete(source);

        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool IsValidFile(string file)
        {
            var invalidCharacters = Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars());
            return file.ToCharArray().Any(item => invalidCharacters.Any(innerItem => item == innerItem));
        }
    }
}
