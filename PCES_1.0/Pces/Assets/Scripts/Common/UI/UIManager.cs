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
        RectTransform topEmpty = new GameObject("topEmpty").AddComponent<RectTransform>();
        topEmpty.transform.SetParent(transform);
        topEmpty.transform.localScale = Vector3.one;
        topEmpty.anchorMax = Vector2.one;
        topEmpty.anchorMin = Vector2.zero;
        topEmpty.transform.SetAsLastSibling();
        topEmpty.anchoredPosition3D = Vector3.zero;
        topEmpty.offsetMax = Vector2.zero;
        topEmpty.offsetMin = Vector2.zero;
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
            //Debug.LogError("InitUI  ::  dic" + uiName);
            ui = uiDic[uiName];
        }
        else
        {
            string path = "Prefabs/UI/" + uiName;
            ui = ResManager.GetPrefab(path);
            uiDic.Add(uiName, ui);
            InitUI(uiName, ui.transform);
            //Debug.LogError("Load  ::  " + path);
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
            //ui.transform.SetSiblingIndex(index);
            ui.transform.SetAsLastSibling();
            ui.transform.parent.Find("topEmpty").SetAsLastSibling();

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

    public void ClearDic()
    {
        Dictionary<string, GameObject>.ValueCollection collec = uiDic.Values;
        var iter = collec.GetEnumerator();
        //Debug.Log(collec.Count + "     UIManager");

        while (iter.MoveNext())
        {
            //Debug.Log(iter.Current.name + "     UIManager");
            Destroy(iter.Current);
        }
        uiDic.Clear();
    }

    public void RemoveDic(string key)
    {
        uiDic.Remove(key);
    }
    private void OnDestroy()
    {
        Debug.Log("UIManager  :: ");
    }
}
