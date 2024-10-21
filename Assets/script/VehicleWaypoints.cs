using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleWaypoints : MonoBehaviour
{
    public Transform[] waypoints; // Array de puntos de referencia (waypoints)
    public float speed = 10f; // Velocidad del cami�n
    public float waypointThreshold = 0.5f; // Distancia m�nima para considerar que lleg� a un waypoint

    private int currentWaypointIndex = 0; // �ndice del waypoint actual
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        // Si no hay waypoints, salir
        if (waypoints.Length == 0)
            return;

        // Obtener el waypoint actual
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calcular la direcci�n hacia el waypoint
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Mover el veh�culo hacia el waypoint
        Vector3 move = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotar el veh�culo hacia el waypoint
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * speed));

        // Comprobar si el veh�culo ha llegado suficientemente cerca al waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            // Si ha llegado, pasar al siguiente waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Ciclar entre los waypoints
        }
    }
}
