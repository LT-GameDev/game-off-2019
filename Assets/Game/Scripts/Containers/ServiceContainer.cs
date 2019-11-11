using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Containers
{
    public interface IServiceContainer
    {
        TService GetService<TService>();
    }
    
    public class ServiceContainer : IServiceContainer
    {
        private Dictionary<Type, object> services;

        public ServiceContainer()
        {
            services = new Dictionary<Type, object>();
        }

        public void AddService<TService>(TService service)
        {
            services.Add(typeof(TService), service);
        }
        
        public TService GetService<TService>()
        {
            if (services.TryGetValue(typeof(TService), out var service))
                return (TService) service;

            return default;
        }
    }
}