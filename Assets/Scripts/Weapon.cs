using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public Transform firePoint;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        firePoint = transform.GetChild(0).transform;
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }
}
