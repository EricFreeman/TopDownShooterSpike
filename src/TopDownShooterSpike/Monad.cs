using System;

namespace TopDownShooterSpike
{
    /// <summary>
    /// Monad operations
    /// </summary>
    public static class Monad
    {
        // maybe add a memoize function

        /// <summary>
        /// if the input is not null, run an evaluation function on it
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="input"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static void IfNotNull<TIn>(this TIn input, Action<TIn> evaluator)
            where TIn : class
        {
            if (input != null && evaluator != null)
                 evaluator(input);
        }

				
        /// <summary>
        /// if the input is not null, run an evaluation function on it
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="input"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TOut IfNotNull<TIn, TOut>(this TIn input, Func<TIn, TOut> evaluator)
            where TIn : class
            where TOut : class
        {
            TOut output = default(TOut);

            if (input != null && evaluator != null)
                output = evaluator(input);

            return output;
        }

        /// <summary>
        /// Executes an action if the input is not null
        /// </summary>
        /// <param name="input"></param>
        /// <param name="action"></param>
        /// <typeparam name="TIn"></typeparam>
        public static void IfNotNullDo<TIn>(this TIn input, Action<TIn> action) where TIn : class
        {
            if (input != null)
                action.Execute(input);
        }

        /// <summary>
        /// if the input is not null, run an evaluation function on it
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="input"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TIn IfNotNull<TIn>(this TIn input, Func<TIn> evaluator) where TIn : class
        {
            TIn output = input;

            if (input != null && evaluator != null)
                output = evaluator();

            return output;
        }
        /// <summary>
        /// Allows for the conversion between two types
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="input"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static TOut To<TIn, TOut>(this TIn input, Func<TIn, TOut> function)
            where TIn : class
            where TOut : class
        {
            TOut output = default(TOut);

            if (function != null && input != null)
                output = function(input);

            return output;
        }

        /// <summary>
        /// Executes an action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="input"></param>
        /// <typeparam name="TInput"></typeparam>
        public static void Execute<TInput>(this Action<TInput> action, TInput input)
        {
            if (action != null)
                action(input);
        }

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns></returns>
        public static TOutput Execute<TOutput>(this Func<TOutput> action)
        {
            TOutput output = default(TOutput);
            if (action != null)
                output = action();
            return output;
        }

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="action"></param>
        /// <param name="input"></param>
        /// <typeparam name="TOutput"></typeparam>
        /// <typeparam name="TInput"></typeparam>
        /// <returns></returns>
        public static TOutput Execute<TOutput, TInput>(this Func<TInput, TOutput> action, TInput input)
        {
            TOutput output = default(TOutput);

            if (action != null)
                output = action(input);

            return output;
        }

        /// <summary>
        /// Executes an action
        /// </summary>
        /// <param name="actionToExecute"></param>
        public static void Execute(this Action actionToExecute)
        {
            if (actionToExecute != null)
                actionToExecute();
        }

        /// <summary>
        /// Executes an action inside of a try-catch-finally
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputCallback"></param>
        /// <param name="onException"></param>
        /// <param name="onFinally"></param>
        /// <typeparam name="TException"></typeparam>
        /// <typeparam name="TInput"></typeparam>
        public static void Try<TInput, TException>(this TInput input, Action<TInput> inputCallback, Action<TException> onException = null, Action onFinally = null) where TException : Exception
        {
            try
            {
                inputCallback.Execute(input);
            }
            catch (TException e)
            {
                onException.Execute(e);
            }
            finally
            {
                onFinally.Execute();
            }
        }

        /// <summary>
        /// Executes an action inside of a try-catch-finally
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputCallback"></param>
        /// <param name="onException"></param>
        /// <param name="onFinally"></param>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        public static TOutput Try<TInput, TOutput, TException>(this TInput input, Func<TInput, TOutput> inputCallback, Func<TException, TOutput> onException = null, Func<TOutput> onFinally = null)
            where TInput : class
            where TOutput : class
            where TException : Exception
        {
            TOutput value = default(TOutput);
            try
            {
                value = inputCallback.Execute(input);
            }
            catch (TException e)
            {
                value = onException.Execute(e);
            }
            finally
            {
                value = onFinally.Execute() ?? value;
            }

            return value;
        }

        /// <summary>
        /// Formats a string with arguments
        /// </summary>
        /// <param name="formatString"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Format(this string formatString, params object[] args)
        {
            return string.Format(formatString, args);
        }
    }
}
