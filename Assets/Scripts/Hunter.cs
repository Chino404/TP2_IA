using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    FSM _fsm;
    public Hunter hunter;

    public Vector3 velocity; //Lo hice publico para que pueda modificarlo en el Patrol y poder pedirlo en el Boid

    public float maxVelocity;
    public float maxForce;

    public Transform[] wayPoints;
    private void Start()
    {
        _fsm = new FSM();

        _fsm.CreateState("Idle", new Idle(_fsm));
        _fsm.CreateState("Patrol", new Patrol(_fsm, wayPoints, hunter));
        _fsm.ChangeState("Idle");
    }
    private void Update()
    {
        _fsm.Execute();
    }
}
