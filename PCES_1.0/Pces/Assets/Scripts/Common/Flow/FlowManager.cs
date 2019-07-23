using UnityEngine;
using System.Collections;
using System.Text;
/// <summary>
/// 流程控制模块View
/// </summary>
public class FlowManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        GlobalEntity.GetInstance().AddListener<ModelTasks>(FlowModel.mEvent.FlowStepFinished, OnFlowStepFinished);
        FlowTask ft = FlowModel.GetInstance().CurrFlowTask;
        StringBuilder sb = new StringBuilder("Prefabs/").Append(ft.FlowEnumID.ToString()).Append("/").Append(ft.FlowEnumID.ToString());
        GameObject go = ResManager.GetPrefab(sb.ToString());
        FlowModel.GetInstance().PushPrefabToMem(ft.FlowEnumID.ToString(), go);
        //PreInitComm();        
    }
    /// <summary>
    /// 预初始化
    /// </summary>
    public static void PreInitComm()
    {
        GameObject uilayer = ResManager.GetPrefab("Prefabs/UI/UILayer");
        UIManager.Instance.InitUI("UILayer", uilayer.transform);
        ResManager.GetPrefab("Prefabs/People/People");
        ResManager.GetPrefab("Prefabs/Objects/Objects");
        GlobalDataManager.GetInstance().InitUI();
    }
    private void OnFlowStepFinished(ModelTasks mt)
    {
        //Debug.LogError(mt.ToString());s
        FlowModel.GetInstance().RemovePrefabFromMem(mt.ToString());
        FlowModel.GetInstance().NextFlowTask();
        FlowTask ft = FlowModel.GetInstance().CurrFlowTask;
        StringBuilder sb = new StringBuilder("Prefabs/").Append(ft.FlowEnumID.ToString()).Append("/").Append(ft.FlowEnumID.ToString());
        GameObject module = ResManager.GetPrefab(sb.ToString());
        FlowModel.GetInstance().PushPrefabToMem(ft.FlowEnumID.ToString(), module);
    }
}
