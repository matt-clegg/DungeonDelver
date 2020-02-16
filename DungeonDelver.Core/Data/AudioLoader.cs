using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Toolbox.Assets;

namespace DungeonDelver.Core.Data
{
    public static class AudioLoader
    {
        public static void Load(AssetStore<string> assets, ContentManager content)
        {
            LoadSoundEffect("rain_looped", content, assets);
            LoadSoundEffect("thunder_01", content, assets);
        }

        private static void LoadSoundEffect(string name, ContentManager content, AssetStore<string> assets)
        {
            SoundEffect soundEffect = content.Load<SoundEffect>("Audio/" + name);
            assets.AddAsset(name, soundEffect);
        }
    }
}
