using DungeonDelver.Core;
using System;

namespace DungeonDelver.DirectX
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            const int gameWidth = 512;
            const int gameHeight = 256;
            const bool fullscreen = false;
            const string title = "Dungeon Delver";

            using (Engine engine = new Engine(gameWidth, gameHeight, fullscreen, title))
            {
                engine.Run();
            }
        }
    }
#endif
}
