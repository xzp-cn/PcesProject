using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
public class ObjectsManager : MonoBehaviour
{
    public static ObjectsManager instanse;
    public List<PropsObject> propList = new List<PropsObject>();
    const int TotalNum = 11;    //道具总数

    private void Awake()
    {
        instanse = this;
        transform.localPosition = new Vector3(0, 0, 1000);
        Init();
    }
    public void CtrlObjectsShow(string objName, bool isShow)
    {
        Transform obj = transform.Find(objName);
        if (obj != null)
        {
            obj.gameObject.SetActive(isShow);
        }
        else
        {
            Debug.LogError("Objecs下没有该物品");
        }
    }
    public void Init()
    {

        InitReinforcement();
        InitTuka();
        InitOthers();
    }
    void InitReinforcement()
    {
        for (int i = 0; i < TotalNum; i++)
        {
            PropsType ptype;
            if (i < 5)
            {
                ptype = PropsType.reinforcement;
            }
            else if (i >= 5 && i < 8)
            {
                ptype = PropsType.neutralStimulator;
            }
            else
            {
                ptype = PropsType.negReinforcement;
            }
            PropsTag tag = (PropsTag)i;
            string path = "Prefabs/Objects/" + tag.ToString();
            GameObject go = ResManager.GetPrefab(path);
            go.name = tag.ToString();
            go.transform.SetParent(transform, false);

            PropsObject po = go.GetComponent<PropsObject>();
            if (po == null)
            {
                po = go.AddComponent<PropsObject>();
            }
            propList.Add(po);
            po.pData = new PropsData(tag.ToString(), i, ptype, GetCnNameOfObject(tag.ToString()));
        }
    }
    /// <summary>
    /// 初始化图卡
    /// </summary>
    public void InitTuka()
    {
        string[] ignores = { "tuka_hua", "tuka_shumu", "tuka_xiaogou", "tuka_orangeJuice" };    //忽略
        Texture[] tukaTexs = Resources.LoadAll<Texture>("Images/tuka");

        for (int i = 0, j = propList.Count; i < tukaTexs.Length; i++)
        {
            if (!tukaTexs[i].name.StartsWith("tuka"))
            {
                continue;
            }

            int index = System.Array.FindIndex(ignores, (ig) =>
            {
                return ig == tukaTexs[i].name;
            });
            if (index >= 0)
            {
                continue;
            }

            GameObject tuka = ResManager.GetPrefab("Prefabs/Objects/tuka");
            tuka.name = tukaTexs[i].name;
            tuka.transform.SetParent(transform, false);
            tuka.transform.localScale = Vector3.one;
            Renderer render = tuka.GetComponent<Renderer>();
            render.materials[1] = new Material(Shader.Find("Standard"));
            render.materials[1].name = "mat_" + tuka.name;
            render.materials[1].mainTexture = tukaTexs[i];

            PropsObject tukObj = tuka.GetComponent<PropsObject>();
            if (tukObj == null)
            {
                tukObj = tuka.AddComponent<PropsObject>();
                propList.Add(tukObj);
                tukObj.pData = new PropsData(tuka.name, j++, PropsType.Tuka, GetCnNameOfObject(tag.ToString()));
            }
        }
        Debug.Log("ObjectsManager.InitTuka(): propList {Nums} " + propList.Count);
    }
    void InitOthers()
    {
        int startIndex = 22;//通用沟通本
        PropsTag tag = (PropsTag)System.Enum.ToObject(typeof(PropsTag), startIndex);
        string path = "Prefabs/Objects/" + tag.ToString();
        GameObject go = ResManager.GetPrefab(path);
        go.name = tag.ToString();
        go.transform.SetParent(transform, false);
        PropsObject po = go.GetComponent<PropsObject>();
        if (po == null)
        {
            po = go.AddComponent<PropsObject>();
        }
        propList.Add(po);
        po.pData = new PropsData(tag.ToString(), startIndex, PropsType.others, GetCnNameOfObject(tag.ToString()));

        startIndex++;
        string[] judais = new string[] { "judai_woyao", "judai_wokanjian", "tuka_shumu", "tuka_hua", "tuka_xiaogou" };
        for (int i = 0; i < judais.Length; i++)
        {
            Texture texture = ResManager.GetTexture("Images/tuka/" + judais[i]);
            GameObject tuka = ResManager.GetPrefab("Prefabs/Objects/tuka");
            tuka.name = texture.name;
            tuka.transform.SetParent(transform, false);
            tuka.transform.localScale = Vector3.one;
            Renderer render = tuka.GetComponent<Renderer>();
            render.materials[1] = new Material(Shader.Find("Standard"));
            render.materials[1].name = "mat_" + tuka.name;
            render.materials[1].mainTexture = texture;

            PropsObject tukObj = tuka.GetComponent<PropsObject>();
            if (tukObj == null)
            {
                tukObj = tuka.AddComponent<PropsObject>();
                propList.Add(tukObj);
                //startIndex += i;
                tag = (PropsTag)System.Enum.ToObject(typeof(PropsTag), startIndex);
                tukObj.pData = new PropsData(tuka.name, startIndex++, PropsType.Tuka, GetCnNameOfObject(tag.ToString()));

                //Debug.Log(startIndex + "   " + tag.ToString());
            }
        }
    }

