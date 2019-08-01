using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceCtrlA : MonoBehaviour
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
    LegacyAnimationOper gtb;
    QHWCtrl qhwCtrl;
    private void Awake()
    {
        this.name = "SentenceCtrlA";
    }
    //public bool Finished;
    private void Start()
    {
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
        //LS.gameObject.SetActive(true);
        //XH.gameObject.SetActive(true);
        LS.PlayForward("idle");
        XH.PlayForward("idle");
        FDLS.PlayForward("idle");
        //FDLS.gameObject.SetActive(false);
        //FDLS.PlayForward("FDLS_A_2ND_D");
        HighLightCtrl.GetInstance().OffAllObjs();
        GetTukaObject();
    }
    /// <summary>
    /// 初始化桌子上的涂卡
    /// </summary>
    void GetTukaObject()
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

        gtb = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_1ST_FBNKT_KA").GetLegacyAnimationOper();
        gtb.name = PropsTag.TY_GTB.ToString();
        gtb.transform.SetParent(transform);
        gtb.name = "XH_D_1ST_FBNKT_KA";
        gtb.transform.SetParent(transform);

        //沟通本我看见图卡
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_wokanjian.ToString()).GetComponent<MeshRenderer>().materials[1];
        Material matTar = gtb.transform.Find("XH_judaiA/XH_judaiA 1/tukaB/tukaB1").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        //沟通本之中性刺激物图卡
        string _tuka = "tuka_" + rfc.pData.name;//沟通本里面图卡  
        matSource = SentenceExpressionModel.GetInstance().GetTuKa(_tuka).GetComponent<MeshRenderer>().materials[1];
        matTar = gtb.transform.Find("XH_judaiA/XH_judaiA 1/tukaB/tukaB 1").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        //设置老师旁边的强化物模型

        pObj = SentenceExpressionModel.GetInstance().GetObj(PropsType.reinforcement);//强化物
        rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        objName = rfc.pData.name;//                          

        obj = ObjectsManager.instanse.GetQHW();
        obj.name = "QHW";
        obj.transform.SetParent(transform);
        qhwCtrl = obj.GetComponent<QHWCtrl>();
        qhwCtrl.ShowObj(objName);
        //qhwCtrl = obj.GetComponent<QHWCtrl>();
        //qhwCtrl.ShowObj(objName);

        //LS.Complete += ClickmicroPhoneTip;
        //LS.PlayForward("LS_E_1ST_ZB");
        //
        bool passPoint = true;
        LS.timePointEvent = (a) =>
        {
            if (a >= 40 && a <= 45 && passPoint)//
            {
                passPoint = false;
                LS.OnPause();
                ClickmicroPhoneTip();
            }
        };
        LS.PlayForward("LS_F_1ST_ZCJW");
        //Debug.Log("ls");
    }
    /// <summary>
    /// 辅导老师手被点击
    /// </summary>
    /// <param name="cobj"></param>
    void ClickFdlsCallBack(ClickedObj cobj)
    {
        Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "fdls_shou")
        {
            ChooseDo.Instance.Clicked();
        }
    }
    /// <summary>
    /// 点击话筒
    /// </summary>
    /// <param name="cobj"></param>
    /// 
    void ClickmicroPhoneTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoLsSpeak, ShowSpeakContent);

        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
    }
    void RedoLsSpeak()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        CancelInvoke("ClickmicroPhoneTip");
        Invoke("ClickmicroPhoneTip", 2);
    }
    void SpeakBtnClickCallback()
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
        LsBack();

    }
    /// <summary>
    /// 老师收手
    /// </summary>
    void LsBack()
    {
        LS.Complete += () =>
        {
            GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
            ClickLsHandTip();
        };
        LS.OnContinue();

    }
    void ClickLsCallBack(ClickedObj cobj)
    {
        Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
        }
    }
    void ClickLsHandTip()
    {
        if (jshand == null)
        {
            jshand = LS.transform.Find("LSB_BD/shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoLsPointJudai, LsPointJudai);
    }
    void RedoLsPointJudai()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOn(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师指句带");
        CancelInvoke("ClickLsHandTip");
        Invoke("ClickLsHandTip", 2);
    }
    void LsPointJudai()
    {
        CancelInvoke("ClickLsHandTip");
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        LS.Complete += LsPointJudaiCallback;
        LS.PlayForward("LS_E_1ST_ZBZK");
    }
    /// <summary>
    /// 教师接收图卡回调
    /// </summary>
    void LsPointJudaiCallback()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        ClickDispatcher.Inst.EnableClick = true;
        ClickFdlsHandTip();
    }
    void ClickFdlsHandTip()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickFdlsHand, FdlsClickXhHand);
        ClickDispatcher.Inst.EnableClick = true;
        if (fdlshand == null)
        {
            fdlshand = FDLS.transform.Find("FDLS/fdls_shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(fdlshand);
    }
    void RedoClickFdlsHand()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要辅助教师协助");
        CancelInvoke("ClickFdlsHandTip");
        Invoke("ClickFdlsHandTip", 2);
    }
    //点中辅导老师手后的回调
    /// <summary>
    ///辅导教师抓住小华的手翻开沟通本
    /// </summary>
    void FdlsClickXhHand()
    {
        CancelInvoke("ClickFdlsHandTip");
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        ClickDispatcher.Inst.EnableClick = false;
        //FDLS.Complete += FdlsClickXhHandCalllback;
        //FDLS.PlayForward("FDLS_A_2ND_D");//TODO:教师动画播放时有位移
        FDLS.PlayForward("FDLS_B_1ST_FGTB");

        //Debug.LogError("FDLS_B_1ST_FGTB");

        bool pass = true;
        FDLS.timePointEvent = (a) =>
        {
            if (a > 41 && a <= 44 && pass)
            {
                pass = false;
                //               
                bool passxh = true;
                XH.timePointEvent = (b) =>
                {
                    if (b > 184 && b < 189 && passxh)
                    {
                        passxh = false;
                        XH.OnPause();

                        XhTakeCardCallback();
                    }
                };
                //XH.Complete += XhTakeCardCallback;
                XH.PlayForward("XH_D_1ST_FBNKT");
                //
                bool passgtb = true;
                gtb.framePointEvent = (b) =>
                {
                    if (b > 175 && b < 180 && passgtb)
                    {
                        passgtb = false;
                        gtb.OnPause();
                    }
                };
                gtb.PlayForward("XH_D_1ST_FBNKT_KA");
            }
        };

    }
    /// <summary>
    /// 小华拿卡递卡回调
    /// </summary>
    void XhTakeCardCallback()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ClickDispatcher.Inst.EnableClick = true;
        ClickLsHandJiekaTip();
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

        bool passJG = true;
        LS.timePointEvent = (a) =>//老师借卡时间点
        {
            if (a > 19 && a < 23 && passJG)
            {
                //Debug.LogError("event");
                passJG = false;
                XH.OnContinue();//小华手收回
                transform.Find("XH_D_1ST_FBNKT_KA/XH_judaiA").gameObject.SetActive(false);//沟通本图卡隐藏
            }
        };

        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/TY_LS_JTKJD_KA").GetLegacyAnimationOper();//跟随老师句带移动卡片
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
        string curObjName = SentenceExpressionModel.GetInstance().CurneutralStimulator.pData.name_cn;
        dlog.SetDialogMessage("是的，小华看见了" + curObjName + "，小华表现得很好!");
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
        tip.SetTipMessage("需要教师给相应物品");
        CancelInvoke("ClickLsGiveObjTip");
        Invoke("ClickLsGiveObjTip", 2);
    }
    void LsGiveObj()
    {
        CancelInvoke("ClickLsGiveObjTip");
        Debug.Log("教师给物品");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);

        bool pass = true;
        bool passXh = true;
        Transform qhw = transform.Find("QHW");
        LS.timePointEvent = (a) =>//老师递给物品
        {
            if (a > 25 && a < 30 && pass)//挂载到老师手上强化物时间点
            {
                pass = false;
                LSCtrl lsctrl = LS.GetComponent<LSCtrl>();//将当前强化物挂在老师手上    
                lsctrl.SetJoint(qhw.gameObject);
                //Debug.LogError("ls");
            }

            if (a > 18 && a < 22 && passXh)//小华接卡动画播放延迟
            {
                passXh = false;
                XH.Complete += XHJiewuCallback;
                XH.PlayForward("TY_XH_JG");
            }
        };

        LS.Complete += LsGiveObjCallback;
        LS.PlayForward("TY_LS_DW");

        bool passJG = true;
        XH.timePointEvent = (a) =>//小华接过物品 挂载强化物
        {
            if (a > 40 && a < 45 && passJG)
            {
                passJG = false;
                XHCtrl xhCtrl = XH.GetComponent<XHCtrl>();
                xhCtrl.SetJoint(qhw.gameObject);
                //Debug.LogError("xh");
            }
        };
    }
    void LsGiveObjCallback()
    {
        ShowFinalUI();
    }
    void XHJiewuCallback()
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
        swapUI.speakEvent -= SpeakBtnClickCallback;
        com = null;

        LS.Complete -= LsGiveObjCallback;
        XH.Complete -= XHJiewuCallback;

        XH.timePointEvent = null;
        LS.timePointEvent = null;

        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
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
