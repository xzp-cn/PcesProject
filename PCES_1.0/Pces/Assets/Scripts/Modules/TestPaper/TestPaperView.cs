using UnityEngine;
using UnityEngine.UI;
public class TestPaperView : MonoBehaviour
{
    public event System.Action evtFinished, evtRedo;
    private CommonUI comUI;
    Button resetBtn, nextBtn;
    int subject = 0;//题目数量
    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        resetBtn = transform.Find("final/reset").GetComponent<Button>();
        resetBtn.onClick.AddListener(OnReDo);
        nextBtn = transform.Find("final/next").GetComponent<Button>();
        nextBtn.onClick.AddListener(OnNextDo);
        Transform canvas = comUI.transform.parent;
        transform.SetParent(canvas.transform, false);

        TestPaperItem[] Items = transform.GetComponentsInChildren<TestPaperItem>();
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].callback = Callback;
        }
    }
    /// <summary>
    /// 做题回调
    /// </summary>
    void Callback()
    {
        subject++;
        Debug.Log(subject);
        if (subject == 5)
        {
            //弹出总结框
            Summary sum = ResManager.GetPrefab("Prefabs/UI/summary").GetComponent<Summary>();
            sum.transform.SetParent(transform, false);
            int curIndex = (int)FlowModel.GetInstance().CurrFlowTask.FlowEnumID;
            sum.ChangeContent(curIndex);
        }
    }
    public void Init()
    {
        int curIndex = (int)FlowModel.GetInstance().CurrFlowTask.FlowEnumID;
        //curIndex = 1;//测试
        //Debug.LogError(indexe)
        Paper paper = TestPaperModel.GetInstance().paperList[curIndex];
        paper.ResetData();
        transform.Find("bg/Image/title").GetComponent<Text>().text = paper.title;
        Transform content = transform.Find("subject/Viewport/Content");
        for (int i = 0; i < paper.itemList.Count; i++)
        {
            TestPaperItem tPaperItem = ResManager.GetPrefab("Prefabs/UI/TestPaperItem").GetComponent<TestPaperItem>();
            tPaperItem.name = i.ToString();
            tPaperItem.transform.SetParent(content);
            tPaperItem.Init(paper.itemList[i]);
        }
    }
    private void OnReDo()
    {
        TestPaperItem[] Items = transform.GetComponentsInChildren<TestPaperItem>();//数据清除
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].ResetAll();
        }
        int curIndex = (int)FlowModel.GetInstance().CurrFlowTask.FlowEnumID;
        TestPaperModel.GetInstance().paperList[curIndex].ResetData();
        Redo();
        //Debug.LogError("redo");
        //Redo();
    }
    private void OnNextDo()
    {
        TestPaperItem[] Items = transform.GetComponentsInChildren<TestPaperItem>();//数据清除
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].item_right == Items[i].item_wrong)
            {
                TipUI tipUI = UIManager.Instance.GetUI<TipUI>("TipUI");
                tipUI.SetTipMessage("还有题目为未做 !");
                //UIManager.Instance.SetUIDepthTop("TipUI");
                transform.SetAsLastSibling();
                tipUI.transform.SetAsLastSibling();
                return;
            }
        }
        int right = 0, wrong = 0;
        for (int i = 0; i < Items.Length; i++)
        {
            right += Items[i].item_right;
            wrong += Items[i].item_wrong;
        }

        int curIndex = (int)FlowModel.GetInstance().CurrFlowTask.FlowEnumID;
        Paper paper = TestPaperModel.GetInstance().paperList[curIndex];
        paper.rightNum = right;
        paper.wrongNum = wrong;

        //Debug.LogError(curIndex);

        Finished();
        if (evtFinished != null)
        {
            evtFinished();
        }
        Debug.Log(FlowModel.GetInstance().CurrFlowTask.thisFlowStepName + "     正确个数和错误个数统计     " + TestPaperModel.GetInstance().TotalCount());
    }

    /// <summary>
    /// 本关完成
    /// </summary>
    void Finished()
    {
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
    }

    void Redo()
    {
        Finished();
        if (evtRedo != null)
        {
            evtRedo();
        }
    }

    public void Dispose()
    {
        //comUI.redoClickEvent -= OnReDo;
        //comUI.nextClickEvent -= OnNextDo;
        evtFinished = null;
        evtRedo = null;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }

}
