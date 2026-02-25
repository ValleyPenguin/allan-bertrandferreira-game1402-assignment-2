using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    
    [SerializeField] private float moveSpeed = 2;
    
    [SerializeField] private float rotationSpeed = 10;
    
    [SerializeField] private float gravity = -9.8f;
    
    [SerializeField] private float groundCheckDistance;
    
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private Vector3 groundCheckOffset;


    [SerializeField]
    private float jumpVelocity = 10f;


    [SerializeField] private LayerMask groundLayer;

    private Vector2 _moveInput;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _moveDirection;
    private CharacterController _characterController;
    private Quaternion _targetRotation;
    private Vector3 _velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        _characterController.Move(_velocity * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (IsGrounded())
        {
            Debug.Log("Jumped");
            _velocity.y = jumpVelocity;
        }
    }

    private void CalculateMovement()
    {
        _camForward = playerCamera.transform.forward;
        _camRight = playerCamera.transform.right;
        _camForward.y = 0;
        _camRight.y = 0;
        _camForward.Normalize();
        _camRight.Normalize();

        _moveDirection = _camRight * _moveInput.x + _camForward * _moveInput.y;

        _targetRotation = Quaternion.LookRotation(_moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);

        //Calculate gravity
        _velocity = _moveDirection * moveSpeed;
        _velocity.y += gravity;
    }

    private bool IsGrounded()
    {
        if(Physics.SphereCast(transform.position + groundCheckOffset, groundCheckRadius, Vector3.down, out RaycastHit hit,groundCheckDistance, groundLayer))
        {
            Debug.Log("SphereCast hit");
            return true;
        }
        if (!Physics.SphereCast(transform.position + groundCheckOffset, groundCheckRadius, Vector3.down, out RaycastHit hitTwo, groundCheckDistance, groundLayer))
        {
            Debug.Log("not grounded");
            return false;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + groundCheckOffset, groundCheckRadius);
        //Gizmos.DrawSphere(transform.position + groundCheckOffset + Vector3.down * groundCheckDistance, groundCheckRadius);
        //Gizmos.DrawCube(transform.position + groundCheckOffset + (Vector3.down * groundCheckDistance)/2, new Vector3(1.5f * groundCheckRadius, groundCheckDistance, 2 * groundCheckRadius));
    }
}



