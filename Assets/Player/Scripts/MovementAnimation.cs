using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    public Animator animator;

    private int isWalking;

    private int isRunning;

    void Start()
    {
        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        var z = Input.GetAxis("Vertical");
        var isWalkingSet = animator.GetBool(isWalking);
        var isRunningSet = animator.GetBool(isRunning);

        if (z > 0)
        {
            if (!isWalkingSet)
            {
                animator.SetBool(isWalking, true);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                animator.SetBool(isRunning, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                animator.SetBool(isRunning, false);
            }
        }

        if (z == 0)
        {
            if (isWalkingSet)
            {
                animator.SetBool(isWalking, false);
            }

            if (isRunningSet)
            {
                animator.SetBool(isRunning, false);
            }
        }
    }
}
