using System;
using System.Collections;
using UnityEngine;

public class TestWWW : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //www = new WWW("http://192.168.3.6:8998/TestScore.php?userName=未来立体&userEmail=qq@163.com");
        // www = new WWW("http://itvlab.ccnu.edu.cn/injson.html?id=100001&startdate=1561537917000&enddate=1561537937000&score=89");
        //www = new WWW("http://192.168.3.6:8998/TestScore.php?userName=未来立体&userEmail=qq@163.com");
        //http://itvlab.ccnu.edu.cn/injson.html?id=100001&startdate=1561537917000&enddate=1561537937000&score=89
    }

    private void OnGUI()
    {
        if (GUILayout.Button("sEND"))
        {

            StartCoroutine(Test());
        }
    }

    IEnumerator Test()
    {
        //string url = WWW.EscapeURL("http://itvlab.ccnu.edu.cn/injson.php?id=10001&startdate=1568863361000&enddate=1568863501793&score=65");
        string url = "http://192.168.3.6:8998/TestScore.php?id=10001查&startdate=1568863361000尚&enddate=1568863501793君&score=65";
        //url = url.Replace("http://localhost/", "");
        //string url = "www.baidu.com";
        //UnityWebRequest uwp = UnityWebRequest.Get(url);
        Uri uri = new Uri(url);
        WWW www = new WWW(uri.AbsoluteUri);
        Debug.Log("url before:  " + url);
        //WWW www = new WWW(url);
        //uwp.timeout = 10;
        yield return www;
        //yield return uwp.SendWebRequest();
        //if (uwp.isNetworkError || uwp.isHttpError)
        //{
        //    Debug.Log(uwp.error);
        //}
        //else
        //{ 
        //    Debug.Log("image arrived");
        //}

        Debug.Log("error:  " + www.error);
        Debug.Log("return:  " + www.text);
        Debug.Log("url:   " + www.url);
        // Debug.Log("URI:   " + uri.AbsoluteUri);

    }

}
