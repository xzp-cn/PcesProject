using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三阶段--分辨图片
/// </summary>
public class DistinguishPictureModel : SingleTemplate<DistinguishPictureModel>
{

    public DistinguishPictureModel()
    {

    }

    /// <summary>
    /// 随机一个强化物
    /// </summary>
    /// <returns></returns>
    public GameObject GetRndReinforcement()
    {
        int rnd = Random.Range(0,6);
        return ObjectsManager.instanse.propList[rnd].gameObject;
    }

    /// <summary>
    /// 随机一个中性刺激物
    /// </summary>
    /// <returns></returns>
    public GameObject GetRndNeutralStimulator()
    {
        int rnd = Random.Range(6, 9);
        return ObjectsManager.instanse.propList[rnd].gameObject;
    }

    /// <summary>
    /// 随机一个负强化物
    /// </summary>
    /// <returns></returns>
    public GameObject GetRndNegReinforcement()
    {
        int rnd = Random.Range(9, 12);
        return ObjectsManager.instanse.propList[rnd].gameObject;
    }

    /// <summary>
    /// 获取对应的图卡
    /// </summary>
    /// <param name="tukaName"></param>
    /// <returns></returns>
    public GameObject GetTuKa(string tukaName)
    {
        int index = (int)System.Enum.Parse(typeof(PropsTag), tukaName, true);
        return ObjectsManager.instanse.propList[index].gameObject;
    }
}
