using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class TestPaperModel : SingleTemplate<TestPaperModel>
{
    public List<Paper> paperList = new List<Paper>();

    public TestPaperModel()
    {
        ParseXml("TestPaper/TestPaper", "Subject/Items");
    }

    /// <summary>
    /// 调用举例：TestPaperModel.GetInstance().ParseXml("TestPaper/TestPaper", "Subject");   
    /// </summary>
    /// <param name="path"></param>
    /// <param name="findstr"></param>
    /// <returns></returns>
    public void ParseXml(string path, string findstr = "")
    {
        XmlDocument xmlDoc = new XmlDocument();
        TextAsset XMLFile = Resources.Load<TextAsset>(path);
        xmlDoc.LoadXml(XMLFile.text);
        XmlNodeList courseList = xmlDoc.SelectNodes(findstr);
        //Debug.Log(courseList.Count);
        foreach (XmlNode course in courseList)
        {
            Paper paper = new Paper();
            XmlNode itemRootNode = course;
            XmlElement bookElement = (XmlElement)itemRootNode;
            XmlAttribute attr = bookElement.GetAttributeNode("title");
            paper.title = attr.InnerText;//大标题
            XmlNodeList itemList = course.SelectNodes("item");
            for (int i = 0; i < itemList.Count; i++)
            {
                PaperItem sItem = new PaperItem();
                XmlElement itemElement = (XmlElement)itemList[i];
                sItem.question = itemElement.GetAttributeNode("question").InnerXml;
                sItem.options = itemElement.GetAttributeNode("options").InnerXml.Split('|');
                sItem.answer = itemElement.GetAttributeNode("answer").InnerXml;
                paper.itemList.Add(sItem);
            }
            paperList.Add(paper);
        }
    }

    /// <summary>
    /// 正确和错误计数
    /// </summary>
    /// <returns></returns>
    public string TotalCount()
    {
        int right_num = 0;
        for (int i = 0; i < paperList.Count; i++)
        {
            right_num += paperList[i].rightNum;
        }

        int wrong_num = 0;
        for (int i = 0; i < paperList.Count; i++)
        {
            wrong_num += paperList[i].wrongNum;
        }
        return right_num + "_" + wrong_num;
    }
}
public class PaperItem//单个题目
{
    public string question;//问题
    public string[] options = new string[4];//选项
    public string answer;//答案   
    public PaperItem()
    {
    }
}
public class Paper//阶段
{
    public string title;
    public List<PaperItem> itemList = new List<PaperItem>();
    public int rightNum;
    public int wrongNum;
    public Paper()
    {

    }
    public void ResetData()
    {
        rightNum = 0;
        wrongNum = 0;
    }
}