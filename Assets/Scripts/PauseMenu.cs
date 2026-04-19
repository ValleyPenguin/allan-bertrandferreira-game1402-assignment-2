using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private InputAction escapeInput;

    [SerializeField] private GameObject pauseMenu;

    private bool isGamePaused;

    private void Start()
    {
        isGamePaused = false;
        pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        escapeInput.Enable();
        escapeInput.performed += PauseResume;
    }

    private void OnDisable()
    {
        escapeInput.Disable();
        escapeInput.performed -= PauseResume;
    }

    public void PauseResume(InputAction.CallbackContext context)
    {

        if (!isGamePaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isGamePaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isGamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
