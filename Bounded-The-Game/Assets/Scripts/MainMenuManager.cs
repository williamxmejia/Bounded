
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager _;
    [SerializeField] private bool _debugMode;
    [SerializeField] private string _sceneToLoadAfterClickingPlay;
    public CameraFollow cameraController;

    public GameObject container;

    public enum MainMenuButtons { play, options, quit, resume };

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            container.SetActive(true);
            Time.timeScale = 0;
            cameraController.enabled = false;
        }
    }
    public void Awake()
    {
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("There are more than 1 MainMenuManager in the scene");
        }
    }

    public void MainMenuClicked(MainMenuButtons buttonClicked)
    {
        DebugMessage(buttonClicked.ToString());

        switch (buttonClicked)
        {
            case MainMenuButtons.play:
                PlayClicked();
                break;
            case MainMenuButtons.options:
                break;
            case MainMenuButtons.quit:
                QuitGame();
                break;
            case MainMenuButtons.resume:
                ResumeClicked();
                break;
            default:
                Debug.Log("Button not implemented");
                break;
        }
    }

    private void DebugMessage(string message)
    {
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }

    public void PlayClicked()
    {
        SceneManager.LoadScene(_sceneToLoadAfterClickingPlay);
    }

    public void ResumeClicked()
    {
        container.SetActive(false);
        Time.timeScale = 1;
        cameraController.enabled = true;
        
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }

}
