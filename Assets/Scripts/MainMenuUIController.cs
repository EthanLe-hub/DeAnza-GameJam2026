using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUIController : MonoBehaviour
{
    private VisualElement mainMenu;
    private VisualElement introPanel;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        mainMenu = root.Q<VisualElement>("MainMenuContainer");
        introPanel = root.Q<VisualElement>("IntroPanel");

        root.Q<Button>("PlayButton").clicked += ShowIntro;
        root.Q<Button>("IntroBackButton").clicked += ShowMainMenu;
    }

    void ShowIntro()
    {
        Debug.Log("Play button works!");
        

        mainMenu.style.display = DisplayStyle.None;
        introPanel.style.display = DisplayStyle.Flex;
    }

    void ShowMainMenu()
    {
        introPanel.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.Flex;
    }
}