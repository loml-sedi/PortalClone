using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Bedroom";
    [SerializeField] private float loadingDuration = 10f;

    private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement loadingScreen;
    private VisualElement progressFill;
    private Label progressLabel;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();

        root = uiDocument.rootVisualElement;

        loadingScreen = root.Q<VisualElement>("LoadingScreen");
        progressFill = root.Q<VisualElement>("ProgressFill");
        progressLabel = root.Q<Label>("ProgressLabel");

        if (loadingScreen != null)
        {
            loadingScreen.style.display = DisplayStyle.None;
        }
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadAfterDelay(nextSceneName));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAfterDelay(sceneName));
    }

    private IEnumerator LoadAfterDelay(string sceneName)
    {
        if (loadingScreen != null)
        {
            loadingScreen.style.display = DisplayStyle.Flex;
        }

        float elapsed = 0f;

        while (elapsed < loadingDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / loadingDuration);

            if (progressFill != null)
            {
                progressFill.style.width =
                    Length.Percent(progress * 100f);
            }

            if (progressLabel != null)
            {
                progressLabel.text =
                    Mathf.RoundToInt(progress * 100f) + "%";
            }

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}