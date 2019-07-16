using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptQuesCtrlC : MonoBehaviour
{
    public event System.Action evtFinished;
    public event System.Action evtRedo;
    SwapUI swapUI;
    SelectionUI selectUI;
    GameObject fdlshand;
    GameObject mmhand;
    AnimationOper MM;
    AnimationOper XH;
    //AnimationOper GTB;//沟通本
    LegacyAnimationOper gtb;
    private void Awake()
    {
        this.name = "AcceptQuesCtrlC";
    }
    //public bool Finished;
    private void Start()
    {
        GameObject.Find("model_root").transform.GetChild(0).gameObject.SetActive(false);
        GameObject market = ResManager.GetPrefab("Scenes/supermarket/chaoshi");
        market.transform.SetParent(transform);
        Camera cam = transform.GetComponentInChildren<Camera>();
        ClickDispatcher.Inst.cam = cam;
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
        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        XH.transform.SetParent(transform);
        XH.transform.localPosition = new Vector3(2.224f, 0, 0);
        XH.transform.localEulerAngles = new Vector3(0, -154.7f, 0);
        XH.PlayForward("idle");

        MM = ResManager.GetPrefab("Scenes/supermarket/MM").GetAnimatorOper();
        MM.name = "MM";
        MM.transform.SetParent(transform);
        MM.PlayForward("idle");

        HighLightCtrl.GetInstance().OffAllObjs();
        GetTukaObject();
    }
    /// <summary>
    /// 初始化桌子上的涂卡
    /// </summary>
    void GetTukaObject()
    {
        PropsObject pObj = AcceptQuestionModel.GetInstance().GetObj(PropsType.reinforcement);//强化物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        AcceptQuestionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject");

        Transform objectsTr = new GameObject("objectsParent").transform;
        objectsTr.localPosition = Vector3.zero;
        objectsTr.localScale = Vector3.one;
        objectsTr.rotation = Quaternion.identity;
        objectsTr.SetParent(transform);

        int objId = rfc.pData.id;//强化物
        GameObject obj = Instantiate(pObj.gameObject);
        obj.name = ((PropsTag)objId).ToString();
        obj.transform.SetParent(objectsTr);
        PropsObject curObj = obj.GetComponent<PropsObject>();
        curObj.pData = pObj.pData;
        curObj.setPos(new Vector3(2.55f, 0.57f, -0.27f));//TODO:每个物体的位置缩放,角度,有待调整  
        curObj.transform.localScale = Vector3.one * 0.5f;
        Debug.Log(curObj.pData.name_cn + "    " + curObj.pData.name);

        gtb = ResManager.GetPrefab("Prefabs/AnimationKa/XH_D_2ND_FYFT_KA").GetLegacyAnimationOper(); //通用沟通本
        gtb.name = PropsTag.TY_GTB.ToString();
        gtb.transform.SetParent(objectsTr);
        gtb.name = "goutongben";
        gtb.transform.SetParent(objectsTr);
        gtb.transform.localPosition = new Vector3(-0.048f, 0, -0.373f);

        string _tuka = "tuka_" + ((PropsTag)objId).ToString();//图卡
        GameObject deskTuka = Instantiate(AcceptQuestionModel.GetInstance().GetTuKa(_tuka));
        deskTuka.transform.SetParent(objectsTr);
        deskTuka.name = _tuka;
        PropsObject pot = deskTuka.GetComponent<PropsObject>();
        pot.setPos(new Vector3(2.2878f, 0.57f, 0.0113f));

        //GameObject judai = Instantiate(AcceptQuestionModel.GetInstance().GetObj((int)PropsTag.judai_woyao));
        //judai.transform.SetParent(objectsTr);
        //judai.transform.localEulerAngles = new Vector3(0, -90, 0);
        //judai.transform.localPosition = new Vector3(2.27f, 0.546f, -0.177f);
        //judai.name = PropsTag.judai_woyao.ToString();
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
        ClickDispatcher.Inst.EnableClick = false;
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
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        dlog.SetDialogMessage("小华要什么");
        MM.Complete += MMAskQuesCallback;
        MM.PlayForward("MM_E_3RD_W");
    }
    /// <summary>
    /// 妈妈询问小华回到动画
    /// </summary>
    void MMAskQuesCallback()
    {
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);
        XhTzk();
    }
    /// <summary>
    /// 小华贴字卡
    /// </summary>
    void XhTzk()
    {
        XH.transform.localPosition = Vector3.zero;
        XH.transform.localEulerAngles = Vector3.zero;
        XH.Complete += XhTzkCallback;
        XH.PlayForward("XH_E_3RD_FNN");
        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/XH_E_3RD_FNN_KA").GetLegacyAnimationOper();
        ka.transform.SetParent(transform);
        ka.PlayForward("XH_E_3RD_FNN_KA");
    }
    void XhTzkCallback()
    {
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ClickLsHandJiekaTip();
    }
    void ClickLsCallBack(ClickedObj cobj)
    {
        Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "click")
        {
            ChooseDo.Instance.Clicked();
        }
    }
    void ClickLsHandJiekaTip()
    {
        if (mmhand == null)
        {
            mmhand = MM.transform.Find("mama/mama_shenti").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(mmhand);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoLsJieka, LsJieka);
    }
    void RedoLsJieka()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(mmhand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        CancelInvoke("ClickLsHandJiekaTip");
        Invoke("ClickLsHandJiekaTip", 2);
    }
    void LsJieka()
    {
        HighLightCtrl.GetInstance().FlashOff(mmhand);
        ClickDispatcher.Inst.EnableClick = false;
        MM.Complete += LsJiekaCallback;
        MM.PlayForward("MM_E_3RD_JG");
        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_E_3RD_JG_KA").GetLegacyAnimationOper();
        ka.transform.SetParent(transform);
        ka.PlayForward("MM_E_3RD_JG_KA");
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
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈说话");
        CancelInvoke("ClickmicroPhoneTip");
        Invoke("ClickmicroPhoneTip", 2);
    }
    void ShowSpeakJiekaContent()
    {
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = AcceptQuestionModel.GetInstance().CurReinforcement.pData.name_cn;
        string behaveMode = "吃";
        if (curObjName.Equals("小汽车"))
        {
            behaveMode = "玩";
        }
        dlog.SetDialogMessage("你要" + behaveMode + curObjName);
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
        HighLightCtrl.GetInstance().FlashOn(mmhand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void RedoLsGiveObj()
    {
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈给相应物品");
        ClickLsGiveObjTip();
    }
    void LsGiveObj()
    {
        Debug.Log("妈妈给物品");
        HighLightCtrl.GetInstance().FlashOff(mmhand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        MM.Complete += LsGiveObjCallback;
        MM.PlayForward("MM_E_3RE_DY");
        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_E_3RE_DY_KA").GetLegacyAnimationOper();
        ka.transform.SetParent(transform);
        ka.PlayForward("MM_E_3RE_DY_KA");
    }
    void LsGiveObjCallback()
    {
        XH.Complete += XHJiewuCallback;
        XH.PlayForward("XH_E_3RD_JG");
        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/XH_E_3RD_JG_KA").GetLegacyAnimationOper();
        ka.transform.SetParent(transform);
        ka.PlayForward("XH_E_3RD_JG_KA");
    }
    void XHJiewuCallback()
    {
        Debug.Log("xh给物品回调用");
        ShowFinalUI();
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
        swapUI.speakEvent -= SpeakBtnClickCallback;
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
        //Destroy(gameObject);
    }
    private void OnDestroy()
    {

    }
}
