using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage = 5;

    private CircleCollider2D col;

    private void Awake()
    {
        col = this.GetComponent<CircleCollider2D>();
    }

    public void DealDamage()
    {
        var colliders = new List<Collider2D>();
        col.OverlapCollider(new ContactFilter2D(), colliders);

        foreach (var item in colliders)
        {
            if (item.gameObject.CompareTag("Player") || item.gameObject.CompareTag("Enemy"))
                item.GetComponent<Character>().TakeDamage(damage);
        }
    }

    public void Vanish()
    {
        Destroy(gameObject);
    }
}
