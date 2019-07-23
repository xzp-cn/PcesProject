using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第三关 -- 区辨多张图片
/// </summary>
public class DistinguishPictureCtrlC : MonoBehaviour {

    public event System.Action evtFinished;
    public event System.Action evtRedo;
    private CommonUI comUI;
    private GameObject emptyRoot;
    private Vector3[] qhwPos;   //强化物初始位置
    private Vector3[] nqhwPos; //负强化物初始位置
    private GameObject gtNotebook; //沟通本
    private AnimationOper xiaohuaAnim;
    private AnimationOper gtbAnim;
    private AnimationOper teacherAnim;

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
        qhwPos = new Vector3[3] { new Vector3(2.5328F, 0.5698F, -0.436F), new Vector3(2.5328F, 0.5698F, -0.229F), new Vector3(2.5328F, 0.5698F, -0.023F) };
        nqhwPos = new Vector3[2] { new Vector3(2.5328f, 0.5698f, 0.166f), new Vector3(2.5328f, 0.5698f, 0.356f) };
        emptyRoot = new GameObject("Root");
    }

    void Start () {

        List<PropsObject> rndReinforcements = new List<PropsObject>();
        DistinguishPictureModel.GetInstance().GetRndReinforcements(3, rndReinforcements);
        int i = 0;
        //初始化摆放强化物
        rndReinforcements.ForEach((ob) => {
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
            nqhw.transform.localPosition = nqhwPos[i++];
        });

        //初始化沟通本
        PropsObject gtbProp = ObjectsManager.instanse.GetProps((int)PropsTag.TY_GTB);
        gtNotebook = GameObject.Instantiate(gtbProp.gameObject);
        gtNotebook.GetComponent<PropsObject>().pData = gtbProp.pData;
        gtNotebook.transform.SetParent(emptyRoot.transform, false);
        gtNotebook.transform.localPosition = new Vector3(2.251f, 0.56572f, 0.56f);

        //1. 进入界面后1秒，触发小华翻开沟通本并拿出图卡，递给老师的动画。
        Invoke("OnXiaoHuaPassGouTongBenToTeacher", 1f);
    }

    private GameObject CreateObj(PropsObject source,int index)
    {
        GameObject scopy = GameObject.Instantiate(source.gameObject);
        scopy.GetComponent<PropsObject>().pData = source.pData;
        GameObject qhw = new GameObject("qhw"+index);
        qhw.transform.SetParent(emptyRoot.transform, false);
        scopy.transform.SetParent(qhw.transform, false);
        scopy.transform.localPosition = Vector3.zero;
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
        return nqhw;
    }

    void OnXiaoHuaPassGouTongBenToTeacher()
    {
        //2. 播放结束，提醒操作者点击教师的手，点击后触发接图卡的动作。播放结束，提醒操作者点击话筒，点击后话筒旁边显示“你要XXX呀”
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        xiaohuaAnim = xiaohuaGo.GetAnimatorOper();

        gtbAnim = gtNotebook.GetAnimatorOper();

        int start = 31;
        int end = 32;
        xiaohuaAnim.timePointEvent = (t) => {
            if(t >= start && t <= end)
            {
                xiaohuaAnim.timePointEvent = null;
                FanGTB();
            }
        };

        xiaohuaAnim.Complete += () =>
        {
            GameObject shou = PeopleManager.Instance.GetPeople("LS_BD").transform.Find("LSB_BD/shou").gameObject;

            HighLightCtrl.GetInstance().FlashOn(shou);
            shou.GetBoxCollider().size = new Vector3(1, 0.2f, 0.5f);
            GlobalEntity.GetInstance().AddListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
            ClickDispatcher.Inst.EnableClick = true;

            ChooseDo.Instance.DoWhat(5, RedoClickTeachersHandFirst, null);
        };
        xiaohuaAnim.PlayForward("XH_C_3RD_FBFY");
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
        gtbAnim.timePointEvent = (t) => {
            if (t >= start0 && t <= end0)
            {
                gtbAnim.timePointEvent = null;
                gtbAnim.OnPause();
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
        tip.SetTipMessage("请点击老师的手");
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
            teacherAnim.Complete += () =>
            {
                OnClickHuaTong();
            };
            teacherAnim.PlayForward("TY_LS_JK");
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
            string gift = emptyRoot.transform.Find("qhw2").GetComponentInChildren<PropsObject>().pData.name_cn;
            dialog.SetDialogMessage("你要"+gift + "呀。");

            //4. 播放结束，触发小华拿起B的动画。
            GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
            AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
            xiaohuaAnim.Complete += () =>
            {
                dialog.Show(false);
                //3. 显示2秒，结束后，提醒操作者点击教师的手，点击后触发教师给小华的动画。
                OnTeacherTakeXiaoHuaB();
            };
            xiaohuaAnim.PlayForward("TY_XH_NKDK");
        };


    }

    private void OnTeacherTakeXiaoHuaB()
    {
        AnimationOper teacherAnim = PeopleManager.Instance.GetPeople("LS_BD").GetAnimatorOper();
        teacherAnim.Complete += () =>
        {
            //4. 播放结束，触发小华接过XXX。
            OnXiaoHuaAccept();
        };
        teacherAnim.PlayForward("TY_LS_JK");
    }

    private void OnXiaoHuaAccept()
    {
        GameObject xiaohuaGo = PeopleManager.Instance.GetPeople("XH_BD");
        AnimationOper xiaohuaAnim = xiaohuaGo.GetAnimatorOper();
        xiaohuaAnim.Complete += () =>
        {
            //5. 播放结束，出现下一关和重做的按钮。
            comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
            comUI.redoClickEvent += OnReDo;
            comUI.nextClickEvent += OnNextDo;
            comUI.ShowFinalUI();
        };
        xiaohuaAnim.PlayForward("TY_XH_NKDK");
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
        comUI = null;
        GlobalEntity.GetInstance().RemoveListener<ClickedObj>(ClickDispatcher.mEvent.DoClick, OnClickTeacherHandFirst);
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
