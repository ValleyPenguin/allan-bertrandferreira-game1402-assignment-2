using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
{
    [SerializeField] private float spiderGroundCheckRadius;

    [SerializeField] private float spiderGroundCheckOffset;

    [SerializeField] private NavMeshAgent agent;

    private GameObject player;

    private Transform playerTarget;

    private bool spiderStartedWalking;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private Rigidbody rb;

    private SpawnWaves wavesSpawner;

    private GameObject waveSpawnManager;

    private PlayerHealth playerHealth;

    private void Start()
    {
        //agent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("PlayerTargetForEnemies");
        waveSpawnManager = GameObject.FindWithTag("WaveManager");
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        agent.enabled = false;
        spiderStartedWalking = false;
        if (player != null)
        {
            playerTarget = player.transform;
        }

        wavesSpawner = waveSpawnManager.GetComponent<SpawnWaves>();
    }


    private void Update()
    {
        /*if (player != null)
        {
            if (SpiderGroundCheck())
            {
                agent.SetDestination(playerTarget.position);
            }
        }*/
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("PlayerTargetForEnemies");
        }

        //pretty much all of this is because unity doesn't like having both rb and navmeshagent at the same time
        //so when falling, i enable the rb and disable the navmeshagent, and do the opposite for when the spiders are on the floor
        if (!SpiderGroundCheck() && !spiderStartedWalking)
        {
            agent.enabled = false;
            rb.isKinematic = false;
            //Debug.Log("Spider is in falling mode!");
        }
        else if (SpiderGroundCheck() && !spiderStartedWalking)
        {
            agent.enabled = true;
            rb.isKinematic = true;
            //Debug.Log("Spider just landed!");
            startSpiderNavMeshMovement();
        }
        else if (spiderStartedWalking)
        { 
            if (!playerHealth.didPlayerDie)
            {
                agent.enabled = true;
                rb.isKinematic = true;
                //Debug.Log("Spider started chasing ya!");
                agent.SetDestination(playerTarget.position);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            wavesSpawner.numberOfSpidersKilled++;
            Debug.Log(wavesSpawner.numberOfSpidersKilled);
            Destroy(gameObject);
        }
    }

    private bool SpiderGroundCheck()
    {
        bool isGrounded = Physics.CheckSphere(transform.position + new Vector3(0f, spiderGroundCheckOffset, 0f), spiderGroundCheckRadius, groundLayerMask);
        //Debug.Log(isGrounded);
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, spiderGroundCheckOffset, 0f), spiderGroundCheckRadius);
    }

    private void startSpiderNavMeshMovement()
    {
        if (!playerHealth.didPlayerDie)
        {
            agent.enabled = true;
            agent.SetDestination(playerTarget.position);
            spiderStartedWalking = true;
        }  
    }
}
