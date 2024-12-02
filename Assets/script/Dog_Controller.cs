using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Controller : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float jumpHeight = 2f; // Altura del salto
    public float gravity = -20f; // Gravedad m�s fuerte
    public float fallMultiplier = 2.5f; // Multiplicador para ca�da r�pida
    public float rotationSpeed = 10f; // Velocidad de rotaci�n hacia la direcci�n de movimiento

    [Header("Ground Check")]
    public Transform groundCheck; // Objeto que detecta el suelo
    public float groundDistance = 0.2f; // Distancia para detectar el suelo
    public LayerMask groundMask; // Capa del suelo

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canJump = true; // Control para permitir saltos

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        HandleGravityAndJump();
        OnDrawGizmos();
    }

    private void Move()
    {
        // Input de movimiento (adelante y atr�s)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Direcci�n del movimiento basado en la c�mara
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.y = 0f; // Ignorar inclinaci�n vertical de la c�mara

        // Aplicar movimiento al CharacterController
        controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

        // Rotar el personaje hacia la direcci�n de movimiento
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleGravityAndJump()
    {
        // Comprobar si el personaje est� tocando el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Restringir saltos si no est� tocando el suelo
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Resetear la velocidad en Y 
            canJump = true; // Permitir saltar nuevamente al tocar el suelo
        }

        // Saltar cuando se presione la tecla de salto
        if (isGrounded && canJump && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            canJump = false; // Evitar m�ltiples saltos
        }

        // Aplicar ca�da r�pida si est� cayendo
        if (velocity.y < 0)
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Aplicar movimiento vertical al CharacterController
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
