using DungeonDelver.Core.Data.Definitions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Assets;
using Toolbox.Data;
using Toolbox.Graphics;
using Toolbox.Graphics.Animations;

namespace DungeonDelver.Core.Data
{
    public static class AnimationLoader
    {
        public static void Load(string path, AssetStore<string> assets)
        {
            foreach (PropertyBag animProp in PropertyBag.FromFile(path))
            {
                string name = animProp.Name;
                Spritesheet sheet = assets.GetAsset<Spritesheet>(animProp.GetOrDefault("sheet", string.Empty));
                float frameDuration = animProp.GetOrDefault("frameDuration", 0.5f);

                string[] frames = animProp.GetOrDefault("frames", string.Empty).Split(',');

                AnimationFrame[] animationFrames = new AnimationFrame[frames.Length];

                for(int i = 0; i < frames.Length; i++)
                {
                    Sprite sprite = assets.GetAsset<Sprite>(frames[i].Trim());
                    animationFrames[i] = new AnimationFrame(sprite);
                }

                Animation animation = new Animation(frameDuration, animationFrames);
                assets.AddAsset(name, animation);
            }
        }
    }
}
