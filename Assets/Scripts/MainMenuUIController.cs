using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MainMenuUIController : MonoBehaviour
{
    [Header("UI Toolkit")]
    public UIDocument uiDocument;

    [Header("Gameplay UI (uGUI)")]
    public GameObject gameplayCanvas; // Assign your gameplay canvas root here

    private VisualElement mainMenuContainer;
    private Button playButton;

    private VisualElement introContainer;
    private List<VisualElement> introPanels = new List<VisualElement>();

    private int currentIndex = -1;
    private bool introActive = false;

    void Start()
    {
        var root = uiDocument.rootVisualElement;

        // Make sure gameplay UI is OFF at start
        gameplayCanvas.SetActive(false);

        // Main Menu
        mainMenuContainer = root.Q<VisualElement>("MainMenuContainer");
        playButton = root.Q<Button>("PlayButton");

        // Intro Container
        introContainer = root.Q<VisualElement>("IntroStorySequenceExposition");

        // Intro Story Panels (1–8)
        for (int i = 1; i <= 8; i++)
        {
            var panel = root.Q<VisualElement>($"IntroStorySequence{i}");
            if (panel != null)
                introPanels.Add(panel);
        }

        // Intro Shop Panels (1–4)
        for (int i = 1; i <= 4; i++)
        {
            var panel = root.Q<VisualElement>($"IntroSequenceShop{i}");
            if (panel != null)
                introPanels.Add(panel);
        }

        // Hide intro at start
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
        // Hide previous panel
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

        // 🔥 Hide entire UI Toolkit document
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;

        // OR completely disable the GameObject (even cleaner)
        uiDocument.gameObject.SetActive(false);

        // 🔥 Enable gameplay UI
        gameplayCanvas.SetActive(true);
    }
}