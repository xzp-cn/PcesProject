using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptQuestionModel : SingleTemplate<AcceptQuestionModel>
{

    /// <summary>
    ///通过名字得到道具/涂卡
    /// </summary>
    /// <param name="tukaName"></param>
    /// <returns></returns>
    public GameObject GetTuKa(string tukaName)
    {
        int index = (int)System.Enum.Parse(typeof(PropsTag), tukaName, true);
        //Debug.Log(tukaName + " /  " + index);
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
    /// 随机得到某一类物品中的一个
    /// </summary>
    /// <returns></returns>
    public PropsObject GetObj(PropsType type)
    {
        //PropsObject pObj = null;
        int index = 0;
        switch (type)
        {
            case PropsType.reinforcement:
                index = Random.Range(101, 1001) % 4;
                break;
            case PropsType.negReinforcement:
                index = Random.Range(101, 1001) % 3 + 5;
                break;
            case PropsType.neutralStimulator:
                index = Random.Range(101, 1001) % 3 + 8;
                break;
            default:
                break;
        }
        return ObjectsManager.instanse.GetProps(index);
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
