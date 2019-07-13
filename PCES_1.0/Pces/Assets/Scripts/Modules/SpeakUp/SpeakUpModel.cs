using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第四阶段--句型表达
/// </summary>
public class SpeakUpModel : SingleTemplate<SpeakUpModel>
{

    /// <summary>
    /// 随机一个强化物
    /// </summary>
    /// <returns></returns>
    public GameObject GetRndReinforcement()
    {
        int rnd = Random.Range(0, 4);
        return ObjectsManager.instanse.propList[rnd].gameObject;
    }
}
