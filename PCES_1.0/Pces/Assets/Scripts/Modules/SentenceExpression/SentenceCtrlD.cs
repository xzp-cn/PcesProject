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
    int index = 0;
    private void Awake()
    {
        this.name = "SentenceCtrlD";
        SentenceExpressionModel.GetInstance().Jiaoshi().SetActive(false);
    }
    //public bool Finished;
    private void Start()
    {
        GlobalEntity.GetInstance().Dispatch<string>(CommonUI.pEvent.LevelChange, "第四关");

        Transform park = ResManager.GetPrefab("Scenes/park/park").transform;
        park.SetParent(transform);
        GameObject cam = transform.Find("park(Clone)/Camera").gameObject;
        HighlightingEffect hf = cam.GetComponent<HighlightingEffect>();
        if (hf == null)
        {
            hf = cam.gameObject.AddComponent<HighlightingEffect>();
        }

        if (swapUI == null)
        {
            swapUI = UIManager.Instance.GetUI<SwapUI>("SwapUI");
            //swapUI.chooseEvent += ChooseBtnClickCallback;
            swapUI.speakEvent += SpeakBtnClickCallback;
            swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);
            swapUI.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
        }
        UIManager.Instance.GetUI<Dialog>("Dialog").SetPos(new Vector3(-55, -490, 0));
        MM = ResManager.GetPrefab("Scenes/park/MM").GetAnimatorOper();
        MM.transform.SetParent(park);

        PeopleManager.Instance.Reset();
        PeopleManager.Instance.GetPeople(PeopleTag.FDLS_BD).transform.localPosition = new Vector3(0, 0, 10000);
        PeopleManager.Instance.GetPeople(PeopleTag.LS_BD).transform.localPosition = new Vector3(0, 0, 10000);
        XH = PeopleManager.Instance.GetPeople(PeopleTag.XH_BD).GetAnimatorOper();
        bool pass = true;
        XH.timePointEvent = (a) =>
          {
              if (a >= 1 && a <= 3 && pass)
              {
                  pass = false;
                  XH.timePointEvent = null;
                  XH.OnPause();
              }
          };
        XH.PlayForward("XH_F_4TH_FNN");

        LegacyAnimationOper dog = ResManager.GetPrefab("Scenes/park/dog").GetLegacyAnimationOper();
        dog.transform.SetParent(park);
        dog.transform.localScale = Vector3.one * 2;
        dog.transform.localPosition = new Vector3(-1.205f, 0, 0);
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
        Transform tk10 = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai4/tuka10");

        Transform Wrist_L = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L");//拿卡
        Material matk6 = Wrist_L.Find("tuka6").GetComponent<MeshRenderer>().materials[1];
        matk6.CopyPropertiesFromMaterial(matSource);

        Material mattk4 = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/goutongben/goutongben_01/tuka4").GetComponent<MeshRenderer>().materials[1];
        mattk4.CopyPropertiesFromMaterial(matSource);


        tk10.localEulerAngles = new Vector3(0, -90, 0);
        matTar = tk10.GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        //实物
        index = Random.Range(101, 1001) % 3 + 25;
        Debug.Log(index);
        matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/goutongben/goutongben_03/tuka7").GetComponent<MeshRenderer>().materials[1];
        matSource = SentenceExpressionModel.GetInstance().GetObj(index).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        Transform tk9 = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai4/tuka9");
        tk9.localEulerAngles = new Vector3(0, -90, 0);
        matTar = tk9.GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        Transform tk8 = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/tuka8");
        Material mat8 = tk8.GetComponent<MeshRenderer>().materials[1];
        mat8.CopyPropertiesFromMaterial(matSource);

        PropsObject pObj = SentenceExpressionModel.GetInstance().GetObj(index).GetComponent<PropsObject>();
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log(rfc.pData.name_cn);

        XH.transitionTime = 0;
        //XH.Complete += XHTZkaCallback;
        XH.OnContinue();
        XH.timePointEvent = (a) =>//
        {
            if (a >= 615 && a <= 617)
            {
                XH.timePointEvent = null;
                XH.OnPause();
                //Debug.LogError("pause");
                XHTZkaCallback();//mm高亮
            }
        };

        LegacyAnimationOper lao = ka.GetLegacyAnimationOper();
        bool pass = true;
        lao.framePointEvent = (a) =>
        {
            if (a >= 615 && a <= 617 && pass)
            {
                lao.framePointEvent = null;
                pass = false;
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
            ClickDispatcher.Inst.EnableClick = false;
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
        //ClickDispatcher.Inst.EnableClick = false;
        //swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈接卡");
        CancelInvoke("ClickMMhandTip");
        Invoke("ClickMMhandTip", 2);
    }
    void MMJieObj()
    {
        CancelInvoke("ClickMMhandTip");
        HighLightCtrl.GetInstance().FlashOff(mmHand);
        ClickDispatcher.Inst.EnableClick = false;

        GameObject ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_F_4TH_DBY_KA");
        ka.name = "MM_F_4TH_DBY_KA";
        ka.transform.SetParent(transform);

        Material matTar = ka.transform.Find("judai/tuka1").GetComponent<MeshRenderer>().materials[1];
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_wokanjian.ToString()).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        matTar = ka.transform.Find("judai/tuka").GetComponent<MeshRenderer>().materials[1];
        matSource = SentenceExpressionModel.GetInstance().GetObj(index).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        ka.transform.Find("Group/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai3").gameObject.SetActive(false);
        ka.transform.Find("judai/tuka").localEulerAngles = new Vector3(0, -90, 0);
        ka.transform.Find("judai/tuka1").localEulerAngles = new Vector3(0, -90, 0);

        LegacyAnimationOper lao = ka.GetLegacyAnimationOper();
        lao.PlayForward("MM_F_4TH_DBY_KA");

        bool pass = true;
        bool pasxh = true;
        MM.timePointEvent = (a) =>
        {
            if (a >= 38 && a <= 40 && pasxh)
            {
                pasxh = false;
                XH.OnContinue();
                transform.Find("XH_F_4TH_FNN_KA").GetComponent<LegacyAnimationOper>().OnContinue();
                //transform.Find("XH_F_4TH_FNN_KA").gameObject.SetActive(false);
            }
            if (a >= 72 && a <= 75 && pass)
            {
                pass = false;
                MM.timePointEvent = null;
                MM.OnPause();
                lao.OnPause();
                DBYCallback();
            }
        };
        MM.PlayForward("MM_F_4TH_DBY");
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
        //ClickDispatcher.Inst.EnableClick = false;
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈说话");
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
        CancelInvoke("ClickmicroPhoneTip");
        MM.Complete += WyXhZka;
        MM.OnContinue();
        transform.Find("MM_F_4TH_DBY_KA").GetComponent<LegacyAnimationOper>().OnContinue();
        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        dlog.transform.localPosition = new Vector3(403, 420, 0);
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name_cn;
        dlog.SetDialogMessage("是的，小华看到了" + curObjName + ", 小华表现的很好。");
    }


    /// <summary>
    /// 小华拿出我要字卡
    /// </summary>
    void WyXhZka()
    {
        GameObject k = transform.Find("MM_F_4TH_DBY_KA").gameObject;
        if (k != null)
        {
            Destroy(k);
        }

        MM.PlayForward("idle");
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);

        List<int> indexList = new List<int> { 0, 2, 3, 6 };
        int index = SentenceExpressionModel.GetInstance().GetIndex(indexList);
        PropsObject pObj = SentenceExpressionModel.GetInstance().GetObj(index).GetComponent<PropsObject>();//强化物
        Reinforcement rfc = new Reinforcement(pObj.pData);//测试代码 
        SentenceExpressionModel.GetInstance().CurReinforcement = rfc;//设置强化物
        Debug.Log("GetTukaObject  " + rfc.pData.name);


        XH.transitionTime = 0;
        //XH.Complete += XHTZkaCallback;
        XH.PlayForward("XH_F_4TH_FNN");
        bool pass = true;
        XH.timePointEvent = (a) =>//
        {
            if (a > 613 && a < 617 && pass)
            {
                pass = false;
                XH.timePointEvent = null;
                XH.OnPause();

                WyXhZkaCallback();//mm高亮
            }
        };

        GameObject kaa = transform.Find("XH_F_4TH_FNN_KA").gameObject;
        if (kaa != null)
        {
            DestroyImmediate(kaa);
        }
        GameObject ka = ResManager.GetPrefab("Prefabs/AnimationKa/XH_F_4TH_FNN_KA");
        ka.name = "XH_F_4TH_FNN_KA";
        ka.transform.SetParent(transform);
        //ka.name = "XH_F_4TH_FNN_KA";
        //ka.transform.SetParent(transform);

        //我要
        Material matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/goutongben/goutongben_02/tuka5").GetComponent<MeshRenderer>().materials[1];
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_woyao.ToString()).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        Transform tk10 = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai4/tuka10");
        tk10.localEulerAngles = new Vector3(0, -90, 0);
        matTar = tk10.GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        matTar = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/Elbow_R/Wrist_R/goutongben/goutongben_03/tuka7").GetComponent<MeshRenderer>().materials[1];
        matSource = SentenceExpressionModel.GetInstance().GetTuKa("tuka_" + rfc.pData.name).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        Transform tk9 = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai4/tuka9");
        tk9.localEulerAngles = new Vector3(0, -90, 0);
        matTar = tk9.GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        Transform tk8 = ka.transform.Find("Group1/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/tuka8");
        Material mat8 = tk8.GetComponent<MeshRenderer>().materials[1];
        mat8.CopyPropertiesFromMaterial(matSource);

        LegacyAnimationOper lao = ka.GetLegacyAnimationOper();
        bool kpass = true;
        lao.framePointEvent = (a) =>
        {
            if (a >= 613 && a <= 617 && kpass)
            {
                kpass = false;
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
        //ClickDispatcher.Inst.EnableClick = false;
        HighLightCtrl.GetInstance().FlashOff(mmHand);
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈接卡");
        CancelInvoke("WyClickMMHandTip");
        Invoke("WyClickMMHandTip", 2);
    }
    void ClickMMhandCallback()
    {
        CancelInvoke("WyClickMMHandTip");
        HighLightCtrl.GetInstance().FlashOff(mmHand);
        ClickDispatcher.Inst.EnableClick = false;
        MMGiveObj();
        //WYXhBY();
    }
    void MMGiveObj()//接卡给物
    {
        GameObject ka = ResManager.GetPrefab("Prefabs/AnimationKa/MM_F_4TH_DBY_KA");
        ka.name = "MM_F_4TH_DBY_KA";
        ka.transform.SetParent(transform);

        string name = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name;
        //Debug.LogError(name);
        ka.transform.Find("judai/tuka1").localEulerAngles = new Vector3(0, -90, 0);
        Material matTar = ka.transform.Find("judai/tuka1").GetComponent<MeshRenderer>().materials[1];
        Material matSource = SentenceExpressionModel.GetInstance().GetTuKa(PropsTag.judai_woyao.ToString()).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);

        ka.transform.Find("judai/tuka").localEulerAngles = new Vector3(0, -90, 0);
        matTar = ka.transform.Find("judai/tuka").GetComponent<MeshRenderer>().materials[1];
        matSource = SentenceExpressionModel.GetInstance().GetTuKa("tuka_" + name).GetComponent<MeshRenderer>().materials[1];
        matTar.CopyPropertiesFromMaterial(matSource);
        ka.transform.Find("Group/Main/DeformationSystem/Root_M/Spine1_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/Wrist_L/judai3").gameObject.SetActive(false);

        ka.gameObject.SetActive(true);
        LegacyAnimationOper lao = ka.GetLegacyAnimationOper();

        bool passxh = true;
        bool passmm = true;
        bool pass = true;
        MM.timePointEvent = (a) =>
        {
            if (a >= 38 && a <= 40 && passxh)
            {
                passxh = false;
                XH.OnContinue();
                transform.Find("XH_F_4TH_FNN_KA").GetComponent<LegacyAnimationOper>().OnContinue();
                //transform.Find("XH_F_4TH_FNN_KA").gameObject.SetActive(false);

            }
            if (a >= 72 && a <= 75 && pass)
            {
                pass = false;
                MM.OnPause();
                lao.OnPause();
                //Debug.LogError("WYXhBYTip");
                WYXhBYTip();
            }
            if (a >= 160 && a <= 162 && passmm)
            {
                passmm = false;
                MM.timePointEvent = null;
                //MM.OnPause();                
                MMGiveObjCallback();
            }
        };
        MM.PlayForward("MM_F_4TH_DBY");
        lao.PlayForward("MM_F_4TH_DBY_KA");
    }
    void WYXhBYTip()
    {
        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, true);
        ChooseDo.Instance.DoWhat(5, RedoWYXhBY, WYXhBYSpeak);
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
    }
    void RedoWYXhBY()
    {
        swapUI.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要妈妈说话");
        CancelInvoke("WYXhBYTip");
        Invoke("WYXhBYTip", 2);
    }
    void WYXhBYSpeak()
    {
        CancelInvoke("WYXhBYTip");

        swapUI.SetButtonVisiable(SwapUI.BtnName.microButton, false);

        MM.OnContinue();
        transform.Find("MM_F_4TH_DBY_KA").gameObject.GetLegacyAnimationOper().OnContinue();

        Dialog dlog = UIManager.Instance.GetUI<Dialog>("Dialog");
        UIManager.Instance.SetUIDepthTop("Dialog");
        string curObjName = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name_cn;
        //Debug.LogError(curObjName);
        dlog.SetDialogMessage("好的,小华要" + curObjName + "呀");
    }
    void MMGiveObjCallback()
    {
        transform.Find("MM_F_4TH_DBY_KA").gameObject.SetActive(false);
        UIManager.Instance.GetUI<Dialog>("Dialog").Show(false);

        GameObject ka = transform.Find("MM_F_4TH_DBY_KA").gameObject;
        if (ka != null)
        {
            Destroy(ka);
        }
        //MM.Complete += WYXHJiewu;
        GameObject KA = ResManager.GetPrefab("Prefabs/AnimationKa/MM_F_4TH_DY_KA");
        KA.name = "MM_F_4TH_DY_KA";
        KA.transform.SetParent(transform);
        LegacyAnimationOper lao = KA.GetLegacyAnimationOper();

        bool pass = true;
        MM.timePointEvent = (a) =>
        {
            if (a >= 58 && a <= 60 && pass)
            {
                pass = false;
                MM.timePointEvent = null;
                //MMCtrl ctrl = MM.GetComponent<MMCtrl>();
                int index = Random.Range(101, 1001) % 4;
                string _name = SentenceExpressionModel.GetInstance().CurReinforcement.pData.name;
                //Debug.LogError(name);
                //GameObject go = Instantiate(SentenceExpressionModel.GetInstance().GetTuKa(name));
                //go.name = "QHW";
                //go.transform.SetParent(transform);
                //go.transform.localEulerAngles = Vector3.zero;
                //ctrl.SetJoint(go);
                XhQHW xhqhw = KA.GetComponent<XhQHW>();
                xhqhw.ShowObj(_name);

                WYXHJiewu();
            }
        };
        MM.transitionTime = 0;
        MM.PlayForward("MM_F_4TH_DY", 0.1f);
        //MM.OnContinue();
        lao.PlayForward("MM_F_4TH_DY_KA", 0.1f);

    }
    void WYXHJiewu()
    {
        XH.Complete += WYXHJiewuCallback;
        bool pass = true;
        XH.timePointEvent = (a) =>
        {
            if (a >= 19 && a <= 21 && pass)
            {
                pass = false;
                XH.timePointEvent = null;
                //XHCtrl ctrl = XH.GetComponent<XHCtrl>();
                //GameObject go = MM.GetComponent<MMCtrl>().l_guadian.transform.Find("QHW").gameObject;
                //go.transform.localEulerAngles = Vector3.zero;
                //ctrl.SetJointL(go);
                //go.transform.localPosition = Vector3.zero;
                //go.transform.localEulerAngles = Vector3.zero;
            }
        };
        XH.PlayForward("XH_F_4TH_JG");
        //GameObject ka = ResManager.GetPrefab("Prefabs/AnimationKa/XH_F_4TH_JG_KA");
        //ka.transform.SetParent(transform);
        //ka.GetLegacyAnimationOper().PlayForward("XH_F_4TH_JG_KA");
        transform.Find("XH_F_4TH_FNN_KA").gameObject.SetActive(false);
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
        ResetGuaDian();
        RemoveAllListeners();
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
    void NextDo()
    {
        Finish();
        if (evtFinished != null)
        {
            evtFinished();
        }
    }
    void ResetGuaDian()
    {
        XHCtrl xhctrl = XH.GetComponent<XHCtrl>();
        xhctrl.DestroyGuadian();

        //LSCtrl lsctrl = LS.GetComponent<LSCtrl>();
        //lsctrl.DestroyGuadian();
    }
    void RemoveAllListeners()
    {
        CommonUI com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        com.redoClickEvent -= NextDo;
        com.redoClickEvent -= ReDo;
        swapUI.speakEvent -= SpeakBtnClickCallback;
        com = null;

        //MM.Complete -= KJLsGiveObjCallback;
        XH.Complete -= WYXHJiewuCallback;

        XH.timePointEvent = null;
        MM.timePointEvent = null;

        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, ClickMMhandCallback);
    }
    public void RedoDispose()
    {
        RemoveAllListeners();
        //Destroy(gameObject);
        evtRedo = null;
        evtFinished = null;
        Destroy(gameObject);
    }
    public void Dispose()
    {
        RemoveAllListeners();
        //Destroy(gameObject);
        evtRedo = null;
        evtFinished = null;
    }
    private void OnDestroy()
    {

    }
}
