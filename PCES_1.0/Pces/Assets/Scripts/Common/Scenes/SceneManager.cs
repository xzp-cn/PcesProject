using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneManager : SingleTon<SceneManager>
{
    /// <summary>
    /// 每个场景是单独的预制体
    /// </summary>
    public static Dictionary<string, GameObject> sceneDic;
    public void ChangeScene(string curScene, string nextScene)
    {
        if (sceneDic.ContainsKey(curScene))
        {
            //ToDo:0.切换方式，1.每个场景处理切换的处理
        }
        else
        {
            //ResManager.GetPrefab();
        }
    }

}
