
using UnityEngine;
using System.Collections;


/// <summary>
/// 点击射线检测事件
/// </summary>
public class ClickDispatcher : MonoBehaviour
{

    public enum mEvent
    {
        DoClick,    //点击事件
    }

    void Awake()
    {
        EnableClick = true;
    }

    private static ClickDispatcher instance;
    public static ClickDispatcher Inst
    {
        get
        {
            if (instance == null)
            {
                GameObject clickDisp = new GameObject("ClickDispatcher");
                instance = clickDisp.AddComponent<ClickDispatcher>();
            }
            return instance;
        }
    }
    public Camera cam;
    private bool _doClick;
    private ClickedObj objClicked;  //被点中的物体


    public virtual bool DoClick
    {
        set
        {
            _doClick = value;
            if (_doClick)  //处理点击操作
            {
                _doClick = false;

                if (!cam)
                {
                    return;
                }
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, 1000))
                {

                    if (objClicked == null)
                    {
                        objClicked = new ClickedObj();
                    }
                    objClicked.objname = hitInfo.collider.gameObject.name;
                    objClicked.objLayer = hitInfo.collider.gameObject.layer;
                    objClicked.go = hitInfo.collider.gameObject;
                    objClicked.Distance = Vector3.Distance(objClicked.go.transform.position, cam.transform.position);
                    //                   Debug.Log(hitInfo.collider.name);
                    GlobalEntity.GetInstance().Dispatch<ClickedObj>(mEvent.DoClick, objClicked);
                }
            }
        }
    }

    /// <summary>
    /// 设置当前摄像机
    /// </summary>
    /// <param name="_cam"></param>
    public void SetCurrentCamera(Camera _cam)
    {
        cam = _cam;
    }

    /// <summary>
    /// 启用/禁用点击
    /// </summary>
    private bool _enableClick;
    public bool EnableClick
    {
        get
        {
            return _enableClick;
        }
        set
        {
            _enableClick = value;
        }
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && EnableClick)
        {
            DoClick = true;
        }
    }
}

/// <summary>
/// 点击物体类
/// </summary>
public class ClickedObj
{
    public string objname;  //被点击中的物体名称

    public int objLayer;

    public GameObject go;

    public float Distance;

    public ClickedObj()
    { }
}
