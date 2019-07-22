using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    Dictionary<string, GameObject> uiDic = new Dictionary<string, GameObject>();
    Canvas canvas;
    public override void Awake()
    {
        base.Awake();
        //uiDic = new Dictionary<string, GameObject>();
        Debug.Log("调用全局初始化方法 GlobalDataManager");
    }
    public void InitUI(string name, Transform cur)
    {
        cur.name = name;
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
        }
        cur.transform.SetParent(canvas.transform, false);
        //cur.parent = canvas.transform;
        cur.transform.localScale = Vector3.one;
        cur.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// 获取ui
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public T GetUI<T>(string uiName) where T : MonoBehaviour
    {
        GameObject ui = null;
        if (uiDic.ContainsKey(uiName))
        {
            ui = uiDic[uiName];
        }
        else
        {
            string path = "Prefabs/UI/" + uiName;
            ui = ResManager.GetPrefab(path);
            uiDic.Add(uiName, ui);
            InitUI(uiName, ui.transform);
        }
        ui.name = uiName;
        T uiCom = ui.GetComponent<T>();
        if (uiCom == null)
        {
            uiCom = ui.AddComponent<T>();
            //Debug.LogError("UI没有挂载相应组件");
        }
        return uiCom;
    }
    /// <summary>
    /// 将ui设置为最上面显示
    /// </summary>
    /// <param name="uiName"></param>
    public void SetUIDepthTop(string uiName)
    {
        if (!uiDic.ContainsKey(uiName))
        {
            Debug.LogError("canvas下没有该UI");
            return;
        }
        else
        {
            GameObject ui = uiDic[uiName];
            ui.transform.SetAsLastSibling();
        }
    }
    /// <summary>
    /// 关闭所有UI显示
    /// </summary>
    public void CloseAllUI()
    {
        if (uiDic.Count == 0)
        {
            return;
        }
        foreach (KeyValuePair<string, GameObject> item in uiDic)
        {
            item.Value.SetActive(false);
        }
    }

    //public void UiFlash
}
