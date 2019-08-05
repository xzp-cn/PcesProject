using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三关 -- 区辨多张图片
/// </summary>
public class DistinguishPictureCtrlC : MonoBehaviour
{

    public event System.Action evtFinished;
    public event System.Action evtRedo;
    private CommonUI comUI;
    private GameObject emptyRoot;
    private Vector3[] qhwPos;   //强化物初始位置
    private Vector3[] nqhwPos; //负强化物初始位置
    private Vector3[] qhwtkPos;  //强化物图卡初始位置
    private Vector3[] nqhwtkPos; //负强化物图卡初始位置
    private GameObject gtNotebook; //沟通本
    private AnimationOper xiaohuaAnim;
    private AnimationOper gtbAnim;
    private AnimationOper teacherAnim;

    private GameObject onepage;  //沟通本第一页
    private GameObject twopage;  //沟通本第二页
    private GameObject[] qhwtks;
    private GameObject[] nqhwtks;
    private XHCtrl xhctrl;
    private LSCtrl lsCtrl;
    private QHWCtrl qhwCtrl;
    private GameObject RndReinforcementA;
    private GameObject goodA;
    /*
     小华坐在桌子的一边，老师坐在对面。桌上沟通本里有3张强化物和2张负强化物的图卡，桌子上有5个实物。
  小华翻开沟通本，第一页（橙色）有4张图卡（有2负强化物和2强化物），第二页（红色）只有一张图卡（是强化物）。
  小华拿到第二页的图卡后，递给老师。 老师接过图卡后，老师说:"哦，你要A“
  然后将物品递给小华。
     */

    /*

播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作。播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要XXX呀”
显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
播放结束，触发小华接过XXX。
播放结束，出现下一关和重做的按钮。

     */

    private void Awake()
    {
        qhwtkPos = new Vector3[3] { new Vector3(-0.049F, 0.0006f, -0.04F), new Vector3(-0.1152f, 0.0006f, -0.0413f), new Vector3(-0.096f, 0F, 0F) };
        nqhwtkPos = new Vector3[2] { new Vector3(-0.0485F, 0.0006f, 0.043F), new Vector3(-0.1129f, 0.0006f, 0.0389f) };
        qhwtks = new GameObject[3];
        nqhwtks = new GameObject[2];
        qhwPos = new Vector3[3] { new Vector3(2.5328F, 0.57F, -0.436F), new Vector3(2.5328F, 0.57F, -0.229F), new Vector3(2.5328f, 0.57f, 0.166f) };
        nqhwPos = new Vector3[2] { new Vector3(2.5328F, 0.57F, -0.023F), new Vector3(2.5328f, 0.57f, 0.879f) };
        emptyRoot = new GameObject("Root");
    }

    void Start()
    {
        GlobalEntity.GetInstance().Dispatch<string>(CommonUI.pEvent.LevelChange, "第三关");
        GameObject qhwm = ObjectsManager.instanse.GetQHW();
        qhwm.transform.SetParent(emptyRoot.transform);
        qhwCtrl = qhwm.GetComponent<QHWCtrl>();

        //初始化沟通本
        PropsObject gtbProp = ObjectsManager.instanse.GetProps((int)PropsTag.TY_GTB);
        gtNotebook = GameObject.Instantiate(gtbProp.gameObject);
        gtNotebook.GetComponent<PropsObject>().pData = gtbProp.pData;
        gtNotebook.transform.SetParent(emptyRoot.transform, false);
        gtNotebook.transform.localPosition = new Vector3(2.266f, 0.56572f, 0.433f);
        onepage = gtNotebook.transform.Find("goutongben_02").gameObject;
        twopage = gtNotebook.transform.Find("goutongben_03").gameObject;

        List<PropsObject> rndReinforcements = new List<PropsObject>();
        DistinguishPictureModel.GetInstance().GetRndReinforcements(3, rndReinforcements);
        int i = 0;
        //初始化摆放强化物
        rndReinforcements.ForEach((ob) =>
        {
            GameObject qhw = CreateObj(ob, i);
            qhw.transform.localPosition = qhwPos[i++];
        });


        List<PropsObject> rndNegReinforcements = new List<PropsObject>();
        DistinguishPictureModel.GetInstance().GetRndNegReinforcements(2, rndNegReinforcements);
        i = 0;
        //初始化摆放负强化物
        rndNegReinforcements.ForEach((ob) =>
        {
            GameObject nqhw = CreateNegObj(ob, i);
            if(ob.name == "apple")
            {
                nqhwPos[i].y = 0.604f;
            }
            nqhw.transform.localPosition = nqhwPos[i++];
        });

        teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();

        //1. 进入界面后1秒，触发小华翻开沟通本并拿出图卡，递给老师的动画。
        Invoke("OnXiaoHuaPassGouTongBenToTeacher", 1f);
    }

