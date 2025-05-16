using Castle.DynamicProxy;

namespace Core.Utilities.Interceptors
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        // Öncelik sırasını belirlemek için kullanılabilir
        public int Priority { get; set; } = 1;

        // Metodun öncesinde yapılacak işlemler
        public virtual void OnBefore(IInvocation invocation) { }

        // Metodun sonrasında yapılacak işlemler
        public virtual void OnAfter(IInvocation invocation) { }

        // Metodun hata alması durumunda yapılacak işlemler
        public virtual void OnException(IInvocation invocation, Exception exception) { }

        // Metod başarıyla çalıştıktan sonra yapılacak işlemler
        public virtual void OnSuccess(IInvocation invocation) { }

        // Metod çalışırken yapılacak işlemler
        public virtual void Intercept(IInvocation invocation) { }
    }
}