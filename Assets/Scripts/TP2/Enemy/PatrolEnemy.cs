using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PatrolEnemy : IState
{
    FSM _fsm;
    Transform[] _wayPoints;
    Transform _target;
    Enemy _enemy;

    int _actualIndex;

    public PatrolEnemy(FSM fsm, Transform[] wayPoints, Transform target, Enemy enemy)
    {
        _fsm = fsm;
        _wayPoints = wayPoints;
        _target = target;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        Debug.Log("Patrullando");
        _actualIndex = 0;

        if (!_enemy.InFOV(_wayPoints[_actualIndex])) _enemy.GoBackToPatrol();
   
    }

    public void OnUpdate()
    {
        if(_enemy.InFOV(_target))
        {
            Debug.Log("Te veo");
            GameManager.Instance.LookPlayer(_target.transform.position);
            _fsm.ChangeState("Perseguir");
        }


        AddForce(Seek(_wayPoints[_actualIndex].position));

        if (Vector3.Distance(_enemy.gameObject.transform.position, _wayPoints[_actualIndex].position) <= 0.3f)
        {
            _actualIndex++;
            if (_actualIndex >= _wayPoints.Length)
                _actualIndex = 0;
        }

        _enemy.transform.position += _enemy.velocity * Time.deltaTime;
        _enemy.transform.forward = _enemy.velocity;
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
