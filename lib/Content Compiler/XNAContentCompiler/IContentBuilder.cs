using System;

namespace XNAContentCompiler
{
    public interface IContentBuilder : IDisposable
    {
        /// <summary>
        /// Gets the output directory, which will contain the generated .xnb files.
        /// </summary>
        string OutputDirectory { get; }

        string BuildArtifactsDirectory { get; }

        /// <summary>
        /// Builds all the content files which have been added to the project,
        /// dynamically creating .xnb files in the OutputDirectory.
        /// Returns an error message if the build fails.
        /// </summary>
        string Build();

        void Add(string filename, string name);

        /// <summary>
        /// Removes all content files from the MSBuild project.
        /// </summary>
        void Clear();
    }
}