using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform[] wayPoints;
    public Transform target;
    Vector3 _velocity;
    public float maxVelocity;
    public float maxForce;
    int _actualIndex;
   

    void Update()
    {
       
        AddForce(Seek(wayPoints[_actualIndex].position));

        if (Vector3.Distance(transform.position, wayPoints[_actualIndex].position) <= 0.3f)
        {
            _actualIndex++;
            if (_actualIndex >= wayPoints.Length)
                _actualIndex = 0;
        }


        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }

    Vector3 Seek(Vector3 target)
    {
        var desired = target - transform.position;
        desired.Normalize();
        desired *= maxVelocity;

        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    Vector3 Pursuit(Vector3 target)
    {
        var desired = target - transform.position;
        desired.Normalize();
        desired *= maxVelocity;

        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    public void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, maxVelocity);
    }
}
