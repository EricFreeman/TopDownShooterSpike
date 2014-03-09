using System;
using System.Collections.Generic;

namespace TopDownShooterSpike
{
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
    }
}