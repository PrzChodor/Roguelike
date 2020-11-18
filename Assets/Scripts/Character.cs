using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public bool dead;
    public int health;
    public int maxHealth;
    protected bool isHitted;

    SpriteRenderer sprite;

    public abstract void Die();

    public virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage; 

        if (!isHitted)
        {
            isHitted = true;
            StartCoroutine("Hurt");
        }

        if (health <= 0)
            Die();
    }

    public IEnumerator Hurt()
    {
        if (!dead)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sprite.color = Color.white;
            isHitted = false;
        }
    }

}
