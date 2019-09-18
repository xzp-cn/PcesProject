using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : SingleTon<PeopleManager>
{
    Dictionary<string, GameObject> pDic = new Dictionary<string, GameObject>();
    Vector3[] posArr = new Vector3[3];
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            posArr[i] = transform.GetChild(i).localPosition;
        }
    }
    /// <summary>
    /// 将人物状态全部初始化
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform temp = transform.GetChild(i);
            temp.localPosition = posArr[i];
            //temp.gameObject.SetActive(false);
            //if (temp.name == "FDLS_BD")
            //{
            //    temp.Find("Group3/Main").localPosition = Vector3.zero;
            //}
            //else if (temp.name == "LS_BD")
            //{
            //    temp.Find("Group2/Main").localPosition = Vector3.zero;
            //}
            //else
            //{
            //    temp.Find("Group/Main").localPosition = Vector3.zero;
            //}
            temp.gameObject.SetActive(true);
            AnimationOper ao = temp.gameObject.GetAnimatorOper();
            ao.OnContinue();
            ao.ClearCompleteEvent();
            ao.timePointEvent = null;
            temp.gameObject.GetAnimatorOper().PlayForward("idle");

            XHCtrl xhctrl = temp.GetComponent<XHCtrl>();
            if (xhctrl != null)
            {
                xhctrl.DestroyGuadian();
            }
            LSCtrl lsctrl = temp.GetComponent<LSCtrl>();
            if (lsctrl != null)
            {
                lsctrl.DestroyGuadian();
            }
        }

    }
    public void CtrlShow(string name, bool isShow = true)
    {
        if (!pDic.ContainsKey(name))
        {
            //Transform temp = transform.Find(name);

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
                Debug.Log("PeopleManager::GetPeople():" + temp.name);
                pDic.Add(name, temp.gameObject);
            }
            else
            {
                //z
                string path = "Prefabs/People/" + pName.ToString("g");
                temp = Instantiate<GameObject>(Resources.Load<GameObject>(path)).transform;
                temp.SetParent(transform);
            }
            go = temp.gameObject;
        }
        return go;
    }

    public void GetNewXH()
    {
        string path = "Prefabs/People/XH_BD";
        Transform temp = Instantiate<GameObject>(Resources.Load<GameObject>(path)).transform;
        temp.SetParent(transform);
        temp.name = "XH_BD";
        pDic[PeopleTag.XH_BD.ToString()] = temp.gameObject;
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
                Debug.Log("PeopleManager::GetPeople():" + temp.name);
                pDic.Add(name, temp.gameObject);
            }
            else
            {
                string path = "Prefabs/People/" + pName;
                temp = Instantiate<GameObject>(Resources.Load<GameObject>(path)).transform;
                temp.SetParent(transform);
            }
            go = temp.gameObject;
        }
        return go;
    }

    public void RemovePeople(PeopleTag pName)
    {
        string name = pName.ToString();
        if (pDic.ContainsKey(name))
        {
            DestroyImmediate(pDic[name]);
            pDic.Remove(name);
        }
    }

    /// <summary>
    /// 充值模型
    /// </summary>
    public void ResetModel()
    {
        RemovePeople(PeopleTag.XH_BD);
        GetPeople("XH_BD");
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
