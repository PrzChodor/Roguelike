using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private bool canAttack;
    public uint numberOfSlimesToSpawn;
    public GameObject slimeToSpawn;

    public override void Attack()
    {
    }

    public override void Die()
    {
        dead = true;
        rb.simulated = false;
        Deactivate();
    }

    public override void Move()
    {
        var dir = rb.position.x - player.position.x;
        sprite.flipX = !(dir < 0);
        agent.SetDestination(player.position);
    }

    public void StartMovement()
    {
        agent.isStopped = false;
        GetComponent<AudioSource>().Play();
    }

    public void StopMovement()
    {
        agent.isStopped = true;
    }

    public void StartAttack()
    {
        canAttack = true;
    }
    public void StopAttack()
    {
        canAttack = false;
        if (dead)
        {
            animator.SetTrigger("Die");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            player.GetComponent<PlayerController>().TakeDamage(1);
            canAttack = false;
        }
    }

    public void ToggleColliderDirection()
    {
        GetComponent<CapsuleCollider2D>().direction = GetComponent<CapsuleCollider2D>().direction == CapsuleDirection2D.Horizontal ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal;
    }

    public override void OnDeath()
    {
        var dir = Vector2.up;
        for (int i = 0; i < numberOfSlimesToSpawn; i++)
        {
            var newSlime = Instantiate(slimeToSpawn, transform.position, Quaternion.identity, currentLevel.transform);
            newSlime.GetComponent<Rigidbody2D>().AddForce(dir * 75);
            currentLevel.AddEnemy(newSlime);
            dir = dir.Rotate(360f / numberOfSlimesToSpawn);
        }

        Deactivate();

        var loot = Random.value;
        if (loot < 0.025f && heart != null)
        {
            var item = Instantiate(heart, transform.parent);
            item.transform.position = this.transform.position;
        }

        OnThisDeath.Invoke();
    }
}
