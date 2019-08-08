using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceCtrlC : MonoBehaviour
{
    public event System.Action evtFinished;
    public event System.Action evtRedo;
    SwapUI swapUI;
    SelectionUI selectUI;
    GameObject fdlshand;
    GameObject jshand;
    GameObject lsObject;
    GameObject xhObject;
    AnimationOper LS;
    AnimationOper XH;
    AnimationOper FDLS;
    //AnimationOper GTB;//沟通本
    LegacyAnimationOper gtb, gtbKJ;
    QHWCtrl qhwCtrl;
    //QHWCtrl qhwCtrl;
    private void Awake()
    {
        this.name = "SentenceCtrlC";
    }
    //public bool Finished;
    private void Start()
    {
        GlobalEntity.GetInstance().Dispatch<string>(CommonUI.pEvent.LevelChange, "第三关");
        Init();
    }
    public void Init()
    {
        if (swapUI == null)
        {
            swapUI = UIManager.Instance.GetUI<SwapUI>("SwapUI");
            //swapUI.chooseEvent += ChooseBtnClickCallback;
            swapUI.speakEvent += SpeakBtnClickCallback;
            swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
            swapUI.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
        }
        UIManager.Instance.GetUI<Dialog>("Dialog").SetPos(new Vector3(-55, -490, 0));
        PeopleManager.Instance.Reset();

        LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();
        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        FDLS = PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).GetAnimatorOper();
        FDLS.transform.localPosition = new Vector3(0, 0, 10000);
        LS.PlayForward("idle");
        XH.PlayForward("idle");

        //FDLS.PlayForward("idle");
        //FDLS.gameObject.SetActive(false);
        //FDLS.PlayForward("FDLS_A_2ND_D");
        HighLightCtrl.GetInstance().OffAllObjs();
        //KJLsGiveObj();
        //InitKanjian();
        GetTukaObject();
    }
    /// <summary>
    /// 初始化桌子上的涂卡
    /// </summary>
    void GetTukaObject()
    {
        PropsObject pObj = SentenceExpressionModel.GetInstance().GetObj(PropsType.reinforcement);//中性刺激物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject");

        gtb = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_1ST_FBNKT_KA").GetLegacyAnimationOper();//沟通本      
        gtb.transform.SetParent(transform);
        gtb.name = "XH_D_1ST_FBNKT_KA";//沟通本更新      

        gtbKJ = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_3RD_FBNKTK_KA").GetLegacyAnimationOper();//沟通本
        gtbKJ.name = PropsTag.TY_GTB.ToString();
        gtbKJ.transform.SetParent(transform);
        gtbKJ.name = "XH_D_3RD_FBNKTK_KA";//沟通本更新在日志
        gtbKJ.transform.Find("goutongben").gameObject.SetActive(false);

        //沟通本我要图卡
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_woyao.ToString()).GetComponent<MeshRenderer>().materials[1];
        Material matTar = gtb.transform.Find("XH_judaiA/XH_judaiA 1/tukaB/tukaB1").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        //沟通本之中性刺激物图卡
        string _tuka = "tuka_" + rfc.pData.name;//沟通本里面图卡  
        matSource = SentenceExpressionModel.GetInstance().GetTuKa(_tuka).GetComponent<MeshRenderer>().materials[1];
        matTar = gtb.transform.Find("XH_judaiA/XH_judaiA 1/tukaB/tukaB 1").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        //设置老师旁边的中性刺激物模型
        string objName = rfc.pData.name;//       
        GameObject obj = ObjectsManager.instanse.GetQHW();
        obj.name = "QHW";
        obj.transform.SetParent(transform);
        qhwCtrl = obj.GetComponent<QHWCtrl>();
        qhwCtrl.ShowObj(objName);
        //qhwCtrl = obj.GetComponent<QHWCtrl>();
        //qhwCtrl.ShowObj(objName);

        //LS.Complete += ClickmicroPhoneTip;
        //LS.PlayForward("LS_E_1ST_ZB");

        XhTJudai();

    }
    void XhTJudai()
    {
        ClickDispatcher.Inst.EnableClick = false;

        XH.timePointEvent = (b) =>
        {
            if (b > 184 && b < 189)
            {
                XH.timePointEvent = null;
                XH.OnPause();
                XhTakeCardCallback();
            }
        };
        //XH.Complete += XhTakeCardCallback;
        XH.PlayForward("XH_D_1ST_FBNKT");
        //      
        gtb.framePointEvent = (b) =>
        {
            if (b > 175 && b < 180)
            {
                gtb.framePointEvent = null;
                gtb.OnPause();
            }
        };
        gtb.PlayForward("XH_D_1ST_FBNKT_KA");
    }
    /// <summary>
    /// 小华拿卡递卡回调
    /// </summary>
    void XhTakeCardCallback()
    {
        //gtb.PlayForward("XH_D_2ND_FYFT_KA");
        //GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ClickDispatcher.Inst.EnableClick = true;
        ClickLsHandJiekaTip();
    }
    void ClickLsCallBack(ClickedObj cobj)
    {
        Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
        }
    }
    void ClickLsHandJiekaTip()
    {
        if (jshand == null)
        {
            jshand = LS.transform.Find("LSB_BD/shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoLsJieka, LsJieka);
    }
    void RedoLsJieka()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOn(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        CancelInvoke("ClickLsHandJiekaTip");
        Invoke("ClickLsHandJiekaTip", 2);
    }
    void LsJieka()
    {
        CancelInvoke("ClickLsHandJiekaTip");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        LS.Complete += LsJiekaCallback;
        LS.PlayForward("TY_LS_JTKJD_JG");

        LS.timePointEvent = (a) =>//老师借卡时间点
        {
            if (a >= 19 && a <= 21)
            {
                //Debug.LogError("event");
                LS.timePointEvent = null;
                XH.OnContinue();//小华手收回
                transform.Find("XH_D_1ST_FBNKT_KA/XH_judaiA").gameObject.SetActive(false);//沟通本图卡隐藏
            }
        };

        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/TY_LS_JTKJD_KA").GetLegacyAnimationOper();//跟随老师提示移动卡片
        ka.name = "TY_LS_JTKJD_KA";
        ka.transform.SetParent(transform);
        ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_1").gameObject.SetActive(false);//隐藏不需要图卡
        Material matWy = ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_2").GetComponent<MeshRenderer>().materials[1];//老师我要
        Material matObj = ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_3").GetComponent<MeshRenderer>().materials[1];//老师图卡物品
        Material matSourceWy = transform.Find("XH_D_1ST_FBNKT_KA/XH_judaiA/XH_judaiA 1/tukaB/tukaB1").GetComponent<MeshRenderer>().materials[1];//小华我要图卡
        Material matSourceObj = transform.Find("XH_D_1ST_FBNKT_KA/XH_judaiA/XH_judaiA 1/tukaB/tukaB 1").GetComponent<MeshRenderer>().materials[1];//小华递卡物品。
        matWy.CopyPropertiesFromMaterial(matSourceWy);
        matObj.CopyPropertiesFromMaterial(matSourceObj);//给物品

        ka.transform.Find("tuka4").gameObject.SetActive(false);//
        ka.PlayForward("TY_LS_JTKJD_KA");//播放老师图卡动画        图卡等待一帧隐藏
    }
    /// <summary>
    /// 教师接收图卡回调
    /// </summary>
    void LsJiekaCallback()
    {
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        ClickDispatcher.Inst.EnableClick = false;
        ClickmicroPhoneJiekaTip();
    }
    void ClickmicroPhoneJiekaTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoLsJiekaSpeak, ShowSpeakJiekaContent);
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
    }
    void SpeakBtnClickCallback()
    {
        UIFlah uf = swapUI.GetMicroBtn.gameObject.GetUIFlash();
        uf.StopFlash();
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        ChooseDo.Instance.Clicked();
    }
    void RedoLsJiekaSpeak()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        CancelInvoke("ClickmicroPhoneJiekaTip");
        Invoke("ClickmicroPhoneJiekaTip", 2);
    }
    void ShowSpeakJiekaContent()
    {
        CancelInvoke("ClickmicroPhoneJiekaTip");
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("小华要" + curObjName + "呀");
        CancelInvoke("LsGiveInit");
        Invoke("LsGiveInit", 2);
    }
    /// <summary>
    ///  老师给物品
    /// </summary>
    void LsGiveInit()
    {
        Debug.Log("给物品");
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);
        ClickLsGiveObjTip();
    }
    void ClickLsGiveObjTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoLsGiveObj, LsGiveObj);
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void RedoLsGiveObj()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师奖励强化物");
        CancelInvoke("ClickLsGiveObjTip");
        Invoke("ClickLsGiveObjTip", 2);
        //ClickLsGiveObjTip();
    }
    void LsGiveObj()
    {
        CancelInvoke("ClickLsGiveObjTip");
        Debug.Log("教师给物品");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);

        Transform qhw = transform.Find("QHW");
        bool passXh = true;
        LS.timePointEvent = (a) =>//老师递给物品
        {
            if (a > 25 && a < 28)//挂载到老师手上强化物时间点
            {
                LSCtrl lsctrl = LS.GetComponent<LSCtrl>();//将当前强化物挂在老师手上    
                lsctrl.SetJoint(qhw.gameObject);
                //Debug.LogError("ls");
            }

            if (a > 40 && a < 43 && passXh)//小华接卡动画播放延迟
            {
                LS.timePointEvent = null;
                passXh = false;

                LegacyAnimationOper go = ResManager.GetPrefab("Prefabs/AnimationKa/TY_XH_JG_KA").GetLegacyAnimationOper();
                go.name = "TY_XH_JG_KA";
                go.transform.SetParent(transform);

                XH.Complete += XHJiewuCallback;
                XH.timePointEvent = (b) =>//小华接过物品 挂载强化物
                {
                    if (b >= 40 && b <= 42)
                    {
                        //Debug.LogError(b);
                        XH.timePointEvent = null;
                        //XHCtrl xhCtrl = XH.GetComponent<XHCtrl>();
                        //xhCtrl.SetJoint(qhw.gameObject);   
                        XhQHW xhqhw = go.GetComponent<XhQHW>();
                        string name = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name;
                        xhqhw.ShowObj(name);
                        qhw.gameObject.SetActive(false);
                    }
                };
                XH.PlayForward("TY_XH_JG");
                go.PlayForward("TY_XH_JG_KA");
            }
        };

        LS.Complete += LsGiveObjCallback;
        LS.PlayForward("TY_LS_DW");

    }
    void LsGiveObjCallback()
    {
        //ShowFinalUI();
        transform.Find("TY_LS_JTKJD_KA").gameObject.SetActive(false);
        swapUI.speakEvent -= SpeakBtnClickCallback;
        swapUI.speakEvent += KJSpeakBtnClickCallback;

        gtb.gameObject.SetActive(false);//原来沟通本隐藏
        XHCtrl xhctrl = XH.GetComponent<XHCtrl>();
        for (int i = 0; i < xhctrl.r_guadian.transform.childCount; i++)
        {
            xhctrl.r_guadian.transform.GetChild(i).gameObject.SetActive(false);
        }
        XH.timePointEvent = null;
        XH.gameObject.SetActive(false);
        XH.gameObject.SetActive(true);

        LS.timePointEvent = null;
        LS.gameObject.SetActive(false);
        LS.gameObject.SetActive(true);

        transform.Find("TY_XH_JG_KA").gameObject.SetActive(false);

        InitKanjian();
    }
    void XHJiewuCallback()
    {
        Debug.Log("xh给物品回调用");
    }


    /// <summary>
    /// 看见模块初始化
    /// </summary>
    void InitKanjian()
    {
        PropsObject pObj = SentenceExpressionModel.GetInstance().GetObj(PropsType.neutralStimulator);//中性刺激物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurneutralStimulator = rfc;//设置强化物
        Debug.Log("中性刺激物  " + rfc.pData.name);

        string objName = rfc.pData.name;//桌子上中兴刺激物
        GameObject obj = Instantiate(SentenceExpressionModel.GetInstance().GetTuKa(objName));
        obj.name = "neutralStimulator";
        obj.transform.SetParent(transform, false);
        obj.transform.localPosition = new Vector3(2.453f, 0.578f, 0.798f);
        obj.transform.localScale = Vector3.one * 0.6F;

        gtbKJ.transform.Find("goutongben").gameObject.SetActive(true);

        //沟通本我要图卡
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_wokanjian.ToString()).GetComponent<MeshRenderer>().materials[1];
        Material matTar = gtbKJ.transform.Find("XH_judaiA/XH_judaiA 1/tukaB/tukaB1").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        //沟通本之中性刺激物图卡
        string _tuka = "tuka_" + rfc.pData.name;//沟通本里面图卡  
        matSource = SentenceExpressionModel.GetInstance().GetTuKa(_tuka).GetComponent<MeshRenderer>().materials[1];
        matTar = gtbKJ.transform.Find("XH_judaiA/XH_judaiA 1/tukaB/tukaB 1").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        //设置老师旁边的中性刺激物模型
        pObj = SentenceExpressionModel.GetInstance().GetObj(PropsType.reinforcement);//强化物
        rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        objName = rfc.pData.name;//                          

        obj = ObjectsManager.instanse.GetQHW();
        obj.name = "QHW";
        obj.transform.SetParent(transform);
        qhwCtrl = obj.GetComponent<QHWCtrl>();
        qhwCtrl.ShowObj(objName);

        ClickmicroPhoneTip();
    }
    void ClickmicroPhoneTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoLsSpeak, ShowSpeakContent);
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
    }
    void RedoLsSpeak()
    {
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        CancelInvoke("ClickmicroPhoneTip");
        Invoke("ClickmicroPhoneTip", 2);
    }
    void KJSpeakBtnClickCallback()
    {
        UIFlah uf = swapUI.GetMicroBtn.gameObject.GetUIFlash();
        uf.StopFlash();
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        ChooseDo.Instance.Clicked();
    }
    void ShowSpeakContent()
    {
        CancelInvoke("ClickmicroPhoneTip");
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        dlog.SetDialogMessage("小华看见什么");
        Invoke("CancelDialogShow", 2);
        KJXhTJudai();
    }
    void CancelDialogShow()
    {
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);
    }
    //点中辅导老师手后的回调
    /// <summary>
    ///小华贴提示
    /// </summary>
    void KJXhTJudai()
    {
        ClickDispatcher.Inst.EnableClick = false;
        XH.timePointEvent = (b) =>
        {
            //Debug.Log(b);
            if (b > 181 && b < 185)
            {
                XH.timePointEvent = null;
                XH.OnPause();
                KBXhTakeCardCallback();
            }
        };
        XH.PlayForward("XH_D_3RD_FBNKTK");

        gtbKJ.framePointEvent = (b) =>
        {
            if (b > 180 && b < 185)
            {
                gtbKJ.timePointEvent = null;
                gtbKJ.OnPause();
            }
        };
        gtbKJ.PlayForward("XH_D_3RD_FBNKTK_KA");
    }
    /// <summary>
    /// 小华拿卡递卡回调
    /// </summary>
    void KBXhTakeCardCallback()
    {
        //GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ClickDispatcher.Inst.EnableClick = true;
        KJClickLsHandJiekaTip();
    }
    void KJClickLsHandJiekaTip()
    {
        if (jshand == null)
        {
            jshand = LS.transform.Find("LSB_BD/shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ChooseDo.Instance.DoWhat(5, KJRedoLsJieka, KJLsJieka);
    }
    void KJRedoLsJieka()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOn(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        CancelInvoke("KJClickLsHandJiekaTip");
        Invoke("KJClickLsHandJiekaTip", 2);
    }
    void KJLsJieka()
    {
        CancelInvoke("KJClickLsHandJiekaTip");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;

        LS.Complete += KJLsJiekaCallback;
        LS.PlayForward("TY_LS_JTKJD_JG");

        LS.timePointEvent = (a) =>//老师借卡时间点
        {
            if (a >= 19 && a <= 23)
            {
                //Debug.LogError("event");
                LS.timePointEvent = null;
                XH.OnContinue();//小华手收回
                transform.Find("XH_D_3RD_FBNKTK_KA/XH_judaiA").gameObject.SetActive(false);//沟通本图卡隐藏
            }
        };

        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/TY_LS_JTKJD_KA").GetLegacyAnimationOper();//跟随老师提示移动卡片
        ka.name = "TY_LS_JTKJD_KA";
        ka.transform.SetParent(transform);
        ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_1").gameObject.SetActive(false);//隐藏不需要图卡
        Material matWy = ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_2").GetComponent<MeshRenderer>().materials[1];//老师我要
        Material matObj = ka.transform.Find("LS_judai_1/ls_judai_1/ls_jd_tuka_3").GetComponent<MeshRenderer>().materials[1];//老师图卡物品
        Material matSourceWy = transform.Find("XH_D_3RD_FBNKTK_KA/XH_judaiA/XH_judaiA 1/tukaB/tukaB1").GetComponent<MeshRenderer>().materials[1];//小华我要图卡
        Material matSourceObj = transform.Find("XH_D_3RD_FBNKTK_KA/XH_judaiA/XH_judaiA 1/tukaB/tukaB 1").GetComponent<MeshRenderer>().materials[1];//小华递卡物品。
        matWy.CopyPropertiesFromMaterial(matSourceWy);
        matObj.CopyPropertiesFromMaterial(matSourceObj);//给物品

        ka.transform.Find("tuka4").gameObject.SetActive(false);//
        ka.PlayForward("TY_LS_JTKJD_KA");//播放老师图卡动画        图卡等待一帧隐藏
    }
    /// <summary>
    /// 教师接收图卡回调
    /// </summary>
    void KJLsJiekaCallback()
    {
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        ClickDispatcher.Inst.EnableClick = false;
        KJClickmicroPhoneJiekaTip();
    }
    void KJClickmicroPhoneJiekaTip()
    {
        ChooseDo.Instance.DoWhat(5, KJRedoLsJiekaSpeak, KJShowSpeakJiekaContent);
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
    }
    void KJRedoLsJiekaSpeak()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        CancelInvoke("KJClickmicroPhoneJiekaTip");
        Invoke("KJClickmicroPhoneJiekaTip", 2);
    }
    void KJShowSpeakJiekaContent()
    {
        CancelInvoke("KJClickmicroPhoneJiekaTip");
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SentenceExpressionModel.GetInstance().CurneutralStimulator.pData.name_cn;
        dlog.SetDialogMessage("是的，小华看见了" + curObjName + "，小华表现得很好!");
        CancelInvoke("KJLsGiveInit");
        Invoke("KJLsGiveInit", 2);
    }
    /// <summary>
    ///  老师给物品
    /// </summary>
    void KJLsGiveInit()
    {
        Debug.Log("给物品");
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);
        KJClickLsGiveObjTip();
    }
    void KJClickLsGiveObjTip()
    {
        ChooseDo.Instance.DoWhat(5, KJRedoLsGiveObj, KJLsGiveObj);
        if (jshand == null)
        {
            jshand = LS.transform.Find("LSB_BD/shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void KJRedoLsGiveObj()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师奖励强化物");
        CancelInvoke("KJClickLsGiveObjTip");
        Invoke("KJClickLsGiveObjTip", 2);
    }
    void KJLsGiveObj()
    {
        CancelInvoke("KJClickLsGiveObjTip");
        Debug.Log("教师给物品");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);

        Transform qhw = transform.Find("QHW");
        bool passXh = true;
        LS.timePointEvent = (a) =>//老师递给物品
        {
            if (a >= 25 && a <= 27)//挂载到老师手上强化物时间点
            {

                LSCtrl lsctrl = LS.GetComponent<LSCtrl>();//将当前强化物挂在老师手上    
                lsctrl.SetJoint(qhw.gameObject);
                //Debug.LogError("ls");
            }

            if (a > 40 && a < 43 && passXh)//小华接卡动画播放延迟
            {
                passXh = false;
                LS.timePointEvent = null;
                XH.timePointEvent = null;

                LegacyAnimationOper go = ResManager.GetPrefab("Prefabs/AnimationKa/TY_XH_JG_KA").GetLegacyAnimationOper();
                go.name = "TY_XH_JG_KA";
                go.transform.SetParent(transform);

                XH.timePointEvent = (b) =>//小华接过物品 挂载强化物
                {
                    //Debug.Log(b);
                    if (b > 40 && b < 43)//卡在一帧，多帧updae -多次进入该方法-多次执行覆盖，B参数用的上一次
                    {
                        XH.timePointEvent = null;
                        //XHCtrl xhCtrl = XH.GetComponent<XHCtrl>();
                        //xhCtrl.SetJoint(qhw.gameObject);
                        XhQHW xhqhw = go.GetComponent<XhQHW>();
                        string name = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name;
                        xhqhw.ShowObj(name);
                        qhw.gameObject.SetActive(false);
                    }
                };
                XH.Complete += KJXHJiewuCallback;
                XH.PlayForward("TY_XH_JG");
                go.PlayForward("TY_XH_JG_KA");
            }
        };

        LS.Complete += KJLsGiveObjCallback;
        LS.PlayForward("TY_LS_DW");
    }
    void KJLsGiveObjCallback()
    {
        ShowFinalUI();
    }
    void KJXHJiewuCallback()
    {
        Debug.Log("xh给物品回调用");
    }
    /// <summary>
    /// 显示结算界面
    /// </summary>
    void ShowFinalUI()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.nextClickEvent += NextDo;
        com.redoClickEvent += ReDo;
        UIManager.Instance.SetUIDepthTop("CommonUI");
        com.ShowFinalUI();
    }
    void Finish()
    {
        ChooseDo.Instance.ResetAll();
        UIManager.Instance.GetUI<CommonUI>("CommonUI").HideFinalUI();
        ResetGuaDian();
        RemoveAllListeners();
    }
    void NextDo()
    {
        Finish();
        if (evtFinished != null)
        {
            evtFinished();
        }
    }
    void RemoveAllListeners()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.redoClickEvent -= NextDo;
        com.redoClickEvent -= ReDo;
        com = null;

        LS.Complete -= KJLsGiveObjCallback;
        XH.Complete -= KJXHJiewuCallback;

        XH.timePointEvent = null;
        LS.timePointEvent = null;

        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);

    }
    void ReDo()
    {
        Debug.Log("redo");
        Finish();
        if (evtRedo != null)
        {
            evtRedo();
        }
    }
    void ResetGuaDian()
    {
        XHCtrl xhctrl = XH.GetComponent<XHCtrl>();
        xhctrl.DestroyGuadian();

        LSCtrl lsctrl = LS.GetComponent<LSCtrl>();
        lsctrl.DestroyGuadian();
    }
    public void Dispose()
    {
        RemoveAllListeners();
        evtFinished = null;
        evtRedo = null;
        Destroy(gameObject);
    }
    private void OnDestroy()
    {

    }

}
