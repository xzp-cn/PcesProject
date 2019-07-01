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
}
