using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class GameOverCtrl : MonoBehaviour
{

    Button again;
    void Awake()
    {
        again = transform.Find("Button").GetComponent<Button>();
        again.onClick.AddListener(PlayAgain);
    }
    public void Init()
    {
        //分数统计界面UI显示
        Text txt = again.transform.Find("Text").GetComponent<Text>();
        txt.text = "再来一次";
        string[] str = TestPaperModel.GetInstance().TotalCount().Split('_');
        int right = int.Parse(str[0]);
        int wrong = int.Parse(str[1]);
        double a = ((double)right) / (right + wrong);
        double ratio = Math.Round(a, 4) * 100;
        transform.Find("Text").GetComponent<Text>().text = @"答对题目: " + right + "\n" + "答错题目: " + wrong + "\n" + "正确率: " + ratio.ToString() + "%";
        Settle();
    }
    /// <summary>
    /// 结算
    /// </summary>
    void Settle()
    {
        //当前场景处理
        SentenceExpressionModel.GetInstance().Jiaoshi().SetActive(true);
        //模块数据的处理
        //分数统计显示
        gameObject.SetActive(true);
        //分数清零

    }
    /// <summary>
    /// 再玩一次
    /// </summary>
    void PlayAgain()
    {
        gameObject.SetActive(false);

        GlobalEntity.GetInstance().Dispatch<ModelTasks>(FlowModel.mEvent.FlowStepFinished, ModelTasks.SentenceExpression);
        UIManager.Instance.ClearDic();
        HighLightCtrl.GetInstance().cameras = null;
        //FlowTask ft = FlowModel.GetInstance().CurrFlowTask;
        //StringBuilder sb = new StringBuilder("Prefabs/").Append(ft.FlowEnumID.ToString()).Append("/").Append(ft.FlowEnumID.ToString());
        //GameObject go = ResManager.GetPrefab(sb.ToString());
        //FlowModel.GetInstance().PushPrefabToMem(ft.FlowEnumID.ToString(), go);

        PeopleManager.Instance.Reset();
    }
}
