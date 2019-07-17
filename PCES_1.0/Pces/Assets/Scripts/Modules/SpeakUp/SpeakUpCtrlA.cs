using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第一关 -- 句型表达
/// </summary>
public class SpeakUpCtrlA : MonoBehaviour
{
    public System.Action evtFinished;
    public System.Action evtRedo;
    private CommonUI comUI;
    private GameObject gtNotebook; //沟通本
    private GameObject emptyRoot;
    private GameObject judaiGobj; //我要句带
    private GameObject RndReinforcementA; //强化物
    private GameObject tukaA;  //图卡
    private GameObject judaiParent;  //句带
    private GameObject _tukaA;

    void Start()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        if(xiaohuaGo.GetComponent<XHCtrl>() == null)
        {
            xiaohuaGo.AddComponent<XHCtrl>().InitComplete = ()=> {
                xiaohuaGo.GetComponent<XHCtrl>().r_tuka.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_tuka2.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_judai.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_judai2.SetActive(false);
            };
        }
        emptyRoot = new GameObject("Root");
        GameObject fdlsObj = PeopleManager.Instance.GetPeople("FDLS_BD");
        fdlsObj.transform.localPosition = Vector3.zero;

        //生成沟通本
        PropsObject gtbProp = ObjectsManager.instanse.GetProps((int)PropsTag.TY_GTB);
        gtNotebook = GameObject.Instantiate(gtbProp.gameObject);
        gtNotebook.GetComponent<PropsObject>().pData = gtbProp.pData;
        gtNotebook.transform.SetParent(emptyRoot.transform, false);
        gtNotebook.transform.localPosition = new Vector3(2.274f, 0.56572f, 0.059f);

        //生成句带
        judaiParent = ResManager.GetPrefab("Prefabs/Objects/judai");
        judaiParent.transform.SetParent(emptyRoot.transform, false);
        judaiParent.transform.localPosition = new Vector3(2.281f, 0.549f, -0.334f);
        judaiParent.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //生成我要句带
        judaiGobj = GameObject.Instantiate(ObjectsManager.instanse.propList[(int)PropsTag.judai_woyao].gameObject);
        judaiGobj.GetComponent<PropsObject>().pData = ObjectsManager.instanse.propList[(int)PropsTag.judai_woyao].pData;
        judaiGobj.transform.SetParent(judaiParent.transform, false);
        judaiGobj.transform.localPosition = new Vector3(0.083f, 0.0019f, 0);

        //随机一个强化物A
        GameObject goodA = SpeakUpModel.GetInstance().GetRndReinforcement();
        RndReinforcementA = GameObject.Instantiate(goodA);
        RndReinforcementA.GetComponent<PropsObject>().pData = goodA.GetComponent<PropsObject>().pData;
        GameObject qhwA = new GameObject("ReinforcementA");
        qhwA.transform.SetParent(emptyRoot.transform, false);
        RndReinforcementA.transform.SetParent(qhwA.transform, false);
        RndReinforcementA.transform.localPosition = Vector3.zero;
        qhwA.transform.localPosition = new Vector3(2.5328F, 0.5698F, -0.118F);
        //强化物图卡A
        string tukaNameA = "tuka_" + goodA.name;
        tukaA = DistinguishPictureModel.GetInstance().GetTuKa(tukaNameA);
        _tukaA = new GameObject("tukaA");
        _tukaA.transform.SetParent(emptyRoot.transform, false);
        _tukaA.transform.localPosition = new Vector3(2.297f, 0.5535f, -0.013f);
        tukaA.transform.SetParent(_tukaA.transform, false);
        _tukaA.transform.localRotation = Quaternion.Euler(-4, 0, 0);

