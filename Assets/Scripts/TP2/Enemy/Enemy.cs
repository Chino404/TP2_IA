using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    FSM _fsm;

    [Header("Params")]
    [HideInInspector] public Vector3 velocity; //Lo hice publico para que pueda modificarlo en el PatrolEnemy
    public float maxVelocity;
    public float maxForce;

    [Header ("Values")]
    public Transform target;
    public Transform[] wayPointsPatrol;
    public float viewRadius; //Area de vision
    public float viewAngle;  //Angulo de vision


    private void Start()
    {
        _fsm = new FSM();

        _fsm.CreateState("Perseguir", new ChaseEnemy(_fsm, target, this));
        _fsm.CreateState("Patrullar", new PatrolEnemy(_fsm, wayPointsPatrol, this));
        _fsm.CreateState("Pathfinding", new PathfindingEnemy(_fsm, target ,this));

        _fsm.ChangeState("Patrullar");
    }

    private void Update()
    {
        if (InFOV(target))
        {
            print("Te veo");
            //GameManager.Instance.eventCall += ViAlPlAYER();

        }

        _fsm.Execute();

    }

    public bool InFOV(Transform obj) //Si lo estoy viendo
    {
        var dir = obj.position - transform.position;

        if (dir.magnitude < viewRadius)
        {
            //Calculo un angulo de mi vision hacia adelante y la direccion de mi target 
            if (Vector3.Angle(transform.forward, dir) <= viewAngle * 0.5f)
            {
                return GameManager.Instance.InLineOfSight(transform.position, obj.position); //Si no hya nada entre medio me devuelve True
            }
        }

        return false;
    }

    void ViAlPlAYER(Vector3 obj)
    {
        obj = target.transform.position;
    }

    #region Visualizar el rango de Vision
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 lineA = GetVectorFromAngle(viewAngle * 0.5f + transform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle * 0.5f + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    #endregion
}
