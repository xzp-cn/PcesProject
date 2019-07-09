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
        int rnd = Random.Range(0,4);
        return ObjectsManager.instanse.propList[rnd].gameObject;
    }

    /// <summary>
    /// 随机一个中性刺激物
    /// </summary>
    /// <returns></returns>
    public GameObject GetRndNeutralStimulator()
    {
        int rnd = Random.Range(7, 11);
        return ObjectsManager.instanse.propList[rnd].gameObject;
    }

    /// <summary>
    /// 随机一个负强化物
    /// </summary>
    /// <returns></returns>
    public GameObject GetRndNegReinforcement()
    {
        int rnd = Random.Range(4, 7);
        return ObjectsManager.instanse.propList[rnd].gameObject;
    }

    /// <summary>
    /// 获取对应的图卡
    /// </summary>
    /// <param name="tukaName"></param>
    /// <returns></returns>
    public GameObject GetTuKa(string tukaName)
    {
        return ObjectsManager.instanse.propList.Find(tuka => { return tuka.name == tukaName; }).gameObject;
    }
}
