using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptQuesCtrlA : MonoBehaviour
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
    LegacyAnimationOper gtb;
    QHWCtrl qhwCtrl;
    private void Awake()
    {
        this.name = "AcceptQuesCtrlA";
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
            swapUI.speakEvent += SpeakBtnClickCallback;
            swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
            swapUI.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
        }

        PeopleManager.Instance.Reset();

        LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();
        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        FDLS = PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).GetAnimatorOper();
        LS.PlayForward("idle");
        XH.PlayForward("idle");
        //FDLS.PlayForward("idle");
        FDLS.transform.localPosition = new Vector3(0, 0, 10000);

        ResetGuaDian();
        HighLightCtrl.GetInstance().OffAllObjs();
        GetTukaObject();
    }
    /// <summary>
    /// 初始化桌子上的涂卡
    /// </summary>
    void GetTukaObject()
    {
        //随机得到一个强化物
        PropsObject pObj = AcceptQuestionModel.GetInstance().GetObj(PropsType.reinforcement);//强化物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        AcceptQuestionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject  " + rfc.pData.name);

        Transform objectsTr = new GameObject("objectsParent").transform;
        objectsTr.localPosition = Vector3.zero;
        objectsTr.localScale = Vector3.one;
        objectsTr.rotation = Quaternion.identity;
        objectsTr.SetParent(transform);

        //设置老师旁边的强化物模型
        string objName = rfc.pData.name;//强化物
        GameObject obj = ObjectsManager.instanse.GetQHW();
        obj.name = "QHW";
        obj.transform.SetParent(objectsTr, false);
        qhwCtrl = obj.GetComponent<QHWCtrl>();
        qhwCtrl.ShowObj(objName);

        //PropsObject curObj = obj.GetComponent<PropsObject>();
        //curObj.pData = pObj.pData;
        //curObj.setPos(new Vector3(2.5328F, 0.5698F, -0.118F));//TODO:每个物体的位置缩放,角度,有待调整  
        //curObj.transform.localScale = Vector3.one;
        //Debug.Log(curObj.pData.name_cn + "    " + curObj.pData.name);

        gtb = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_1ST_FBNKT_KA").GetLegacyAnimationOper(); //沟通本   
        gtb.name = "XH_D_1ST_FBNKT_KA";
        gtb.transform.SetParent(transform);
        //gtb.transform.localPosition = new Vector3(-0.048f, 0, -0.373f);

        Transform tkPar = gtb.transform.Find("XH_judaiA/XH_judaiA 1/tukaB");

        string _tuka = "tuka_" + rfc.pData.name;//沟通本里面图卡        
        GameObject tkSource = AcceptQuestionModel.GetInstance().GetTuKa(_tuka);
        //Debug.Log(tkSource);
        Material[] matsSour = tkSource.GetComponent<MeshRenderer>().materials;
        Transform tkGtb = tkPar.Find("tukaB 1");
        Material[] matsTar = tkGtb.GetComponent<MeshRenderer>().materials;
        matsTar[1].CopyPropertiesFromMaterial(matsSour[1]);
        //Debug.Log(_tuka);

        tkGtb = tkPar.Find("tukaB1");
        matsTar = tkGtb.GetComponent<MeshRenderer>().materials;
        matsSour = AcceptQuestionModel.GetInstance().GetObj((int)PropsTag.judai_woyao).GetComponent<MeshRenderer>().materials;
        matsTar[1].CopyPropertiesFromMaterial(matsSour[1]);

        ClickmicroPhoneTip();

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
            ClickDispatcher.Inst.EnableClick = false;
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
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        ChooseDo.Instance.Clicked();
    }
    void ShowSpeakContent()
    {
        CancelInvoke("ClickmicroPhoneTip");
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        dlog.SetDialogMessage("小华要什么");
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ClickDispatcher.Inst.EnableClick = true;
        ClickLsHandTip();
    }
    void ClickLsCallBack(ClickedObj cobj)
    {
        Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "shou")
        {
            ClickDispatcher.Inst.EnableClick = false;
            ChooseDo.Instance.Clicked();
        }
    }
    void ClickLsHandTip()
    {
        UIManager.Instance.GetUI<Dialog>("Dialog").HideCtrl(1);
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
        HighLightCtrl.GetInstance().FlashOff(jshand);
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
        XhTJudai();
    }
    //点中辅导老师手后的回调
    /// <summary>
    ///辅导教师抓住小华的手翻开沟通本
    /// </summary>
    void XhTJudai()
    {
        ClickDispatcher.Inst.EnableClick = false;
        XH.PlayForward("XH_D_1ST_FBNKGK");
        gtb.PlayForward("XH_D_1ST_FBNKT_GKA");
        //GTB.PlayForward("onePaper");
        XH.Complete += XhTakeCardCallback;
    }
    /// <summary>
    /// 小华拿卡递卡回调
    /// </summary>
    void XhTakeCardCallback()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
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
        LS.PlayForward("TY_LS_JTKJD");

        LS.timePointEvent = (a) =>//老师借卡时间点
        {
            if (a >= 19 && a < 22)
            {
                LS.timePointEvent = null;
                transform.Find("XH_D_1ST_FBNKT_KA/XH_judaiA").gameObject.SetActive(false);//沟通本图卡隐藏
                XH.PlayForward("XH_D_1ST_BACK");//小华手收回
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
        string curObjName = AcceptQuestionModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("你要" + curObjName);
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

        LS.timePointEvent = (a) =>//老师递给物品
        {
            if (a >= 25 && a <= 27)//挂载到老师手上强化物时间点
            {
                LS.timePointEvent = null;
                LSCtrl lsctrl = LS.GetComponent<LSCtrl>();//将当前强化物挂在老师手上    
                lsctrl.SetJoint(qhwCtrl.gameObject);
                //qhwCtrl.SetPos();
                //Debug.LogError("ls");
            }

            if (a >= 21 && a < 24)//小华接卡动画播放延迟一边挂载强化物
            {
                XH.Complete += XHJiewuCallback;
                XH.timePointEvent = (b) =>//小华接过物品
                {
                    if (b >= 40 && b < 43)
                    {
                        XH.timePointEvent = null;
                        XHCtrl xhCtrl = XH.GetComponent<XHCtrl>();
                        xhCtrl.SetJoint(qhwCtrl.gameObject);
                    }
                };
                XH.PlayForward("TY_XH_JG");
            }
        };

        LS.Complete += LsGiveObjCallback;
        LS.PlayForward("TY_LS_DW");
    }
    void LsGiveObjCallback()
    {
        ShowFinalUI();
    }
    void XHJiewuCallback()
    {
        Debug.Log("xh给物品回调");
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
    void ResetGuaDian()
    {
        XHCtrl xhctrl = XH.GetComponent<XHCtrl>();
        xhctrl.DestroyGuadian();

        LSCtrl lsctrl = LS.GetComponent<LSCtrl>();
        lsctrl.DestroyGuadian();
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
        com.nextClickEvent -= NextDo;
        com.redoClickEvent -= ReDo;
        com = null;

        LS.timePointEvent = null;
        XH.timePointEvent = null;
        //FDLS.timePointEvent = null;
        swapUI.speakEvent -= SpeakBtnClickCallback;
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
    }
    void ReDo()
    {
        Finish();
        if (evtRedo != null)
        {
            evtRedo();
        }
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
