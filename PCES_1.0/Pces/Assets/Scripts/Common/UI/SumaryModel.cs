using System.Collections.Generic;
using System.Xml;

public class SumaryModel : SingleTemplate<SumaryModel>
{
    List<SumText> sumList = new List<SumText>();
    //public void Init()
    //{
    //    ParseXml("/TestPaper/sum.xml", "summary/Items");
    //}
    public void ParseXml(string path, string findstr)
    {
        XmlDocument xmlDoc = new XmlDocument();
        //TextAsset XMLFile = Resources.Load<TextAsset>(path);
        //path = Application.streamingAssetsPath + path;
        //string XMLFile = File.ReadAllText(path);
        xmlDoc.LoadXml(path);
        XmlNodeList courseList = xmlDoc.SelectNodes(findstr);
        foreach (XmlNode course in courseList)
        {
            XmlNode itemRootNode = course;
            XmlElement bookElement = (XmlElement)itemRootNode;
            XmlNodeList itemList = course.SelectNodes("item");
            for (int i = 0; i < itemList.Count; i++)
            {
                SumText st = new SumText();
                XmlElement itemElement = (XmlElement)itemList[i];
                string title = itemElement.GetAttributeNode("title").InnerXml;
                string content = itemElement.GetAttributeNode("options").InnerXml;
                st.title = title;
                st.content = content;
                sumList.Add(st);
                //Debug.Log(title + "       " + content);
            }
        }
    }
    public SumText GetContent(int level)
    {
        return sumList[level - 1];
    }
}
public class SumText
{
    public string title;
    public string content;
    public SumText()
    {

    }
}
