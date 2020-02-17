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
            const int scale = 3;
            const int width = 640;
            const int height = 360;
            const int windowWidth = width * scale;
            const int windowHeight = height * scale;
            const bool fullscreen = false;
            const string title = "Dungeon Delver";

            using (Engine engine = new Engine(width, height, windowWidth, windowHeight, scale, title, fullscreen))
            {
                engine.Run();
            }
        }
    }
#endif
}

