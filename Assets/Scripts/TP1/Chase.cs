using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : IState
{
    float _counter;
    FSM _fsm;
    Boid[] _target;

    float _closestDistance = Mathf.Infinity;
    public Boid currentTarget;
    Hunter _hunter;

    public Chase (FSM fsm, Boid[] target, Hunter hunter)
    {
        _fsm = fsm;
        _target = target;
        _hunter = hunter;
    }

    public void OnEnter()
    {
        _counter = _hunter.counter;
        Debug.Log("enter chase");
    }

    public void OnUpdate()
    {
        foreach (Boid target in _target)
        {
            float _distance = Vector3.Distance(_hunter.gameObject.transform.position, target.transform.position);

            if(_distance<_closestDistance)
            {
                _closestDistance = _distance;
                currentTarget = target;
            }
        }
        AddForce(Pursuit(currentTarget.transform.position+currentTarget.Velocity));
        
        _hunter.gameObject.transform.position += _hunter.velocity * Time.deltaTime;
        _hunter.gameObject.transform.forward = _hunter.velocity;
        _counter -= Time.deltaTime;

        if(_counter<=0)
        {
            _fsm.ChangeState("Idle");
        }

        else if (Vector3.Distance(_hunter.gameObject.transform.position, currentTarget.transform.position) < 10)
            _fsm.ChangeState("Patrol");

    }

    public void OnExit()
    {
        Debug.Log("exit chase");
    }

    Vector3 Pursuit(Vector3 target)
    {
        return Seek(target);
    }

    Vector3 Seek(Vector3 target)
    {
        var desired = target - _hunter.gameObject.transform.position;
        desired.Normalize();
        desired *= _hunter.maxVelocity;
        
        var steering = desired - _hunter.velocity;
        steering = Vector3.ClampMagnitude(steering, _hunter.maxForce);

        return steering;
    }

    public void AddForce(Vector3 dir)
    {
        _hunter.velocity += dir;

        _hunter.velocity = Vector3.ClampMagnitude(_hunter.velocity, _hunter.maxVelocity);
    }
}
