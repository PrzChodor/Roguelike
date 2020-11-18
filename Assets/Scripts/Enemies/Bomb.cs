using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Enemy
{
    public GameObject explosion;
    public float trigerDistance = 0.7f;
    public float speed = 3.0f;

    private Animator animator;
    private Rigidbody2D enemy;
    private Vector3Int playerPos;
    private Vector3Int enemyPos;
    private Vector2 nextPos;
    private PathFinding pathFinder;

    public override void Awake()
    {
        base.Awake();
        animator = this.GetComponent<Animator>();
        enemy = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        pathFinder = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PathFinding>();
    }

    private void Start()
    {
        Vector3 temp = pathFinder.NextStep(enemy.position, player.position);
        nextPos = new Vector2(temp.x, temp.y);
    }

    public override void Die()
    {
        dead = true;
        enemy.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("Explode");
    }

    public override void OnDeath()
    {
        base.OnDeath();
        Instantiate(explosion, this.transform.position, this.transform.rotation);
        Destroy(gameObject);
    }

    public override void Move()
    {
        if (!dead)
        {
            if (playerPos != pathFinder.walkable.WorldToCell(player.position) || enemyPos != pathFinder.walkable.WorldToCell(enemy.position))
            {
                playerPos = pathFinder.walkable.WorldToCell(player.position);
                enemyPos = pathFinder.walkable.WorldToCell(enemy.position);
                Vector3 temp = pathFinder.NextStep(enemy.position, player.position);
                nextPos = new Vector2(temp.x,temp.y);
            }

            Vector2 movement = nextPos - enemy.position;

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            enemy.MovePosition(enemy.position + movement.normalized * speed * Time.deltaTime);
        }
    }

    public override void Attack()
    {
        if (!dead && Vector2.Distance(enemy.position, player.position) < trigerDistance)
        {
            Die();
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        animator.SetFloat("Speed", 0);
    }
}
