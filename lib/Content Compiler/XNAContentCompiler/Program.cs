using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XNAContentCompiler.Console;

namespace XNAContentCompiler
{
    public class Program
    {
        const string OutputDirectory = "./content";
        const string InputDirectory = "./source";

        private static readonly string[] HelpOptions = {"help", "h"};

        private readonly ConsoleExecutor _conExec;
        private readonly ILogger _logger;

        public Program()
        {
            _logger = new ConsoleLogger();
            _conExec = new ConsoleExecutor(_logger);


            _conExec.AddCommand("watch", InitFileWatch, strings => strings.Count() != 2,
                                "Watches the input directory for file changes.");

            _conExec.AddCommand("build", ExecAssetsBuild, strings => strings.Count() >= 2,
                                "Builds the assets in the input directory and copies the artifacts to the output directory.");

            _conExec.AddCommand("build-file", ExecAssetBuild, strings => strings.Count() >= 2,
                                "Builds the assets in the input directory and copies the artifacts to the output directory.");

            new[] {"h", "help"}.ToList()
                               .ForEach(commandString => _conExec.AddCommand(commandString, 
                                        (logger, args) => logger.Message(_conExec.CommandDescriptions.Aggregate((acc, item) => acc + string.Format("{0}\n", item)))));
        }

        public void Execute(string[] args)
        {
            var inputModel = new CommandInputModel(args);
            var completedSuccessfully = _conExec.Execute(inputModel); 

            _logger.Message("\n");

            if(completedSuccessfully)
                _logger.Success("Build Finished Successfully.") ;
            else
                _logger.Error("Build Failed.");
        }

        private void ExecAssetsBuild(ILogger logger, string[] arg)
        {
            var inputDirectory = Path.GetFullPath(arg[0]);
            var outputDirectory = Path.GetFullPath(arg[1]);

            var files = Directory.GetFiles(inputDirectory, "*", SearchOption.AllDirectories).Select(Path.GetFullPath);

            BuildFiles(files, outputDirectory);
        }

        private void ExecAssetBuild(ILogger logger, string[] arg)
        {
            var outputDirectory = Path.GetFullPath(arg[0]);
            var files = arg.Skip(1).Select(Path.GetFullPath);

             BuildFiles(files, outputDirectory);
        }

        private void InitFileWatch(ILogger logger, string[] arg)
        {
            var inputDirectory = Path.GetFullPath(arg[0]);
            var outputDirectory = Path.GetFullPath(arg[1]);

            FileWatch(inputDirectory, outputDirectory);

            _logger.Message(String.Format("Watching {0}...", inputDirectory));
        }

        public void BuildFiles(IEnumerable<string> files, string outputDirectory)
        {
            if (!files.Any())
                throw new ArgumentNullException("files");

            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentNullException("outputDirectory");


            _logger.Success("Building {0}....", Path.GetDirectoryName(files.First()));
            var contentBuilder = new MSBuildContentBuilder(outputDirectory);

            files.ToList().ForEach(filePath =>
            {
                var name = Path.GetFileName(filePath);
                _logger.Message("Queueing {0}....", filePath);
                contentBuilder.Add(filePath, name);
            });

            contentBuilder.Build();

            var buildDirectory = contentBuilder.OutputDirectory;
            
            _logger.Success("\nCopying files into {0}....\n", outputDirectory);
            // copy files from build directory into output
            CopyFiles(buildDirectory, outputDirectory);

            _logger.Success("\nDeleting build artifacts...\n");
            // delete build directory
            DeleteDirectory(buildDirectory);
        }

        public void FileWatch(string inputDirectory, string outputDirectory)
        {
            throw new NotImplementedException();
        }

        private void CopyFiles(string source, string outputDir)
        {
            var fullPath = Path.GetFullPath(source);
            var fullOutputDir = Path.GetFullPath(outputDir);

            var files = Directory.GetFiles(fullPath, "*.xnb", SearchOption.AllDirectories);

            files.ToList().ForEach(filePath =>
            {
                var fileName = Path.GetFileName(filePath);
                var finalPath = Path.Combine(fullOutputDir, fileName);
                _logger.Message("\t{0}....", finalPath);
                File.Copy(filePath, finalPath);
            });
        }

        private void DeleteDirectory(string directory)
        {
            if(Directory.Exists(directory))
                Directory.Delete(directory, true);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Any())
            {
                var p = new Program();
                p.Execute(args);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
