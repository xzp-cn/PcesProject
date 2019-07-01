using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIView : MonoBehaviour
{
    public Transform bottomContent;
    public Transform leftContent;

    public GameObject source;

    private GameObject goJiBenGaiNian;
    private GameObject goDazhouDaYang;
    private GameObject leftUI;   //左侧基本概念按钮组
    private GameObject leftUI2;   //左侧大洲与大洋按钮组
    private GameObject leftSecondUI;  //左侧二级按钮组

    void Awake()
    {
        InitUI();

        //基本概念按钮监听
        UGUIEventListener.Get(goJiBenGaiNian).onClick += OnClickJiBenGaiNian;

        //大洲与大洋按钮监听
        UGUIEventListener.Get(goDazhouDaYang).onClick += OnClickDazhouDaYang;

        //大陆按钮监听
        UGUIEventListener.Get(leftUI.transform.Find("MainLand").gameObject).onClick += OnClickMainLand;
        //半岛按钮监听
        UGUIEventListener.Get(leftUI.transform.Find("Peninsula").gameObject).onClick += OnClickPeninsula;

        //岛屿按钮监听
        UGUIEventListener.Get(leftUI.transform.Find("Islands").gameObject).onClick += OnClickIslands;

        //海峡按钮监听
        UGUIEventListener.Get(leftUI.transform.Find("Gullets").gameObject).onClick += OnClickGullets;

        //大洲按钮监听
        UGUIEventListener.Get(leftUI2.transform.Find("Continents").gameObject).onClick += OnClickContinents;

        //大洋按钮监听
        UGUIEventListener.Get(leftUI2.transform.Find("Oceans").gameObject).onClick += OnClickOceans;

        //经纬网按钮监听
        UGUIEventListener.Get(leftSecondUI.transform.Find("Continents_Graticules").gameObject).onClick += OnClickContinentsGraticules;

        //大洲分界线按钮监听
        UGUIEventListener.Get(leftSecondUI.transform.Find("Boundaries").gameObject).onClick += OnClickBoundaries;
    }

    private void InitUI()
    {
        GameObject temp = Instantiate(source);
        goJiBenGaiNian = temp.transform.Find("BToggleGaiNian").gameObject;
        goJiBenGaiNian.transform.SetParent(bottomContent);

        goDazhouDaYang = temp.transform.Find("BToggleDaZhouDaYang").gameObject;
        goDazhouDaYang.transform.SetParent(bottomContent);

        leftUI = temp.transform.Find("LUIButton1").gameObject;
        leftUI.transform.SetParent(leftContent, true);
        leftUI.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(114.6f, 4.768221f, 0);

        leftUI2 = temp.transform.Find("LUIButton2").gameObject;
        leftUI2.transform.SetParent(leftContent, true);
        leftUI2.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(114.6f, 4.768221f, 0);

        leftSecondUI = temp.transform.Find("LUIButton3").gameObject;
        leftSecondUI.transform.SetParent(leftContent, true);
        leftSecondUI.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(320f, 72.1f, 0);

        leftUI.SetActive(false);
        leftUI2.SetActive(false);
        leftSecondUI.SetActive(false);

        Destroy(temp);
    }

    void Start()
    {
        
    }


    void OnClickMainLand(GameObject go)
    {
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickDaLu);
    }

    void OnClickPeninsula(GameObject go)
    {
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickBanDao);
    }

    void OnClickIslands(GameObject go)
    {
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickDaoyu);
    }

    void OnClickGullets(GameObject go)
    {
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickHaiXia);
    }

    private void OnClickJiBenGaiNian(GameObject go){
        Debug.Log("Click JiBen");
        //显示基本概念一级按钮
        leftUI.SetActive(true);
        System.Array.ForEach(leftUI.GetComponentsInChildren<Toggle>(), (t) => { t.isOn = false; });
        leftUI2.SetActive(false);
        leftSecondUI.SetActive(false);
        goJiBenGaiNian.GetComponent<Toggle>().isOn = true;
        goDazhouDaYang.GetComponent<Toggle>().isOn = false;
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickJiBenGaiNian);
    }

    private void OnClickDazhouDaYang(GameObject go)
    {
        Debug.Log("Click DazhouDaYang");
        leftUI.SetActive(false);
        leftUI2.SetActive(true);
        System.Array.ForEach(leftUI2.GetComponentsInChildren<Toggle>(), (t) => { t.isOn = false; });
        goJiBenGaiNian.GetComponent<Toggle>().isOn = false;
        goDazhouDaYang.GetComponent<Toggle>().isOn = true;
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickDaZhouAndDaYang);
    }

    private void OnClickBoundaries(GameObject go)
    {
        Debug.Log("Click Boundaries");
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickDaZhouFenJieXian);
    }

    private void OnClickContinentsGraticules(GameObject go)
    {
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickJinWeiWang);
    }

    private void OnClickContinents(GameObject go)
    {
        Debug.Log("Click Continents");
        leftSecondUI.SetActive(true);
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickDaZhou);
    }

    private void OnClickOceans(GameObject go)
    {
        Debug.Log("Click Oceans");
        leftSecondUI.SetActive(false);
        GlobalEntity.GetInstance().Dispatch(MainUIModel.CommandFlag.ClickDaYang);
    }

    void OnDestroy() {
        UGUIEventListener.Get(goJiBenGaiNian).onClick -= OnClickJiBenGaiNian;
        UGUIEventListener.Get(goDazhouDaYang).onClick -= OnClickDazhouDaYang;

        UGUIEventListener.Get(leftUI.transform.Find("MainLand").gameObject).onClick -= OnClickMainLand;
        UGUIEventListener.Get(leftUI.transform.Find("Peninsula").gameObject).onClick -= OnClickPeninsula;
        UGUIEventListener.Get(leftUI.transform.Find("Islands").gameObject).onClick -= OnClickIslands;
        UGUIEventListener.Get(leftUI.transform.Find("Gullets").gameObject).onClick -= OnClickGullets;
        UGUIEventListener.Get(goDazhouDaYang).onClick -= OnClickGullets;
        UGUIEventListener.Get(leftSecondUI.transform.Find("Continents_Graticules").gameObject).onClick -= OnClickContinentsGraticules;
        UGUIEventListener.Get(leftSecondUI.transform.Find("Boundaries").gameObject).onClick -= OnClickBoundaries;
        UGUIEventListener.Get(leftUI2.transform.Find("Continents").gameObject).onClick -= OnClickContinents;
        UGUIEventListener.Get(leftUI2.transform.Find("Oceans").gameObject).onClick -= OnClickOceans;
    }
}
