﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第一关 -- 句型表达
/// </summary>
public class SpeakUpCtrlA : MonoBehaviour
{
    public System.Action evtFinished;
    public System.Action evtRedo;
    private CommonUI comUI;
    private GameObject gtNotebook; //沟通本
    private GameObject emptyRoot;
    private GameObject judaiGobj; //句带
    private GameObject RndReinforcementA; //强化物
    private GameObject tukaA;  //图卡

    void Start()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        if(xiaohuaGo.GetComponent<XHCtrl>() == null)
        {
            xiaohuaGo.AddComponent<XHCtrl>().InitComplete = ()=> {
                xiaohuaGo.GetComponent<XHCtrl>().r_tuka.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_tuka2.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_judai.SetActive(false);
                xiaohuaGo.GetComponent<XHCtrl>().r_judai2.SetActive(false);
            };
        }
        emptyRoot = new GameObject("Root");

        //生成沟通本
        PropsObject gtbProp = ObjectsManager.instanse.GetProps((int)PropsTag.TY_GTB);
        gtNotebook = GameObject.Instantiate(gtbProp.gameObject);
        gtNotebook.GetComponent<PropsObject>().pData = gtbProp.pData;
        gtNotebook.transform.SetParent(emptyRoot.transform, false);
        gtNotebook.transform.localPosition = new Vector3(2.276f, 0.56572f, 0.1f);

        //生成我要句带
        GameObject judaiParent = new GameObject("judaiParent");
        judaiParent.transform.SetParent(emptyRoot.transform, false);
        judaiParent.transform.localPosition = new Vector3(2.281f, 0.549f, -0.334f);
        judaiGobj = GameObject.Instantiate(ObjectsManager.instanse.propList[(int)PropsTag.judai_woyao].gameObject);
        judaiGobj.GetComponent<PropsObject>().pData = ObjectsManager.instanse.propList[(int)PropsTag.judai_woyao].pData;
        judaiGobj.transform.SetParent(judaiParent.transform, false);

        //随机一个强化物A
        GameObject goodA = SpeakUpModel.GetInstance().GetRndReinforcement();
        RndReinforcementA = GameObject.Instantiate(goodA);
        RndReinforcementA.GetComponent<PropsObject>().pData = goodA.GetComponent<PropsObject>().pData;
        GameObject qhwA = new GameObject("ReinforcementA");
        qhwA.transform.SetParent(emptyRoot.transform, false);
        RndReinforcementA.transform.SetParent(qhwA.transform, false);
        RndReinforcementA.transform.localPosition = Vector3.zero;
        qhwA.transform.localPosition = new Vector3(2.5328F, 0.5698F, -0.118F);
        //强化物图卡A
        string tukaNameA = "tuka_" + goodA.name;
        tukaA = DistinguishPictureModel.GetInstance().GetTuKa(tukaNameA);
        GameObject _tukaA = new GameObject("tukaA");
        _tukaA.transform.SetParent(emptyRoot.transform, false);
        _tukaA.transform.localPosition = new Vector3(2.297f, 0.565f, 0.093f);
        tukaA.transform.SetParent(_tukaA.transform, false);
        tukaA.transform.localPosition = Vector3.zero;


