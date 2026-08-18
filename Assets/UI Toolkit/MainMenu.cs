using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UIDocument uiDocument;

    [Header("Scenes")]
    [SerializeField] private string loadingSceneName = "LoadingScreen";

    private Button playButton;
    private Button settingsButton;
    private Button extrasButton;
    private Button quitButton;

    private void Awake()
    {
        // Get UIDocument automatically if it hasn't been assigned
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }

        VisualElement root = uiDocument.rootVisualElement;

        // Find buttons from the UXML
        playButton = root.Q<Button>("PlayButton");
        settingsButton = root.Q<Button>("SettingsButton");
        extrasButton = root.Q<Button>("ExtrasButton");
        quitButton = root.Q<Button>("QuitButton");

        // Connect button events
        if (playButton != null)
        {
            playButton.clicked += StartGame;
        }

        if (settingsButton != null)
        {
            settingsButton.clicked += OpenSettings;
        }

        if (extrasButton != null)
        {
            extrasButton.clicked += OpenExtras;
        }

        if (quitButton != null)
        {
            quitButton.clicked += QuitGame;
        }
    }

    private void StartGame()
    {
        Debug.Log("Starting game...");

        SceneManager.LoadScene(loadingSceneName);
    }

    private void OpenSettings()
    {
        Debug.Log("Settings opened.");

        // Add settings UI here later.
    }

    private void OpenExtras()
    {
        Debug.Log("Extras opened.");

        // Add extras UI here later.
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        if (playButton != null)
        {
            playButton.clicked -= StartGame;
        }

        if (settingsButton != null)
        {
            settingsButton.clicked -= OpenSettings;
        }

        if (extrasButton != null)
        {
            extrasButton.clicked -= OpenExtras;
        }

        if (quitButton != null)
        {
            quitButton.clicked -= QuitGame;
        }
    }
}