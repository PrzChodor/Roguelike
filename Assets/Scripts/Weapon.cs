using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;
    public int damage;
    public float spread;
    public float fireRate;

    [HideInInspector]
    public bool isShooting;
    [HideInInspector]
    public Transform firePoint;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        firePoint = transform.GetChild(0).transform;
    }

    public IEnumerator Shoot(float angle)
    {
        isShooting = true;

        animator.SetTrigger("Shoot");
        angle = Random.Range(angle - spread, angle + spread);
        var rotation = Quaternion.Euler(0, 0, angle);
        var fired = Instantiate(bullet, firePoint.position, rotation);
        fired.GetComponent<Rigidbody2D>().AddForce(rotation * Vector2.left * bulletSpeed);
        fired.GetComponent<Bullet>().damage = damage;

        yield return new WaitForSeconds(1/fireRate);

        isShooting = false;
    }
}
