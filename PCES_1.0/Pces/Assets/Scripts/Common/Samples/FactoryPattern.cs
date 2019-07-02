using UnityEngine;
using System.Collections;

/// <summary>
/// 定义飞行类接口
/// </summary>
public interface IFly
{
    void fly();
}

public class Bird : IFly
{
    public void fly()
    {
        Debug.Log("I can Fly!");
    }
}

/// <summary>
/// 定义陆地上跑的
/// </summary>
public interface IRun
{
    void Run();
}

public class Dog : IRun
{
    public void Run()
    {
        Debug.Log("I can Run!");
    }
}

/// <summary>
/// 宠物狗
/// </summary>
public class Pet : Dog
{

}


public class BirdFactory
{
    public IFly CreateFly()
    {
        return new Bird();
    }
}


public class DogFactory
{
    public IRun CreateRun()
    {
        return new Dog();
    }

    //如果新增了宠物狗
    public Pet CreatePet()
    {
        return new Pet();
    }
}



public class FactoryPattern : MonoBehaviour {

    DogFactory aFactory;
    //BirdFactory bFactory;

    void Start () {
        aFactory = new DogFactory();
        aFactory.CreateRun().Run();

        aFactory.CreatePet().Run();  //增加宠物狗不影响其他创建代码

        //bFactory = new BirdFactory();
    }


    void Update () {

    }
}
