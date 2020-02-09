using System;
using System.Collections.Generic;

namespace Toolbox.Assets
{
    public class AssetStore<TKey> where TKey : IEquatable<TKey>
    {
        private readonly Dictionary<Type, Dictionary<TKey, object>> _caches = new Dictionary<Type, Dictionary<TKey, object>>();

        public int Count<TAsset>() => GetCache<TAsset>().Count;
        public bool IsEmpty<TAsset>() => GetCache<TAsset>().Count == 0;
        public bool Has<TAsset>(TKey key) => GetCache<TAsset>().ContainsKey(key);
        public void Clear<TAsset>() => GetCache<TAsset>().Clear();

        public void AddAsset<TAsset>(TKey key, TAsset asset)
        {
            if (!TryGetCache<TAsset>(out Dictionary<TKey, object> cache))
            {
                cache = AddCache<TAsset>();
            }

            if (cache.ContainsKey(key))
            {
                throw new ArgumentException($"Key '{key}' already exists in {typeof(TAsset).Name} cache.");
            }

            cache.Add(key, asset);
        }

        public TAsset GetAsset<TAsset>(TKey key)
        {
            if (!Has<TAsset>(key))
            {
                throw new KeyNotFoundException($"Unknown key '{key.ToString()}' in {typeof(TAsset).Name} cache.");
            }

            return (TAsset)GetCache<TAsset>()[key];
        }

        public Dictionary<TKey, object>.ValueCollection GetAssetsOfType<TAsset>()
        {
            Dictionary<TKey, object> cache = GetCache<TAsset>();
            return cache.Values;
        }

        public bool RemoveAsset<TAsset>(TKey key)
        {
            return GetCache<TAsset>().Remove(key);
        }

        public void ClearAll()
        {
            foreach (Dictionary<TKey, object> cache in _caches.Values)
            {
                cache.Clear();
            }
        }

        private Dictionary<TKey, object> AddCache<TAsset>()
        {
            var cache = new Dictionary<TKey, object>();
            _caches.Add(typeof(TAsset), cache);
            return cache;
        }

        private Dictionary<TKey, object> GetCache<TAsset>()
        {
            if (!TryGetCache<TAsset>(out Dictionary<TKey, object> cache))
            {
                throw new StoreNotFoundException($"Asset cache for type '{typeof(TAsset).Name}' not found.");
            }

            return cache;
        }

        private bool TryGetCache<TAsset>(out Dictionary<TKey, object> cache)
        {
            return _caches.TryGetValue(typeof(TAsset), out cache);
        }
    }
}
