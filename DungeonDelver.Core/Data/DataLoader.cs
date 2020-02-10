using DungeonDelver.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Data
{
    public static class DataLoader
    {
        private const string TexturesPath = "Textures/";
        private const float AnimationFrameDuration = 0.4f;

        public static void Initialize()
        {
            Spritesheet tiles = LoadSpritesheet("environment");
            Spritesheet monsters = LoadSpritesheet("monsters");

            Sprite wallSprite = LoadSprite("wall", 0, 0, tiles);
            Sprite floorSprite = LoadSprite("floor", 9, 0, tiles);

            LoadTile(0, "wall", wallSprite, Color.Brown, true, false);
            LoadTile(1, "floor", floorSprite, Color.Gray, false, true);

            LoadMonsterAnimation("player", 0, 0, monsters);

            LoadMonsterAnimation("crab", 0, 4, monsters);
            LoadMonsterAnimation("rat", 1, 4, monsters);
            LoadMonsterAnimation("spider", 2, 4, monsters);
            //LoadMonsterAnimation("", 3, 4, monsters);
            LoadMonsterAnimation("giant_rat", 4, 4, monsters);
            LoadMonsterAnimation("pig", 5, 4, monsters);
            LoadMonsterAnimation("bat", 6, 4, monsters);
            //LoadMonsterAnimation("crab", 7, 4, monsters);
            LoadMonsterAnimation("cobra", 8, 4, monsters);
            LoadMonsterAnimation("lizard", 9, 4, monsters);
            LoadMonsterAnimation("frog", 10, 4, monsters);
            //LoadMonsterAnimation("crab", 11 4, monsters);
            //LoadMonsterAnimation("crab", 12, 4, monsters);
            LoadMonsterAnimation("cat", 13, 4, monsters);
            //LoadMonsterAnimation("crab", 14, 4, monsters);
            LoadMonsterAnimation("bird_a", 15, 4, monsters);
            LoadMonsterAnimation("bird_b", 16, 4, monsters);
            LoadMonsterAnimation("centipede", 17, 4, monsters);
            LoadMonsterAnimation("salamander", 18, 4, monsters);

        }

        public static Tile LoadTile(byte id, string name, Sprite sprite, Color color, bool isSolid, bool isTransparent)
        {
            Tile tile;

            if (Engine.Assets.Has<Tile>(name))
            {
                tile = Engine.Assets.GetAsset<Tile>(name);
            }
            else
            {
                tile = new Tile(id, sprite, color, isSolid, isTransparent);
                Engine.Assets.AddAsset(name, tile);
            }

            return tile;
        }

        public static AnimatedSprite LoadMonsterAnimation(string name, int x, int y, Spritesheet sheet)
        {
            AnimatedSprite animation;

            if (Engine.Assets.Has<AnimatedSprite>(name))
            {
                animation = Engine.Assets.GetAsset<AnimatedSprite>(name);
            }
            else
            {
                Sprite sprite1 = LoadSprite($"{name}_0", x, y, sheet);
                Sprite sprite2 = LoadSprite($"{name}_1", x, y + 1, sheet);

                AnimationFrame frame1 = new AnimationFrame(sprite1);
                AnimationFrame frame2 = new AnimationFrame(sprite2);

                animation = new AnimatedSprite(AnimationFrameDuration, frame1, frame2);
                Engine.Assets.AddAsset(name, animation);
            }

            return animation;
        }

        public static Sprite LoadSprite(string name, int x, int y, Spritesheet sheet, Vector2? origin = null)
        {
            Sprite sprite;

            if (Engine.Assets.Has<Sprite>(name))
            {
                sprite = Engine.Assets.GetAsset<Sprite>(name);
            }
            else
            {
                sprite = sheet.CutSprite(x, y, Game.SpriteWidth, Game.SpriteHeight, name, origin);
                Engine.Assets.AddAsset(name, sprite);
            }

            return sprite;
        }

        public static Spritesheet LoadSpritesheet(string name)
        {
            Spritesheet sheet;

            if (Engine.Assets.Has<Spritesheet>(name))
            {
                sheet = Engine.Assets.GetAsset<Spritesheet>(name);
            }
            else
            {
                sheet = new Spritesheet(name, LoadTexture(Path.Combine(TexturesPath, name)));
                Engine.Assets.AddAsset(name, sheet);
            }

            return sheet;
        }

        public static Texture2D LoadTexture(string path)
        {
            return Engine.Instance.Content.Load<Texture2D>(path);
        }
    }
}
