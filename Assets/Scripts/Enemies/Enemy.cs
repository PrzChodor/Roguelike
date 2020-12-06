using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent),typeof(Animator),typeof(Rigidbody2D))]
public abstract class Enemy : Character
{
    public bool active = false;

    protected Rigidbody2D player;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Rigidbody2D rb;

    private Level currentLevel;
    private UnityEvent OnThisDeath;

    public abstract void Move();
    public abstract void Attack();

    public override void Awake()
    {
        base.Awake();
        OnThisDeath = new UnityEvent();
        currentLevel = this.gameObject.GetComponentInParent<Level>();
        OnThisDeath.AddListener(currentLevel.OnEnemyDeath);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public virtual void Update()
    {
        if (active)
        {
            if (IsPlayerInSight())
            {
                Move();
                Attack();
            }
        }
    }

    public void Activate()
    {
        active = true;
    }

    public virtual void Deactivate()
    {
        active = false;
    }

    public virtual void OnDeath()
    {
        Deactivate();
        OnThisDeath.Invoke();
    }

    public bool IsPlayerInSight()
    {
        return sprite.isVisible && !Physics2D.Linecast(transform.position, player.position, LayerMask.GetMask("Walls"));
    }
}
