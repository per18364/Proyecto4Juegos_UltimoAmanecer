using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int hp = 100;
    private Animation anim;
    private NavMeshAgent agent;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            agent.SetDestination(transform.position);
            anim.Play("Death");
            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.Play("Attack1");
            other.GetComponent<Player>().TakeDamage(10);
        }
    }
}
