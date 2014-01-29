using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace XNAContentCompiler
{
    public class MSBuildContentBuilder : IContentBuilder
    {
        private static int _directorySalt = 0xCAFE; 

        // What importers or processors should we load?
        private const string XnaVersion = ", Version=4.0.0.0, PublicKeyToken=842cf8be1de50553";
        private const string BUILD_ARTIFACTS = "bin";

        private static readonly string[] PipelineAssemblies =
        {
            "Microsoft.Xna.Framework.Content.Pipeline.FBXImporter" + XnaVersion,
            "Microsoft.Xna.Framework.Content.Pipeline.XImporter" + XnaVersion,
            "Microsoft.Xna.Framework.Content.Pipeline.TextureImporter" + XnaVersion,
            "Microsoft.Xna.Framework.Content.Pipeline.EffectImporter" + XnaVersion,
            "Microsoft.Xna.Framework.Content.Pipeline.AudioImporters" + XnaVersion,
            "Microsoft.Xna.Framework.Content.Pipeline.VideoImporters" + XnaVersion,

            // If you want to use custom importers or processors from
            // a Content Pipeline Extension Library, add them here.
            //
            // If your extension DLL is installed in the GAC, you should refer to it by assembly
            // name, eg. "MyPipelineExtension, Version=1.0.0.0, PublicKeyToken=1234567812345678".
            //
            // If the extension DLL is not in the GAC, you should refer to it by
            // file path, eg. "c:/MyProject/bin/MyPipelineExtension.dll".
        };

        private readonly List<ProjectItem> _projectItems = new List<ProjectItem>();
        private readonly ComboItemCollection _importers;

        private readonly string _tempBuildDirectory;

        // MSBuild objects used to dynamically build content.
        private ErrorLogger _errorLogger;
        private Project _buildProject;
        private ProjectRootElement _projectRootElement;
        private BuildParameters _buildParameters;


        /// <summary>
        /// Initializes a new instance of <see cref="MSBuildContentBuilder"/>
        /// </summary>
        public MSBuildContentBuilder()
        {
            var assemblyPath = Assembly.GetEntryAssembly().Location;
            _tempBuildDirectory = Path.Combine(Path.GetDirectoryName(assemblyPath), (_directorySalt++).ToString());

            _importers = new ComboItemCollection
            {
                new ComboItem(".mp3", "Mp3Importer", "SongProcessor"),
                new ComboItem(".wav", "WavImporter", "SoundEffectProcessor"),
                new ComboItem(".wma", "WmaImporter", "SongProcessor"),

                new ComboItem(".bmp", "TextureImporter", "TextureProcessor"),
                new ComboItem(".jpg", "TextureImporter", "TextureProcessor"),
                new ComboItem(".png", "TextureImporter", "TextureProcessor"),
                new ComboItem(".tga", "TextureImporter", "TextureProcessor"),
                new ComboItem(".dds", "TextureImporter", "TextureProcessor"),

                new ComboItem(".spritefont", "FontDescriptionImporter", "FontDescriptionProcessor")
            };

            CreateBuildProject();
        }

        ~MSBuildContentBuilder()
        { Dispose(false); }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implements the standard .NET IDisposable pattern.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            //delete 
        }

        /// <summary>
        /// Creates a temporary MSBuild content project in memory.
        /// </summary>
        void CreateBuildProject()
        {
            string projectPath = Path.Combine(_tempBuildDirectory, "content.contentproj");
            string outputPath = Path.Combine(_tempBuildDirectory, BUILD_ARTIFACTS);

            // Create the build project.
            _projectRootElement = ProjectRootElement.Create(projectPath);

            // Include the standard targets file that defines how to build XNA Framework content.
            _projectRootElement.AddImport("$(MSBuildExtensionsPath)\\Microsoft\\XNA Game Studio\\" +
                                          "v4.0\\Microsoft.Xna.GameStudio.ContentPipeline.targets");

            _buildProject = new Project(_projectRootElement);

            _buildProject.SetProperty("XnaPlatform", "Windows");
            _buildProject.SetProperty("XnaProfile", "Reach");
            _buildProject.SetProperty("XnaFrameworkVersion", "v4.0");
            _buildProject.SetProperty("Configuration", "Release");
            _buildProject.SetProperty("OutputPath", outputPath);

            // Register any custom importers or processors.
            foreach (string pipelineAssembly in PipelineAssemblies)
            {
                _buildProject.AddItem("Reference", pipelineAssembly);
            }

            // Hook up our custom error logger.
            _errorLogger = new ErrorLogger();

            _buildParameters = new BuildParameters(ProjectCollection.GlobalProjectCollection)
            {
                Loggers = new ILogger[] {_errorLogger}
            };
        }

        public string Build()
        {
            // Clear any previous errors.
            _errorLogger.Errors.Clear();

            // Create and submit a new asynchronous build request.
            BuildManager.DefaultBuildManager.BeginBuild(_buildParameters);

            var request = new BuildRequestData(_buildProject.CreateProjectInstance(), new string[0]);
            BuildSubmission submission = BuildManager.DefaultBuildManager.PendBuildRequest(request);

            submission.ExecuteAsync(null, null);

            // Wait for the build to finish.
            submission.WaitHandle.WaitOne();

            BuildManager.DefaultBuildManager.EndBuild();

            var output = new StringBuilder();

            // If the build failed, return an error string.
            if (submission.BuildResult.OverallResult == BuildResultCode.Failure)
            {
                output.Append(string.Join("\n", _errorLogger.Errors.ToArray()));
            }


            return output.ToString();
        }


        public void Add(ComboItem item)
        {
            ComboItem importer = _importers.FindByName(Path.GetExtension(item.Value).ToLower());
            Add(item.Value, Path.GetFileNameWithoutExtension(item.Name), importer.Value, importer.Other);
        }

        public void Add(string filename, string name)
        {
            Add(filename, name, null, null);
        }

        /// <summary>
        /// Adds a new content file to the MSBuild project. The importer and
        /// processor are optional: if you leave the importer null, it will
        /// be autodetected based on the file extension, and if you leave the
        /// processor null, data will be passed through without any processing.
        /// </summary>
        public void Add(string filename, string name, string importer, string processor)
        {
            ProjectItem item = _buildProject.AddItem("Compile", filename)[0];

            item.SetMetadataValue("Link", Path.GetFileName(filename));
            item.SetMetadataValue("Name", name);

            if (!string.IsNullOrEmpty(importer))
                item.SetMetadataValue("Importer", importer);

            if (!string.IsNullOrEmpty(processor))
                item.SetMetadataValue("Processor", processor);

            _projectItems.Add(item);
        }

        public void Clear()
        {
            _buildProject.RemoveItems(_projectItems);

            _projectItems.Clear();
        }

        public string OutputDirectory
        {
            get { return Path.Combine(_tempBuildDirectory); }
        }

        public string BuildArtifactsDirectory
        {
            get { return Path.Combine(_tempBuildDirectory, BUILD_ARTIFACTS, "content"); }
        }

        public IEnumerable<string> SupportedFileTypes
        {
            get { return _importers.Select(comboItem => comboItem.Name); }
        }

    }
}