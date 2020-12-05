using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HomingProjectile : MonoBehaviour
{
    public bool active;
    public float timeToActivation;
    public int damage;
    public GameObject target;

    private NavMeshAgent agent;

    void Start()
    {
        StartCoroutine(OnCreate());
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if(active)
            agent.SetDestination(target.transform.position);
    }

    IEnumerator OnCreate()
    {
        yield return new WaitForSeconds(timeToActivation);
        active = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
        {
            print(collision.gameObject.name);
            if (collision.gameObject.tag == "Wall")
                GameObject.Destroy(gameObject);
            else if (collision.gameObject.tag == "Player")
            {
                collision.GetComponent<Character>().TakeDamage(damage);
                GameObject.Destroy(gameObject);
            }
        }
    }
}
