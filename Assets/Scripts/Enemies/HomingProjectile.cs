using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(ParticleSystem), typeof(TrailRenderer))]
public class HomingProjectile : MonoBehaviour
{
    public bool active;
    public float timeToActivation;
    public float activeTime;
    public int damage;
    public GameObject target;
    public AudioClip activate;
    public AudioClip despawn;

    private NavMeshAgent agent;
    private ParticleSystem particles;
    private SpriteRenderer sprite;
    private TrailRenderer trail;
    private IEnumerator creation;
    private bool destroyed;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        particles = GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        creation = OnCreate();
        StartCoroutine(creation);
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (active)
            agent.SetDestination(target.transform.position);
    }

    IEnumerator OnCreate()
    {
        yield return new WaitForSeconds(timeToActivation);
        GetComponent<AudioSource>().PlayOneShot(activate);
        active = true;
        yield return new WaitForSeconds(activeTime);
        Destroy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!destroyed)
        {
            if (collision.gameObject.tag == "Wall")
            {
                StopCoroutine(creation);
                Destroy();
            }
            else if (collision.isTrigger && collision.gameObject.tag == "Player")
            {
                collision.GetComponent<Character>().TakeDamage(damage);
                StopCoroutine(creation);
                Destroy();
            }
        }
    }

    private void Destroy()
    {
        GetComponent<AudioSource>().PlayOneShot(despawn);
        destroyed = true;
        particles.Clear();
        particles.Play();
        agent.velocity = Vector3.zero;
        agent.ResetPath();
        sprite.enabled = false;
        trail.enabled = false;
        GameObject.Destroy(gameObject, particles.main.duration);
    }
}
