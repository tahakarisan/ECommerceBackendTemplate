using Castle.DynamicProxy;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        // Method çağrılmadan önce yapılacak işlemler
        protected virtual void OnBefore(IInvocation invocation) { }

        // Method çağrıldıktan sonra yapılacak işlemler
        protected virtual void OnAfter(IInvocation invocation) { }

        // Hata durumu ile karşılaşıldığında yapılacak işlemler
        protected virtual void OnException(IInvocation invocation, Exception exception) { }

        // Method başarıyla tamamlandığında yapılacak işlemler
        protected virtual void OnSuccess(IInvocation invocation) { }

        // Metod çalışırken yapılacak işlemler
        public override void Intercept(IInvocation invocation)
        {
            bool isSuccess = true;
            try
            {
                OnBefore(invocation);// Method öncesi işlem
                invocation.Proceed(); // Methodu çalıştır
            }
            catch (Exception e)
            {
                isSuccess = false;
                OnException(invocation, e);// Hata durumunda işlem
                throw; // Hata dışarıya fırlatılır
            }
            finally
            {
                if (isSuccess)
                {
                    OnSuccess(invocation);// Başarıyla tamamlanmışsa işlem
                }
            }
            OnAfter(invocation);// Her durumda yapılacak işlem
        }
    }
}
