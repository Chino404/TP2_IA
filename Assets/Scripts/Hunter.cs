using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    public Transform target;
    Vector3 _velocity;
    public float maxVelocity;

    void Update()
    {
        AddForce(transform.forward);

        transform.position += _velocity * Time.deltaTime;
    }


    public void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, maxVelocity);
    }
}
