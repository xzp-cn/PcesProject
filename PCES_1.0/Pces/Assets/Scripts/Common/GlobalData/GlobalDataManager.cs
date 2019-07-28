using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDataManager : SingleTemplate<GlobalDataManager>
{
    Camera main;
    public void InitUI()
    {
        UIManager.Instance.GetUI<CommonUI>("CommonUI");
    }
    public Camera GetCamera()
    {
        if (main == null)
        {
            main = Camera.main;
            GameObject.DontDestroyOnLoad(main.transform.root.gameObject);
        }
        return main;
    }
    /// <summary>
    /// 设置clasroom相机位置
    /// </summary>
    public void SetPcesCamera(Vector3? pos=null)
    {
        Transform cam= GameObject.Find("PcesCamera").transform;
        if (cam!=null)
        {
            if (pos != null)
            {
                cam.localPosition =(Vector3)pos;
            }
            else
            {
                cam.localPosition = new Vector3(2.636f, 1.071f, 0.33f);//相机默认位置
            }
        }
        else
        {
            Debug.LogError("未找到相机");
        }
      
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
