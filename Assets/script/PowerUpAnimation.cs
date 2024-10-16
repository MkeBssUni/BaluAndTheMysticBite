using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAnimation : MonoBehaviour
{
    private Material material;
    private Color originalEmissionColor;
    public Color shineColor = Color.white;
    public float shineTime = 1f;
    public float dimTime = 1.5f;
    public float shineIntensity = 1f;

    private float shineLerpTime = 0f; // Lerp time for shine
    private bool isShining = false;
    private bool changingShine = false;

    // Movement variables
    public float hoverHeight = 0.1f; // How high the object moves up
    public float moveDuration = 1f;  // How long the movement lasts

    private Vector3 initialPosition;
    private float moveLerpTime = 0f; // Separate Lerp time for movement
    private bool isMovingUp = true;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        originalEmissionColor = material.GetColor("_EmissionColor");

        material.EnableKeyword("_EMISSION");

        InvokeRepeating("ToggleShine", 0f, shineTime + dimTime);

        // Save the initial position for movement effect
        initialPosition = transform.position;
    }

    void ToggleShine()
    {
        changingShine = true;
        isShining = !isShining;
        shineLerpTime = 0f;
    }

    void Update()
    {
        // Update shine gradually
        UpdateShineEffect();

        // Movement logic: move up and then back to original position
        MoveObjectUpAndDown();
    }

    void UpdateShineEffect()
    {
        if (changingShine)
        {
            shineLerpTime += Time.deltaTime;

            if (isShining)
            {
                float t = Mathf.Clamp01(shineLerpTime / shineTime);
                Color newShine = Color.Lerp(originalEmissionColor, shineColor * shineIntensity, t);
                material.SetColor("_EmissionColor", newShine);

                if (t >= 1f)
                {
                    changingShine = false;
                }
            }
            else
            {
                float t = Mathf.Clamp01(shineLerpTime / dimTime);
                Color newShine = Color.Lerp(shineColor * shineIntensity, originalEmissionColor, t);
                material.SetColor("_EmissionColor", newShine);

                if (t >= 1f)
                {
                    changingShine = false;
                }
            }
        }
    }

    void MoveObjectUpAndDown()
    {
        // Smoothly move the object up and then back down to the initial position
        moveLerpTime += Time.deltaTime / moveDuration; // Use independent Lerp time for movement

        if (isMovingUp)
        {
            transform.position = Vector3.Lerp(initialPosition, initialPosition + Vector3.up * hoverHeight, moveLerpTime);

            if (moveLerpTime >= 1f)
            {
                moveLerpTime = 0f; // Reset the Lerp time when reaching the top
                isMovingUp = false; // Start moving down
            }
        }
        else
        {
            transform.position = Vector3.Lerp(initialPosition + Vector3.up * hoverHeight, initialPosition, moveLerpTime);

            if (moveLerpTime >= 1f)
            {
                moveLerpTime = 0f; // Reset the Lerp time when reaching the bottom
                isMovingUp = true;  // Start moving up again
            }
        }
    }
}
