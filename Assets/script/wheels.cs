using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheels : MonoBehaviour
{
    public float rotationSpeed = 100f; // Velocidad de rotación de la rueda
    public Vehicle vehicle; // Referencia al script de movimiento del vehículo

    void Update()
    {
        // Rotar la rueda alrededor del eje local X (el eje hacia adelante de la rueda)
        if (vehicle != null)
        {
            float rotationAmount = vehicle.speed * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, rotationAmount);
        }
    }
}
