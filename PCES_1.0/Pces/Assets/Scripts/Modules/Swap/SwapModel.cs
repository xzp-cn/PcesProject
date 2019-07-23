using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 以物换物  数据
/// </summary>
public class SwapModel : SingleTemplate<SwapModel>
{
    public bool selectedA = false;
    public bool selectedB = false;
    public bool selectedC = false;
    /// <summary>
    ///通过名字得到道具/涂卡
    /// </summary>
    /// <param name="tukaName"></param>
    /// <returns></returns>
    public GameObject GetTuKa(string tukaName)
    {
        int index = (int)System.Enum.Parse(typeof(PropsTag), tukaName, true);
        return ObjectsManager.instanse.propList[index].gameObject;
    }
    /// <summary>
    /// 通过索引得到道具
    /// </summary>
    /// <param name="selectObj"></param>
    /// <returns></returns>
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
    public PropsData pData;
    public Reinforcement(PropsData _pdata)
    {
        pData = _pdata;
    }
}
