using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "TestChamber1";
    [SerializeField] private float loadingDuration = 10f;

    private UIDocument uiDocument;
    private VisualElement progressFill;
    private Label progressLabel;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();

        VisualElement root = uiDocument.rootVisualElement;

        progressFill = root.Q<VisualElement>("ProgressFill");
        progressLabel = root.Q<Label>("ProgressLabel");
    }

    private void Start()
    {
        StartCoroutine(LoadAfterDelay());
    }

    private IEnumerator LoadAfterDelay()
    {
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

        SceneManager.LoadScene(nextSceneName);
    }
}