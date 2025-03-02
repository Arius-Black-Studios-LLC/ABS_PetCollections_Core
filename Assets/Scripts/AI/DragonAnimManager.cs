using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DragonWardorbManager))]
public class DragonAnimManager : MonoBehaviour
{

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSleepingTrue()
    {
        animator.SetBool("Sleeping", true);
    }

    public void SetSleepingFalse()
    {
        animator.SetBool("Sleeping", false);
    }

    public void SetAnimatorVelocity(Vector3 movement)
    {
        animator.SetFloat("Movement", movement.x);
        animator.SetFloat("Rotation", movement.z);
    }



}
