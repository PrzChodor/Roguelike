using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Character : MonoBehaviour
{
    public bool dead;
    public int health;
    public int maxHealth;
    protected bool isHitted;
    protected SpriteRenderer sprite;

    public abstract void Die();

    public virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }


    public virtual void TakeDamage(int damage)
    {
        if (!dead)
        {
            health -= damage;

            if (!isHitted)
                StartCoroutine("Hurt");

            if (health <= 0)
                Die();
        }
    }

    public IEnumerator Hurt()
    {
        if (!dead)
        {
            isHitted = true;
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            sprite.color = Color.white;
            isHitted = false;
        }
    }

}
