using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AllButtons : MonoBehaviour
{
    private GameObject temporarystorage;

    private GameObject earth;

    void Awake()
    {
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickJiBenGaiNian, _BasicConcepts2D);
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickDaZhouAndDaYang, _LandAndOcean2D);
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickDaLu, _MainLand2D);
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickBanDao, _Peninsula2D);
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickDaoyu, _Islands2D);
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickHaiXia, _Gullets2D);
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickDaZhou, _Continents2D);
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickDaYang, _Oceans2D);
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickJinWeiWang, _Continents_Graticules2D);
        GlobalEntity.GetInstance().AddListener(MainUIModel.CommandFlag.ClickDaZhouFenJieXian, _Boundaries2D);
    }

    void Start()
    {
        temporarystorage = GameObject.Find("TemporaryStorage");


        transform.Find("TouPing").GetComponent<Button>().onClick.AddListener(TouPing);

        transform.Find("VR_2D").GetComponent<Button>().onClick.AddListener(VR_2DOpen);

        transform.Find("VR_3D").GetComponent<Button>().onClick.AddListener(VR_3DOpen);

        transform.Find("AR_2D").GetComponent<Button>().onClick.AddListener(AR_2DOpen);

        transform.Find("AR_3D").GetComponent<Button>().onClick.AddListener(AR_3DOpen);

        transform.Find("Quit").GetComponent<Button>().onClick.AddListener(Quitquit);

        transform.Find("BasicConcepts").GetComponent<Button>().onClick.AddListener(_BasicConcepts);
        transform.Find("MainLand").GetComponent<Button>().onClick.AddListener(_MainLand);
        transform.Find("Peninsula").GetComponent<Button>().onClick.AddListener(_Peninsula);
        transform.Find("Islands").GetComponent<Button>().onClick.AddListener(_Islands);
        transform.Find("Gullets").GetComponent<Button>().onClick.AddListener(_Gullets);

        transform.Find("LandAndOcean").GetComponent<Button>().onClick.AddListener(_LandAndOcean);
        transform.Find("Continents").GetComponent<Button>().onClick.AddListener(_Continents);
        transform.Find("Oceans").GetComponent<Button>().onClick.AddListener(_Oceans);
        transform.Find("Continents_Graticules").GetComponent<Button>().onClick.AddListener(_Continents_Graticules);
        transform.Find("Boundaries").GetComponent<Button>().onClick.AddListener(_Boundaries);

    }
    bool display = true;
    public void TouPing()
    {
        if (display == true)
        {
            transform.Find("VR_2D").localScale = new Vector3(1f, 1f, 1f);

            transform.Find("VR_3D").localScale = new Vector3(1f, 1f, 1f);

            transform.Find("AR_2D").localScale = new Vector3(1f, 1f, 1f);

            transform.Find("AR_3D").localScale = new Vector3(1f, 1f, 1f);
            display = false;
            return;
        }
        if (display == false)
        {
            transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

            transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

            transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

            transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);
            display = true;
        }

    }
    public void _LandAndOcean()
    {
        StartCoroutine(Clearing());

        transform.Find("Abstract").GetComponent<Text>().text = "";

        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("MainLand").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Peninsula").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Islands").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Gullets").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Continents").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("Continents_Graticules").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Boundaries").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Oceans").localScale = new Vector3(1f, 1f, 1f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/LandAndOcean"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }

    public void _Boundaries()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);

        GameObject missile = Instantiate(Resources.Load("Prefabs/Boundries_bugs"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        //  GameObject missile = Instantiate(Resources.Load("Prefabs/Boundaries"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject; Boundries_bugs
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }
    public void _Continents_Graticules()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Continents_Graticules"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }
    public void _Oceans()
    {
        StartCoroutine(Clearing());

        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Continents_Graticules").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Boundaries").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Oceans"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }

    /// <summary>
    /// 点击大洋按钮
    /// </summary>
    public void _Oceans2D()
    {
        Debug.Log("Click Oceans Handler");
        StartCoroutine(Clearing());

        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Oceans"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
    }

    public void _Continents()
    {
        StartCoroutine(Clearing());

        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Continents_Graticules").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("Boundaries").localScale = new Vector3(1f, 1f, 1f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Continents"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }
    public void _Gullets()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Gullets"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }
    public void _Islands()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Islands"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }
    public void _Peninsula()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Peninsula"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }
    public void _MainLand()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/mainland"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0.146f, 0.1618f, 0.1353f);
        missile.transform.localEulerAngles = new Vector3(0f, -90f, 40f);
        missile.transform.localScale = new Vector3(1f, 1f, 1f) * 0.8f;
    }
    public void _BasicConcepts()
    {
        StartCoroutine(Clearing());
        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("MainLand").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("Peninsula").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("Islands").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("Gullets").localScale = new Vector3(1f, 1f, 1f);

        transform.Find("Continents").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Continents_Graticules").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Boundaries").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Oceans").localScale = new Vector3(0f, 0f, 0f);

        GameObject missile = Instantiate(Resources.Load("Prefabs/BasicConcepts"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.138f);

        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }


    public void _BasicConcepts2D()
    {
        Debug.Log("Clicked 基本概念");
        StartCoroutine(Clearing());
        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        transform.Find("Continents").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Continents_Graticules").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Boundaries").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Oceans").localScale = new Vector3(0f, 0f, 0f);

        GameObject missile = Instantiate(Resources.Load("Prefabs/BasicConcepts"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.138f);

        //missile.transform.localScale = new Vector3(10f, 10f, 10f);
    }

    /// <summary>
    /// 点击大陆
    /// </summary>
    public void _MainLand2D()
    {
        Debug.Log("Clicked 大陆");
        StartCoroutine(Clearing());

        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/mainland"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0.146f, 0.1618f, 0.1353f);
        missile.transform.localEulerAngles = new Vector3(0f, -90f, 40f);
        missile.transform.localScale = new Vector3(1f, 1f, 1f) * 0.8f;
    }

    /// <summary>
    /// 点击半岛
    /// </summary>
    public void _Peninsula2D()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Peninsula"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
    }

    /// <summary>
    /// 点击岛屿
    /// </summary>
    public void _Islands2D()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Islands"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
    }

    /// <summary>
    /// 点击海峡
    /// </summary>
    public void _Gullets2D()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Gullets"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
    }

    /// <summary>
    /// 点击大洲与大洋
    /// </summary>
    public void _LandAndOcean2D()
    {
        StartCoroutine(Clearing());

        transform.Find("TipsBg/Abstract").GetComponent<Text>().text = "";

        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Continents_Graticules").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Boundaries").localScale = new Vector3(0f, 0f, 0f);

        GameObject missile = Instantiate(Resources.Load("Prefabs/LandAndOcean"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
    }

    public void _Continents2D()
    {
        StartCoroutine(Clearing());

        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Continents"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
    }

    public void _Continents_Graticules2D()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);


        GameObject missile = Instantiate(Resources.Load("Prefabs/Continents_Graticules"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
    }

    public void _Boundaries2D()
    {
        StartCoroutine(Clearing());


        transform.Find("VR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("VR_3D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_2D").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AR_3D").localScale = new Vector3(0f, 0f, 0f);

        GameObject missile = Instantiate(Resources.Load("Prefabs/Boundries_bugs"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;

        missile.transform.parent = temporarystorage.transform;
        missile.transform.localPosition = new Vector3(0f, 0.136f, 0.1397f);
        missile.transform.localEulerAngles = new Vector3(0f, 50f, 0f);
    }


    public void VR_2DOpen()
    {

    }

    public void VR_3DOpen()
    {

    }

    public void AR_2DOpen()
    {

    }

    public void AR_3DOpen()
    {

    }

    public void Quitquit()
    {

    }



    public void Rocketstructure()
    {
        //if (temporarystorage.transform.childCount > 0)
        //{
        //    Debug.Log(temporarystorage.transform.childCount);
        //    for (int i = 0; i < temporarystorage.transform.childCount; i++)
        //    {
        //        // DestroyImmediate(temporarystorage.transform.GetChild(i).gameObject);
        //        Destroy(temporarystorage.transform.GetChild(i).gameObject);
        //    }

        //}
        StartCoroutine(Clearing());


        transform.Find("Igniting").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Accelerating").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Decelerating").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("TurnLeft").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("TurnRight").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AirInflation").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Releasing").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("DisplayingDirection").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Lauching").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Separating").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("LabelsShowing").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("Separating").gameObject.GetComponentInChildren<Text>().text = "自动拆分";
        transform.Find("LabelsShowing").GetComponentInChildren<Text>().text = "显示标签";

        GameObject rocket = Instantiate(Resources.Load("Prefabs/RocketStructure"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        rocket.transform.parent = temporarystorage.transform;
        rocket.transform.localPosition = new Vector3(0.27f, 0.13f, 0.08f);
        rocket.transform.localScale = new Vector3(0.016f, 0.016f, 0.016f);

    }
    public void LoaddingRocketEarth()
    {
        //if (temporarystorage.transform.childCount > 0)
        //{
        //    for (int i = 0; i < temporarystorage.transform.childCount; i++)
        //    {
        //        // DestroyImmediate(temporarystorage.transform.GetChild(i).gameObject);
        //        Destroy(temporarystorage.transform.GetChild(i).gameObject);
        //    }
        //}

        StartCoroutine(Clearing());

        transform.Find("Igniting").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Accelerating").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Decelerating").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("TurnLeft").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("TurnRight").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AirInflation").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Releasing").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("DisplayingDirection").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Separating").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("LabelsShowing").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Lauching").localScale = new Vector3(1f, 1f, 1f);


        // temporarystorage = GameObject.Find("TemporaryStorage");
        earth = Instantiate(Resources.Load("Prefabs/RocketEarth"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        earth.transform.parent = temporarystorage.transform;
        earth.transform.localPosition = new Vector3(0.02188686f, -0.27f, 0.16f);
        earth.transform.localScale = new Vector3(4f, 4f, 4f);
    }



    IEnumerator Clearing()
    {
        if (temporarystorage.transform.childCount > 0)
        {
            /*
            for (int i = 0; i < temporarystorage.transform.childCount; i++)
            {
                // DestroyImmediate(temporarystorage.transform.GetChild(i).gameObject);
                DestroyImmediate(temporarystorage.transform.GetChild(i).gameObject);
            }*/
            while (temporarystorage.transform.childCount > 0)
            {
                DestroyImmediate(temporarystorage.transform.GetChild(0).gameObject);
            }
        }
        transform.Find("TipsBg/Abstract").GetComponent<Text>().text = "";
        yield return new WaitForSeconds(0.2f);
    }

    //bool Test()
    //{
    //    return true;
    //}

    // public IEnumerator Experimentofballoon()
    public void Experimentofballoon()
    {
        //    if (temporarystorage.transform.childCount > 0)
        //    {
        //        for (int i = 0; i < temporarystorage.transform.childCount; i++)
        //        {
        //            // DestroyImmediate(temporarystorage.transform.GetChild(i).gameObject);
        //            Destroy(temporarystorage.transform.GetChild(i).gameObject);
        //        }
        //    }
        // yield return new WaitForSeconds(0.2f);
        StartCoroutine(Clearing());

        transform.Find("Igniting").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Accelerating").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("Decelerating").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("TurnLeft").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("TurnRight").localScale = new Vector3(0f, 0f, 0f);


        transform.Find("Lauching").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("Separating").localScale = new Vector3(0f, 0f, 0f);
        transform.Find("LabelsShowing").localScale = new Vector3(0f, 0f, 0f);

        transform.Find("AirInflation").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("Releasing").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("DisplayingDirection").localScale = new Vector3(1f, 1f, 1f);
        transform.Find("DisplayingDirection").GetComponentInChildren<Text>().text = "显示方向";



        GameObject balloonexperiment = Instantiate(Resources.Load("Prefabs/balloonandshsys"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        balloonexperiment.transform.parent = temporarystorage.transform;
        //balloonexperiment.transform.localPosition = new Vector3(0.1261f, 0.17f, 0.11f);
        //balloonexperiment.transform.localEulerAngles = new Vector3(0f, -90f, 0f);

        //GameObject shsys = Instantiate(Resources.Load("Prefabs/shsys"), temporarystorage.transform.position, temporarystorage.transform.rotation) as GameObject;
        //shsys.transform.parent = temporarystorage.transform;
        //shsys.transform.localPosition = new Vector3(0.27f, -2.21f, 3.99f);
        //shsys.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        //shsys.transform.localScale = new Vector3(68f, 68f, 68f);
    }







    void OnDestroy()
    {
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickJiBenGaiNian, _BasicConcepts2D);
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickDaZhouAndDaYang, _LandAndOcean2D);
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickDaLu, _MainLand2D);
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickBanDao, _Peninsula2D);
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickDaoyu, _Islands2D);
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickHaiXia, _Gullets2D);
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickDaZhou, _Continents2D);
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickDaYang, _Oceans2D);
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickJinWeiWang, _Continents_Graticules2D);
        GlobalEntity.GetInstance().RemoveListener(MainUIModel.CommandFlag.ClickDaZhouFenJieXian, _Boundaries2D);
    }

}
