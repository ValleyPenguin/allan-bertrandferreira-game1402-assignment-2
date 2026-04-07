using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System.Collections;

public class Frog : MonoBehaviour
{
    private FrogState _currentState;

    private GameObject player;

    private Transform playerTarget;

    [SerializeField] private Transform minBounds;
    [SerializeField] private Transform maxBounds;

    [SerializeField] private float chaseDistance;

    [SerializeField] private float attackDistance;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private LayerMask _groundLayerMask;

    private Vector3 _currentTarget;

    private bool _isWaiting;

    private void Start()
    {
        _currentState = FrogState.PATROL;
    }
    void FixedUpdate()
    {
        if (_currentState == FrogState.PATROL)
        {

        }
        else if (_currentState == FrogState.CHASE)
        {

        }
        else if (_currentState == FrogState.ATTACK)
        {

        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, playerTarget.position) <= chaseDistance)
        {
            _currentState = FrogState.CHASE;
        }
        else if(Vector3.Distance(transform.position, playerTarget.position) >= chaseDistance)
        {
            _currentState = FrogState.PATROL;
        }
        else if(Vector3.Distance(transform.position, playerTarget.position) <= attackDistance)
        {
            _currentState = FrogState.ATTACK;
        }
    }

    private void ChooseARandomPointAndMove()
    {
        RaycastHit hit;

        float randomX = Random.Range(minBounds.position.x, maxBounds.position.x);
        float randomZ = Random.Range(minBounds.position.z, maxBounds.position.z);

        if (Physics.Raycast(new Vector3(randomX, minBounds.position.y, randomZ), Vector3.down, out hit, 9999f, _groundLayerMask))
        {
            _currentTarget = hit.point;
        }
        else
        {
            Debug.Log("Raycast did not hit!");
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


}
