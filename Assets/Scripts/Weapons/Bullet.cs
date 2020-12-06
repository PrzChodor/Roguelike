using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(ParticleSystem), typeof(TrailRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public int damage;

    private Rigidbody2D rb;
    private ParticleSystem particles;
    private TrailRenderer trail;
    private SpriteRenderer sprite;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
        trail = GetComponent<TrailRenderer>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        particles.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger || collision.gameObject.layer == 9)
        {
            if (collision.tag != "Player" && collision.tag != "Projectile")
            {
                if (collision.tag == "Enemy" && GetComponent<SpriteRenderer>().isVisible)
                {
                    collision.GetComponent<Character>().TakeDamage(damage);
                    ParticleSystem.MainModule newMain = particles.main;
                    newMain.startColor = new Color(1, 0, 0);
                }
                Destroy();
            }
        }
    }

    private void Destroy()
    {
        particles.Clear();
        particles.Play();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        sprite.enabled = false;
        trail.enabled = false;
        GameObject.Destroy(gameObject, particles.main.duration);
    }
}
