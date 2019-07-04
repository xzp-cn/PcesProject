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
    }
    void Init()
    {
        int curIndex = (int)FlowModel.GetInstance().CurrFlowTask.FlowEnumID;
        curIndex = 0;//测试
        Paper paper = TestPaperModel.GetInstance().paperList[curIndex];
        transform.Find("bg/title").GetComponent<Text>().text = paper.title;
        Transform subTr = transform.Find("subject");
        for (int i = 0; i < subTr.childCount; i++)
        {
            TestPaperItem tPaperItem = ResManager.GetPrefab("TestPaperItem").GetComponent<TestPaperItem>();
            tPaperItem.name = i.ToString();
            tPaperItem.transform.SetParent(subTr);
            tPaperItem.Init(paper.subList[i]);
        }
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
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
    }

    void Redo()
    {
        if (evtRedo != null)
        {
            evtRedo();
        }
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
    }

    public void Dispose()
    {
        comUI.redoClickEvent -= OnReDo;
        comUI.nextClickEvent -= OnNextDo;
        evtFinished = null;
        evtRedo = null;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {

    }

}
