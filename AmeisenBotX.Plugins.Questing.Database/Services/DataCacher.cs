using AmeisenBotX.Plugins.Questing.Database.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Services
{
    internal class DataCacher
    {
        private static readonly object cacheLock = new object();
        private static readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions() { });

        public DataCacher(LocalContext localContext, RemoteContext remoteContext)
        {
            LocalContext = localContext;
            RemoteContext = remoteContext;
        }

        public LocalContext LocalContext { get; }
        public RemoteContext RemoteContext { get; }

        public async Task<T?> GetEntityByIdAsync<T>(int id) where T : class
        {
            string cacheKey = $"{typeof(T).FullName}_{id}";
            T? entity;
            lock (cacheLock)
            {
                if (cache.TryGetValue(cacheKey, out entity))
                {
                    return entity;
                }
            }

            try
            {
                T? entityToCache = null;

                // Try to retrieve the entity from the first DbContext asynchronously
                entityToCache = await LocalContext.Set<T>().FindAsync(id);

                if (entityToCache == null && RemoteContext != null)
                {
                    // If not found in the first DbContext and the second DbContext is available, try it asynchronously
                    entityToCache = await RemoteContext.Set<T>().FindAsync(id);
                }

                if (entityToCache != null)
                {
                    lock (cacheLock)
                    {
                        if (!cache.TryGetValue(cacheKey, out entity))
                        {
                            entity = entityToCache;
                            cache.Set(cacheKey, entity, new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) // Adjust cache duration as needed
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return entity;
        }

        public async Task AddEntityByIdAsync<T>(int id, T entity) where T : class
        {
            string cacheKey = $"{typeof(T).FullName}_{id}";
            try
            {
                LocalContext.Set<T>().Add(entity);
                await LocalContext.SaveChangesAsync();

                // Clear the cache for this entity type after adding a new entity
                cache.Remove(cacheKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task RemoteEntity<T>(int id, T entity) where T : class
        {
            string cacheKey = $"{typeof(T).FullName}_{id}";
            try
            {
                LocalContext.Set<T>().Remove(entity);
                await LocalContext.SaveChangesAsync();
                // Clear the cache for this entity type after removing an entity
                cache.Remove(cacheKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task UpdateEntity<T>(int id, T entity) where T: class
        {
            string cacheKey = $"{typeof(T).FullName}_{id}";
            try
            {
                LocalContext.Set<T>().Update(entity);
                await LocalContext.SaveChangesAsync();
                cache.Remove(cacheKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
