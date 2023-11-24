using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; //this

public class UIController : MonoBehaviour
{
    public GameManager GameManager;
    public Camera m_camera;
    public PlayerController m_player;
    public Label m_ScoreLabel;
    public Label m_HealthLabel;

    public Button m_SwitchButton;
    public Button m_NextButton;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        m_ScoreLabel = root.Q<Label>("Score");
        m_HealthLabel = root.Q<Label>("Health");
        m_SwitchButton = root.Q<Button>("SwitchButton");
        m_NextButton = root.Q<Button>("NextButton");

        m_SwitchButton.clicked += SwitchButtonClicked;
        m_NextButton.clicked += NextButtonClicked;
    }

    // Update is called once per frame
    void Update()
    {
        m_ScoreLabel.text = "SCORE: " + m_player.Score;
        m_HealthLabel.text = "HEALTH: " + m_player.Health;
    }

    void SwitchButtonClicked()
    {
        if (m_camera.orthographic)
        {
            m_camera.transform.rotation = Quaternion.Euler(0, 0, 0);
            m_camera.orthographic = false;
        }
        else
        {
            m_camera.transform.rotation = Quaternion.Euler(90, 0, 0);
            m_camera.orthographic = true;
        }
    }

    void NextButtonClicked()
    {
        GameManager.PlacePuzzle();
    }
}
