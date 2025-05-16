using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                //throw new System.Exception(AspectMessages.WrongValidationType);
                throw new System.Exception("");
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            IValidator<object>? validator = (IValidator<object>)Activator.CreateInstance(_validatorType);
            Type entityType = _validatorType.BaseType.GetGenericArguments()[0];
            IEnumerable<object> entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            foreach (object? entity in entities)
            {
                ValidationTool.Validate<object>(validator, entity);
            }
        }
    }
}
