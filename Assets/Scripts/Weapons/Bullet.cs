using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger || collision.gameObject.layer == 9)
        {
            if (collision.tag != "Player" && collision.tag != "Projectile")
            {
                if (collision.tag == "Enemy")
                    collision.GetComponent<Character>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