    private GameObject CreateObj(PropsObject source, int index)
    {
        GameObject qhw = new GameObject("qhw" + index);
        qhw.transform.SetParent(emptyRoot.transform, false);
        if (index < 2)
        {
            GameObject scopy = GameObject.Instantiate(source.gameObject);
            scopy.GetComponent<PropsObject>().pData = source.pData;

            qhw.transform.SetParent(emptyRoot.transform, false);
            scopy.transform.SetParent(qhw.transform, false);
            scopy.transform.localPosition = Vector3.zero;
        }
        else
        {
            //第2个索引为触发的强化物
            goodA = GameObject.Instantiate(source.gameObject);
            goodA.transform.SetParent(emptyRoot.transform, false);
            goodA.GetComponent<PropsObject>().pData = source.pData;
            goodA.transform.localPosition = new Vector3(9999, 9999, 9999);
            RndReinforcementA = qhwCtrl.GetObj(source.name);
        }
        string tukaNameA = "tuka_" + source.gameObject.name;
        qhwtks[index] = GameObject.Instantiate(DistinguishPictureModel.GetInstance().GetTuKa(tukaNameA));
        qhwtks[index].transform.SetParent(index > 1 ? twopage.transform : onepage.transform, false);
        qhwtks[index].transform.localPosition = qhwtkPos[index];
        qhwtks[index].transform.localScale = new Vector3(0.5f, 1, 0.5f);
        return qhw;
    }

    private GameObject CreateNegObj(PropsObject source, int index)
    {
        GameObject scopy = GameObject.Instantiate(source.gameObject);
        scopy.GetComponent<PropsObject>().pData = source.pData;
        GameObject nqhw = new GameObject("nqhw" + index);
        nqhw.transform.SetParent(emptyRoot.transform, false);
        scopy.transform.SetParent(nqhw.transform, false);
        scopy.transform.localPosition = Vector3.zero;

        string tukaNameB = "tuka_" + source.gameObject.name;
        nqhwtks[index] = GameObject.Instantiate(DistinguishPictureModel.GetInstance().GetTuKa(tukaNameB));

        nqhwtks[index].transform.SetParent(onepage.transform, false);
        nqhwtks[index].transform.localPosition = nqhwtkPos[index];
        nqhwtks[index].transform.localScale = new Vector3(0.5f, 1, 0.5f);
        return nqhw;
    }

