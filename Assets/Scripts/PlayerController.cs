using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    public float moveSpeed;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float gravity = -9.8f;
    public float jumpVelocity = 10f;

    [Space(10)]
    [Header("Ground Check")]
    [SerializeField] private float groundCheckOffset;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    public event Action OnJumpEvent;

    private Vector2 _moveInput;

    private Vector2 _lookInput;

    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _moveDirection;
    private CharacterController _characterController;
    private Quaternion _targetRotation;
    private Vector3 _velocity;
    public bool _isGrounded;

    
    [SerializeField] private float moveSpeedAimed = 2f;
    [SerializeField] private float rotationSpeedAimed = 10f;

    [SerializeField] private Transform aimTrack;

    [SerializeField] private float maxAimHeight;
    [SerializeField] private float minAimHeight;

    [SerializeField] private float maxAimAngle;
    [SerializeField] private float minAimAngle;
    

    [SerializeField] private Vector3 _defaultAimTrackerPosition;

    [SerializeField] private Vector3 _tempAimTrackerPosition;

    private PlayerState _currentState;

    public event Action<PlayerState> OnStateUpdated;

    private float _aimAngleUpDown;

    [SerializeField] private Transform playerSpineBone;

    [SerializeField] private Transform camTransform;

    private Vector3 _directionToAimTarget;

    private Quaternion _targetPlayerDirection;

    private Quaternion _cameraInitialLocalRotation;

    [SerializeField] private Transform camPivot;

    [SerializeField] private Transform rootBoneTransform;

    private float _aimAngleLeftRight;

    private Quaternion _initialSpineLocalRotation;

    //private SpeedPowerUp _speedPowerUp;

    public Transform playerTransform;

    [SerializeField] private Transform playerCapsule;

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    public Vector3 GetPlayerVelocity()
    {
        return _velocity;
    }


    public bool _IsGrounded
    {
        get => _isGrounded;
        private set { _isGrounded = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _currentState = PlayerState.EXPLORE;
        _cameraInitialLocalRotation = playerCamera.transform.localRotation;
        _initialSpineLocalRotation = playerSpineBone.localRotation;

        //_speedPowerUp = new SpeedPowerUp();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
  
        if (_currentState == PlayerState.EXPLORE)
        {
            ShowHideCrosshair.Instance.HideCrosshair();
            CalculateMovementExplore();
        }

        else if (_currentState == PlayerState.AIM)
        {

            _aimAngleLeftRight += _lookInput.x * rotationSpeedAimed;
            transform.rotation = Quaternion.Euler(0f, _aimAngleLeftRight, 0f);


            ShowHideCrosshair.Instance.ShowCrosshair();
            CalculateMovementAim();

            _camForward = playerCamera.transform.forward;
            _camForward.y = 0f;
        }

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -0.2f;
        }
    }

    //late update because im messing with the spine of the player, don't want animations to break
    private void LateUpdate()
    {
        if (_currentState == PlayerState.AIM)
        {
            UpdateAimTrack();
        }
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (_isGrounded)
        {
            Debug.Log("JUMP");
            _velocity.y = jumpVelocity;
            OnJumpEvent?.Invoke();
        }
    }

    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    public void OnAim(InputValue value)
    {
        _currentState = value.isPressed ? PlayerState.AIM : PlayerState.EXPLORE;

        if(_currentState == PlayerState.AIM)
        {
            _camForward = playerCamera.transform.forward;
            _camForward.y = 0f;
            _camForward.Normalize();

            transform.rotation = Quaternion.LookRotation(_camForward);

            _aimAngleLeftRight = transform.eulerAngles.y;

            _aimAngleUpDown = playerCamera.transform.eulerAngles.x;
            

            if (_aimAngleUpDown > 180f)
            {
                _aimAngleUpDown -= 360f;
            }
        }
        OnStateUpdated?.Invoke(_currentState);
    }

    private void CalculateMovementExplore()
    {
        _camForward = playerCamera.transform.forward;
        _camRight = playerCamera.transform.right;
        _camForward.y = 0;
        _camRight.y = 0;
        _camForward.Normalize();
        _camRight.Normalize();

        _moveDirection = _camRight * _moveInput.x + _camForward * _moveInput.y;

        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            _targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
        }

        _velocity = Vector3.up * _velocity.y + _moveDirection * moveSpeed;
        //Calculate gravity
        _velocity.y += gravity * Time.deltaTime;
    }

    private void CalculateMovementAim()
    {
        // WASD relates to where the player is currently facing
        // Left/Right goes sideways, forward/back moves along the players facing direction
        // scalar multiplied by vector is faster than vector multiplied by scalar, order matters
        _moveDirection = _moveInput.x * transform.right + _moveInput.y * transform.forward;
        _velocity = _velocity.y * Vector3.up + moveSpeedAimed * _moveDirection;
        _velocity.y += gravity * Time.deltaTime;
        
    }

    private void UpdateAimTrack()
    {
        _aimAngleUpDown -= _lookInput.y * rotationSpeedAimed;
        _aimAngleUpDown = Mathf.Clamp(_aimAngleUpDown, minAimAngle, maxAimAngle);


        camPivot.localRotation = Quaternion.Euler(_aimAngleUpDown, 0f, 0f);
        playerSpineBone.localRotation = _initialSpineLocalRotation * Quaternion.Euler(_aimAngleUpDown, 0f, 0f);
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(transform.position + new Vector3(0f, groundCheckOffset, 0f), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, groundCheckOffset, 0f), groundCheckRadius);
    }
}
