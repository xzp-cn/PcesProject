using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.ComponentModel;
using System.Xml;


public class FlowModel : SingleTemplate<FlowModel>
{
    public enum mEvent
    {
        Notify,
        FlowStepFinished,   //阶段完成
        LevelFinished,       //关卡完成
        ChangeFlowStep,         //改变实验步骤
        IntroduceLab,       //实验介绍
        StartLab,           //开始实验
        StartExamine,       //开始考核
        ResetFlowStep,    //重置实验步骤
        EnterGame,        //进入游戏
    }

    public FlowModel()
    {
        InitFlowTasks();
        CurrFlowTask = FlowTaskList[2];
    }

    /// <summary>
    /// 所有流程实验任务列表
    /// </summary>
    private List<FlowTask> FlowTaskList = new List<FlowTask>();


    /// <summary>
    /// 缓存由加载器创建出来的各人负责模块，方便销毁释放。
    /// </summary>
    public Dictionary<string, GameObject> CreatedFlowStepPrefabMaps = new Dictionary<string, GameObject>();

    /// <summary>
    /// 缓存装载的预制体
    /// </summary>
    /// <param name="task"></param>
    /// <param name="vb"></param>
    public void PushPrefabToMem(string task, GameObject go)
    {
        if (!CreatedFlowStepPrefabMaps.ContainsKey(task))
        {
            CreatedFlowStepPrefabMaps.Add(task, go);
        }
    }

    /// <summary>
    /// 移除
    /// </summary>
    /// <param name="task"></param>
    public void RemovePrefabFromMem(string task)
    {
        if (CreatedFlowStepPrefabMaps.ContainsKey(task))
        {
            CreatedFlowStepPrefabMaps[task].SendMessage("Dispose");
            CreatedFlowStepPrefabMaps.Remove(task);
        }
    }

    /// <summary>
    /// 初始化所有实验任务列表
    /// </summary>
    private void InitFlowTasks()
    {
        FieldInfo[] fields = typeof(ModelTasks).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        int i = 0;
        foreach (FieldInfo fi in fields)
        {
            DescriptionAttribute descriptionAttribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute;
            FlowTask task = FlowTask.New((ModelTasks)System.Enum.Parse(typeof(ModelTasks), fi.Name), descriptionAttribute.Description, i, FlowStepState.UnFinished);
            FlowTaskList.Add(task);
            i++;
        }
    }


    /// <summary>
    /// 当前流程步骤
    /// </summary>
    private FlowTask _currFlowTask;
    public FlowTask CurrFlowTask
    {
        get
        {
            return _currFlowTask;
        }
        set { _currFlowTask = value; }
    }

    /// <summary>
    /// 下一个流程步骤
    /// </summary>
    public void NextFlowTask()
    {
        if (CurrFlowTask != null)
        {
            int currIndex = FlowTaskList.IndexOf(CurrFlowTask);
            if (currIndex != -1)
            {
                CurrFlowTask.thisFlowStepState = FlowStepState.Finished;
                int nextNo = currIndex + 1;
                if (nextNo < FlowTaskList.Count)
                {
                    CurrFlowTask = FlowTaskList[nextNo];
                    return;
                }
                else
                {
                    ResetAllFlowTask();
                    GameOverCtrl ctrl = UIManager.Instance.GetUI<GameOverCtrl>("GameOverCtrl");
                    ctrl.Init();
                    Debug.LogError("索引有问题");
                    //throw new Exception("索引有问题");
                }
            }

        }
    }

    /// <summary>
    /// 重置所有流程状态
    /// </summary>
    public void ResetAllFlowTask()
    {
        for (int i = 0; i < FlowTaskList.Count; i++)
        {
            FlowTaskList[i].thisFlowStepState = FlowStepState.UnFinished;
        }
        CurrFlowTask = FlowTaskList[0];
    }

    /// <summary>
    /// 查找指定流程步骤
    /// </summary>
    /// <param name="modeltask"></param>
    /// <returns></returns>
    public FlowTask FindFlowTask(ModelTasks modeltask)
    {
        return FlowTaskList.Find((item) => { return item.FlowEnumID == modeltask; });
    }

}

/// <summary>
/// 流程步骤
/// </summary>
public class FlowTask
{
    public ModelTasks FlowEnumID;
    public string thisFlowStepName;
    public int thisFlowNo;
    public FlowStepState thisFlowStepState;

    public static FlowTask New(ModelTasks _enumId, string _name, int _no, FlowStepState _state)
    {
        FlowTask ft = new FlowTask();
        ft.FlowEnumID = _enumId;
        ft.thisFlowStepName = _name;
        ft.thisFlowNo = _no;
        ft.thisFlowStepState = _state;
        return ft;
    }
}

/// <summary>
/// 流程步骤状态
/// </summary>
public enum FlowStepState
{
    Finished,
    UnFinished,
}
