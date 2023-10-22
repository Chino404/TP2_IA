using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [Header("Radios")]
    public float viewRadius;
    public float separationRadius;
    [Header("Velocidades")]
    public float maxSpeed;
    public float maxForce; //La fuerza con la cual va a girar (El margen de giro)
    Vector3 _velocity; //Para donde miro
    public Vector3 Velocity { get { return _velocity; } }

    void Start()
    {
        AddForce(new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f)) * maxSpeed); //Se mueve a una direccion random, multplicado por la velocidad

        GameManager.Instance.boids.Add(this); //Me agrego a su lista de Boids
    }

    void Update()
    {
        //Huir del cazador
        if (Vector3.Distance(GameManager.Instance.hunter.transform.position, transform.position) <= viewRadius)
        {
            AddForce(Evade(GameManager.Instance.hunter.transform.position + GameManager.Instance.hunter.velocity));
        }

        //Ir a la manzana
        //else if()
        //{

        //}

        //Flocking
        else
            Flocking();

        transform.position = GameManager.Instance.ApplyBounds(transform.position + _velocity * Time.deltaTime);
        transform.forward = _velocity; //Que mire para donde se esta moviendo
    }

    #region FLOCKING
    void Flocking()
    {
        AddForce(Separation(GameManager.Instance.boids ,separationRadius) * GameManager.Instance.weightSeparation);
        AddForce(Alignment(GameManager.Instance.boids, viewRadius) * GameManager.Instance.weightAlignment);
        AddForce(Cohesion(GameManager.Instance.boids, viewRadius) * GameManager.Instance.weightCohesion);
    }

    Vector3 Separation(List<Boid> boids, float radius)
    {
        Vector3 desired = Vector3.zero; //Dir deseada
        foreach (var item in boids)
        {
            var dir = item.transform.position - transform.position; //Saco la direccion de la posicion del boid menos la mia

            if (dir.magnitude > radius || item == this) //Si la magnitud de la direccion es mayor al radio o soy yo...
                continue;

            desired -= dir; //Voy restando a mi direccion deseado para eventualmente separarme (ir al lado opuesto)
        }

        if(desired == Vector3.zero) //Si no hay nadie en mi radio...
            return desired;

        desired.Normalize(); //Lo normalizo para que me lo deje en uno...
        desired *= maxSpeed; //y lo multiplico por la velocidad para que se mueva acorde a ese valor de velocidad

        return CalculateSteering(desired);
    }

    Vector3 Alignment(List<Boid> boids, float radius)
    {
        var desired = Vector3.zero;
        int count = 0;

        foreach (var item in boids)
        {
            if(item == this) continue;

            if(Vector3.Distance(transform.position, item.transform.position) <= radius) //Si la distancia entre los dos es menor al radio...
            {
                desired += item._velocity; //La direccion donde estanviendo los demas boids la voy sumando a mi dir deseada
                count++; //Sumo al contador de que alguien esta en mi radio
            }
        }

        if(count <= 0) return Vector3.zero; //Porque no hay nadie dentro de mi radio

        desired /= count; //Promedio de donde estan mirando todos los que estan adentro de mi radio

        desired.Normalize();
        desired *= maxSpeed;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion(List<Boid> boids, float radius) //Acercarme a la manada
    {
        var desired = transform.position; //Mi posicion
        var count = 0;

        foreach (var item in boids)
        {
            var dist = Vector3.Distance(transform.position, item.transform.position);

            if (dist > radius || item == this) //Si esta afuera de mi radio o soy yo, paso al siguiente
                continue;

            desired += item.transform.position;
            count++;
        }

        if(count <= 0) return Vector3.zero;

        desired /= count;

        desired.Normalize();
        desired *= maxSpeed;

        return CalculateSteering(desired);
    }
    #endregion

    //Calculo la fuerza con la que va a girar su direccion
    Vector3 CalculateSteering(Vector3 desired)
    {
        var steering = desired - _velocity; //direccion = la dir. deseada - hacia donde me estoy moviendo
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    Vector3 Evade(Vector3 target)
    {
        return Flee(target);
    }

    Vector3 Flee(Vector3 targetFlee)
    {
        return -Seek(targetFlee); //Es negativo para que huya
    }

    Vector3 Seek(Vector3 targetSeek)
    {
        var desired = targetSeek - transform.position; //Me va a dar un valor
        desired.Normalize(); //Lo normalizo para que sea mas comodo
        desired *= maxSpeed; //Lo multiplico por la velocidad

        return CalculateSteering(desired);
    }

    public void AddForce(Vector3 dir)
    {
        _velocity += dir;
        _velocity = Vector3.ClampMagnitude( _velocity, maxSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

    }
}
