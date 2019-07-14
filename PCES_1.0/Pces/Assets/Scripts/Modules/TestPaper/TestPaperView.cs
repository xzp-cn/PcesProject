using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestPaperView : MonoBehaviour
{
    public event System.Action evtFinished, evtRedo;
    private CommonUI comUI;
    Button resetBtn, nextBtn;
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
    }
    public void Init()
    {
        int curIndex = (int)FlowModel.GetInstance().CurrFlowTask.FlowEnumID;
        //curIndex = 1;//测试
        Paper paper = TestPaperModel.GetInstance().paperList[curIndex];
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
        Redo();
    }
    private void OnNextDo()
    {
        Finished();
        if (evtFinished != null)
        {
            evtFinished();
        }

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
