using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Unity.VisualScripting
{
    public sealed class InstanceFunctionInvoker<TTarget, TParam0, TParam1, TParam2, TResult> : InstanceFunctionInvokerBase<TTarget, TResult>
    {
        public InstanceFunctionInvoker(MethodInfo methodInfo) : base(methodInfo) { }

        private Func<TTarget, TParam0, TParam1, TParam2, TResult> invoke;

        public override object Invoke(object target, params object[] args)
        {
            if (args.Length != 3)
            {
                throw new TargetParameterCountException();
            }

            return Invoke(target, args[0], args[1], args[2]);
        }

        public override object Invoke(object target, object arg0, object arg1, object arg2)
        {
            if (OptimizedReflection.safeMode)
            {
                VerifyTarget(target);
                VerifyArgument<TParam0>(methodInfo, 0, arg0);
                VerifyArgument<TParam1>(methodInfo, 1, arg1);
                VerifyArgument<TParam2>(methodInfo, 2, arg2);

                try
                {
                    return InvokeUnsafe(target, arg0, arg1, arg2);
                }
                catch (TargetInvocationException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new TargetInvocationException(ex);
                }
            }
            else
            {
                return InvokeUnsafe(target, arg0, arg1, arg2);
            }
        }

        public object InvokeUnsafe(object target, object arg0, object arg1, object arg2)
        {
            return invoke.Invoke((TTarget)target, (TParam0)arg0, (TParam1)arg1, (TParam2)arg2);
        }

        protected override Type[] GetParameterTypes()
        {
            return new[] { typeof(TParam0), typeof(TParam1), typeof(TParam2) };
        }

        protected override void CompileExpression(MethodCallExpression callExpression, ParameterExpression[] parameterExpressions)
        {
            invoke = Expression.Lambda<Func<TTarget, TParam0, TParam1, TParam2, TResult>>(callExpression, parameterExpressions).Compile();
        }

        protected override void CreateDelegate()
        {
            invoke = (Func<TTarget, TParam0, TParam1, TParam2, TResult>)methodInfo.CreateDelegate(typeof(Func<TTarget, TParam0, TParam1, TParam2, TResult>));
        }
    }
}
