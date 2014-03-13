using System;
using System.Collections.Generic;
using System.Linq;

namespace TopDownShooterSpike
{
    /// <summary>
    /// Linq extensions
    /// </summary>
    public static class LinqExtensions
    {
        public static void For<T>(this IEnumerable<T> collection, Action<T> loopBody)
        {
            if(loopBody == null)
                throw new ArgumentNullException();

            foreach (var item in collection)
            {
                loopBody(item);
            }
        }
        /// <summary>
				/// Pushes a new value onto an <see cref="IEnumerable{TInput}"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="source2"></param>
        /// <typeparam name="TInput"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TInput> Push<TInput>(this IEnumerable<TInput> source, TInput source2)
        {
            foreach (TInput current in source)
                yield return current;

                yield return source2;
        }
        /// <summary>
        /// Appends two <see cref="IEnumerable{T}" />s 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="source2"></param>
        /// <typeparam name="TInput"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TInput> Append<TInput>(this IEnumerable<TInput> source, IEnumerable<TInput> source2)
        {
            foreach (TInput current in source)
                yield return current;

            foreach (TInput current2 in source2)
                yield return current2;
        }

        /// <summary>
        /// Returns items from an <see cref="T:System.Collections.Generic.IEnumerable`1" /> until a condition is met
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="source"></param>
        /// <param name="evaluatorFunc"></param>
        /// <returns></returns>
        public static IEnumerable<TInput> Until<TInput>(this IEnumerable<TInput> source, Func<TInput, bool> evaluatorFunc)
        {
            return source.TakeWhile(current => !evaluatorFunc.Execute(current));
        }

        /// <summary>
        /// Runs an action on an <see cref="IEnumerable{T}" /> if the input is neither null or empty
        /// </summary>
        /// <param name="input"></param>
        /// <param name="callback"></param>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TOutput> IfNotNullOrEmpty<TInput, TOutput>(this IEnumerable<TInput> input, Func<TInput, TOutput> callback)
        {
            if (input.Any())
                foreach (var tOutput in input.Select(callback.Execute))
                    yield return tOutput;
        }

        /// <summary>
        /// runs an action on each item in the enumeration
        /// </summary>
        /// <param name="inputCollection"></param>
        /// <param name="action"></param>
        /// <typeparam name="TInput"></typeparam>
        public static void ForEach<TInput>(this IEnumerable<TInput> inputCollection, Action<TInput> action)
        {
            foreach (TInput input in inputCollection)
                action.Execute(input);
        }

        /// <summary>
        /// Gets the head of the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue Head<TValue>(this IEnumerable<TValue> source)
        {
            return source.FirstOrDefault();
        }

        /// <summary>
        /// Gets the tail of the <see cref="IEnumerable{T}" />
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TValue> Tail<TValue>(this IEnumerable<TValue> source)
        {
            return source.Any() ? source.Skip(1) : Enumerable.Empty<TValue>();
        }

        /// <summary>
        ///  does an operation on an item when the evaluator returns true
        /// </summary>
        /// <param name="input"></param>
        /// <param name="evaluator"></param>
        /// <param name="action"></param>
        /// <typeparam name="TInput"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TInput> When<TInput>(this IEnumerable<TInput> input, Func<TInput, bool> evaluator, Action<TInput> action)
        {
            foreach (var current in input)
            {
                if (evaluator.Execute(current))
                    action.Execute(current);

                yield return current;
            }
        }

        /// <summary>
        /// Enumerates a collection and returns it
        /// </summary>
        /// <param name="inputCollection"></param>
        /// <typeparam name="TInput"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TInput> Enumerate<TInput>(this IEnumerable<TInput> inputCollection)
        {
            return inputCollection.ToArray();
        }
    }
}