    public PropsObject GetProps(int index)
    {
        //Debug.LogAssertion(propList.Count);
        return propList[index];
    }
    /// <summary>
    /// 获得道具中文名字
    /// </summary>
    private Dictionary<string, string> objectCns = new Dictionary<string, string>();
    public string GetCnNameOfObject(string enName)
    {
        if (objectCns.Count == 0)
        {
            FieldInfo[] fields = typeof(PropsTag).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            int i = 0;
            foreach (FieldInfo fi in fields)
            {
                DescriptionAttribute descriptionAttribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true)[0] as DescriptionAttribute;
                string cnName = descriptionAttribute.Description;
                objectCns.Add(ConvertToPropsTag(i).ToString(), cnName);
                i++;
            }
        }
        if (!objectCns.ContainsKey(enName))
        {
            return "";
        }
        return objectCns[enName];
    }
    /// <summary>
    /// 获取老师手旁边的TY强化物
    /// </summary>
    /// <returns></returns>
    public GameObject GetQHW()
    {
        GameObject qhw = ResManager.GetPrefab("Prefabs/QHW/TY_QHW");
        //qhw.transform.SetParent(transform, false);
        qhw.name = "QHW";
        return qhw;
    }

    public GameObject GetdeskTuka()
    {
        return ResManager.GetPrefab("Prefabs/Objects/deskTuka");
    }
    private PropsTag ConvertToPropsTag(int i)
    {
        PropsTag pt = (PropsTag)i;
        return pt;
    }
}

public class PropsObject : MonoBehaviour
{
    [SerializeField]
    public PropsData pData;
    private void Awake()
    {
    }
    public void setPos(Vector3 pos)
    {
        transform.localPosition = pos;
    }

    private void OnDestroy()
    {
        pData = null;
    }
}

/// <summary>
/// 道具数据
/// </summary>
public class PropsData
{
    public string name;
    public int id;
    public PropsType pType;
    public string name_cn;
    public PropsData(string _name, int _id, PropsType _pType, string _name_cn)
    {
        name = _name;
        id = _id;
        pType = _pType;
        name_cn = _name_cn;
    }
    //~PropsData()
    //{
    //}
}
public enum PropsType
{
    /// <summary>
    /// 强化物
    /// </summary>
    reinforcement,
    /// <summary>
    /// 负强化物
    /// </summary>
    negReinforcement,
    /// <summary>
    /// 中性刺激物
    /// </summary>
    neutralStimulator,

