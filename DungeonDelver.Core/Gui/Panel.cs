using Microsoft.Xna.Framework.Graphics;

namespace DungeonDelver.Core.Gui
{
    public enum PanelEdgeType
    {
        Thick,
        Checkered,
        Ornate
    }

    public class Panel
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        private readonly PanelEdgeType _edgeType;

        public Panel(int x, int y, int width, int height, PanelEdgeType edgeType = PanelEdgeType.Thick)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _edgeType = edgeType;
        }

        public void Render(SpriteBatch batch)
        {

        }
    }
}