    void OnXiaoHuaPassGouTongBenToTeacher()
    {
        //2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作。播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要XXX呀”
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        xhctrl = xiaohuaGo.GetComponent<XHCtrl>();
        gtbAnim = gtNotebook.GetAnimatorOper();
        xhctrl.r_tuka.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(qhwtks[2].GetComponentInChildren<MeshRenderer>().materials[1]);
        lsCtrl = teacherAnim.GetComponent<LSCtrl>();
        lsCtrl.ls_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(qhwtks[2].GetComponentInChildren<MeshRenderer>().materials[1]);

        int start = 25;
        int end = 27;
        int st = 169;
        int et = 171;
        int start0 = 198;
        int end0 = 200;
        bool passA = false;
        bool passB = false;
        bool passC = false;
        xiaohuaAnim.timePointEvent = (t) =>
        {
            if (t >= start && t <= end && !passA)
            {
                passA = true;
                FanGTB();
            }

            if (t >= st && t <= et && !passB)
            {
                passB = true;
                xhctrl.r_tuka.SetActive(true);
                qhwtks[2].SetActive(true);
                qhwtks[2].transform.SetParent(emptyRoot.transform, false);
            }

            if (t >= start0 && t <= end0 && !passC)
            {
                passC = true;
                xiaohuaAnim.timePointEvent = null;
                ProcessClickTeacherHandFirst();
            }
        };

        xiaohuaAnim.PlayForward("XH_C_3RD_FBFY");
    }

    private void ProcessClickTeacherHandFirst()
    {
        xiaohuaAnim.OnPause();
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;

        HighLightCtrl.GetInstance().FlashOn(shou);
        shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
        ClickDispatcher.Inst.EnableClick = true;

        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
    }

    private bool passed;
    private void FanGTB()
    {
        if (passed)
        {
            return;
        }
        passed = true;
        int start0 = 52;
        int end0 = 54;
        bool passB = false;
        gtbAnim.timePointEvent = (t) =>
        {
            if (t >= start0 && t <= end0 && !passB)
            {
                passB = true;
                gtbAnim.timePointEvent = null;
            }
        };
        gtbAnim.PlayForward("twoPaper");
    }