    /// <summary>
    /// 图卡
    /// </summary>
    Tuka,
    /// <summary>
    /// 其他类型物品
    /// </summary>
    others,
}

[System.Flags]//
public enum PropsTag : int
{
    /// <summary>
    /// 巧克力
    /// </summary>
    [Description("巧克力")]
    chocolate = 0,
    /// <summary>
    /// 饼干
    /// </summary>
    [Description("海苔饼干")]
    biscuit = 1,
    /// <summary>
    /// 薯片
    /// </summary>
    [Description("薯片")]
    chips = 2,
    /// <summary>
    /// 橙汁
    /// </summary>
    //[Description("橙汁")]
    //orangeJuice = 3,
    /// <summary>
    /// 小汽车
    /// </summary>
    [Description("小汽车")]
    car = 3,
    /// <summary>
    /// 香蕉
    /// </summary>
    [Description("香蕉")]
    banana = 4,
    /// <summary>
    /// 苹果
    /// </summary>
    [Description("苹果")]
    apple = 5,
    /// <summary>
    /// 雪饼
    /// </summary>
    [Description("雪饼")]
    snowBiscuit = 6,
    /// <summary>
    /// 牛奶
    /// </summary>
    [Description("牛奶")]
    milk = 7,
    /// <summary>
    /// 帽子
    /// </summary>
    [Description("帽子")]
    hat = 8,
    /// <summary>
    /// 积木
    /// </summary>
    [Description("积木")]
    juggle = 9,
    /// <summary>
    ///故事书
    /// </summary>
    [Description("故事书")]
    storyBooks = 10,
    /// <summary>
    /// 图卡苹果
    /// </summary>
    [Description("图卡苹果")]
    tuka_apple = 11,
    /// <summary>
    /// 图卡香蕉
    /// </summary>
    [Description("图卡香蕉")]
    tuka_banana = 12,
    /// <summary>
    /// 图卡饼干
    /// </summary
    [Description("图卡饼干")]
    tuka_biscuit = 13,
    /// <summary>
    /// 图卡小汽车
    /// </summary>
    [Description("图卡小汽车")]
    tuka_car = 14,
    /// <summary>
    /// 图卡薯片
    /// </summary>
    [Description("图卡薯片")]
    tuka_chips = 15,
    /// <summary>
    /// 图卡巧克力
    /// </summary>
    [Description("图卡巧克力")]
    tuka_chocolate = 16,
    /// <summary>
    /// 图卡帽子
    /// </summary>
    [Description("图卡帽子")]
    tuka_hat = 17,
    /// <summary>
    /// 图卡积木
    /// </summary>
    [Description("图卡积木")]
    tuka_juggle = 18,
    /// <summary>   
    /// 图卡牛奶
    /// </summary>
    [Description("图卡牛奶")]
    tuka_milk = 19,
    ///// <summary>
    ///// 图卡橙汁
    ///// </summary>
    //[Description("图卡橙汁")]
    //tuka_orangeJuice = 15,
    /// <summary>
    /// 图卡雪饼
    /// </summary>
    [Description("图卡雪饼")]
    tuka_snowBiscuit = 20,
    /// <summary>
    /// 图卡故事书
    /// </summary>
    [Description("图卡故事书")]
    tuka_storyBooks = 21,
    /// <summary>
    /// 沟通本
    /// </summary>
    [Description("沟通本")]
    TY_GTB = 22,
    /// <summary>
    /// 句带
    /// </summary>
    [Description("我要句带")]
    judai_woyao = 23,
    [Description("我看见句带")]
    judai_wokanjian = 24,
    //[Description("我要句带正方向")]
    //judai_woyao_normal = 23,
    [Description("树木")]
    tuka_shumu = 25,
    [Description("花")]
    tuka_hua = 26,
    [Description("小狗")]
    tuka_xiaogou = 27,
}
