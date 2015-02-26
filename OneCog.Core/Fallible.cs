using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Core
{
    public static class Fallible
    {
        public static IFallible<T> FromValue<T>(T value)
        {
            return new Fallible<T>(value);
        }

        public static IFallible<T> FromError<T>(Exception error)
        {
            return new Fallible<T>(error);
        }

        public static IFallible<T> From<T>(Func<T> func)
        {
            try
            {
                return FromValue(func());
            }
            catch (Exception error)
            {
                return FromError<T>(error);
            }
        }

        public static async Task<IFallible<T>> FromAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return FromValue(await func());
            }
            catch (Exception error)
            {
                return FromError<T>(error);
            }
        }
    }

    public interface IFallible<T>
    {
        Exception Error { get; }
        T Value { get; }
        bool IsValue { get; }
        bool IsError { get; }
    }

    public class Fallible<T> : IFallible<T>
    {
        public Fallible(T value)
        {
            Value = value;
            IsValue = true;
        }

        public Fallible(Exception error)
        {
            Error = error;
            IsError = true;
        }

        public Exception Error { get; set; }
        public T Value { get; set; }
        public bool IsValue { get; set; }
        public bool IsError { get; set; }
    }
}
