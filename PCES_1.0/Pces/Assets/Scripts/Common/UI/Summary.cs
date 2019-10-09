using UnityEngine;
using UnityEngine.UI;
public class Summary : MonoBehaviour
{
    Text title, content;
    // public System.Action redo, next;
    void Awake()
    {
        //SumaryModel.GetInstance().Init();
        title = transform.Find("bg/title/text").GetComponent<Text>();
        content = transform.Find("bg/content").GetComponent<Text>();

        CommonUI comUI = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        Transform canvas = comUI.transform.parent;
        transform.SetParent(canvas.transform, false);
    }
    public void ChangeContent(int level)
    {
        SumText sum = SumaryModel.GetInstance().GetContent(level);
        title.text = sum.title;
        content.text = "\u3000\u3000" + sum.content;
        Show(true);
    }
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
    public void Dispose()
    {
        //redo = next = null;
        Destroy(gameObject);
    }
}
