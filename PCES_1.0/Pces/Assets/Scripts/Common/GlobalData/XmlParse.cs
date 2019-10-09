using System.Collections;
using UnityEngine;

public class XmlParse : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ReadXml("/TestPaper/TestPaper.xml", "Subject/Items", TestPaperModel.GetInstance().ParseXml));
        StartCoroutine(ReadXml("/TestPaper/sum.xml", "summary/Items", SumaryModel.GetInstance().ParseXml));
    }
    IEnumerator ReadXml(string path, string findstr, System.Action<string, string> callback)
    {
        path = Application.streamingAssetsPath + path;
        WWW www = new WWW(path);
        yield return www;
        callback(www.text, findstr);
    }
}
