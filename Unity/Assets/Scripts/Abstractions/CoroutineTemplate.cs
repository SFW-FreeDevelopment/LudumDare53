using System;
using System.Collections;
using UnityEngine;

namespace LD53.Abstractions
{
    public static class CoroutineTemplate
    {
        /// <param name="delay">Delay in seconds</param>
        /// <param name="action">Action to invoke</param>
        public static IEnumerator DelayAndFireRoutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
        
        /// <param name="delay">Delay in seconds</param>
        /// <param name="action">Action to invoke</param>
        public static IEnumerator DelayAndFireLoopRoutine(float delay, Action action)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                action?.Invoke();
            }
        }
        
        /// <param name="delay">Delay in seconds</param>
        /// <param name="action">Action to invoke</param>
        public static IEnumerator FireAndDelayLoopRoutine(float delay, Action action)
        {
            while (true)
            {
                action?.Invoke();
                yield return new WaitForSeconds(delay);
            }
        }
    }
}