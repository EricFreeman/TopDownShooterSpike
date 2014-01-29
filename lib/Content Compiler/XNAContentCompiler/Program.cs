using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows.Forms;
using XNAContentCompiler.Console;

namespace XNAContentCompiler
{
    public class Program
    {
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
                                        (logger, args) => logger.Success(_conExec.CommandDescriptions.Aggregate((acc, item) => acc + string.Format("{0}\n", item)))));
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

            Environment.CurrentDirectory = inputDirectory;

            var files =
                Directory.GetFiles(inputDirectory, "*", SearchOption.AllDirectories)
                    .Select(filePath => filePath.Replace(inputDirectory + "\\", ""));

            BuildFiles(files, outputDirectory);
        }

        private void ExecAssetBuild(ILogger logger, string[] arg)
        {
            var outputDirectory = Path.GetFullPath(arg[0]);
            var files = arg.Skip(1);

            Environment.CurrentDirectory = Assembly.GetEntryAssembly().Location;

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

            var targetDirectory = Environment.CurrentDirectory;
            _logger.Success("Building {0}....\n", targetDirectory);
            var contentBuilder = new MSBuildContentBuilder();

            var tempBuildItemDirectory = contentBuilder.OutputDirectory;

            _logger.Success("Queueing files for build....");
            files.Where(file => contentBuilder.SupportedFileTypes.Contains(Path.GetExtension(file)))
                 .ToList()
                 .ForEach(filePath =>
                 {
                     var fullPath = Path.Combine(targetDirectory, filePath);
                     var transformedFilePath = Path.Combine(Path.GetDirectoryName(filePath), 
                                                            Path.GetFileNameWithoutExtension(filePath));

                     _logger.Message("\t{0}", transformedFilePath);
                     contentBuilder.Add(fullPath, transformedFilePath);
                 });

            var buildErrors = contentBuilder.Build();
            _logger.Error(buildErrors);

            if (string.IsNullOrWhiteSpace(buildErrors))
            {
                var buildDirectory = contentBuilder.BuildArtifactsDirectory;
                // copy files from build directory into output
                CopyFiles(buildDirectory, outputDirectory);
            }
            else
            {
                throw new InvalidOperationException(buildErrors);
            }

            // delete build directory
            DeleteDirectory(tempBuildItemDirectory);
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

            if (!Directory.Exists(fullOutputDir))
            {
                _logger.Warn("Directory doesn't exist. Creating {0}.....", fullOutputDir);
                Directory.CreateDirectory(fullOutputDir);
            }

            _logger.Success("\nCopying files...\nTarget: {0}\n", outputDir);
            files.Select(filePath => filePath.Replace(source, "").Trim('\\'))
                 .Select(assetName => Tuple.Create(assetName, Path.Combine(source, assetName), Path.Combine(fullOutputDir, assetName)))
                 .ToList().ForEach(tuple =>
                 {
                     var destinationDirectory = Path.GetDirectoryName(tuple.Item3);
                     if (!Directory.Exists(destinationDirectory))
                         Directory.CreateDirectory(destinationDirectory);

                     _logger.Message("\t{0}", tuple.Item1);
                     File.Copy(tuple.Item2, tuple.Item3);
                });
        }

        private void DeleteDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                _logger.Warn("\nDeleting {0}\n", directory);
                Directory.Delete(directory, true);
            }
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
