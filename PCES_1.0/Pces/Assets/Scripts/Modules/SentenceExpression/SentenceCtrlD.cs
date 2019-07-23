﻿using System.Collections;
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
    int index = 0;
    private void Awake()
    {
        this.name = "SentenceCtrlD";
        PeopleManager.Instance.gameObject.SetActive(false);
        foreach (GameObject item in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (item.name == "jiaoshi")
            {
                item.SetActive(false);
            }
        }
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
        dog.transform.localScale = Vector3.one * 2;
        dog.PlayForward("idle");
        dog.SetWrapMode = WrapMode.Loop;

        MM.PlayForward("idle");
        //XH.PlayForward("idle");
        //XH.PlayForward("XH_F_4TH_FNN");
        PropsObject pObj = SentenceExpressionModel.GetInstance().GetObj(PropsType.neutralStimulator);//中性刺激物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject");

        XHTZka();
        //Invoke("XHTZka", 1);
    }
    /// <summary>
    /// 小华贴字体动画
    /// </summary>
    void XHTZka()
    {
        GameObject ka = ResManager.GetPrefab("Prefabs/AnimationKa/XH_F_4TH_FNN_KA");
        ka.name = "XH_F_4TH_FNN_KA";
        ka.transform.SetParent(transform);
        //我看见
        Material matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/goutongben/goutongben_02/tuka5").GetComponent<MeshRenderer>().materials[1];
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_wokanjian.ToString()).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai4/tuka10").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        //实物
        index = Random.Range(101, 1001) % 3 + 25;
        Debug.Log(index);
        matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/goutongben/goutongben_03/tuka7").GetComponent<MeshRenderer>().materials[1];
        matSource = SentenceExpressionModel.GetInstance().GetObj(index).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai4/tuka9").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        PropsObject pObj = SentenceExpressionModel.GetInstance().GetObj(index).GetComponent<PropsObject>();
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log(rfc.pData.name_cn);

        XH.transitionTime = 0;
        //XH.Complete += XHTZkaCallback;
        XH.PlayForward("XH_F_4TH_FNN");
        XH.timePointEvent = (a) =>//
        {
            if (a == 617)
            {
                XH.timePointEvent = null;
                XH.OnPause();

                XHTZkaCallback();//mm高亮
            }
        };

        LegacyAnimationOper lao = ka.GetLegacyAnimationOper();
        lao.framePointEvent = (a) =>
        {
            if (a == 617)
            {
                lao.framePointEvent = null;
                lao.OnPause();
            }
        };
        lao.PlayForward("XH_F_4TH_FNN_KA");

    }
    void ClickMMhandCallback(ClickedObj cobj)
    {
        Debug.Log("点中 " + cobj.objname);
        if (cobj.objname == "click")
        {
            ChooseDo.Instance.Clicked();
        }
    }
    void XHTZkaCallback()
    {
        ClickDispatcher.Inst.cam = transform.Find("park(Clone)/Camera").GetComponent<Camera>();
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickMMhandCallback);
        ClickMMhandTip();
    }
    void ClickMMhandTip()
    {
        if (mmHand == null)
        {
            mmHand = MM.transform.Find("mama/mama_shenti").gameObject;
        }
        ClickDispatcher.Inst.EnableClick = true;
        HighLightCtrl.GetInstance().FlashOn(mmHand);
        ChooseDo.Instance.DoWhat(5, RedoClickMMHand, MMJieObj);
    }
    void RedoClickMMHand()
    {
        ClickDispatcher.Inst.EnableClick = false;
        //swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈接卡");
        CancelInvoke("ClickMMhandTip");
        Invoke("ClickMMhandTip", 2);
    }
    void MMJieObj()
    {
        HighLightCtrl.GetInstance().FlashOff(mmHand);
        ClickDispatcher.Inst.EnableClick = false;

        MM.timePointEvent = (a) =>
        {
            if (a == 40)
            {
                XH.OnContinue();
                transform.Find("XH_F_4TH_FNN_KA").GetComponent<LegacyAnimationOper>().OnContinue();
            }
            if (a == 181)
            {
                MM.timePointEvent = null;
                MM.OnPause();
                DBYCallback();
            }
        };
        MM.PlayForward("MM_F_4TH_DBY");

        GameObject ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_F_4TH_DBY_KA");
        ka.name = "MM_F_4TH_DBY_KA";
        ka.transform.SetParent(transform);

        Material matTar = ka.transform.Find("judai/tuka1").GetComponent<MeshRenderer>().materials[1];
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_wokanjian.ToString()).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        matTar = ka.transform.Find("judai/tuka").GetComponent<MeshRenderer>().materials[1];
        matSource = SentenceExpressionModel.GetInstance().GetObj(index).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        ka.GetLegacyAnimationOper().PlayForward("MM_F_4TH_DBY_KA");
        //Invoke("ClickmicroPhoneTip", 1);
    }
    /// <summary>
    /// 妈妈
    /// </summary>
    void DBYCallback()
    {
        Debug.Log("话筒提示");
        ClickmicroPhoneTip();
    }
    void ClickmicroPhoneTip()
    {
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
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
        MM.OnContinue();
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
        MM.PlayForward("idle");
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);

        PropsObject pObj = SentenceExpressionModel.GetInstance().GetObj(PropsType.reinforcement);//强化物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject  " + rfc.pData.name);



        XH.transitionTime = 0;
        //XH.Complete += XHTZkaCallback;
        XH.PlayForward("XH_F_4TH_FNN");
        XH.timePointEvent = (a) =>//
        {
            if (a == 617)
            {
                XH.timePointEvent = null;
                XH.OnPause();

                WyXhZkaCallback();//mm高亮
            }
        };

        GameObject ka = transform.Find("XH_F_4TH_FNN_KA").gameObject;
        //ka.name = "XH_F_4TH_FNN_KA";
        //ka.transform.SetParent(transform);

        //我要
        Material matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/goutongben/goutongben_02/tuka5").GetComponent<MeshRenderer>().materials[1];
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_woyao.ToString()).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai4/tuka10").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/goutongben/goutongben_03/tuka7").GetComponent<MeshRenderer>().materials[1];
        matSource = SentenceExpressionModel.GetInstance().GetTuKa(rfc.pData.name).GetComponent<MeshRenderer>().materials[0];
        matTar.CopyPropertiesFromMaterial(matSource);
        matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai4/tuka9").GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        LegacyAnimationOper lao = ka.GetLegacyAnimationOper();
        lao.framePointEvent = (a) =>
        {
            if (a == 617)
            {
                lao.framePointEvent = null;
                lao.OnPause();
            }
        };
        //Debug.LogError("ka");
        lao.PlayForward("XH_F_4TH_FNN_KA");
    }
    void WyXhZkaCallback()
    {
        WyClickMMHandTip();
    }
    void WyClickMMHandTip()
    {
        ClickDispatcher.Inst.EnableClick = true;
        HighLightCtrl.GetInstance().FlashOn(mmHand);
        ChooseDo.Instance.DoWhat(5, RedoWyClickMMhand, ClickMMhandCallback);
    }
    void RedoWyClickMMhand()
    {
        ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(mmHand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈接过物品");
        CancelInvoke("WyClickMMHandTip");
        Invoke("WyClickMMHandTip", 2);
    }
    void ClickMMhandCallback()
    {
        HighLightCtrl.GetInstance().FlashOff(mmHand);
        ClickDispatcher.Inst.EnableClick = false;
        MMGiveObj();
        //WYXhBY();
    }
    void MMGiveObj()//接卡给物
    {
        MM.timePointEvent = (a) =>
        {
            if (a == 40)
            {
                XH.OnContinue();
                transform.Find("XH_F_4TH_FNN_KA").GetComponent<LegacyAnimationOper>().OnContinue();
                WYXhBY();
            }
            if (a == 181)
            {
                MM.timePointEvent = null;
                MMGiveObjCallback();
            }
        };
        MM.PlayForward("MM_F_4TH_DBY");

        GameObject ka = transform.Find("MM_F_4TH_DBY_KA").gameObject;
        ka.gameObject.SetActive(false);

        ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_F_4TH_DBY_KA");
        ka.name = "MM_F_4TH_DBY_KA";
        ka.transform.SetParent(transform);

        //string name = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name;

        Material matTar = ka.transform.Find("judai/tuka1").GetComponent<MeshRenderer>().materials[1];
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_woyao.ToString()).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        matTar = ka.transform.Find("judai/tuka").GetComponent<MeshRenderer>().materials[1];
        matSource = SentenceExpressionModel.GetInstance().GetObj(index).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        ka.GetLegacyAnimationOper().PlayForward("MM_F_4TH_DBY_KA");
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
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);

        //MM.Complete += WYXHJiewu;
        MM.timePointEvent = (a) =>
        {
            if (a == 60)
            {
                MM.timePointEvent = null;
                MMCtrl ctrl = MM.GetComponent<MMCtrl>();
                string name = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name;
                GameObject go = Instantiate(SentenceExpressionModel.GetInstance().GetTuKa(name));
                go.name = "QHW";
                go.transform.SetParent(transform);
                ctrl.SetJoint(go);
                WYXHJiewu();
            }
        };
        MM.PlayForward("MM_F_4TH_DY");
    }
    void WYXHJiewu()
    {
        XH.Complete += WYXHJiewuCallback;
        XH.timePointEvent = (a) =>
        {
            if (a == 20)
            {
                XH.timePointEvent = null;
                XHCtrl ctrl = XH.GetComponent<XHCtrl>();
                GameObject go = MM.GetComponent<MMCtrl>().l_guadian.transform.Find("QHW").gameObject;
                ctrl.SetJointL(go);
            }
        };
        XH.PlayForward("XH_F_4TH_JG");
        //GameObject ka = ResManager.GetPrefab("Prefabs/AnimationKa/XH_F_4TH_JG_KA");
        //ka.transform.SetParent(transform);
        //ka.GetLegacyAnimationOper().PlayForward("XH_F_4TH_JG_KA");
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
        //Destroy(gameObject);
    }
    private void OnDestroy()
    {

    }
}
