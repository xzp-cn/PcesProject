using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensClass
{
    public static AnimationOper GetAnimatorOper(this GameObject go)
    {
        AnimationOper oper = go.GetComponent<AnimationOper>();
        if (oper == null)
        {
            oper = go.AddComponent<AnimationOper>();
        }
        return oper;
    }

    /// <summary>
    /// 获取UI高光
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static UIFlah GetUIFlash(this GameObject go)
    {
        UIFlah oper = go.GetComponent<UIFlah>();
        if (oper == null)
        {
            oper = go.AddComponent<UIFlah>();
        }
        return oper;
    }
    /// <summary>
    /// 获取碰撞体
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static BoxCollider GetBoxCollider(this GameObject go)
    {
        BoxCollider oper = go.GetComponent<BoxCollider>();
        if (oper == null)
        {
            oper = go.AddComponent<BoxCollider>();
        }
        return oper;
    }
    public static AnimationKA GetAnimationKa(this GameObject go)
    {
        AnimationKA ak = go.GetComponent<AnimationKA>();
        if (ak == null)
        {
            ak = go.AddComponent<AnimationKA>();
        }
        return ak;
    }
}
