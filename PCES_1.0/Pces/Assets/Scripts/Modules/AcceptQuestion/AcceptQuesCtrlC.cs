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
    GameObject qhw;
    private void Awake()
    {
        this.name = "AcceptQuesCtrlC";
    }
    //public bool Finished;
    private void Start()
    {
        GlobalEntity.GetInstance().Dispatch<string>(CommonUI.pEvent.LevelChange, "第三关");

        AcceptQuestionModel.GetInstance().Jiaoshi().SetActive(false);

        GameObject market = ResManager.GetPrefab("Scenes/supermarket/chaoshi");
        market.transform.SetParent(transform);
        market.name = "chaoshi";

        Camera cam = transform.GetComponentInChildren<Camera>();
        ClickDispatcher.Inst.cam = cam;
        //HighlightingEffect hf = cam.GetComponent<HighlightingEffect>();
        //if (hf == null)
        //{
        //    hf = cam.gameObject.AddComponent<HighlightingEffect>();
        //    //hf.stencilZBufferDepth = 0;
        //    //hf.downsampleFactor = 1;
        //    //hf.iterations = 2;
        //}
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

        PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).transform.localPosition = new Vector3(0, 0, 10000);
        PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).transform.localPosition = new Vector3(0, 0, 10000);

        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        XH.PlayForward("XH_E_3RD_FNN");
        XH.OnPause();

        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/XH_E_3RD_FNN_KA").GetLegacyAnimationOper();
        ka.transform.SetParent(transform);
        ka.name = "XH_E_3RD_FNN_KA";
        ka.PlayForward("XH_E_3RD_FNN_KA");
        ka.OnPause();
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
        PropsObject pObj = AcceptQuestionModel.GetInstance().GetIndex(new List<int> { 0, 2 }).GetComponent<PropsObject>();//强化物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        AcceptQuestionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject  " + rfc.pData.name);

        Transform panzi = transform.Find("chaoshi/chaoshi_sw/panzi");
        GameObject panziGo = panzi.gameObject;
        PanziCtrl pc = panziGo.GetComponent<PanziCtrl>();
        if (pc == null)
        {
            pc = panziGo.AddComponent<PanziCtrl>();
        }

        for (int i = 0; i < panzi.childCount; i++)
        {
            panzi.GetChild(i).gameObject.SetActive(false);
        }
        qhw = panzi.Find(rfc.pData.name).gameObject;
        qhw.SetActive(true);

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
        tip.SetTipMessage("需要妈妈说话");
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
        dlog.transform.localPosition = new Vector3(475, 196, 0);
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
        XH.OnContinue();
        Debug.Log("continue");

        LegacyAnimationOper ka = transform.Find("XH_E_3RD_FNN_KA").gameObject.GetLegacyAnimationOper();

        Transform tk9 = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai1/tuka9");
        Material mat = tk9.GetComponent<MeshRenderer>().materials[1];
        Reinforcement rfc = AcceptQuestionModel.GetInstance().CurReinforcement;
        Material matTar = AcceptQuestionModel.GetInstance().GetTuKa("tuka_" + rfc.pData.name).GetComponent<MeshRenderer>().materials[1];
        mat.CopyPropertiesFromMaterial(matTar);
        tk9.localEulerAngles = Vector3.zero;

        //Material matWy = AcceptQuestionModel.GetInstance().GetTuKa(PropsTag.judai_woyao.ToString()).GetComponent<MeshRenderer>().materials[1];
        Material matSource = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/tuka8").GetComponent<MeshRenderer>().materials[1];//我要图卡       
        matSource.CopyPropertiesFromMaterial(matTar);//更换我要图卡物体材质

        //bool pass = true;
        ka.Complete = () =>
        {
            ka.Complete = null;
            Vector3 tkeular = new Vector3(0, -90, 0);
            tk9.localEulerAngles = tkeular;
            Debug.LogError(tk9.localEulerAngles);
        };
        ka.OnContinue();
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
            ClickDispatcher.Inst.EnableClick = false;
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
        //ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(mmhand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈接卡");
        CancelInvoke("ClickLsHandJiekaTip");
        Invoke("ClickLsHandJiekaTip", 2);
    }
    void LsJieka()
    {
        CancelInvoke("ClickLsHandJiekaTip");
        HighLightCtrl.GetInstance().FlashOff(mmhand);
        ClickDispatcher.Inst.EnableClick = false;
        MM.OnContinue();
        MM.timePointEvent = (a) =>//mama借卡时间点
        {
            if (a >= 41 && a <= 43)//给定一个帧区间范围
            {
                MM.timePointEvent = null;

                Transform fnnka = transform.Find("XH_E_3RD_FNN_KA");
                fnnka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai1").gameObject.SetActive(false);
                //沟通本图卡隐藏
                XHCtrl xhctrl = XH.GetComponent<XHCtrl>();
                Transform xh_r1 = xhctrl.XH_R2.transform.parent.Find("XH_R1");
                fnnka.transform.SetParent(xh_r1);

                XH.PlayForward("XH_E_3RD_JG");
                XH.OnPause();
                //int stateHash = XH.anim.GetCurrentAnimatorStateInfo(0).tagHash;
                //float length = XH.anim.GetCurrentAnimatorStateInfo(0).length;
                //XH.anim.Play("XH_E_3RD_JG", 0, -length);            
                //XH.PlayForward("idle");
                //XH.anim.speed = -1;
                //XH.PlayForward("XH_E_3RD_JG");//小华手收回
            }
        };
        MM.Complete += LsJiekaCallback;
        MM.PlayForward("MM_E_3RD_JG");

        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_E_3RD_JG_KA").GetLegacyAnimationOper();
        ka.transform.SetParent(transform);
        ka.name = "MM_E_3RD_JG_KA";
        Transform tk9 = ka.transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/judai2/tuka9");
        Vector3 tk9Angle = tk9.localEulerAngles;
        tk9Angle.y = -90.3f;
        tk9.localEulerAngles = tk9Angle;

        Material matSource = ka.transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/judai2/tuka9").GetComponent<MeshRenderer>().materials[1];
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
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈说话");
        CancelInvoke("ClickmicroPhoneJiekaTip");
        Invoke("ClickmicroPhoneJiekaTip", 2);
    }
    void ShowSpeakJiekaContent()
    {
        CancelInvoke("ClickmicroPhoneJiekaTip");
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = AcceptQuestionModel.GetInstance().CurReinforcement.pData.name_cn;
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
        HighLightCtrl.GetInstance().FlashOn(mmhand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void RedoLsGiveObj()
    {
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈给相应物品");
        CancelInvoke("ClickLsGiveObjTip");
        Invoke("ClickLsGiveObjTip", 2);
        //ClickLsGiveObjTip();
    }
    void LsGiveObj()
    {
        CancelInvoke("ClickLsGiveObjTip");
        Debug.Log("妈妈接卡");
        transform.Find("MM_E_3RD_JG_KA").gameObject.SetActive(false);
        HighLightCtrl.GetInstance().FlashOff(mmhand);
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);

        MM.Complete += LsGiveObjCallback;
        MM.timePointEvent = (a) =>
        {
            if (a >= 120 && a <= 123)//
            {
                //Debug.LogError("MM");
                MM.timePointEvent = null;
                MMCtrl mctrl = MM.GetComponent<MMCtrl>();
                if (mctrl == null)
                {
                    mctrl = MM.gameObject.AddComponent<MMCtrl>();
                }
                //qhw.SetActive(false);
                //Debug.LogError(qhw.name);
                transform.Find("chaoshi/chaoshi_sw/panzi").GetComponent<PanziCtrl>().ShowObj(qhw.name);
                //mctrl.SetJoint(qhw);
                //qhw.transform.localPosition = Vector3.zero;
                //mctrl.ls_Lf.transform.localPosition = Vector3.zero;
            }
        };
        MM.PlayForward("MM_E_3RE_DY");

        LegacyAnimationOper ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_E_3RE_DY_KA").GetLegacyAnimationOper();//mm手中卡显示
        ka.transform.SetParent(transform);
        ka.name = "MM_E_3RE_DY_KA";
        Transform par = ka.transform.Find("Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/XH_R2");
        //for (int i = 0; i < par.childCount; i++)
        //{
        //    par.GetChild(i).gameObject.SetActive(false);
        //}
        Transform jd = par.Find("judai2");
        jd.gameObject.SetActive(true);
        Material tkmat = jd.Find("tuka9 1").GetComponent<MeshRenderer>().materials[1];
        //par.Find(AcceptQuestionModel.GetInstance().CurReinforcement.pData.name).gameObject.SetActive(true);

        Reinforcement rfc = AcceptQuestionModel.GetInstance().CurReinforcement;
        Material matSource = AcceptQuestionModel.GetInstance().GetTuKa("tuka_" + rfc.pData.name).GetComponent<MeshRenderer>().materials[1];
        tkmat.CopyPropertiesFromMaterial(matSource);

        bool passA = true;
        bool passB = true;
        ka.framePointEvent = (a) =>
        {
            if (a >= 90 && a <= 92 && passB)
            {
                //Debug.LogError("xh");
                passB = false;
                jd.gameObject.SetActive(false);
            }
            if (a >= 118 && a <= 120 && passA)
            {
                passA = false;
                ka.framePointEvent = null;
                //par.Find(rfc.pData.name).gameObject.SetActive(true);
                //qhw.SetActive(false);
                jd.gameObject.SetActive(false);
                XhQHW xhqhw = ka.GetComponent<XhQHW>();
                xhqhw.ShowObj(rfc.pData.name);
            }
        };
        ka.PlayForward("MM_E_3RE_DY_KA");
    }
    void LsGiveObjCallback()
    {
        XH.Complete += XHJiewuCallback;
        XH.OnContinue();
    }
    void XHJiewuCallback()
    {
        Debug.Log("xh给物品回调用");
        //XHCtrl xhctrl = XH.GetComponent<XHCtrl>();
        //xhctrl.SetJointL(qhw);
        //qhw.transform.localPosition = Vector3.zero;
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
        ResetGuaDian();
        HighLightCtrl.GetInstance().OffAllObjs();
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

        if (swapUI != null)
        {
            swapUI.speakEvent -= SpeakBtnClickCallback;
        }

        if (XH != null)
        {
            XH.Complete -= XHJiewuCallback;
            XH.timePointEvent = null;
        }
        if (MM != null)
        {
            MM.Complete -= LsGiveObjCallback;
            MM.timePointEvent = null;
        }

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
        if (XH != null)
        {
            XHCtrl xhctrl = XH.GetComponent<XHCtrl>();
            xhctrl.DestroyGuadian();
        }
        //LSCtrl lsctrl = LS.GetComponent<LSCtrl>();
        //lsctrl.DestroyGuadian();
    }
    public void RedoDispose()
    {
        RemoveAllListeners();
        evtFinished = null;
        evtRedo = null;
        Destroy(gameObject);
    }
    public void Dispose()
    {
        //RemoveAllListeners();
        Finish();
        evtFinished = null;
        evtRedo = null;
    }
    private void OnDestroy()
    {
        CancelInvoke();
    }
}

public class PanziCtrl : MonoBehaviour
{
    /// <summary>
    /// 控制盘子中的物体显示
    /// </summary>
    public void ShowObj(string goodsName)
    {
        Transform par = transform.Find(goodsName);
        switch (goodsName)
        {
            case "biscuit":
                for (int i = 23; i < 29; i++)
                {
                    par.Find("haitaibinggan" + i.ToString()).gameObject.SetActive(false);
                }
                break;
            case "chocolate":
                par.Find("qiaokeli3").gameObject.SetActive(false);
                break;
            case "chips":
                for (int i = 33; i < 38; i++)
                {
                    par.Find("shupian" + i.ToString()).gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}