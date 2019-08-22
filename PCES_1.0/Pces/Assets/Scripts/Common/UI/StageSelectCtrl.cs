using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectCtrl : MonoBehaviour
{
    StageSelectUI stageUI;
    private void Awake()
    {
        stageUI = GetComponent<StageSelectUI>();
        if (stageUI == null)
        {
            stageUI = gameObject.AddComponent<StageSelectUI>();
        }
        stageUI.selectStageEvent += OnSelectBtnClickCallback;
        stageUI.okEvent += OnOKBtnClickCallback;
        stageUI.closeEvent += OnCloseBtnClickCallback;
    }
    private void Start()
    {
        stageUI.transform.SetParent(transform.parent.Find("topEmpty"));
    }
    /// <summary>
    /// 选择阶段按钮点击回调
    /// </summary>
    void OnSelectBtnClickCallback()
    {
        //Debug.LogError("OnSelectBtnClickCallback");
        //transform.SetAsLastSibling();
        transform.SetParent(UIManager.Instance.transform.Find("topEmpty"), false);
        ClickDispatcher.Inst.EnableClick = false;

    }
    void OnCloseBtnClickCallback()
    {
        //Debug.LogError("OnCloseBtnClickCallback");
        ClickDispatcher.Inst.EnableClick = true;
    }
    void OnOKBtnClickCallback(int index)
    {
        //Debug.LogError("OnOKBtnClickCallback");
        ClickDispatcher.Inst.EnableClick = true;

        PeopleManager.Instance.Reset();
        ChooseDo.Instance.ResetAll();
        HighLightCtrl.GetInstance().OffAllObjs();
        UIManager.Instance.GetUI<SwapUI>("SwapUI").ResetUI();
        GlobalEntity.GetInstance().RemoveAllListeners(ClickDispatcher.mEvent.DoClick);

        //上一个阶段处理
        Debug.LogFormat((string.Format("  当前阶段:   {0}  ", (ModelTasks)index)));
        ModelTasks mt = FlowModel.GetInstance().CurrFlowTask.FlowEnumID;
        FlowModel.GetInstance().RemovePrefabFromMem(mt.ToString());

        //跳转阶段
        FlowTask ft = FlowModel.GetInstance().CurrFlowTask;
        ft = FlowModel.GetInstance().FindFlowTask((ModelTasks)index);
        FlowModel.GetInstance().CurrFlowTask = ft;

        StringBuilder sb = new StringBuilder("Prefabs/").Append(ft.FlowEnumID.ToString()).Append("/").Append(ft.FlowEnumID.ToString());
        GameObject module = ResManager.GetPrefab(sb.ToString());
        FlowModel.GetInstance().PushPrefabToMem(ft.FlowEnumID.ToString(), module);

    }

    private void OnDisable()
    {

    }
    private void OnDestroy()
    {
        stageUI.selectStageEvent -= OnSelectBtnClickCallback;
        stageUI.okEvent -= OnOKBtnClickCallback;
        stageUI.closeEvent -= OnCloseBtnClickCallback;
        ClickDispatcher.Inst.EnableClick = true;
    }
}
