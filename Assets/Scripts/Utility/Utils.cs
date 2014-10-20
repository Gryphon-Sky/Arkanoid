using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public static class Utils
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region Helpers
    
    [Conditional("UNITY_EDITOR")]
    public static void Log(string message)
    {
        UnityEngine.Debug.Log(message);
    }

    public static void InvokeAction(Action action)
    {
        if(action != null)
        {
            action();
        }
    }

    public static void InvokeAction<T>(Action<T> action, T param)
    {
        if(action != null)
        {
            action(param);
        }
    }
    
    public static void InvokeActionWithDelay(MonoBehaviour starter, float delay, Action action)
    {
        if(action != null)
        {
            starter.StartCoroutine(DelayedActionCoroutine(delay, action));
        }
    }
    
    public static Vector2 DegreesToVector2(float degrees)
    {
        float radians = Mathf.Deg2Rad * degrees;
        return new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region internal

    private static IEnumerator DelayedActionCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        InvokeAction(action);
    }
    
    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
