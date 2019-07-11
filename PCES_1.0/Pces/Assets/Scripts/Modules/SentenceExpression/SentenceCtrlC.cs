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
    LegacyAnimationOper gtb;
    Transform objectsTr;
    private void Awake()
    {
        this.name = "SentenceCtrlC";
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
        LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();
        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        FDLS = PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).GetAnimatorOper();
        FDLS.gameObject.SetActive(false);
        LS.PlayForward("idle");
        XH.PlayForward("idle");

        //FDLS.PlayForward("idle");
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
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject");
        objectsTr = new GameObject("objectsParent").transform;
        objectsTr.localPosition = Vector3.zero;
        objectsTr.localScale = Vector3.one;
        objectsTr.rotation = Quaternion.identity;
        objectsTr.SetParent(transform);

        int objId = rfc.pData.id;//中性刺激物
        GameObject obj = Instantiate(pObj.gameObject);
        obj.name = ((PropsTag)objId).ToString();
        obj.transform.SetParent(objectsTr);
        PropsObject curObj = obj.GetComponent<PropsObject>();
        curObj.pData = pObj.pData;
        curObj.setPos(new Vector3(2.55f, 0.57f, -0.27f));//TODO:每个物体的位置缩放,角度,有待调整  
        curObj.transform.localScale = Vector3.one * 0.5f;
        curObj.transform.localEulerAngles = new Vector3(0, -90, 0);
        Debug.Log(curObj.pData.name_cn + "    " + curObj.pData.name);

        gtb = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_1ST_FBNKT_KA").GetLegacyAnimationOper();//沟通本
        gtb.name = PropsTag.TY_GTB.ToString();
        gtb.transform.SetParent(objectsTr);
        gtb.name = "goutongben";//沟通本更新在日志
        gtb.transform.SetParent(objectsTr);
        gtb.transform.localPosition = new Vector3(0f, 0, -0.373f);

        string _tuka = "tuka_" + ((PropsTag)objId).ToString();//沟通本中的涂卡沟通本
        GameObject deskTuka = Instantiate(SentenceExpressionModel.GetInstance().GetTuKa(_tuka));
        deskTuka.transform.SetParent(objectsTr);
        deskTuka.name = _tuka;
        PropsObject pot = deskTuka.GetComponent<PropsObject>();
        pot.setPos(new Vector3(2.18f, 0.57f, -0.001f));

        GameObject judai_woyao = Instantiate(SentenceExpressionModel.GetInstance().GetObj((int)PropsTag.judai_woyao));//我要
        judai_woyao.transform.SetParent(objectsTr);
        judai_woyao.transform.localEulerAngles = new Vector3(0, -90, 0);
        judai_woyao.transform.localPosition = new Vector3(2.32f, 0.546f, -0.177f);
        judai_woyao.name = PropsTag.judai_woyao.ToString();

        GameObject judai_wokanjian = Instantiate(SentenceExpressionModel.GetInstance().GetObj((int)PropsTag.judai_wokanjian));//我看见
        judai_wokanjian.transform.SetParent(objectsTr);
        judai_wokanjian.transform.localEulerAngles = new Vector3(0, -90, 0);
        judai_wokanjian.transform.localPosition = new Vector3(2.214f, 0.546f, -0.177f);
        judai_wokanjian.name = PropsTag.judai_wokanjian.ToString();
        XhTJudai();
    }
    void XhTJudai()
    {
        ClickDispatcher.Inst.EnableClick = false;
        gtb.PlayForward("XH_D_1ST_FBNKT_KA");
        XH.PlayForward("XH_D_1ST_FBNKT");
        //GTB.PlayForward("onePaper");
        XH.Complete += XhTakeCardCallback;
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
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOn(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        CancelInvoke("ClickLsHandTip");
        Invoke("ClickLsHandTip", 2);
    }
    void LsJieka()
    {
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        LS.Complete += LsJiekaCallback;
        LS.PlayForward("TY_LS_JK");
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
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        CancelInvoke("ClickmicroPhoneTip");
        Invoke("ClickmicroPhoneJiekaTip", 2);
    }
    void ShowSpeakJiekaContent()
    {
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("小华要" + curObjName);
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
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师给相应物品");
        ClickLsGiveObjTip();
    }
    void LsGiveObj()
    {
        Debug.Log("教师给物品");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        LS.Complete += LsGiveObjCallback;
        LS.PlayForward("TY_LS_DW");
        XH.Complete += XHJiewuCallback;
        XH.PlayForward("TY_XH_JG");
    }
    void LsGiveObjCallback()
    {
        //ShowFinalUI();
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
        swapUI.speakEvent -= SpeakBtnClickCallback;
        swapUI.speakEvent += KJSpeakBtnClickCallback;

        gtb = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_3RD_FBNKTK_KA").GetLegacyAnimationOper();//沟通本
        gtb.name = PropsTag.TY_GTB.ToString();
        gtb.transform.SetParent(objectsTr);
        gtb.name = "goutongben";//沟通本更新在日志
        gtb.transform.SetParent(objectsTr);
        gtb.transform.localPosition = new Vector3(0f, 0, -0.373f);

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
    ///小华贴句带
    /// </summary>
    void KJXhTJudai()
    {
        ClickDispatcher.Inst.EnableClick = false;
        XH.PlayForward("XH_D_3RD_FBNKTK");
        gtb.PlayForward("XH_D_3RD_FBNKTK_KA");
        //GTB.PlayForward("onePaper");
        XH.Complete += KBXhTakeCardCallback;
    }
    /// <summary>
    /// 小华拿卡递卡回调
    /// </summary>
    void KBXhTakeCardCallback()
    {
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
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
        ChooseDo.Instance.DoWhat(5, KJRedoLsJieka, KJLsJieka);
    }
    void KJRedoLsJieka()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOn(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        CancelInvoke("KJClickLsHandJiekaTip");
        Invoke("KJClickLsHandJiekaTip", 2);
    }
    void KJLsJieka()
    {
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        LS.Complete += KJLsJiekaCallback;
        LS.PlayForward("TY_LS_JK");
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
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师说话");
        CancelInvoke("ClickmicroPhoneTip");
        Invoke("ClickmicroPhoneTip", 2);
    }
    void KJShowSpeakJiekaContent()
    {
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("是的，你看见了" + curObjName + "，你表现得很好!");
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
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void KJRedoLsGiveObj()
    {
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师给相应物品");
        CancelInvoke("KJClickLsGiveObjTip");
        Invoke("KJClickLsGiveObjTip", 2);
    }
    void KJLsGiveObj()
    {
        Debug.Log("教师给物品");
        HighLightCtrl.GetInstance().FlashOff(jshand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        LS.Complete += KJLsGiveObjCallback;
        LS.PlayForward("TY_LS_DW");
        XH.Complete += KJXHJiewuCallback;
        XH.PlayForward("TY_XH_JG");
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
        RemoveAllListeners();
    }
    void NextDo()
    {
        Finish();
        evtFinished();
    }
    void RemoveAllListeners()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.redoClickEvent -= NextDo;
        com.redoClickEvent -= ReDo;
        //swapUI.speakEvent -= SpeakBtnClickCallback;
    }
    void ReDo()
    {
        Debug.Log("redo");
        Finish();
        evtRedo();
    }
    public void Dispose()
    {
        RemoveAllListeners();
        Destroy(gameObject);
    }
    private void OnDestroy()
    {

    }

}
