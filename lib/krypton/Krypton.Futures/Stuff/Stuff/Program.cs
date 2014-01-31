// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Christopher Harris">
//   2011 Christopher Harris
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Futures
{
    /// <summary>
    /// The Applition which launches the Game
    /// </summary>
    public static class Program
    {
#if WINDOWS || XBOX
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            using (var game = new Game2())
            {
                game.Run();
            }
        }
#endif
    }
}