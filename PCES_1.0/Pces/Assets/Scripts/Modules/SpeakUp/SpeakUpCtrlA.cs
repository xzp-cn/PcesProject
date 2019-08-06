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
    private GameObject emptyRoot;
    private GameObject judaiGobj; //我要句带
    private GameObject tukaA;  //图卡
    private GameObject _tukaA;
    private GameObject FBNKT_KA_Anim;

    private AnimationOper xiaohuaAnim;
    private AnimationOper fdlsAnim;
    private LegacyAnimationOper FBNKT_KA_AnimOper;
    private AnimationOper LS;
    private QHWCtrl qhwCtrl;
    private GameObject goodA;

    void Start()
    {
        GlobalEntity.GetInstance().Dispatch<string>(CommonUI.pEvent.LevelChange, "第一关");
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        if (xiaohuaGo.GetComponent<XHCtrl>() == null)
        {
            xiaohuaGo.AddComponent<XHCtrl>().InitComplete = () =>
            {
                xiaohuaGo.GetComponent<XHCtrl>().r_tuka.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_tuka2.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_judai.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_judai2.SetActive(false);
            };
        }
        emptyRoot = new GameObject("Root");
        GameObject fdlsObj = PeopleManager.Instance.GetPeople("FDLS_BD");
        fdlsObj.transform.localPosition = Vector3.zero;

        GameObject qhwm = ObjectsManager.instanse.GetQHW();
        qhwm.transform.SetParent(emptyRoot.transform);
        qhwCtrl = qhwm.GetComponent<QHWCtrl>();

        //生成我要句带源
        judaiGobj = GameObject.Instantiate(ObjectsManager.instanse.propList[(int)PropsTag.judai_woyao].gameObject);
        judaiGobj.GetComponent<PropsObject>().pData = ObjectsManager.instanse.propList[(int)PropsTag.judai_woyao].pData;
        judaiGobj.transform.SetParent(emptyRoot.transform, false);
        judaiGobj.transform.localPosition = new Vector3(0.083f, 0.0019f, 0);

        //随机一个强化物A
        goodA = SpeakUpModel.GetInstance().GetRndReinforcement();
        qhwCtrl.GetObj(goodA.name);

        //强化物图卡A源
        string tukaNameA = "tuka_" + goodA.name;
        tukaA = GameObject.Instantiate(DistinguishPictureModel.GetInstance().GetTuKa(tukaNameA));
        _tukaA = new GameObject("tukaA");
        _tukaA.transform.SetParent(emptyRoot.transform, false);
        _tukaA.transform.localPosition = new Vector3(999f, 999f, 999f);
        tukaA.transform.SetParent(_tukaA.transform, false);
        _tukaA.transform.localRotation = Quaternion.Euler(-4, 0, 0);

        FBNKT_KA_Anim = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_1ST_FBNKT_KA");
        FBNKT_KA_Anim.name = "XH_D_1ST_FBNKT_KA";
        FBNKT_KA_Anim.transform.SetParent(emptyRoot.transform, false);
        //我要图卡
        FBNKT_KA_Anim.transform.Find("XH_judaiA/XH_judaiA 1/tukaB/tukaB1").GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(judaiGobj.GetComponent<MeshRenderer>().materials[1]);
        //强化物图卡
        FBNKT_KA_Anim.transform.Find("XH_judaiA/XH_judaiA 1/tukaB/tukaB 1").GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(tukaA.GetComponent<MeshRenderer>().materials[1]);

        //1. 进入界面1秒后，触动小华翻开沟通本的动画。
        Invoke("OnXiaoHuaBring", 1f);
    }

    void OnXiaoHuaBring()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        xiaohuaAnim = xiaohuaGo.GetAnimatorOper();


        xiaohuaAnim.PlayForward("XH_D_1ST_FBNKT");
        GameObject fdlsObj2 = PeopleManager.Instance.GetPeople("FDLS_BD");
        fdlsObj2.SetActive(true);
        fdlsAnim = fdlsObj2.GetAnimatorOper();

        fdlsAnim.PlayForward("FDLS_D_1ST_TJD");

        FBNKT_KA_AnimOper = FBNKT_KA_Anim.GetLegacyAnimationOper();
        FBNKT_KA_AnimOper.PlayForward("XH_D_1ST_FBNKT_GKA");

        int start = 65;
        int end = 67;
        bool passA = false;
        xiaohuaAnim.timePointEvent = (t) =>
        {
            if (t >= start && t <= end && !passA)
            {
                passA = true;
                xiaohuaAnim.timePointEvent = null;
                xiaohuaAnim.OnPause();
                fdlsAnim.OnPause();
                FBNKT_KA_AnimOper.OnPause();

                //2. 播放结束，提醒操作者点击辅导教师的手，点击后触发辅导教师抓着小华的手把图卡粘在句带上的动画。
                GameObject fdlsObj = PeopleManager.Instance.GetPeople("FDLS_BD");
                GameObject shou = fdlsObj.transform.Find("FDLS/fdls_shou").gameObject;
                HighLightCtrl.GetInstance().FlashOn(shou);
                shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
                GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickFDTeacherHandFirst);
                ClickDispatcher.Inst.EnableClick = true;
                ChooseDo.Instance.DoWhat(5, RedoClickFDTeachersHandFirst, null);
            }
        };
    }


    private void ClickFDTeachersPromptFirst()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickFDTeachersHandFirst, null);
    }

    private void RedoClickFDTeachersHandFirst()
    {
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要辅导教师协助");
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

            int start = 180;
            int end = 182;
            bool passB = false;
            xiaohuaAnim.timePointEvent = (t) =>
            {
                if (t >= start && t <= end && !passB)
                {
                    passB = true;
                    xiaohuaAnim.timePointEvent = null;
                    xiaohuaAnim.OnPause();
                    fdlsAnim.OnPause();
                    FBNKT_KA_AnimOper.OnPause();

                    GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
                    HighLightCtrl.GetInstance().FlashOn(shou);
                    GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
                    GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
                    ClickDispatcher.Inst.EnableClick = true;
                    ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFinal, null);
                }
            };

            xiaohuaAnim.OnContinue();
            fdlsAnim.OnContinue();
            FBNKT_KA_AnimOper.OnContinue();
        }
    }

    private void RedoClickTeachersHandFinal()
    {
        CancelInvoke("ClickTeachersPromptFinal");
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师给相应物品");
        Invoke("ClickTeachersPromptFinal", 2);
    }

    private void ClickTeachersPromptFinal()
    {
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        Debug.Log("DistinguishPictureCtrlA.ClickTeachersPromptFinal(): 10. 播放结束，提醒操作者点击教师的手，点击后触发教师给小华的动画。");
        HighLightCtrl.GetInstance().FlashOn(shou);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFinal, null);
    }

    private void OnClickTeacherHandFinal(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptFinal");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
            ClickDispatcher.Inst.EnableClick = false;

            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);

            LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();

            LS.OnContinue();
            bool passA = false;
            LS.timePointEvent = (a) =>//老师借卡时间点
            {
                if (a >= 22 && a < 24 && !passA)
                {
                    passA = true;
                    UnityEngine.Debug.Log("SpeakUpCtrlA::OnClickTeacherHandFinal(): 隐藏沟通本句带");
                    LS.timePointEvent = null;
                    xiaohuaAnim.OnContinue();
                    FBNKT_KA_Anim.transform.Find("XH_judaiA").gameObject.SetActive(false);//沟通本图卡隐藏

                    //5. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”
                    SwapUI swapui = UIManager.Instance.GetUI<SwapUI>("SwapUI");
                    swapui.SetButtonVisiable(SwapUI.BtnName.microButton, true);
                    swapui.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
                    swapui.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
                    swapui.speakEvent = () =>
                    {
                        CancelInvoke("ClickPromptMicoUI");
                        ChooseDo.Instance.Clicked();
                        swapui.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
                        swapui.speakEvent = null;
                        swapui.SetButtonVisiable(SwapUI.BtnName.microButton, false);
                        Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
                        string gift = goodA.GetComponent<PropsObject>().pData.name_cn;
                        dialog.SetDialogMessage("小华要" + gift + "呀");

                        //6. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
                        Invoke("ClickTeachersHandFinal", 2f);
                    };

                    ChooseDo.Instance.DoWhat(5, RedoClickMicoUI, null);
                }
            };

            LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/TY_LS_JTKJD_KA").GetLegacyAnimationOper();//跟随老师句带移动卡片
            ka.name = "TY_LS_JTKJD_KA";
            ka.transform.SetParent(transform);
            ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_1").gameObject.SetActive(false);//隐藏不需要图卡
            Material matWy = ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_2").GetComponent<MeshRenderer>().materials[1];//老师我要
            Material matObj = ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_3").GetComponent<MeshRenderer>().materials[1];//老师图卡物品
            Material matSourceWy = emptyRoot.transform.Find("XH_D_1ST_FBNKT_KA/XH_judaiA/XH_judaiA 1/tukaB/tukaB1").GetComponent<MeshRenderer>().materials[1];//小华我要图卡
            Material matSourceObj = emptyRoot.transform.Find("XH_D_1ST_FBNKT_KA/XH_judaiA/XH_judaiA 1/tukaB/tukaB 1").GetComponent<MeshRenderer>().materials[1];//小华递卡物品。
            matWy.CopyPropertiesFromMaterial(matSourceWy);
            matObj.CopyPropertiesFromMaterial(matSourceObj);//给物品

            ka.transform.Find("tuka4").gameObject.SetActive(false);//
            ka.PlayForward("TY_LS_JTKJD_KA");//播放老师图卡动画        图卡等待一帧隐藏

            LS.PlayForward("TY_LS_JTKJD_JG");
        }
    }

    private void RedoClickMicoUI()
    {
        CancelInvoke("ClickPromptMicoUI");
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        Invoke("ClickPromptMicoUI", 2);
    }

    private void ClickPromptMicoUI()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickMicoUI, null);
    }

    private void ClickTeachersHandFinal()
    {
        Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
        dialog.Show(false);
        bool passA = false;
        bool passB = false;
        xiaohuaAnim.Complete += () =>
        {
            //8. 播放结束，出现下一关和重做的按钮。
            Debug.Log("SpeakUpCtrlA.OnClickTeacherHandFinal(): 8. 播放结束，出现下一关和重做的按钮。");
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
            comUI.redoClickEvent += OnReDo;
            comUI.nextClickEvent += OnNextDo;
            comUI.ShowFinalUI();
        };

        LegacyAnimationOper go = null;
        LS.timePointEvent = (a) =>//老师递给物品
        {
            if (a >= 25 && a <= 27 && !passA)//挂载到老师手上强化物时间点
            {
                passA = true;
                LSCtrl lsctrl = LS.GetComponent<LSCtrl>();//将当前强化物挂在老师手上
                lsctrl.SetJoint(qhwCtrl.gameObject);
            }

            if (a >= 21 && a < 24 && !passB)//小华接卡动画播放延迟一边挂载强化物
            {
                passB = true;
                go = ResManager.GetPrefab("Prefabs/AnimationKa/TY_XH_JG_KA").GetLegacyAnimationOper();
                go.transform.SetParent(transform);

                bool pass3 = false;
                xiaohuaAnim.timePointEvent = (b) =>//小华接过物品 挂载强化物
                {
                    if (b >= 42 && b <= 44 && !pass3)
                    {
                        pass3 = true;
                        xiaohuaAnim.timePointEvent = null;
                        qhwCtrl.gameObject.SetActive(false);
                        XhQHW xhqhw = go.GetComponent<XhQHW>();
                        xhqhw.ShowObj(goodA.name);
                        goodA.transform.parent.gameObject.SetActive(false);
                    }
                };
                xiaohuaAnim.PlayForward("TY_XH_JG");
                go.PlayForward("TY_XH_JG_KA");
            }
        };

        LS.PlayForward("TY_LS_DW");
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
        fdlsAnim.OnContinue();
        if (evtFinished != null)
        {
            evtFinished();
        }
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

        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
        xiaohuaAnim.timePointEvent = null;
        xiaohuaAnim.OnContinue();
        fdlsAnim.OnContinue();
        fdlsAnim.timePointEvent = null;
        LS.timePointEvent = null;
        LS.OnContinue();

        XHCtrl xhctrl = xiaohuaAnim.GetComponent<XHCtrl>();
        if (xhctrl != null)
        {
            xhctrl.DestroyGuadian();
        }

        LSCtrl lsctrl = LS.GetComponent<LSCtrl>();
        if (lsctrl != null)
        {
            lsctrl.DestroyGuadian();
        }

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
