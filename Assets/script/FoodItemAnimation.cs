using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItemAnimation : MonoBehaviour
{
    // Movement variables
    public float hoverHeight = 0.1f; // How high the object moves up
    public float moveDuration = 1f;  // How long the movement lasts

    private Vector3 initialPosition;
    private float moveLerpTime = 0f; // Separate Lerp time for movement
    private bool isMovingUp = true;
    // Start is called before the first frame update
    void Start()
    {
        // Save the initial position for movement effect
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement logic: move up and then back to original position
        MoveObjectUpAndDown();
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