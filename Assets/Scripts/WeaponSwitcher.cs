using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject smg;
    [SerializeField] private InputAction equipPistol;
    [SerializeField] private InputAction equipSMG;
    [SerializeField] private InputAction scrollWheel;

    private int _currentWeapon = 0;

    private void Start()
    {
        EquipWeapon(0);
    }

    private void OnEnable()
    {
        equipPistol.Enable();
        equipSMG.Enable();
        scrollWheel.Enable();
        equipPistol.performed += EquipPistol;
        equipSMG.performed += EquipSMG;
        scrollWheel.performed += Scroll;
    }

    private void OnDisable()
    {
        equipPistol.Disable();
        equipSMG.Disable();
        scrollWheel.Disable();
        equipPistol.performed -= EquipPistol;
        equipSMG.performed -= EquipSMG;
        scrollWheel.performed -= Scroll;
    }

    private void EquipPistol(InputAction.CallbackContext context)
    {
        EquipWeapon(0);
    }

    private void EquipSMG(InputAction.CallbackContext context)
    {
        EquipWeapon(1);
    }

    private void Scroll(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<Vector2>().y;
        if (scrollValue > 0f) EquipWeapon(0);
        if (scrollValue < 0f) EquipWeapon(1);
    }

    private void EquipWeapon(int index)
    {
        if (index == _currentWeapon) return;

        _currentWeapon = index;

        if (_currentWeapon == 0)
        {
            pistol.SetActive(true);
        }
        else
        {
            pistol.SetActive(false);
        }

        if (_currentWeapon == 1)
        {
            smg.SetActive(true);
        }
        else
        {
            smg.SetActive(false);
        }
        
    }
}