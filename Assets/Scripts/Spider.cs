using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
{

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private GameObject player;

    [SerializeField] private Transform playerTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //agent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("PlayerTargetForEnemies");
        if (player != null)
        {
            playerTarget = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(playerTarget.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            Destroy(gameObject);
        }
    }

}
