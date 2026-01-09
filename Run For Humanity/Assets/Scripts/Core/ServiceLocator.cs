using System;
using System.Collections.Generic;

namespace RunForHumanity.Core
{
    /// <summary>
    /// Service Locator pattern for dependency injection
    /// SOLID: Dependency Inversion Principle
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        
        public static void RegisterService<T>(T service) where T : class
        {
            Type type = typeof(T);
            
            if (_services.ContainsKey(type))
            {
                UnityEngine.Debug.LogWarning($"[ServiceLocator] Service {type.Name} already registered. Overwriting.");
                _services[type] = service;
            }
            else
            {
                _services.Add(type, service);
                UnityEngine.Debug.Log($"[ServiceLocator] Registered service: {type.Name}");
            }
        }
        
        public static T GetService<T>() where T : class
        {
            Type type = typeof(T);
            
            if (_services.TryGetValue(type, out object service))
            {
                return service as T;
            }
            
            UnityEngine.Debug.LogError($"[ServiceLocator] Service {type.Name} not found!");
            return null;
        }
        
        public static bool TryGetService<T>(out T service) where T : class
        {
            Type type = typeof(T);
            
            if (_services.TryGetValue(type, out object foundService))
            {
                service = foundService as T;
                return service != null;
            }
            
            service = null;
            return false;
        }
        
        public static void UnregisterService<T>() where T : class
        {
            Type type = typeof(T);
            
            if (_services.Remove(type))
            {
                UnityEngine.Debug.Log($"[ServiceLocator] Unregistered service: {type.Name}");
            }
        }
        
        public static void Clear()
        {
            _services.Clear();
            UnityEngine.Debug.Log("[ServiceLocator] All services cleared");
        }
    }
}
