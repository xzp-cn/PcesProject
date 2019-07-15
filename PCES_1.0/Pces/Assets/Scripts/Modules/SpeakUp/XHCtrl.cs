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

    public System.Action InitComplete;

    void Start()
    {
        r_tuka = GameObject.Find("tuka");
        r_tuka2 = GameObject.Find("tuka2");
        r_judai = GameObject.Find("XH_judai");
        r_judai2 = GameObject.Find("XH_judai_2");

        if (InitComplete != null)
        {
            InitComplete();
            InitComplete = null;
        }
    }
}
