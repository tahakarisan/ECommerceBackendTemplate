using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override async void Intercept(IInvocation invocation)
        {
            string methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            List<object> arguments = invocation.Arguments.ToList();
            string key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";

            if (await _cacheManager.IsAddAsync(key))
            {
                object returnValue = await _cacheManager.GetAsync(key);
                invocation.ReturnValue = returnValue;
                return;
            }
            // Metodu çağır
            invocation.Proceed();

            // Eğer metot asenkron ise ReturnValue bir Task olacaktır
            if (invocation.Method.ReturnType.IsGenericType && invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                // Task<T> için Task'ı çöz ve sonucu cache'e ekle
                await (Task)invocation.ReturnValue;
            }
            // Asenkron olmayan durumlar için
            await _cacheManager.AddAsync(key, invocation.ReturnValue, _duration);
        }
    }
}
