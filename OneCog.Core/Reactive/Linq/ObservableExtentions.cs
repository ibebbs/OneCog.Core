using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace OneCog.Core.Reactive.Linq
{
    public static class ObservableExtentions
    {
        /// <summary>
        /// Emits values when the value of the predicate is false
        /// </summary>
        /// <remarks>
        /// This function is the opposite of the Where operator and provided for simplicity and readability.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source observable</param>
        /// <param name="predicate">A function that must return true to exclude an item from the observable sequence</param>
        /// <returns></returns>
        public static IObservable<T> Except<T>(this IObservable<T> source, Func<T,bool> predicate)
        {
            return source.Where(value => !predicate(value));
        }

        /// <summary>
        /// Emits an error to the observer when the specified predicate returns true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source observable</param>
        /// <param name="predicate">A function which, when it returns true, causes an exception to be emitted to the observer</param>
        /// <param name="projection">A function which takes the item which caused an exception to be emitted and returns a strongly types exception</param>
        /// <returns></returns>
        public static IObservable<T> ThrowWhen<T>(this IObservable<T> source, Func<T,bool> predicate, Func<T,Exception> projection)
        {
            return Observable.Create<T>(
                observer =>
                {
                    Action<T> handler = 
                        value =>
                        {
                            if (predicate(value))
                            {
                                observer.OnError(projection(value));
                            }
                            else
                            {
                                observer.OnNext(value);
                            }
                        };

                    return source.Subscribe(handler, observer.OnError, observer.OnCompleted);
                }
            );
        }
    }
}
