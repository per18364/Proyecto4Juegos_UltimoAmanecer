using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int hp = 100;
    [SerializeField] private GameObject weapon;
    private Animation anim;
    private NavMeshAgent agent;
    private Transform player;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     agent.SetDestination(player.position);
    // }

    // public void TakeDamage(int damage)
    // {
    //     hp -= damage;
    //     if (hp <= 0)
    //     {
    //         agent.SetDestination(transform.position);
    //         anim.Play("Death");
    //         Destroy(gameObject, 1f);
    //     }
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         anim.Play("Attack1");
    //         other.GetComponent<Player>().TakeDamage(10);
    //     }
    // }

    void Update()
    {
        if (!isAttacking)
        {
            agent.SetDestination(player.position);
        }
    }

    public void SetHP(int newHP)
    {
        hp = newHP;
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

    private void OnDestroy()
    {
        Vector3 rotation = new Vector3(0, 0, 90);
        Instantiate(weapon, transform.position, Quaternion.Euler(rotation));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(AttackPlayer(other));
        }
    }

    private IEnumerator AttackPlayer(Collider playerCollider)
    {
        isAttacking = true;
        agent.SetDestination(transform.position); // Detiene al zombie para atacar
        anim.Play("Attack1");
        playerCollider.GetComponent<Player>().TakeDamage(10);

        // Espera a que termine la animación de ataque
        while (anim.IsPlaying("Attack1"))
        {
            yield return null;
        }

        isAttacking = false;
        anim.Play("Run");
    }
}
