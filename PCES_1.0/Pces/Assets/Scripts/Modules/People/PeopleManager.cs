using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : SingleTon<PeopleManager>
{
    Dictionary<string, GameObject> pDic = new Dictionary<string, GameObject>();
    public void CtrlShow(string name, bool isShow = true)
    {
        if (!pDic.ContainsKey(name))
        {
            Transform temp = transform.Find(name);

        }
        else
        {
            pDic[name].gameObject.SetActive(isShow);
        }
    }
    /// <summary>
    /// 获取人物
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetPeople(PeopleTag pName)
    {
        GameObject go = null;
        string name = pName.ToString();
        if (pDic.ContainsKey(name))
        {
            go = pDic[name];
        }
        else
        {
            Transform temp = transform.Find(name);
            if (temp != null)
            {
                pDic.Add(name, temp.gameObject);
            }
            else
            {
                string path = "Prefabs/People/";
                temp = Instantiate<GameObject>(Resources.Load<GameObject>(path)).transform;
                temp.SetParent(transform);
            }
            go = temp.gameObject;
        }
        return go;
    }
    public GameObject GetPeople(string pName)
    {
        GameObject go = null;
        string name = pName.ToString();
        if (pDic.ContainsKey(name))
        {
            go = pDic[name];
        }
        else
        {
            Transform temp = transform.Find(name);
            if (temp != null)
            {
                pDic.Add(name, temp.gameObject);
            }
            else
            {
                string path = "Prefabs/People/";
                temp = Instantiate<GameObject>(Resources.Load<GameObject>(path)).transform;
                temp.SetParent(transform);
            }
            go = temp.gameObject;
        }
        return go;
    }



    private void OnDestroy()
    {

    }
}

public enum PeopleTag
{
    /// <summary>
    /// 辅导老师
    /// </summary>
    FDLS_BD = 0,
    /// <summary>
    /// 小华
    /// </summary>
    XH_BD = 1,
    /// <summary>
    /// 老师
    /// </summary>
    LS_BD = 2
}
