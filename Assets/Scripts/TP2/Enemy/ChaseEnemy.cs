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
        if (GameManager.Instance.InLineOfSight(_enemy.transform.position, _target.transform.position))
        {
            AddForce(Seek(_target.transform.position));
        }
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }

    Vector3 Seek(Vector3 target)
    {
        var desired = target - _enemy.gameObject.transform.position;
        desired.Normalize();
        desired *= _enemy.maxVelocity;

        var steering = desired - _enemy.velocity;
        steering = Vector3.ClampMagnitude(steering, _enemy.maxForce);
        return steering;
    }

    public void AddForce(Vector3 dir)
    {
        _enemy.velocity += dir;

        _enemy.velocity = Vector3.ClampMagnitude(_enemy.velocity, _enemy.maxVelocity);
    }
}
