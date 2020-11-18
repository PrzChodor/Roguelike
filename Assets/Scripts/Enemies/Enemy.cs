using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : Character
{
    public bool active = false;

    protected Rigidbody2D player;

    private Level currentLevel;
    private UnityEvent OnThisDeath;

    public abstract void Move();
    public abstract void Attack();

    public override void Awake()
    {
        base.Awake();
        OnThisDeath = new UnityEvent();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        currentLevel = this.gameObject.GetComponentInParent<Level>();
        OnThisDeath.AddListener(currentLevel.OnEnemyDeath);
    }

    private void FixedUpdate()
    {
        if (active)
        {
            Move();
            Attack();
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
}
