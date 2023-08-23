using FluentCloudMusic.DataModels.JSONModels;
using System;
using System.Collections.Generic;

namespace FluentCloudMusic.Services
{
    public static class CacheService
    {
        private static readonly Dictionary<string, CachedResource> CachedResources = new Dictionary<string, CachedResource>();

        static CacheService()
        {
            AccountService.Login += OnLogin;
            AccountService.Logout += OnLogout;
        }

        public static void RegisterCachedResource(string key, TimeSpan updateInterval)
        {
            CachedResources.Add(key, new CachedResource(updateInterval));
        }

        public static void UnregisterCachedResource(string key)
        {
            CachedResources.Remove(key);
        }

        public static CachedResource GetCachedResource(string key)
        {
            return CachedResources[key];
        }

        private static void ResetAll()
        {
            foreach (var resource in CachedResources.Values) resource.Reset();
        }

        private static void OnLogin(Profile profile) => ResetAll();

        private static void OnLogout() => ResetAll();
    }

    public class CachedResource
    {
        private object _Value;

        private DateTime UpdateTime;

        private TimeSpan UpdateInterval;

        public bool IsValid
        {
            get => DateTime.Now - UpdateTime <= UpdateInterval;
        }

        public object Value
        {
            get => _Value;
            set
            {
                UpdateTime = DateTime.Now;
                _Value = value;
            }
        }

        public CachedResource(TimeSpan updateInterval)
        {
            _Value = new object();
            UpdateTime = DateTime.MinValue;
            UpdateInterval = updateInterval;
        }

        public void Reset()
        {
            _Value = new object();
            UpdateTime = DateTime.MinValue;
        }
    }
}
