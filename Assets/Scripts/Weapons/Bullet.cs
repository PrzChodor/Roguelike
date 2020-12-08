using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(ParticleSystem), typeof(TrailRenderer))]
public class Bullet : MonoBehaviour
{
    public LayerMask layers;
    [HideInInspector]
    public int damage;

    private Rigidbody2D rb;
    private ParticleSystem particles;
    private TrailRenderer trail;
    private SpriteRenderer sprite;
    private Vector2 lastPos;
    private bool destroyed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        particles = GetComponent<ParticleSystem>();
        trail = GetComponent<TrailRenderer>();
        sprite = GetComponent<SpriteRenderer>();
        lastPos = rb.position;
    }

    private void Start()
    {
        particles.Stop();
    }

    private void FixedUpdate()
    {
        if (!destroyed)
        {
            var hits = Physics2D.LinecastAll(lastPos, rb.position, layers);

            foreach (var hit in hits)
            {
                if (hit.collider.isTrigger)
                {
                    if (hit.collider.CompareTag("Enemy") && GetComponent<SpriteRenderer>().isVisible)
                    {
                        hit.collider.GetComponent<Character>().TakeDamage(damage);
                        ParticleSystem.MainModule newMain = particles.main;
                        newMain.startColor = new Color(1, 0, 0);
                    }
                    rb.position = hit.point;
                    Destroy();
                    break;
                }
            }
        }
    }

    private void Destroy()
    {
        destroyed = true;
        particles.Play();
        rb.velocity = Vector2.zero;
        sprite.enabled = false;
        trail.enabled = false;
        GameObject.Destroy(gameObject, particles.main.duration);
    }
}
