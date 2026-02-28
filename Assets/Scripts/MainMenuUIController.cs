using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem; // new Input System

public class MainMenuUIController : MonoBehaviour
{
    private VisualElement mainMenu;
    private VisualElement introSequence;
    private VisualElement[] steps;
    private int currentStep = 0;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        mainMenu = root.Q<VisualElement>("MainMenuContainer");
        introSequence = root.Q<VisualElement>("IntroStorySequence");

        introSequence.style.display = DisplayStyle.None;

        int stepCount = 8;
        steps = new VisualElement[stepCount];
        for (int i = 1; i <= stepCount; i++)
        {
            steps[i - 1] = introSequence.Q<VisualElement>($"IntroStorySequence{i}");
            if (steps[i - 1] != null)
                steps[i - 1].style.display = DisplayStyle.None;
        }

        root.Q<Button>("PlayButton").clicked += ShowIntroSequence;
    }

    private void Update()
    {
        // Only advance story if intro is active
        if (introSequence.style.display == DisplayStyle.Flex)
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                StepToNext();
            }
        }
    }

    private void ShowIntroSequence()
    {
        mainMenu.style.display = DisplayStyle.None;
        introSequence.style.display = DisplayStyle.Flex;
        currentStep = 0;

        if (steps.Length > 0)
            steps[currentStep].style.display = DisplayStyle.Flex;
    }

    private void StepToNext()
    {
        if (currentStep < steps.Length)
        {
            steps[currentStep].style.display = DisplayStyle.None;
            currentStep++;

            if (currentStep < steps.Length)
                steps[currentStep].style.display = DisplayStyle.Flex;
            else
                ShowMainMenu(); // optional: go back to main menu
        }
    }

    private void ShowMainMenu()
    {
        introSequence.style.display = DisplayStyle.None;
        foreach (var step in steps)
            step.style.display = DisplayStyle.None;

        mainMenu.style.display = DisplayStyle.Flex;
    }
} 