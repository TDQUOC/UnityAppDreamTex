using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Alta.Plugin
{
    public static class DelayAction
    {
        /// <summary>
        /// run "action" after number of "time" second
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerator wait(this Action action, float time=0)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        /// <summary>
        /// run "action" after done "waitFunc"
        /// </summary>
        /// <param name="action"></param>
        /// <param name="waitFunc"></param>
        /// <returns></returns>
        public static IEnumerator wait(this Action action, Func<YieldInstruction> waitFunc)
        {
            yield return waitFunc();
            action();
        }
        public static IEnumerator wait(this Func<bool> function, Func<YieldInstruction> waitFunc)
        {
            yield return waitFunc();
            function();
        }
        public static IEnumerator wait<T>(this Func<T,bool> func,T param, float time = 0)
        {
            yield return new WaitForSeconds(time);
            func(param);
        }
        public static IEnumerator wait<T>(this Func<bool> func, float time = 0)
        {
            yield return new WaitForSeconds(time);
            func();
        }
    }
}
