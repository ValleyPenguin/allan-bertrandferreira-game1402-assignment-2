using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    private FrogState _currentState;

    //private GameObject player;

    private Transform playerTarget;

    [SerializeField] private Transform minBounds;
    [SerializeField] private Transform maxBounds;

    [SerializeField] private float chaseDistance;

    [SerializeField] private float attackDistance;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private float frogJumpForce;

    [SerializeField] private float frogGroundCheckOffset;

    [SerializeField] private float frogGroundCheckRadius;

    [SerializeField] private Animator animator;

    [SerializeField] private float frogRotationSpeed;

    private Vector3 _currentTarget;

    private bool _isWaiting;

    private void Start()
    {
        _currentState = FrogState.PATROL;
        playerTarget = GameObject.FindGameObjectWithTag("PlayerTargetForEnemies").GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        if (_currentState == FrogState.PATROL)
        {
            if(!_isWaiting) StartCoroutine(WaitAndChooseARandomPointAndMove(2f));
        }
        else if (_currentState == FrogState.CHASE)
        {
            if (!_isWaiting) StartCoroutine(WaitAndChooseARandomPointAndMove(2f));
        }
        else if (_currentState == FrogState.ATTACK)
        {
            if (!_isWaiting) StartCoroutine(WaitAndChooseARandomPointAndMove(2f));
        }

        Vector3 frogJumpDirection = (_currentTarget - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(frogJumpDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, frogRotationSpeed * Time.deltaTime);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, playerTarget.position) <= attackDistance)
        {
            _currentState = FrogState.ATTACK;
        }
        else if(Vector3.Distance(transform.position, playerTarget.position) <= chaseDistance)
        {
            _currentState = FrogState.PATROL;
        }
        else if(Vector3.Distance(transform.position, playerTarget.position) >= chaseDistance)
        {
            _currentState = FrogState.CHASE;
        }
    }

    private void ChooseARandomPointAndMove()
    {
        RaycastHit hit;

        float randomX = Random.Range(minBounds.position.x, maxBounds.position.x);
        float randomZ = Random.Range(minBounds.position.z, maxBounds.position.z);

        if (Physics.Raycast(new Vector3(randomX, minBounds.position.y, randomZ), Vector3.down, out hit, 9999f, groundLayerMask))
        {
            _currentTarget = hit.point;
        }
        else
        {
            Debug.Log("Raycast did not hit!");
        }

        Vector3 frogJumpDirection = (_currentTarget - transform.position).normalized;
        if (FrogGroundCheck())
        {
            animator.SetTrigger("justJumped");
            rb.AddForce((frogJumpDirection + Vector3.up) * frogJumpForce, ForceMode.Force);
        }
    }

    IEnumerator WaitAndChooseARandomPointAndMove(float timeToWait)
    {
        _isWaiting = true;
        yield return new WaitForSeconds(timeToWait);
        _currentState = FrogState.PATROL;
        ChooseARandomPointAndMove();
        _isWaiting = false;
    }

    private bool FrogGroundCheck()
    {
        bool isGrounded = Physics.CheckSphere(transform.position + new Vector3(0f, frogGroundCheckOffset, 0f), frogGroundCheckRadius, groundLayerMask);
        //Debug.Log(isGrounded);
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, frogGroundCheckOffset, 0f), frogGroundCheckRadius);
    }
}
