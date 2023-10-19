using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    float _counter;
    FSM _fsm;
    public Idle(FSM fsm)
    {
        _fsm = fsm;
    }
    public void OnEnter()
    {
        _counter = 3;
        Debug.Log("enter idle");
    }
    public void OnUpdate()
    {
        Debug.Log("estoy en idle, contando" + _counter);
        _counter -= Time.deltaTime;
        if(_counter<=0)
        {
            _fsm.ChangeState("Patrol");
        }

    }

    public void OnExit()
    {
        Debug.Log("exit idle");

    }


}