    private void ClickTeachersPromptFirst()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
    }

    private void RedoClickTeachersHandFirst()
    {
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师接图卡");
        CancelInvoke("ClickTeachersPromptFirst");
        Invoke("ClickTeachersPromptFirst", 2);
    }

    private void OnClickTeacherHandFirst(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptFirst");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = false;
            HighLightCtrl.GetInstance().FlashOff(cobj.go);

            //播放接图卡动画
            teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
            int st = 42;
            int et = 43;
            bool passA = false;
            bool passA2 = false;
            bool passA3 = false;
            teacherAnim.timePointEvent = (tt) =>
            {
                if (tt >= st && tt <= et && !passA)
                {
                    passA = true;
                    xhctrl.r_tuka.SetActive(false);
                    lsCtrl.ls_tuka2.SetActive(true);
                    xiaohuaAnim.OnContinue();
                }

                if (tt >= 81 && tt <= 83 && !passA2)
                {
                    passA2 = true;
                    //老师放下图卡A
                    //lsCtrl.ls_tuka2.SetActive(false);
                    xhctrl.r_tuka2.GetComponentInChildren<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(qhwtks[2].GetComponentInChildren<MeshRenderer>().materials[1]);
                }

                if (tt >= 94 && tt <= 96 && !passA3)
                {
                    passA3 = true;
                    //老师接图卡A动画结束
                    teacherAnim.timePointEvent = null;
                    teacherAnim.OnPause();
                    lsCtrl.ls_tuka2.SetActive(false);
                    qhwtks[2].SetActive(true);
                    qhwtks[2].transform.localPosition = new Vector3(2.502f, 0.5464f, 0.472f);
                    qhwtks[2].transform.localScale = Vector3.one;
                    OnClickHuaTong();
                }
            };

            teacherAnim.PlayForward("TY_LS_JKDW");
        }
    }

    void OnClickHuaTong()
    {
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
            string gift = goodA.GetComponentInChildren<PropsObject>().pData.name_cn;
            dialog.SetDialogMessage("小华要" + gift + "呀。");

            Invoke("ClickTeachersHandFinal", 1);
        };
    }


    private void ClickTeachersHandFinal()
    {
        Dialog dialog = UIManager.Instance.GetUI<Dialog>("Dialog");
        dialog.Show(false);
        GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
        HighLightCtrl.GetInstance().FlashOn(shou);

        GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
        ClickDispatcher.Inst.EnableClick = true;
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFinal, null);
    }

    private void RedoClickTeachersHandFinal()
    {
        CancelInvoke("ClickTeachersPromptFinal");
        TipUI tip = UIManager.Instance.GetUI<TipUI>("TipUI");
        tip.SetTipMessage("需要教师给相应物品");
        Invoke("ClickTeachersPromptFinal", 2);
    }

    private void ClickTeachersPromptFinal()
    {
        ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFinal, null);
    }

    private void OnClickTeacherHandFinal(ClickedObj cobj)
    {
        if (cobj.objname == "shou")
        {
            ChooseDo.Instance.Clicked();
            CancelInvoke("ClickTeachersPromptFinal");
            GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFinal);
            ClickDispatcher.Inst.EnableClick = false;

            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;
            HighLightCtrl.GetInstance().FlashOff(shou);

            xiaohuaAnim.Complete += () =>
            {
                //11. 播放结束，触发小华接过XXX。

                //12. 播放结束，出现下一关和重做的按钮。
                Debug.Log("DistinguishPictureCtrlB.OnClickTeacherHandFinal(): 11. 播放结束，出现下一关和重做的按钮。");
                comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
                comUI.redoClickEvent += OnReDo;
                comUI.nextClickEvent += OnNextDo;
                comUI.ShowFinalUI();
            };

            int st = 37;
            int et = 39;
            int stm = 45;
            int etm = 47;
            int xhjgs = 42;
            int xhjge = 44;
            bool passA = false;
            bool passB = false;
            bool passD = false;
            teacherAnim.timePointEvent = (a) =>//老师递给物品
            {
                if (a > st && a < et && !passA)//挂载到老师手上强化物时间点
                {
                    passA = true;

                    //将当前强化物挂在老师手上
                    lsCtrl.SetJoint(RndReinforcementA.transform.parent.gameObject);
                    lsCtrl.l_guadian.transform.localPosition = Vector3.zero;
                    RndReinforcementA.transform.parent.localPosition = Vector3.zero;
                    RndReinforcementA.transform.parent.localRotation = Quaternion.Euler(Vector3.zero);
                    RndReinforcementA.transform.localPosition = Vector3.zero;
                }

                if (a > stm && a < etm && !passB)//小华接卡动画播放延迟一边挂载强化物
                {
                    passB = true;

                    LegacyAnimationOper go = ResManager.GetPrefab("Prefabs/AnimationKa/TY_XH_JG_KA").GetLegacyAnimationOper();
                    go.transform.SetParent(transform, false);
                    xiaohuaAnim.timePointEvent = (b) => {
                        if (b > xhjgs && b < xhjge && !passD)
                        {
                            passD = true;
                            XhQHW xhqhw = go.GetComponent<XhQHW>();
                            xhqhw.ShowObj(goodA.GetComponent<PropsObject>().pData.name);
                            RndReinforcementA.transform.parent.gameObject.SetActive(false);
                        }
                    };

                    xiaohuaAnim.OnContinue();
                    xiaohuaAnim.PlayForward("TY_XH_JG");
                    go.PlayForward("TY_XH_JG_KA");
                }
            };
            teacherAnim.OnContinue();
            teacherAnim.PlayForward("TY_LS_DW");
        }
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
    }

    private void OnReDo()
    {
        Redo();
    }

    void Redo()
    {
        if (evtRedo != null)
        {
            evtRedo();
        }

    }

    public void Dispose()
    {

        if (comUI == null)
        {
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;

        comUI = null;

        xiaohuaAnim.timePointEvent = null;
        teacherAnim.timePointEvent = null;
        xhctrl.DestroyGuadian();
        lsCtrl.DestroyGuadian();

        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
        if (goodA != null)
        {
            Destroy(goodA);
        }

        if (emptyRoot != null)
        {
            Destroy(emptyRoot);
            emptyRoot = null;
        }
        evtFinished = null;
        evtRedo = null;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }
}
