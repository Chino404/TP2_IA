using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    float _counter;
    FSM _fsm;
    Hunter _hunter;

    public Idle(Hunter hunter,FSM fsm)
    {
        _hunter = hunter;
        _fsm = fsm;
    }

    public void OnEnter()
    {
        _counter = _hunter.counterIdle;
        Debug.Log("enter idle");
    }

    public void OnUpdate()
    {
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
