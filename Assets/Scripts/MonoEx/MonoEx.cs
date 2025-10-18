using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MonoEx
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : MonoBehaviour
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            return go.AddComponent<T>();
        }
        
        return component;
    }
    
    public static void SetActive<T>(this T component, bool state) where T : MonoBehaviour
    {
        component.gameObject.SetActive(state);
    }
}