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
        Debug.Log("Persiguiendo a Enemigo");
    }

    public void OnUpdate()
    {
        if (_enemy.InFOV(_target))
        {
            //GameManager.Instance.LookPlayer(_target.transform.position);

            AddForce(Seek(_target.transform.position));
            _enemy.transform.position += _enemy.velocity * Time.deltaTime;
            _enemy.transform.forward = _enemy.velocity;
        }
        else _fms.ChangeState("Patrullar");
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
