using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePageModel : SingleTemplate<HomePageModel>
{
    public HomePageModel()
    {

    }
    public Dictionary<string, GameObject> hDic = new Dictionary<string, GameObject>();
    public void PushHomePageObject(GameObject go)
    {
        if (!hDic.ContainsKey(go.name))
        {
            hDic.Add(go.name, go);
        }
    }
    public void ClearAllGo()
    {
        foreach (var item in hDic)
        {
            UnityEngine.Object.Destroy(item.Value);
        }
    }
    public void Call()
    {
        //GlobalEntity.GetInstance().Dispatch("事件枚举", 当前模块枚举);
    }
}
