using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace TopDownShooterSpike.Managers
{
    public static class InputAggregatorExtensions
    {
				/// <summary>
				/// Starts the input key press chaining DSL
				/// </summary>
				/// <param name="aggregator"></param>
				/// <returns></returns>
        public static Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> Combine(this IPlayerInputAggregator aggregator)
        {
            return Tuple.Create(aggregator, Enumerable.Empty<KeyPressType>());
        }

				/// <summary>
				/// Combines a key press that is active on the 'up' state
				/// </summary>
				/// <param name="keyCombinator"></param>
				/// <param name="key"></param>
				/// <returns></returns>
        public static Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> Up(this Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> keyCombinator, Keys key)
        {
            return keyCombinator.With(KeyPressType.Up(key));
        }

				/// <summary>
				/// Combines a key press that is active on the 'down' state
				/// </summary>
				/// <param name="keyCombinator"></param>
				/// <param name="key"></param>
				/// <returns></returns>
        public static Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> Down(this Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> keyCombinator, Keys key)
        {
            return keyCombinator.With(KeyPressType.Down(key));
        }

				/// <summary>
				/// Combines a key press that is active when it has cycled through both 'down' and 'up' states
				/// </summary>
				/// <param name="keyCombinator"></param>
				/// <param name="key"></param>
				/// <returns></returns>
        public static Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> Pressed(this Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> keyCombinator, Keys key)
        {
            return keyCombinator.With(KeyPressType.Pressed(key));
        }

				/// <summary>
				/// Allows you to define a key state binding explicitly in the chaining DSL
				/// </summary>
				/// <param name="keyCombinator"></param>
				/// <param name="key"></param>
				/// <returns></returns>
        public static Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> With(this Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> keyCombinator, KeyPressType key)
        {
            return Tuple.Create(keyCombinator.Item1, keyCombinator.Item2.Push(key));
        }

				/// <summary>
				/// Applies the key press chain to the <see cref="IPlayerInputAggregator"/>
				/// </summary>
				/// <param name="tuple"></param>
				/// <param name="callback"></param>
        public static void Apply(this Tuple<IPlayerInputAggregator, IEnumerable<KeyPressType>> tuple, Action callback)
        {
            if(callback == null)
                throw new ArgumentNullException("callback");

            var keyBindings = tuple.Item2;
            var service = tuple.Item1;

            service.OnCombination(callback, keyBindings.ToArray());
        }
    }
}