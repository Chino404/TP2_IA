using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : IState
{
    FSM _fms;
    Transform _target;
    Enemy _enemy;

    public ChaseEnemy(FSM fms, Transform target, Enemy enemy)
    {
        _fms = fms;
        _target = target;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
