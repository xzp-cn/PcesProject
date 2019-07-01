using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    public static ObjectsManager instanse;
    public List<PropsObject> propList = new List<PropsObject>();
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
        for (int i = 0; i < 12; i++)
        {
            PropsType ptype;
            if (i < 6)
            {
                ptype = PropsType.reinforcement;
            }
            else if (i >= 6 && i < 9)
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
            //go.SetActive(false);
            PropsObject po = go.GetComponent<PropsObject>();
            if (po == null)
            {
                po = go.AddComponent<PropsObject>();
            }
            po.pData = new PropsData(tag.ToString(), i, ptype);
            propList.Add(po);
        }

        InitTuka();
    }

    /// <summary>
    /// 初始化图卡
    /// </summary>
    public void InitTuka()
    {
        string[] ignores = { "tuka_hua", "tuka_shumu", "tuka_xiaogou" };    //忽略
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
                tukObj.pData = new PropsData(tuka.name, j++, PropsType.Tuka);
                propList.Add(tukObj);
            }
        }
        Debug.Log("ObjectsManager.InitTuka(): propList {Nums} " + propList.Count);
    }

    public PropsObject GetProps(int index)
    {
        return propList[index];
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
}

/// <summary>
/// 道具数据
/// </summary>
public class PropsData
{
    public string name;
    public int id;
    public PropsType pType;
    public PropsData(string _name, int _id, PropsType _pType)
    {
        name = _name;
        id = _id;
        pType = _pType;
    }
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
}
public enum PropsTag
{
    /// <summary>
    /// 巧克力
    /// </summary>
    chocolate = 0,
    /// <summary>
    /// 饼干
    /// </summary>
    biscuit = 1,
    /// <summary>
    /// 薯片
    /// </summary>
    chips = 2,
    /// <summary>
    /// 橙汁
    /// </summary>
    orangeJuice = 3,
    /// <summary>
    /// 小汽车
    /// </summary>
    car = 4,
    /// <summary>
    /// 香蕉
    /// </summary>
    banana = 5,
    /// <summary>
    /// 苹果
    /// </summary>
    apple = 6,
    /// <summary>
    /// 雪饼
    /// </summary>
    snowBiscuit = 7,
    /// <summary>
    /// 牛奶
    /// </summary>
    milk = 8,
    /// <summary>
    /// 帽子
    /// </summary>
    hat = 9,
    /// <summary>
    /// 积木
    /// </summary>
    juggle = 10,
    /// <summary>
    ///故事书
    /// </summary>
    storyBooks = 11,

    /// <summary>
    /// 图卡巧克力
    /// </summary>
    tuka_chocolate = 12,
    /// <summary>
    /// 图卡饼干
    /// </summary>
    tuka_biscuit = 13,
    /// <summary>
    /// 图卡薯片
    /// </summary>
    tuka_chips = 14,
    /// <summary>
    /// 图卡橙汁
    /// </summary>
    tuka_orangeJuice = 15,
    /// <summary>
    /// 图卡小汽车
    /// </summary>
    tuka_car = 16,
    /// <summary>
    /// 图卡香蕉
    /// </summary>
    tuka_banana = 17,
    /// <summary>
    /// 图卡苹果
    /// </summary>
    tuka_apple = 18,
    /// <summary>
    /// 图卡雪饼
    /// </summary>
    tuka_snowBiscuit = 19,
    /// <summary>
    /// 图卡牛奶
    /// </summary>
    tuka_milk = 20,
    /// <summary>
    /// 图卡帽子
    /// </summary>
    tuka_hat = 21,
    /// <summary>
    /// 图卡积木
    /// </summary>
    tuka_juggle = 22,
    /// <summary>
    /// 图卡故事书
    /// </summary>
    tuka_storyBooks = 23
}
