using AspectCore.DynamicProxy;
using Byte.Core.EntityFramework.Extensions;
using Byte.Core.EntityFramework.Models;
using System.Reflection;

namespace Byte.Core.EntityFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class FromDbContextFactoryAttribute : Attribute
    {
        public string DbContextTagName { get; set; }

        public FromDbContextFactoryAttribute(string tagName)
        {
            DbContextTagName = tagName;
        }
    }


    public class FromDbContextFactoryInterceptor : AbstractInterceptorAttribute
    {
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var impType = context.Implementation.GetType();
            var properties = impType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.IsDefined(typeof(FromDbContextFactoryAttribute))).ToList();
            if (properties.Any())
            {
                foreach (var property in properties)
                {
                    var attribute = property.GetCustomAttribute<FromDbContextFactoryAttribute>();
                    var dbContext = context.ServiceProvider.GetDbContext((DatabaseType)Enum.Parse(typeof(DatabaseType), attribute.DbContextTagName));
                    property.SetValue(context.Implementation, dbContext);
                }
            }
            return context.Invoke(next);
        }
    }
}
