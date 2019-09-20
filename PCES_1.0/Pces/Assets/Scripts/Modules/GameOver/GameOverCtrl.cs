using System;
using System.Collections;
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
        string text = @"答对题目: " + right + "\n" + "答错题目: " + wrong + "\n" + "正确率: " + ratio.ToString() + "%";
        transform.Find("Text").GetComponent<Text>().text = text;

        //发送统计分数时间戳       
        string guid = BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0).ToString();

        //格式：guid+开始时间+结束时间+分数
        string msg = "id=" + guid + "&" + HomePageModel.GetInstance().SendTimeStampStr() + "&score=" + ratio.ToString();
        StartCoroutine(SendMsg(msg));
        Settle();
    }

    IEnumerator SendMsg(string str)
    {
        string path = Application.streamingAssetsPath + "/ip.txt";
        Debug.Log("GameOverCtrl::SendMsg()::streamingAssets::path " + path);
        WWW wwwIP = new WWW(path);//加载本地文件
        yield return wwwIP;
        string txtIP = string.Empty;
        txtIP = wwwIP.text;
        Debug.Log("GameOverCtrl::SendMsg():: txt = " + txtIP);
        WWW www = new WWW(txtIP + str);//请求消息
        yield return www;
        Debug.Log(www.text);
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
