using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCtrlA : MonoBehaviour
{
    public event System.Action evtFinished;
    public event System.Action evtRedo;
    SwapUI swapUI;
    SelectionUI selectUI;
    GameObject choosebtn;
    GameObject fdlshand;
    GameObject jshand;
    GameObject lsObject;
    GameObject xhObject;
    AnimationOper LS;
    AnimationOper XH;
    AnimationOper FDLS;
    Tick tick;
    Transform objectsTr;
    Stage curStage;
    private void Awake()
    {
        this.name = "SwapA";
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
            swapUI.chooseEvent += ChooseBtnClickCallback;
            swapUI.speakEvent += SpeakBtnClickCallback;
        }
        if (selectUI == null)
        {
            selectUI = UIManager.Instance.GetUI<SelectionUI>("selectionUI");
            selectUI.okEvent += SelectUIOkBtnCallback;
        }
        UIManager.Instance.SetUIDepthTop("selectionUI");
        if (tick == null)
        {
            tick = new GameObject("tick").AddComponent<Tick>();
            tick.transform.SetParent(transform);
        }
        choosebtn = swapUI.GetChooseBtn;
        LS = PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).GetAnimatorOper();
        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        FDLS = PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).GetAnimatorOper();
        objectsTr = new GameObject("objectsParent").transform;
        objectsTr.localPosition = Vector3.zero;
        objectsTr.localScale = Vector3.one;
        objectsTr.rotation = Quaternion.identity;
        Tip();
    }
    /// <summary>
    /// 提示点击强化物按钮
    /// </summary>    
    void Tip()
    {
        UIFlah chooeFlash = choosebtn.gameObject.GetUIFlash();
        chooeFlash.StartFlash();
    }
    /// <summary>
    /// 选择强化物按钮点击回调
    /// </summary>
    void ChooseBtnClickCallback()
    {
        UIFlah uf = choosebtn.gameObject.GetUIFlash();
        uf.StopFlash();
        selectUI.gameObject.SetActive(true);
        //对当前场景做一些处理
    }
    void SpeakBtnClickCallback()
    {
        //TODO：显示老师说话内容             
    }
    void SelectUICloseCallback()
    {

    }
    /// <summary>
    /// 选择强化物界面ok按钮回调用
    /// </summary>  
    void SelectUIOkBtnCallback(int selectObj)
    {
        //老师手上显示物品
        lsObject = Instantiate(SwapModel.GetInstance().GetObj(selectObj));
        lsObject.transform.SetParent(objectsTr);
        PropsObject po = lsObject.GetComponent<PropsObject>();
        po.setPos(new Vector3(2.55f, 0.57f, -0.11f));//TODO:每个物体的位置有待调整        
        lsObject.name = ((PropsTag)selectObj).ToString();
        //小华桌子上面涂卡
        string tuka = "tuka_" + ((PropsTag)selectObj).ToString();
        GameObject deskTuka = Instantiate(SwapModel.GetInstance().GetTuKa(tuka));
        deskTuka.transform.SetParent(objectsTr);
        deskTuka.name = tuka;
        po = deskTuka.GetComponent<PropsObject>();
        po.setPos(new Vector3(2.18f, 0.57f, -0.001f));
        //播放小华抢东西动画 
        XH.Complete += SnatchXh;
        XH.PlayForward("TY_XH_QIANG");
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
    #region 小华抓手拿卡
    /// <summary>
    /// 小华抢东西动画回调
    /// </summary>
    void SnatchXh()
    {
        //计时开始     
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        ChooseDo.Instance.DoWhat(5, RedoClickFdlsHand, FdlsDragHandTakeCard);
        ClickFdlsHand();
    }
    void ClickFdlsHand()
    {
        ClickDispatcher.Inst.EnableClick = true;
        if (fdlshand == null)
        {
            fdlshand = FDLS.transform.Find("FDLS/fdls_shou").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(fdlshand);
    }
    void RedoClickFdlsHand()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要辅导老师协助");
        Invoke("ClickFdlsHand", 2);
    }
    //点中辅导老师手后的回调
    /// <summary>
    ///辅导老师抓手拿卡
    /// </summary>
    void FdlsDragHandTakeCard()
    {
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        ClickDispatcher.Inst.EnableClick = false;
        FDLS.Complete += FdlsDragHandTakeCardCallback;
        FDLS.PlayForward("FDLS_A_1ST_ZD");
    }
    /// <summary>
    /// 辅导老师抓手拿卡回调
    /// </summary>
    void FdlsDragHandTakeCardCallback()
    {
        ChooseDo.Instance.DoWhat(5, RedoFdlsDika, FdlsDika);//
        ClickFdlsDikaHandTip();
    }
    #endregion
    #region 辅导老师抓手递卡
    void ClickFdlsDikaHandTip()
    {
        HighLightCtrl.GetInstance().FlashOn(fdlshand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void RedoFdlsDika()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要辅导老师协助");
        Invoke("ClickFdlsDikaHandTip", 2);
    }
    /// <summary>
    /// 辅导老师抓小华手递卡
    /// </summary>
    void FdlsDika()
    {
        HighLightCtrl.GetInstance().FlashOff(fdlshand);
        ClickDispatcher.Inst.EnableClick = false;
        FDLS.Complete += FdlsDikaCallBack;
        FDLS.PlayForward("XH_A_1ST_BZDK");
    }
    //辅导老师抓手递卡回调
    void FdlsDikaCallBack()
    {
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickFdlsCallBack);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickLsCallBack);
        ChooseDo.Instance.DoWhat(5, RedoLsJieka, LsJieka);
        ClickLsHandTip();
    }
    void ClickLsCallBack(ClickedObj cobj)
    {
        //Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
        }
    }
    void ClickLsHandTip()
    {
        HighLightCtrl.GetInstance().FlashOn(jshand);
        ClickDispatcher.Inst.EnableClick = true;
    }
    void RedoLsJieka()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(jshand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        Invoke("ClickFdlsDikaHandTip", 2);
    }
    void LsJieka()
    {

    }
    #endregion
    #region  教师接收图卡

    #endregion
    //void RedoStageProcessing()
    //{
    //    ClickDispatcher.Inst.EnableClick = true;
    //    switch (curStage)
    //    {
    //        case Stage.getCard:
    //            //GetCardStage();
    //            break;
    //        case Stage.handingCard:
    //            break;
    //        case Stage.pickUpCard:
    //            break;
    //        case Stage.talk:
    //            break;
    //        case Stage.giveGoods:
    //            break;
    //        default:
    //            break;
    //    }
    //}
    //void FinishStageProcessing()
    //{
    //    ClickDispatcher.Inst.EnableClick = false;
    //    switch (curStage)
    //    {
    //        case Stage.getCard:
    //            GetCardAction();
    //            break;
    //        case Stage.handingCard:
    //            HandingCardAction();
    //            break;
    //        case Stage.pickUpCard:
    //            PickUpCardStage();
    //            break;
    //        case Stage.talk:
    //            TalkAcion();
    //            break;
    //        case Stage.giveGoods:
    //            GiveGoodsAcion();
    //            break;
    //        default:
    //            break;
    //    }
    //}
    /// <summary>
    /// 得到卡片
    /// </summary>
    //void GetCardStage()
    //{
    //    curStage = Stage.getCard;
    //    HighLightCtrl.GetInstance().FlashOn(fdlshand);
    //    ClickDispatcher.Inst.EnableClick = true;
    //    //计时开始       
    //    tick.TimingStart(5, TimeCallBack);
    //}
    //void HandingCardStage()
    //{
    //    curStage = Stage.handingCard;
    //    HighLightCtrl.GetInstance().FlashOn(fdlshand);
    //    ClickDispatcher.Inst.EnableClick = true;
    //    //计时开始       
    //    tick.TimingStart(5, TimeCallBack);
    //}
    //void PickUpCardStage()
    //{
    //    curStage = Stage.pickUpCard;
    //}
    //void TalkStage()
    //{
    //    curStage = Stage.talk;
    //}
    //void GivegoodsStage()
    //{
    //    curStage = Stage.giveGoods;
    //}
    #region 动画触发事件
    void GetCardAction()
    {
        //播放老师拿小华手拿卡动画
    }
    void GetCardActionCallBack()
    {
        //HandingCardStage();
    }
    void HandingCardAction()
    {
        //播放老师拿小华手拿卡动画
    }
    void HandingCardActionCallBack()
    {
        //PickUpCardStage();
    }
    void PickUpCardAction()
    {

    }
    void PickUpCardCallBack()
    {
        //TalkStage();
    }
    void TalkAcion()
    {

    }
    void TalkAcionCallBack()
    {
        //GivegoodsStage();
    }
    void GiveGoodsAcion()
    {

    }
    void GiveGoodsActionCallBack()
    {
        XHReceive();
    }
    #endregion
    /// <summary>
    /// 小华接受东西动画
    /// </summary>
    void XHReceive()
    {

    }
    void XHReceiveCallBack()
    {
        //ui事件注册
        ShowFinalUI();
    }
    /// <summary>
    /// 显示结算界面
    /// </summary>
    void ShowFinalUI()
    {

    }
    void Finish()
    {
        RemoveAllListeners();
        Destroy(gameObject);
    }
    void RemoveAllListeners()
    {

    }
    private void OnDestroy()
    {

    }
    public enum Stage//阶段枚举
    {
        getCard,//小华拿卡
        handingCard,//辅导教师，小华递卡
        pickUpCard,//教师接卡
        talk,//教师说话
        giveGoods//教师给物品
    }
    public void Dispose()
    {

    }
}

