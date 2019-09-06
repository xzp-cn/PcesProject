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
        int rnd = Random.Range(0, 4);
        return ObjectsManager.instanse.propList[rnd].gameObject;
    }

    /// <summary>
    /// 随机一批不重复的强化物
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public void GetRndReinforcements(int n, List<PropsObject> results)
    {
        List<int> tmps = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            tmps.Add(i);
        }

        List<int> rets = new List<int>();
        for (int j = 0; j < n; j++)
        {
            int rnd = Random.Range(0, tmps.Count);
            rets.Add(tmps[rnd]);
            tmps.RemoveAt(rnd);
        }


        rets.ForEach((index) =>
        {
            Debug.Log("强化物索引:" + index);
            results.Add(ObjectsManager.instanse.propList[index]);
        });
        //Debug.Log("====================================================");
    }

    /// <summary>
    /// 随机一批不重复的负强化物
    /// </summary>
    /// <param name="n"></param>
    /// <param name="results"></param>
    public void GetRndNegReinforcements(int n, List<PropsObject> results)
    {
        List<int> tmps = new List<int>();
        for (int i = 4; i < 7; i++)
        {
            tmps.Add(i);
        }

        List<int> rets = new List<int>();
        for (int j = 0; j < n; j++)
        {
            int rnd = Random.Range(0, tmps.Count);
            rets.Add(tmps[rnd]);
            tmps.RemoveAt(rnd);
        }


        rets.ForEach((index) =>
        {
            Debug.Log("负强化物索引:" + index);
            results.Add(ObjectsManager.instanse.propList[index]);
        });
        Debug.Log("====================================================");
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

    public Vector3 GetVecPos(string name)
    {
        switch (name)
        {
            case "apple":
                return new Vector3(0.0113f, -0.0466f, 0.0374f);
        }
        return Vector3.zero;
    }
}
