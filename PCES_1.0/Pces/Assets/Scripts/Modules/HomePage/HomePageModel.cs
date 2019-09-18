using System;
using System.Collections.Generic;
using UnityEngine;

public class HomePageModel : SingleTemplate<HomePageModel>
{
    public string timeStamp_start = string.Empty;//开始事件戳    
    public HomePageModel()
    {
        timeStamp_start = GetTimeStamp();
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
    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    public string GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        string str = Convert.ToInt64(ts.TotalMilliseconds).ToString();
        return str;
    }
    /// <summary>
    /// 起始时间戳_终止时间戳 游戏结束界面回调
    /// </summary>
    public string SendTimeStampStr()
    {
        string stampStr = "startdate=" + timeStamp_start + "&enddate=" + GetTimeStamp();
        return stampStr;
    }
}
