using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Ball : MonoBehaviour
{
    public int damage;
    private ParticleSystem particles;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool destroyed;

    public AudioClip despawn;
    public AudioClip spawn;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        particles.Stop();
        GetComponent<AudioSource>().PlayOneShot(spawn);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!destroyed)
        {
            if (collision.gameObject.tag == "Wall")
            {
                Destroy();
            }
            else if (collision.isTrigger && collision.gameObject.tag == "Player")
            {
                collision.GetComponent<Character>().TakeDamage(damage);
                Destroy();
            }
        }
    }

    private void Destroy()
    {
        GetComponent<AudioSource>().PlayOneShot(despawn);
        destroyed = true;
        particles.Play();
        rb.velocity = Vector2.zero;
        sprite.enabled = false;
        GameObject.Destroy(gameObject, particles.main.duration);
    }
}
