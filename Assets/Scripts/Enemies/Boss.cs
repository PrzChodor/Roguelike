using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float attackCooldown;
    public GameObject projectile;

    private bool attacked;
    List<Transform> castPoints;

    public override void Awake()
    {
        base.Awake();
        castPoints = new List<Transform>();
        foreach (Transform child in transform.GetChild(0))
            castPoints.Add(child);
    }

    public override void Attack()
    {
        if (!attacked)
            StartCoroutine(OnAttack());
    }

    public override void Die()
    {
        animator.SetTrigger("Die");
        dead = true;
        rb.simulated = false;
    }

    public override void Move()
    {
        var dir = rb.position.x - player.position.x;
        sprite.flipX = dir < 0;
    }

    IEnumerator OnAttack()
    {
        attacked = true;
        animator.SetTrigger("Action" + Random.Range(0, 4));
        yield return new WaitForSeconds(attackCooldown);
        attacked = false;
    }
}
