using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Enemy
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

    public void CastProjectiles()
    {
        GetComponent<AudioSource>().Play();
        transform.GetChild(0).up = player.position - (Vector2)transform.GetChild(0).position;
        foreach (var point in castPoints)
        {
            var proj = GameObject.Instantiate(projectile, point.position, Quaternion.identity);
            proj.transform.parent = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelMaster>().currentLevel.transform;
        }
    }

    IEnumerator OnAttack()
    {
        attacked = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);
        attacked = false;
    }
}
