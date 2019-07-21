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
        GameObject.Find("jiaoshi").gameObject.SetActive(false);
        PeopleManager.Instance.gameObject.SetActive(false);

        GameObject market = ResManager.GetPrefab("Scenes/supermarket/chaoshi");
        market.transform.SetParent(transform);
        market.name = "chaoshi";

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
        //XH.transform.localPosition = Vector3.zero;
        //XH.transform.localScale = Vector3.zero;

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
        PropsObject pObj = ObjectsManager.instanse.GetProps(Random.Range(101, 1001) % 3);//强化物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        AcceptQuestionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject  " + rfc.pData.name);

        Transform panzi = transform.Find("chaoshi/chaoshi_sw/panzi");
        for (int i = 0; i < panzi.childCount; i++)
        {
            panzi.GetChild(i).gameObject.SetActive(false);
        }
        panzi.Find(rfc.pData.name).gameObject.SetActive(true);

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
        ka.name = "XH_E_3RD_FNN_KA";
        ka.PlayForward("XH_E_3RD_FNN_KA");

        Material matSource = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai1/tuka10").GetComponent<MeshRenderer>().materials[1];//物品图卡
        Reinforcement rfc = AcceptQuestionModel.GetInstance().CurReinforcement;
        Material matTar = AcceptQuestionModel.GetInstance().GetTuKa("tuka_" + rfc.pData.name).GetComponent<MeshRenderer>().materials[1];
        matSource.CopyPropertiesFromMaterial(matTar);//更换图卡物体材质
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
        float st = 1.7f;
        float et = 1.73f;
        MM.timePointEvent = (a) =>//mama借卡时间点
        {
            Debug.Log(a);
            if (a >= st && a < et)
            {
                Debug.LogError("event");
                transform.Find("XH_E_3RD_FNN_KA").gameObject.SetActive(false);//沟通本图卡隐藏
                                                                              //XH.PlayForward("XH_D_1ST_BACK");//小华手收回
            }
        };
        MM.Complete += LsJiekaCallback;
        MM.PlayForward("MM_E_3RD_JG");

        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_E_3RD_JG_KA").GetLegacyAnimationOper();
        ka.transform.SetParent(transform);
        ka.name = "MM_E_3RD_JG_KA";
        Material matSource = ka.transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/judai2/tuka10").GetComponent<MeshRenderer>().materials[1];
        Reinforcement rfc = AcceptQuestionModel.GetInstance().CurReinforcement;
        Material matTar = AcceptQuestionModel.GetInstance().GetTuKa("tuka_" + rfc.pData.name).GetComponent<MeshRenderer>().materials[1];
        matSource.CopyPropertiesFromMaterial(matTar);//更换图卡物体材质
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
        CancelInvoke("ClickmicroPhoneJiekaTip");
        Invoke("ClickmicroPhoneJiekaTip", 2);
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

        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_E_3RE_DY_KA").GetLegacyAnimationOper();//mm手中卡显示
        ka.transform.SetParent(transform);
        ka.name = "MM_E_3RE_DY_KA";
        Transform par = ka.transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R");
        for (int i = 0; i < par.childCount; i++)
        {
            par.GetChild(i).gameObject.SetActive(false);
        }
        par.Find(AcceptQuestionModel.GetInstance().CurReinforcement.pData.name).gameObject.SetActive(true);
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
        Destroy(gameObject);
        GameObject.Find("jiaoshi").gameObject.SetActive(false);
        PeopleManager.Instance.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {

    }
}
