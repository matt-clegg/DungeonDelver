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
            const int windowWidth = 640 * 2;
            const int windowHeight = 384 * 2;
            const int gameWidth = 640 * 2;
            const int gameHeight = 384 * 2;
            const int scale = 1;
            const string title = "Dungeon Delver";

            using (Engine engine = new Engine(windowWidth, windowHeight, gameWidth, gameHeight, scale, title))
            {
                engine.Run();
            }
        }
    }
#endif
}
