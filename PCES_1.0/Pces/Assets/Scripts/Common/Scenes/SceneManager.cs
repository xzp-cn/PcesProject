using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneManager : SingleTon<SceneManager>
{
    /// <summary>
    /// 每个场景是单独的预制体
    /// </summary>
    public static Dictionary<string, GameObject> sceneDic;
    public GameObject ChangeScene(string scene)
    {
        if (!sceneDic.ContainsKey(scene))
        {
            GameObject go = Instantiate(ResManager.GetPrefab("Scenes/" + scene));
            sceneDic.Add(scene, go);
        }
        return sceneDic[scene];
    }
    public void ChangeScene(SceneEnum scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)scene);
    }
    public enum SceneEnum
    {
        /// <summary>
        /// 首页
        /// </summary>
        Preload = 0,
        /// <summary>
        /// 教室
        /// </summary>
        ClassRoom = 1,
        /// <summary>
        /// 超市
        /// </summary>
        Supermarket = 2,
        /// <summary>
        /// 公园
        /// </summary>
        Park = 3,
    }
}
