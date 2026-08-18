using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UIDocument uiDocument;

    [Header("Input")]
    [SerializeField] private InputActionReference pauseAction;

    private VisualElement pauseMenu;

    private Button resumeButton;
    private Button doneButton;
    private Button settingsButton;
    private Button mainMenuButton;

    private bool isPaused = false;

    private void Awake()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }

        VisualElement root = uiDocument.rootVisualElement;

        pauseMenu = root.Q<VisualElement>("PauseMenu");

        resumeButton = root.Q<Button>("ResumeButton");
        doneButton = root.Q<Button>("DoneButton");
        settingsButton = root.Q<Button>("SettingsButton");
        mainMenuButton = root.Q<Button>("MainMenuButton");

        if (pauseMenu != null)
        {
            pauseMenu.style.display = DisplayStyle.None;
        }

        if (resumeButton != null)
        {
            resumeButton.clicked += ResumeGame;
        }

        if (doneButton != null)
        {
            doneButton.clicked += ResumeGame;
        }


        if (settingsButton != null)
        {
            settingsButton.clicked += OpenSettings;
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.clicked += ReturnToMainMenu;
        }
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPausePressed;
            pauseAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePressed;
            pauseAction.action.Disable();
        }
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0f;

        if (pauseMenu != null)
        {
            pauseMenu.style.display = DisplayStyle.Flex;
        }
        if (resumeButton != null)
        {
            resumeButton.Focus();
        }
    }

    private void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;

        if (pauseMenu != null)
        {
            pauseMenu.style.display = DisplayStyle.None;
        }
    }

    private void OpenSettings()
    {
        Debug.Log("Settings opened.");
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        Debug.Log("Returning to Main Menu.");

        SceneManager.LoadScene("MainMenu");

    }

    private void OnDestroy()
    {
        if (resumeButton != null)
        {
            resumeButton.clicked -= ResumeGame;
        }

        if (doneButton != null)
        {
            doneButton.clicked -= ResumeGame;
        }

        if (settingsButton != null)
        {
            settingsButton.clicked -= OpenSettings;
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.clicked -= ReturnToMainMenu;
        }
        Time.timeScale = 1f;
    }
}
