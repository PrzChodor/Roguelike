using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float attackCooldown;
    public BallSpawner spawner;
    public GameObject ball;
    private UIManager ui;

    private bool attacked;
    public override void Awake()
    {
        base.Awake();
        ui = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIManager>();
        ui.ShowBossHP();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        ui.UpdateBossHP((float)health / maxHealth);
    }

    public override void Attack()
    {
        if (!attacked)
            OnAttack();
    }

    public override void Die()
    {
        animator.SetTrigger("Die");
        dead = true;
        rb.simulated = false;
        Deactivate();
    }

    public override void OnDeath()
    {
        ui.GetComponent<GameMaster>().End();
    }

    public override void Move()
    {
        var dir = rb.position.x - player.position.x;
        sprite.flipX = dir > 0;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attacked = false;
    }

    public void OnAttack()
    {
        attacked = true;
        animator.SetTrigger("Action" + Random.Range(1, 3));
    }

    public void SpawnSpiral()
    {
        var newSpawner = Instantiate(spawner, this.transform.position, Quaternion.identity).GetComponent<BallSpawner>();
        newSpawner.timeBetweenBalls = 0.05f;
        newSpawner.numberOfBalls = 16;
        newSpawner.balls = ball;
        newSpawner.Spawn();
    }

    public void SpawnCircle()
    {
        var newSpawner = Instantiate(spawner, this.transform.position, Quaternion.identity).GetComponent<BallSpawner>();
        newSpawner.timeBetweenBalls = 0f;
        newSpawner.numberOfBalls = 32;
        newSpawner.balls = ball;
        newSpawner.Spawn();
    }
}
