using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 老师手上道具控制
/// </summary>
public class LSCtrl : MonoBehaviour {
    public GameObject ls_Lf; //老师左手指
    public GameObject l_guadian; //老师左手指挂点
    public GameObject ls_judai; //句带
    public GameObject ls_tuka1; //老师图卡1
    public GameObject ls_tuka2; //老师图卡2

    private void Awake()
    {
        ShowGroup(true);
        
    }
    void Start() {
        if (ls_judai != null)
        {
            ls_judai.SetActive(false);

            ls_tuka1.SetActive(false);

            ls_tuka2.SetActive(false);
        }

        if (ls_Lf != null)
        {
            l_guadian = new GameObject("L_guadian");
            l_guadian.transform.SetParent(ls_Lf.transform,false);
        }
    }

    /// <summary>
    /// 指定物体挂载到挂点上
    /// </summary>
    /// <param name="obj"></param>
    public void SetJoint(GameObject obj)
    {
        if (l_guadian != null) {
            obj.transform.SetParent(l_guadian.transform, false);
        }
    }

    /// <summary>
    /// 开始关闭组节点
    /// </summary>
    /// <param name="val"></param>
    public void ShowGroup(bool val)
    {
        transform.Find("Group2/Main").gameObject.SetActive(val);
    }
}
