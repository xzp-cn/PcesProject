using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小华手上道具控制
/// </summary>
public class XHCtrl : MonoBehaviour
{
    /// <summary>
    /// 小华右手图卡
    /// </summary>
    public GameObject r_tuka;

    /// <summary>
    /// 小华右手图卡2
    /// </summary>
    public GameObject r_tuka2;

    /// <summary>
    /// 小华右手句带
    /// </summary>
    public GameObject r_judai;

    /// <summary>
    /// 小华右手句带2
    /// </summary>
    public GameObject r_judai2;
    /// <summary>
    /// 小华右手接过物体挂点
    /// </summary>
    public GameObject XH_R2;
    /// <summary>
    /// 小华左手接过物体的挂点
    /// </summary>
    public GameObject XH_L1;

    public GameObject r_guadian;
    public GameObject l_guadian;

    public GameObject jd_tk1;  //句带下图卡1
    public GameObject jd_tk2;  //句带下图卡2
    public GameObject jd_tk3;  //句带下图卡3

    public System.Action InitComplete;

    void Start()
    {
        if (r_tuka == null)
        {
            r_tuka = GameObject.Find("tuka");
        }
        if (r_tuka2 == null)
        {
            r_tuka2 = GameObject.Find("tuka2");
        }
        if (r_judai == null)
        {
            r_judai = GameObject.Find("XH_judai");
        }
        if (r_judai2 == null)
        {
            r_judai2 = GameObject.Find("XH_judai_2");
        }

        r_tuka.SetActive(false);
        r_tuka2.SetActive(false);
        r_judai.SetActive(false);
        r_judai2.SetActive(false);

        if (XH_R2 != null)
        {
            if (r_guadian == null)
            {
                r_guadian = new GameObject("r_guadian");
                r_guadian.transform.SetParent(XH_R2.transform, false);
            }
        }

        if (XH_L1 != null)
        {
            if (l_guadian == null)
            {
                l_guadian = new GameObject("l_guadian");
                l_guadian.transform.SetParent(XH_L1.transform, false);
            }
        }

        if (InitComplete != null)
        {
            InitComplete();
            InitComplete = null;
        }
    }
    public void SetJoint(GameObject go)
    {
        if (XH_R2 != null)
        {
            go.transform.SetParent(r_guadian.transform);
        }
    }
    /// <summary>
    /// 左手挂点
    /// </summary>
    /// <param name="go"></param>
    public void SetJointL(GameObject go)
    {
        if (XH_L1 != null)
        {
            go.transform.SetParent(l_guadian.transform);
        }
    }
    public void DestroyGuadian()
    {
        if (l_guadian != null)
        {
            for (int i = 0; i < l_guadian.transform.childCount; i++)
            {
                GameObject go = l_guadian.transform.GetChild(i).gameObject;
                go.SetActive(false);
                Destroy(go);
            }
        }

        if (r_guadian != null)
        {
            for (int i = 0; i < r_guadian.transform.childCount; i++)
            {
                GameObject go = r_guadian.transform.GetChild(i).gameObject;
                go.SetActive(false);
                Destroy(go);
            }
        }
    }
}
