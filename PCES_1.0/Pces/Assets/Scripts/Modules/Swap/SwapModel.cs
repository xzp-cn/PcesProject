using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 以物换物  数据
/// </summary>
public class SwapModel : SingleTemplate<SwapModel>
{
    int idLevel = 1;

    public GameObject GetTuKa(string tukaName)
    {
        int index = (int)System.Enum.Parse(typeof(PropsTag), tukaName, true);
        return ObjectsManager.instanse.propList[index].gameObject;
    }
    public GameObject GetObj(int selectObj)
    {
        return ObjectsManager.instanse.GetProps(selectObj).gameObject;
    }
    /// <summary>
    /// 当前选择的强化物
    /// </summary>
    Reinforcement rf;
    public Reinforcement CurReinforcement
    {
        get
        {
            return rf;
        }
        set
        {
            rf = value;
        }
    }
}
/// <summary>
/// 强化物实体记录
/// </summary>
public class Reinforcement
{
    public string name;
    public Reinforcement(string _name)
    {
        name = _name;
    }
}
