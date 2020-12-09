using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BallSpawner))]
public class Bomb : Enemy
{
    public float trigerDistance = 0.7f;

    public override void Update()
    {
        base.Update();
        animator.SetFloat("Horizontal", agent.velocity.x);
        animator.SetFloat("Vertical", agent.velocity.y);
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    public override void Die()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("Explode");
        Deactivate();
        dead = true;
    }

    public override void OnDeath()
    {
        base.OnDeath();
        GetComponent<BallSpawner>().Spawn();
        Destroy(gameObject);
    }

    public override void Move()
    {
        agent.SetDestination(player.position);
    }

    public override void Attack()
    {
        if (Vector2.Distance(rb.position, player.position) < trigerDistance)
        {
            Die();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        agent.ResetPath();
    }
}
