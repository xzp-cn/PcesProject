using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceCtrlD : MonoBehaviour
{
    public event System.Action evtFinished;
    public event System.Action evtRedo;
    SwapUI swapUI;
    GameObject xhHand;
    GameObject mmHand;
    AnimationOper MM;
    AnimationOper XH;
    AnimationOper GTB;//沟通本
    private void Awake()
    {
        this.name = "SentenceCtrlD";
    }
    //public bool Finished;
    private void Start()
    {
        Transform park = ResManager.GetPrefab("Scenes/park/park").transform;
        park.SetParent(transform);
        if (swapUI == null)
        {
            swapUI = UIManager.Instance.GetUI<SwapUI>("SwapUI");
            //swapUI.chooseEvent += ChooseBtnClickCallback;
            swapUI.speakEvent += SpeakBtnClickCallback;
            swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
            swapUI.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
        }
        MM = ResManager.GetPrefab("Scenes/park/MM").GetAnimatorOper();
        MM.transform.SetParent(park);
        XH = ResManager.GetPrefab("Scenes/park/XH").GetAnimatorOper();
        XH.transform.SetParent(park);
        LegacyAnimationOper dog = ResManager.GetPrefab("Scenes/park/dog").GetLegacyAnimationOper();
        dog.transform.SetParent(park);
        dog.PlayForward("idle");
        MM.PlayForward("idle");
        //XH.PlayForward("idle");
        //XH.PlayForward("XH_F_4TH_FNN");
        XHTZka();
        //Invoke("XHTZka", 1);
    }
    /// <summary>
    /// 小华贴字体动画
    /// </summary>
    void XHTZka()
    {
        XH.transitionTime = 0;
        XH.Complete += XHTZkaCallback;
        XH.PlayForward("XH_F_4TH_FNN");
        ResManager.GetPrefab("Prefabs/AnimationKa/XH_F_4TH_FNN_KA").GetLegacyAnimationOper().PlayForward("XH_F_4TH_FNN_KA");
    }
    void ClickMMhandCallback(ClickedObj cobj)
    {
        Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "fdls_shou")
        {
            ChooseDo.Instance.Clicked();
        }
    }
    void XHTZkaCallback()
    {
        XH.PlayForward("XH_F_4TH_JG");
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickMMhandCallback);
        ClickDispatcher.Inst.EnableClick = true;
        ClickMMhandTip();
    }
    void ClickMMhandTip()
    {
        if (mmHand == null)
        {
            mmHand = MM.transform.Find("click").gameObject;
        }
        HighLightCtrl.GetInstance().FlashOn(mmHand);
        ChooseDo.Instance.DoWhat(5, RedoClickMMHand, MMJieObj);
    }
    void RedoClickMMHand()
    {
        ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接卡");
        CancelInvoke("ClickMMhandTip");
        Invoke("ClickMMhandTip", 2);
    }
    void MMJieObj()
    {
        HighLightCtrl.GetInstance().FlashOff(mmHand);
        ClickDispatcher.Inst.EnableClick = false;
        MM.Complete += DBYCallback;
        MM.PlayForward("MM_F_4TH_DBY ");
        ResManager.GetPrefab("Prefabs/AnimationKa/MM_F_4TH_DBY_KA").GetLegacyAnimationOper().PlayForward("MM_F_4TH_DBY_KA");
    }

    /// <summary>
    /// 妈妈
    /// </summary>
    void DBYCallback()
    {
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        ClickmicroPhoneTip();
    }
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
        Debug.Log("按钮点击");
        UIFlah uf = swapUI.GetMicroBtn.gameObject.GetUIFlash();
        uf.StopFlash();
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
        ChooseDo.Instance.Clicked();
    }
    void ShowSpeakContent()
    {
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("是的，你看到了" + curObjName + ", 你表现的很好。");
        Invoke("WyXhZka", 1);
    }

    /// <summary>
    /// 小华拿出我要字卡
    /// </summary>
    void WyXhZka()
    {
        XH.Complete += WyXhZkaCallback;
        XH.PlayForward("XH_F_4TH_FNN");
        ResManager.GetPrefab("Prefabs/AnimationKa/XH_F_4TH_FNN_KA").GetLegacyAnimationOper().PlayForward("XH_F_4TH_FNN_KA");
    }
    void WyXhZkaCallback()
    {
        ClickDispatcher.Inst.EnableClick = true;
        WyClickMMHandTip();
    }
    void WyClickMMHandTip()
    {
        HighLightCtrl.GetInstance().FlashOn(mmHand);
        ChooseDo.Instance.DoWhat(5, RedoWyClickMMhand, ClickMMhandCallback);
    }
    void RedoWyClickMMhand()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(mmHand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接过物品");
        CancelInvoke("ClickmicroPhoneTip");
        Invoke("ClickmicroPhoneTip", 2);
    }
    void ClickMMhandCallback()
    {
        MMGiveObj();
        WYXhBY();
    }
    void MMGiveObj()
    {
        MM.Complete += MMGiveObjCallback;
        MM.PlayForward("MM_F_4TH_DY");
        ResManager.GetPrefab("Prefabs/AnimationKa/MM_F_4TH_DBY_KA").GetLegacyAnimationOper().PlayForward("MM_F_4TH_DBY_KA");
    }
    void WYXhBY()
    {
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("好的你要" + curObjName);
    }
    void MMGiveObjCallback()
    {
        XH.Complete += WYXHJiewuCallback;
        XH.PlayForward("XH_F_4TH_JG");
        ResManager.GetPrefab("Prefabs/AnimationKa/MM_F_4TH_DBY_KA").GetLegacyAnimationOper().PlayForward("MM_F_4TH_DBY_KA");
    }
    void WYXHJiewuCallback()
    {
        Debug.Log("小华接物");
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
    void ReDo()
    {
        Debug.Log("redo");
        Finish();
        evtRedo();
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
    public void Dispose()
    {
        RemoveAllListeners();
        Destroy(gameObject);
    }
    private void OnDestroy()
    {

    }
}
