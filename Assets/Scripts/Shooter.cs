using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] private InputAction shootInput;

    [SerializeField] private Transform shootPoint;

    [SerializeField] private GameObject shootObject;

    [SerializeField] private float shootForce;

    [SerializeField] private Transform aimTrack;

    private GameObject _arrow;
    private Vector3 _shootDirection;
    private PlayerState _currentState;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        shootInput.Enable();
        shootInput.performed += Shoot;


        _playerController.OnStateUpdated += (state) => _currentState = state;
        _playerController.OnStateUpdated += StateUpdate;

    }

    void StateUpdate(PlayerState state)
    {
        _currentState = state;
    }



    private void OnDisable()
    {
        shootInput.Disable();
        shootInput.performed -= Shoot;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (_currentState != PlayerState.AIM) return;

        //calculate the direction
        _shootDirection = aimTrack.position - shootPoint.position;
        _shootDirection.Normalize();

        //create a new arrow OLD: shootPoint.rotation
        _arrow = Instantiate(shootObject, shootPoint.position, Quaternion.LookRotation(_shootDirection));

        //apply a force
        _arrow.GetComponent<Rigidbody>().AddForce(shootForce * _shootDirection, ForceMode.Impulse);
    }
}

//CHALLENGE FROM INDI - Make direction of arrow go with the velocity direction,
// so if its going downwards it should be aiming downwards-might not need to do if using guns instead
// ALSO - Destroy Arrows/Bullets after a while, so they don't go on forever
// ALSO - perhaps add a crosshair(of course)
