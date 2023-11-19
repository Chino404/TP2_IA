using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PatrolEnemy : IState
{
    FSM _fsm;
    Transform[] _wayPoints;
    Enemy _enemy;

    int _actualIndex;

    public PatrolEnemy(FSM fsm, Transform[] wayPoints, Enemy enemy)
    {
        _fsm = fsm;
        _wayPoints = wayPoints;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        AddForce(Seek(_wayPoints[_actualIndex].position));

        if (Vector3.Distance(_enemy.gameObject.transform.position, _wayPoints[_actualIndex].position) <= 0.3f)
        {
            _actualIndex++;
            if (_actualIndex >= _wayPoints.Length)
                _actualIndex = 0;
        }

        _enemy.gameObject.transform.position += _enemy.velocity * Time.deltaTime;
        _enemy.gameObject.transform.forward = _enemy.velocity;
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
