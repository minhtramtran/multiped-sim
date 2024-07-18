using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;

    [SerializeField] private Animator animator;

    private Vector3 lastMoveInput;  // Store the position in the previous frame
    private float movementThreshold = 0.1f; // Threshold to decide if the position change is significant or not
    private Coroutine checkPositionChangeRoutine;

    private void Update()
    {
        Vector3 currentMoveInput = move.action.ReadValue<Vector3>();

        // Start the coroutine if it's not already running
        if (checkPositionChangeRoutine == null)
        {
            checkPositionChangeRoutine = StartCoroutine(CheckPositionChange(currentMoveInput));
        }

        // Store the current position for the next frame
        lastMoveInput = currentMoveInput;
    }

    private IEnumerator CheckPositionChange(Vector3 currentPosition)
    {

        // Wait for 2 seconds
        yield return new WaitForSeconds(0.5f);

        // After 2 seconds, check if the position has changed significantly
        if (Vector3.Distance(currentPosition, lastMoveInput) < movementThreshold)
        {
            StopAnimation(); // A version of this method that doesn't need a parameter
        }
        else
        {
            //Debug.Log("Avatar is moving forward");
            this.animator.SetBool("isWalking", true);
            this.animator.SetFloat("animSpeed", 1.0f);
        }

        // Clear the coroutine so it can be started again in the next frame
        checkPositionChangeRoutine = null;
    }




    // TRAM: In Unity, when you set a GameObject as inactive with the setActive(false) method, all its scripts are stopped.
    // This means that all coroutines that might be running within those scripts are also stopped.
    // Once you set the GameObject as active again with the setActive(true) method,
    // the scripts start running again, but the coroutines that were running previously do not resume automatically.
    private void OnEnable()
    {
        // Restart your coroutine here
        if (move != null && move.action != null)
        {
            Vector3 currentMoveInput = move.action.ReadValue<Vector3>();
            checkPositionChangeRoutine = StartCoroutine(CheckPositionChange(currentMoveInput));
        }
    }


    private void StopAnimation()
    {
        //Debug.Log("StopAnimation called");
        this.animator.SetBool("isWalking", false);
        this.animator.SetFloat("animSpeed", 0.0f);
    }
}


