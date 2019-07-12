using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第一关 -- 句型表达
/// </summary>
public class SpeakUpCtrlA : MonoBehaviour {
    public System.Action evtFinished;
    public System.Action evtRedo;
    private CommonUI comUI;
    private GameObject gtNotebook; //沟通本
    private GameObject emptyRoot;


    void Start () {
        emptyRoot = new GameObject("Root");
        PropsObject gtbProp = ObjectsManager.instanse.GetProps((int)PropsTag.TY_GTB);
        gtNotebook = GameObject.Instantiate(gtbProp.gameObject);
        gtNotebook.GetComponent<PropsObject>().pData = gtbProp.pData;
        gtNotebook.transform.SetParent(emptyRoot.transform, false);
        gtNotebook.transform.localPosition = new Vector3(2.276f, 0.56572f, 0.1f);

        //1. 进入界面1秒后，触动小华翻开沟通本的动画。
        Invoke("OnXiaoHuaBring", 1f);
    }

    void OnXiaoHuaBring()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        //if (tukaMat != null)
        //{
        //    //替换手上图卡材质
        //    Material[] tukaBs = tukaB.GetComponentInChildren<MeshRenderer>().materials;
        //    tukaMat.CopyPropertiesFromMaterial(tukaBs[1]);
        //    XH_tkB.GetComponentInChildren<MeshRenderer>().materials[1] = tukaMat;
        //}

        xiaohuaAnim.Complete += () =>
        {
            //2. 播放结束，提醒操作者点击辅导教师的手，点击后触发辅导教师抓着小华的手把图卡粘在句带上的动画。
            GameObject shou = PeopleManager.Instance.GetPeople("FDLS_BD").transform.Find("FDLS_BD/fdls_shou").gameObject;
            Debug.Log("SpeakUpCtrlA.OnXiaoHuaBring(): 2. 播放结束，提醒操作者点击辅导教师的手，点击后触发辅导教师抓着小华的手把图卡粘在句带上的动画。");
            HighLightCtrl.GetInstance().FlashOn(shou);
            shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
            GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickFDTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = true;
            ChooseDo.Instance.DoWhat(5, RedoClickFDTeachersHandFirst, null);
        };
        xiaohuaAnim.PlayForward("TY_XH_NK");
    }

    private void RedoClickFDTeachersHandFirst()
    {

    }

    private void OnClickFDTeacherHandFirst(ClickedObj cobj)
    {
        Debug.Log("SpeakUpCtrlA.OnClickTeacherHandFirst(): " + cobj.objname);
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptFirst");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickFDTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = false;
            HighLightCtrl.GetInstance().FlashOff(cobj.go);

            //播放小华递卡动画
            GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
            AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
            xiaohuaAnim.PlayForward("TY_XH_DK");

            //播放老师接图卡动画
            AnimationOper teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
            teacherAnim.Complete += () =>
            {
            };
            teacherAnim.PlayForward("TY_LS_JK");
        }
    }
}
