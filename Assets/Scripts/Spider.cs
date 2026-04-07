using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Spider : MonoBehaviour
{
    [SerializeField] private float spiderGroundCheckRadius;

    [SerializeField] private float spiderGroundCheckOffset;

    [SerializeField] private NavMeshAgent agent;

    private GameObject player;

    private Transform playerTarget;
    [SerializeField] private float chaseDistance;

    [SerializeField] private float giveUpDistance;

    [SerializeField] private float chaseCheckAngle;

    private bool spiderStartedWalking;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private Rigidbody rb;

    private SpawnWaves wavesSpawner;

    private GameObject waveSpawnManager;

    private PlayerHealth playerHealth;

    [SerializeField] private Transform[] patrolPoints;

    private SpiderState _currentState;

    private Vector3 _currentTarget;

    private int counter;

    private bool _isWaiting;

    private Vector3 _directionToPlayer;

    [SerializeField] private Transform minBounds;
    [SerializeField] private Transform maxBounds;

    [SerializeField] private Animator animator;


    private void Start()
    {
        _currentState = SpiderState.IDLE;
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

    void FixedUpdate()
    {
        if (_currentState == SpiderState.IDLE)
        {

            animator.SetBool("isIdle", true);
            if (!_isWaiting)
            {
                StartCoroutine(WaitAndChooseARandomPointAndMove(5));
            }
            if (IsPlayerInRange() && IsInFOV())
            {
                _currentState = SpiderState.CHASE;
            }
        }
        else if (_currentState == SpiderState.PATROL)
        {
            if (agent.isOnNavMesh)
            {
                animator.SetBool("isIdle", false);
                if (agent.remainingDistance <= 4f)
                {
                    _currentState = SpiderState.IDLE;
                }

                //check for the player to chase
                if (IsPlayerInRange()) //&& IsInFOV())
                {
                    _currentState = SpiderState.CHASE;
                    agent.SetDestination(playerTarget.position);
                }
            }

            
        }
        else if(_currentState == SpiderState.CHASE)
        {
            if (agent.isOnNavMesh)
            {
                animator.SetBool("isIdle", false);
                agent.SetDestination(playerTarget.position);

                if (HasPlayerGoneAwayFromMeTooSAD())
                {
                    _currentState = SpiderState.PATROL;
                }
            }
        }
    }

    private void Update()
    {
        if (player == null)
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
            _currentState = SpiderState.PATROL;
            spiderStartedWalking = true;
            //Debug.Log("Spider just landed!");
            //startSpiderNavMeshMovement();

        }
        /*else if (spiderStartedWalking)
        {
            if (!playerHealth.didPlayerDie)
            {
                agent.enabled = true;
                rb.isKinematic = true;
                //Debug.Log("Spider started chasing ya!");
                //agent.SetDestination(playerTarget.position);
            }
        }*/
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

    /*private void startSpiderNavMeshMovement()
    {
        if (!playerHealth.didPlayerDie)
        {
            agent.enabled = true;
            agent.SetDestination(playerTarget.position);
            spiderStartedWalking = true;
        }
    }*/

    private void ChooseARandomPointAndMove()
    {

        //if (patrolPoints.Length <= 0) return;
        
        
        RaycastHit hit;

        float randomX = Random.Range(minBounds.position.x, maxBounds.position.x);
        float randomZ = Random.Range(minBounds.position.z, maxBounds.position.z);
        
        if(Physics.Raycast(new Vector3(randomX, minBounds.position.y, randomZ), Vector3.down, out hit, 9999f, groundLayerMask) && SpiderGroundCheck())
        {
            _currentTarget = hit.point;
            agent.SetDestination(_currentTarget);
        }
        else
        {
            Debug.Log("Raycast did not hit!");
        }

        //patrolPoints[Random.Range(0, patrolPoints.Length)];  
    }

    IEnumerator WaitAndChooseARandomPointAndMove(float timeToWait){
        _isWaiting = true;
        Debug.Log("Called WaitAndChooseARandomPointAndMove method!");
        yield return new WaitForSeconds(timeToWait);
        _currentState = SpiderState.PATROL;
        ChooseARandomPointAndMove();
        counter++;
        _isWaiting = false;
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, playerTarget.position) <= chaseDistance;
    }

    private bool HasPlayerGoneAwayFromMeTooSAD()
    {
        return Vector3.Distance(transform.position, playerTarget.position) >= giveUpDistance;
    }

    private bool IsInFOV()
    {
        _directionToPlayer = (playerTarget.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, _directionToPlayer) <= chaseCheckAngle;
    }
}
