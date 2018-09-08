using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CQRSE
{
    public class Invoker
    {
        private static readonly MethodInfo InternalPreserveStackTraceMethod =
            typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
        
        public static void InvokeEvent<T>(T target, object evt)
        {
            Invoke(target, evt, InvocationHandlerType.Event);
        }

        public static void InvokeCommand(Type target, object cmd)
        {
            Invoke(target, cmd, InvocationHandlerType.Command);
        }

        private static void Invoke<T>(T target, object arg, InvocationHandlerType handlerType)
        {
            var type = arg.GetType();
            if (!GetHandlers(target, handlerType).TryGetValue(type, out var method))
                return;

            try
            {
                method.Invoke(target, new[] { arg });
            }
            catch (TargetInvocationException ex)
            {
                if (InternalPreserveStackTraceMethod != null)
                    InternalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);
            }
        }

        public static IDictionary<Type, MethodInfo> GetHandlers<T>(T target, InvocationHandlerType type)
        {
            string typestr;
            switch (type)
            {
                case InvocationHandlerType.Event: typestr = "ApplyEvent"; break;
                case InvocationHandlerType.Command: typestr = "ExecuteCommand"; break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return target.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.Name == typestr)
                .Where(m => m.GetParameters().Length == 1)
                .ToDictionary(m => m.GetParameters().First().ParameterType, m => m);
        }


        public enum InvocationHandlerType
        {
            Event,
            Command
        }
    }
}