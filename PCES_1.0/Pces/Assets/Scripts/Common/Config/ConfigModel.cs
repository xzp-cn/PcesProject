using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

namespace future3d.unityLibs
{
    /// <summary>
    /// 读取配置模板类
    /// </summary>
    public class ConfigBase<T>
    {

        private static T instance;

        public static T GetInstance()
        {
            if (instance == null)
            {
                instance = Activator.CreateInstance<T>();
            }
            return instance;
        }

        /// <summary>
        /// 加载XML配置
        /// </summary>
        /// <param name="path"></param>
        /// <param name="findstr">查询字符串，“根节点/父节点/子节点”</param>"
        public virtual void LoadResXml(string path, string findstr = "") { }

    }

    /// <summary>
    /// 课程配置(使用举例)
    /*
     <?xml version="1.0"?>
    <courses>
        <course>语文</course>
        <course>数学</course>
        <course>英语</course>
    </courses>
     */
    /// </summary>
    class CourseModel : ConfigBase<CourseModel>
    {
        public CourseModel()
        {

        }

        /// <summary>
        /// 调用举例：CourseModel.GetInstance().LoadResXml("course", "courses/course");
        /// </summary>
        /// <param name="path"></param>
        /// <param name="findstr"></param>
        public override void LoadResXml(string path, string findstr = "")
        {
            XmlDocument xmlDoc = new XmlDocument();
            TextAsset XMLFile = Resources.Load<TextAsset>(path);

            xmlDoc.LoadXml(XMLFile.text);

            XmlNodeList courseList = xmlDoc.SelectNodes(findstr);
            foreach (XmlNode course in courseList)
            {
                Debug.Log(course.InnerText);
            }
        }
    }
}
