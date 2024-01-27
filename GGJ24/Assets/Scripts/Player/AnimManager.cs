using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    PlayerMovement playerMovement;
    int horizontal;
    int vertical;
    int tickleTrigger;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        tickleTrigger = Animator.StringToHash("Tickle");
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void UpdateAnimatorValues(float horrizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal;
        float snappedVertical;

        #region Snap Horiziontal
        if (horrizontalMovement > 0 && horrizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.55f;
        }
        else if (horrizontalMovement > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if (horrizontalMovement < 0 && horrizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horrizontalMovement < -0.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion

        #region Snap Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.55f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if(isSprinting)
        {
            snappedHorizontal = horrizontalMovement;
            snappedVertical = 2;
        }
        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public void PlayTickleAnimation()
    {
        // Trigger tickle animation
        animator.SetTrigger(tickleTrigger);
        
    }
}
