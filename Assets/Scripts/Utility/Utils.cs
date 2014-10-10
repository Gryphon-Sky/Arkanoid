using UnityEngine;
using System;
using System.Diagnostics;

public static class Utils
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region publiс fields
    
    public static Settings Settings;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region Helpers
    
    public static void Log(string message)
    {
#if UNITY_EDITOR
        if(Settings.DebugMode)
        {
            UnityEngine.Debug.Log(message);
        }
#endif
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
    
    public static bool IsEqual0(float a)
    {
        return (Math.Abs(a) < EPSILON);
    }

    public static Vector2 DegreesToVector2(float degrees)
    {
        float radians = Mathf.Deg2Rad * degrees;
        return new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region private
    
    private const float EPSILON = 0.0001f;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