        //1. 进入界面1秒后，触动小华翻开沟通本的动画。
        Invoke("OnXiaoHuaBring", 1f);
    }

    void OnXiaoHuaBring()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();

        gtNotebook.GetAnimatorOper().PlayForward("onePaper");


        xiaohuaAnim.Complete += () =>
        {
            //2. 播放结束，提醒操作者点击辅导教师的手，点击后触发辅导教师抓着小华的手把图卡粘在句带上的动画。
            GameObject fdlsObj = PeopleManager.Instance.GetPeople("FDLS_BD");
            GameObject shou = fdlsObj.transform.Find("FDLS/fdls_shou").gameObject;
            //Debug.Log("SpeakUpCtrlA.OnXiaoHuaBring(): 2. 播放结束，提醒操作者点击辅导教师的手，点击后触发辅导教师抓着小华的手把图卡粘在句带上的动画。");
            HighLightCtrl.GetInstance().FlashOn(shou);
            shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
            GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickFDTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = true;
            ChooseDo.Instance.DoWhat(5, RedoClickFDTeachersHandFirst, null);
        };
        xiaohuaAnim.PlayForward("XH_D_1ST_FB");
        //xiaohuaAnim.PlayForward("XH_D_1ST_FBNKT");
        
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Again"))
        {
            GameObject fdlsObj2 = PeopleManager.Instance.GetPeople("FDLS_BD");
            fdlsObj2.GetAnimatorOper().PlayForward("FDLS_D_1ST_TJD");

            GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
            xiaohuaGo.GetAnimatorOper().PlayForward("XH_D_1ST_FBNKT");
        }
    }

    private void ClickFDTeachersPromptFirst()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickFDTeachersHandFirst, null);
    }

    private void RedoClickFDTeachersHandFirst()
    {
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("请点击辅导老师的手");
        CancelInvoke("ClickFDTeachersPromptFirst");
        Invoke("ClickFDTeachersPromptFirst", 2);
    }

    private void OnClickFDTeacherHandFirst(ClickedObj cobj)
    {
        Debug.Log("SpeakUpCtrlA.OnClickTeacherHandFirst(): " + cobj.objname);
        if (cobj.objname == "fdls_shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickFDTeachersPromptFirst");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickFDTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = false;
            HighLightCtrl.GetInstance().FlashOff(cobj.go);

            GameObject fdlsObj2 = PeopleManager.Instance.GetPeople("FDLS_BD");
            AnimationOper fdlsAnim = fdlsObj2.GetAnimatorOper();
            fdlsAnim.Complete += () => {
                _tukaA.transform.localPosition = new Vector3(2.2565f, 0.5515f, -0.333f);
            };
            fdlsAnim.PlayForward("FDLS_D_1ST_TJD_ZHUA");

            GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
            AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();

            //替换小华手上图卡材质贴图
            XHCtrl xhCtrl = xiaohuaGo.GetComponent<XHCtrl>();
            xhCtrl.r_tuka.SetActive(true);
            xhCtrl.r_tuka.GetComponentInChildren<MeshRenderer>().materials[1].mainTexture = tukaA.GetComponentInChildren<MeshRenderer>().materials[1].mainTexture;
            xhCtrl.r_judai2.SetActive(true);
            if (xhCtrl.jd_tk1 != null)
            {
                xhCtrl.jd_tk1.SetActive(false);
            }

            if (xhCtrl.jd_tk2 != null)
            {
                //图卡A
                xhCtrl.jd_tk2.SetActive(true);
                xhCtrl.jd_tk2.GetComponentInChildren<MeshRenderer>().materials[1].mainTexture = tukaA.GetComponentInChildren<MeshRenderer>().materials[1].mainTexture;
            }

            if (xhCtrl.jd_tk2 != null)
            {
                //我要图卡
                xhCtrl.jd_tk3.SetActive(true);
                xhCtrl.jd_tk3.GetComponentInChildren<MeshRenderer>().materials[1].mainTexture = judaiGobj.GetComponentInChildren<MeshRenderer>().materials[1].mainTexture;
            }

            xiaohuaAnim.Complete += () => {
                //5. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”
                SwapUI swapui = UIManager.Instance.GetUI<SwapUI>("SwapUI");
                swapui.SetButtonVisiable(SwapUI.BtnName.microButton, true);
                swapui.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
                swapui.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
                swapui.speakEvent = () =>
                {
                    swapui.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
                    swapui.speakEvent = null;
                    swapui.SetButtonVisiable(SwapUI.BtnName.microButton, false);
                    Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
                    string gift = RndReinforcementA.GetComponent<PropsObject>().pData.name_cn;
                    dialog.SetDialogMessage("小华要" + gift + "呀。");

                    //6. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
                    Invoke("ClickTeachersHandFinal", 2f);
                };
            };

            xiaohuaAnim.PlayForward("XH_D_1ST_FB_NK");
        }
    }

    private void ClickTeachersHandFinal()
    {
        Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
        dialog.Show(false);
        //7. 播放结束，触发小华接过XXX。
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        xiaohuaAnim.Complete += () =>
        {
            //8. 播放结束，出现下一关和重做的按钮。
            Debug.Log("SpeakUpCtrlA.OnClickTeacherHandFinal(): 8. 播放结束，出现下一关和重做的按钮。");
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
            comUI.redoClickEvent += OnReDo;
            comUI.nextClickEvent += OnNextDo;
            comUI.ShowFinalUI();
        };
        xiaohuaAnim.PlayForward("TY_XH_JG");
    }

    private void OnReDo()
    {
        Redo();
    }

    private void OnNextDo()
    {
        Finished();
    }

    /// <summary>
    /// 本关完成
    /// </summary>
    void Finished()
    {
        if (evtFinished != null)
        {
            evtFinished();
        }
        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
    }

    void Redo()
    {
        if (evtRedo != null)
        {
            evtRedo();
        }
        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
    }

    public void Dispose()
    {
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
        comUI = null;
        evtFinished = null;
        evtRedo = null;
        if (emptyRoot != null)
        {
            Destroy(emptyRoot);
            emptyRoot = null;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }
}
