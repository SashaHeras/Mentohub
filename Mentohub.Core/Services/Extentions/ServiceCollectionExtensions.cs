using Mentohub.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mentohub.Core.Services.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes();
            var interfaceTypes = types.Where(type => type.IsInterface && type != typeof(IService));

            foreach (var interfaceType in interfaceTypes)
            {
                var serviceType = assembly.GetTypes()
                    .FirstOrDefault(type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type));

                if (serviceType != null)
                {
                    services.AddScoped(interfaceType, serviceType);
                }
            }
        }
    }
}
