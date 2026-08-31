using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public GameManager GameManager;
    public Camera m_camera;
    public PlayerController m_player;
    public Label m_ScoreLabel;
    public Label m_HealthLabel;

    public VisualElement m_puzzle;
    public Button m_SwitchButton;
    public Button m_NextButton;

    public VisualElement m_DeathScreen;
    public Label m_scoreLabel;
    public Button m_MainMenuDeathButton;

    public VisualElement m_MenuScreen;
    public Button m_startGameButton;
    public Button m_openTutorial;
    public Button m_leaveGameButton;

    public VisualElement m_tutorial;
    public Button m_tutorialButton;

    public VisualElement m_fps;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        var root = GetComponent<UIDocument>().rootVisualElement;

        m_ScoreLabel = root.Q<Label>("Score");
        m_HealthLabel = root.Q<Label>("Health");
        m_puzzle = root.Q<VisualElement>("Puzzle");
        m_SwitchButton = root.Q<Button>("SwitchButton");
        m_NextButton = root.Q<Button>("NextButton");
        m_fps = root.Q<VisualElement>("FPS");
        m_DeathScreen = root.Q<VisualElement>("Death");
        m_MainMenuDeathButton = root.Q<Button>("MainMenuDeath");
        m_scoreLabel = root.Q<Label>("DeathScore");
        m_MenuScreen = root.Q<VisualElement>("MainMenu");
        m_startGameButton = root.Q<Button>("StartGameButton");
        m_openTutorial = root.Q<Button>("OpenTutorialButton");
        m_leaveGameButton = root.Q<Button>("ExitGameButton");
        m_tutorial = root.Q<VisualElement>("TutorialScreen");
        m_tutorialButton = root.Q<Button>("TutorialButton");

        m_SwitchButton.clicked += SwitchButtonClicked;
        m_NextButton.clicked += NextButtonClicked;
        m_MainMenuDeathButton.clicked += ToMainMenu;
        m_startGameButton.clicked += StartGame;
        m_openTutorial.clicked += OpenTutorial;
        m_leaveGameButton.clicked += LeaveGame;
        m_tutorialButton.clicked += ToMainMenu;

        m_fps.visible = false;
        m_tutorial.visible = false;

        // Hide the exit button completely on WebGL since players can't "quit" a webpage
#if UNITY_WEBGL
        if (m_leaveGameButton != null)
        {
            m_leaveGameButton.style.display = DisplayStyle.None;
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        m_ScoreLabel.text = "SCORE: " + m_player.Score;
        m_HealthLabel.text = "HEALTH: " + m_player.Health;
    }

    public void SwitchButtonClicked()
    {
        if (m_camera.orthographic)
        {
            m_camera.transform.rotation = Quaternion.Euler(0, 0, 0);
            m_camera.orthographic = false;
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            m_fps.visible = true;
            m_puzzle.visible = false;
        }
        else
        {
            m_camera.transform.rotation = Quaternion.Euler(90, 0, 0);
            m_camera.orthographic = true;
            UnityEngine.Cursor.visible = true;

            // WebGL does not support Confined cursor lock mode
#if UNITY_WEBGL
            UnityEngine.Cursor.lockState = CursorLockMode.None;
#else
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
#endif

            m_fps.visible = false;
            m_puzzle.visible = true;
        }
    }

    public void Die(int score)
    {
        m_DeathScreen.visible = true;
        m_scoreLabel.text = "SCORE: " + score;
        UnityEngine.Cursor.visible = true;

#if UNITY_WEBGL
        UnityEngine.Cursor.lockState = CursorLockMode.None;
#else
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
#endif

        Time.timeScale = 0;
    }

    void NextButtonClicked()
    {
        GameManager.PlacePuzzle();
        m_player.Eat();
    }

    void ToMainMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void StartGame()
    {
        m_MenuScreen.visible = false;
        m_DeathScreen.visible = false;
        Time.timeScale = 1;
    }

    void LeaveGame()
    {
#if UNITY_WEBGL
        Debug.Log("Application.Quit() is not supported in WebGL.");
#else
        Application.Quit();
#endif
    }

    void OpenTutorial()
    {
        m_tutorial.visible = true;
    }
}