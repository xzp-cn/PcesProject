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
    /// 小华右手接过老师递过来的物体空节点
    /// </summary>
    public GameObject XH_R2;

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

        if (InitComplete != null)
        {
            InitComplete();
            InitComplete = null;
        }
    }
}