        //1. 进入界面1秒后，触动小华翻开沟通本的动画。
        Invoke("OnXiaoHuaBring", 1f);
    }

    void OnXiaoHuaBring()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        //if (tukaMat != null)
        //{
        //    //替换手上图卡材质
        //    Material[] tukaBs = tukaB.GetComponentInChildren<MeshRenderer>().materials;
        //    tukaMat.CopyPropertiesFromMaterial(tukaBs[1]);
        //    XH_tkB.GetComponentInChildren<MeshRenderer>().materials[1] = tukaMat;
        //}

        xiaohuaAnim.Complete += () =>
        {
            //2. 播放结束，提醒操作者点击辅导教师的手，点击后触发辅导教师抓着小华的手把图卡粘在句带上的动画。
            GameObject shou = PeopleManager.Instance.GetPeople("FDLS_BD").transform.Find("FDLS/fdls_shou").gameObject;
            Debug.Log("SpeakUpCtrlA.OnXiaoHuaBring(): 2. 播放结束，提醒操作者点击辅导教师的手，点击后触发辅导教师抓着小华的手把图卡粘在句带上的动画。");
            HighLightCtrl.GetInstance().FlashOn(shou);
            shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
            GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickFDTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = true;
            ChooseDo.Instance.DoWhat(5, RedoClickFDTeachersHandFirst, null);
        };
        xiaohuaAnim.PlayForward("XH_D_1ST_FBNKT");
    }

    private void ClickFDTeachersPromptFirst()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickFDTeachersHandFirst, null);
    }

    private void RedoClickFDTeachersHandFirst()
    {
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("请点击辅导老师的手");
        CancelInvoke("ClickFDTeachersPromptFirst");
        Invoke("ClickFDTeachersPromptFirst", 2);
    }

    private void OnClickFDTeacherHandFirst(ClickedObj cobj)
    {
        Debug.Log("SpeakUpCtrlA.OnClickTeacherHandFirst(): " + cobj.objname);
        if (cobj.objname == "fdls_shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickFDTeachersPromptFirst");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickFDTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = false;
            HighLightCtrl.GetInstance().FlashOff(cobj.go);

            GameObject fdls = PeopleManager.Instance.GetPeople("FDLS_BD");
            AnimationOper fdlsAnimOp = fdls.GetAnimatorOper();
            fdlsAnimOp.Complete += () =>
            {
                //3. 播放结束，触发小华把句带递给教师的动画。
                GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
                AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
                xiaohuaAnim.PlayForward("TY_XH_DK");

                //4. 播放结束，提示操作者点击教师的手，播放教师接卡的动画。
                AnimationOper teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
                teacherAnim.Complete += () =>
                {
                    //5. 播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要吃XXX呀”
                    SwapUI swapui = UIManager.Instance.GetUI<SwapUI>("SwapUI");
                    swapui.SetButtonVisiable(SwapUI.BtnName.microButton, true);
                    swapui.SetButtonVisiable(SwapUI.BtnName.chooseButton, false);
                    swapui.GetMicroBtn.gameObject.GetUIFlash().StartFlash();
                    swapui.speakEvent = () =>
                    {
                        swapui.GetMicroBtn.gameObject.GetUIFlash().StopFlash();
                        swapui.speakEvent = null;
                        swapui.SetButtonVisiable(SwapUI.BtnName.microButton, false);
                        Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
                        string gift = RndReinforcementA.GetComponent<PropsObject>().pData.name_cn;
                        dialog.SetDialogMessage("小华要吃" + gift + "呀。");

                        //6. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
                        Invoke("ClickTeachersHandFinal", 2f);
                    };

                };
                teacherAnim.PlayForward("TY_LS_JK");
            };
            fdlsAnimOp.PlayForward("FDLS_D_1ST_TJD");

        }
    }

    private void ClickTeachersHandFinal()
    {
        Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
        dialog.Show(false);
        //7. 播放结束，触发小华接过XXX。
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        xiaohuaAnim.Complete += () =>
        {
            //8. 播放结束，出现下一关和重做的按钮。
            Debug.Log("SpeakUpCtrlA.OnClickTeacherHandFinal(): 8. 播放结束，出现下一关和重做的按钮。");
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
            comUI.redoClickEvent += OnReDo;
            comUI.nextClickEvent += OnNextDo;
            comUI.ShowFinalUI();
        };
        xiaohuaAnim.PlayForward("TY_XH_JG");
    }

    private void OnReDo()
    {
        Redo();
    }

    private void OnNextDo()
    {
        Finished();
    }

    /// <summary>
    /// 本关完成
    /// </summary>
    void Finished()
    {
        if (evtFinished != null)
        {
            evtFinished();
        }
        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
    }

    void Redo()
    {
        if (evtRedo != null)
        {
            evtRedo();
        }
        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
    }

    public void Dispose()
    {
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
        comUI = null;
        evtFinished = null;
        evtRedo = null;
        if (emptyRoot != null)
        {
            Destroy(emptyRoot);
            emptyRoot = null;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }
}
