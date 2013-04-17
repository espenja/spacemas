using System;

namespace SpaceMAS
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SpaceMAS game = new SpaceMAS())
            {
                game.Run();
            }
        }
    }
#endif
}

