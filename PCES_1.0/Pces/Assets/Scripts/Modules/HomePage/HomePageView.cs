﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomePageView : MonoBehaviour
{
    public enum HomePageEvent
    {
        Enter,//进入游戏
        Exit,//退出游戏
    }
    public Button startButton;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    //HomePageUI homePageUI;
    //CommonUI com;
    void Start()
    {
        //com = UIManager.Instance.GetUI<CommonUI>("CommonUI");
        //com.name = "commonUI";
        //Canvas canvas = FindObjectOfType<Canvas>();
        //com.transform.SetParent(canvas.transform);       
        startButton.onClick.AddListener(StartButtonClick);
    }
    /// <summary>
    /// 跳转到第一阶段
    /// </summary>
    void StartButtonClick()
    {
        startButton.interactable = false;
        Finish();
    }
    void CallBack(int code, string name)
    {
        //GlobalEntity.GetInstance().RemoveListener<int, string>("事件枚举", 当前模块枚举);
    }
    /// <summary>
    /// 当前阶段完成   
    /// </summary>
    void Finish()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLevelLoaded;
        UnityEngine.SceneManagement.SceneManager.LoadScene("ClassRoom");


    }

    private void OnLevelLoaded(UnityEngine.SceneManagement.Scene s, UnityEngine.SceneManagement.LoadSceneMode m)
    {

        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLevelLoaded;
        //通知当前阶段完成
        GlobalEntity.GetInstance().Dispatch<ModelTasks>(FlowModel.mEvent.FlowStepFinished, ModelTasks.HomePage);
        FlowManager.PreInitComm();
    }

    void RemoveListens()
    {

    }
    public void Dispose()
    {
        //销毁、资源释放、监听移除     
        RemoveListens();
        HomePageModel.GetInstance().ClearAllGo();
        Destroy(gameObject);
    }
}
