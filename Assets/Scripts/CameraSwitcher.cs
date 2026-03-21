using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera exploreCamera;
    [SerializeField] private CinemachineCamera aimCamera;

    [SerializeField] private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        playerController.OnStateUpdated += SwitchCamera;
    }

    private void OnDisable()
    {
        playerController.OnStateUpdated -= SwitchCamera;
    }

    private void SwitchCamera(PlayerState state)
    {
        switch (state)
        {

            case PlayerState.EXPLORE:
                exploreCamera.Prioritize();
            break;

            case PlayerState.AIM:
                aimCamera.Prioritize();
            break;

            default:
                //nothing to do here
            break;

        }
    }
}
