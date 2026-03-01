using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour
{
    public UIDocument uiDocument;
    public string gameplaySceneName = "CustomerOrderPanel"; // Change to your real scene name

    private VisualElement mainMenuContainer;
    private Button playButton;

    private VisualElement introContainer;
    private List<VisualElement> introPanels = new List<VisualElement>();

    private int currentIndex = -1;
    private bool introActive = false;

    void Start()
    {
        var root = uiDocument.rootVisualElement;

        // Main Menu
        mainMenuContainer = root.Q<VisualElement>("MainMenuContainer");
        playButton = root.Q<Button>("PlayButton");

        // Intro
        introContainer = root.Q<VisualElement>("IntroStorySequenceExposition");

        for (int i = 1; i <= 8; i++)
        {
            var panel = root.Q<VisualElement>($"IntroStorySequence{i}");
            if (panel != null)
                introPanels.Add(panel);
        }

        // Ensure intro is hidden at start
        introContainer.style.display = DisplayStyle.None;

        playButton.clicked += StartIntro;
    }

    void StartIntro()
    {
        mainMenuContainer.style.display = DisplayStyle.None;
        introContainer.style.display = DisplayStyle.Flex;

        introActive = true;
        currentIndex = -1;

        ShowNextPanel();
    }

    void Update()
    {
        if (!introActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            ShowNextPanel();
        }
    }

    void ShowNextPanel()
    {
        // Hide previous
        if (currentIndex >= 0 && currentIndex < introPanels.Count)
        {
            introPanels[currentIndex].style.display = DisplayStyle.None;
        }

        currentIndex++;

        if (currentIndex >= introPanels.Count)
        {
            EndIntro();
            return;
        }

        introPanels[currentIndex].style.display = DisplayStyle.Flex;
    }

    void EndIntro()
    {
        introActive = false;

        // Load your actual gameplay scene
        SceneManager.LoadScene(gameplaySceneName);
    }
}