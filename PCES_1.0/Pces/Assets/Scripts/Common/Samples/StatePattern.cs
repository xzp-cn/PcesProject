using UnityEngine;
using System.Collections;

public abstract class BaseState{
    public abstract void process();

    public abstract void nextState();
}

public class IdleState : BaseState
{
    public override void process()
    {
        //执行播放Idle动画
    }

    public override void nextState()
    {
        //执行下个状态
    }
}

public class WalkState : BaseState
{
    public override void process()
    {
        //执行播放Walk动画
    }

    public override void nextState()
    {
        //执行下个状态
    }
}

public class StateSys
{
    public void SetState(BaseState state)
    {
        state.process();
        state.nextState();
    }
}

public class StatePattern : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
