using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] private InputAction shootInput;

    [SerializeField] private Transform shootPoint;

    [SerializeField] private GameObject shootObject;

    [SerializeField] private float shootForce;

    private GameObject _arrow;

    private void OnEnable()
    {
        shootInput.Enable();
        shootInput.performed += Shoot;
    }

    private void OnDisable()
    {
        shootInput.Disable();
        shootInput.performed -= Shoot;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        //create a new arrow
        _arrow = Instantiate(shootObject, shootPoint.position, shootPoint.rotation);

        //apply a force
        _arrow.GetComponent<Rigidbody>().AddForce(shootForce * shootPoint.forward);
    }
}
