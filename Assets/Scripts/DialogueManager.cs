using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/**
 * Handles character dialogue sequences, including nested button text sequences.
 * Supports both narrative customers (dynamic A/B/C options) and normal customers (simple dialogue + Continue button)
 **/
public class DialogueManager : MonoBehaviour
{
    [HideInInspector] public CharacterData currentCharacter;
    [HideInInspector] public TextMeshProUGUI dialogueText;

    [HideInInspector] public Button optionAButton;
    [HideInInspector] public Button optionBButton;
    [HideInInspector] public Button optionCButton;

    private CharacterData.VisitDialogue currentVisit;
    private int[] optionTextIndices = new int[3]; // Track which text index we are on per button

    public void StartVisit()
    {
        if (currentCharacter.visitNumber >= currentCharacter.visits.Count)
        {
            Debug.LogWarning("No more visits defined.");
            return;
        }

        currentVisit = currentCharacter.visits[currentCharacter.visitNumber];

        // Reset button indices
        optionTextIndices[0] = 0;
        optionTextIndices[1] = 0;
        optionTextIndices[2] = 0;

        // Determine if any button texts exist (narrative customer)
        bool hasAnyButtonTexts =
            (currentVisit.optionAButtonTexts != null && currentVisit.optionAButtonTexts.Count > 0) ||
            (currentVisit.optionBButtonTexts != null && currentVisit.optionBButtonTexts.Count > 0) ||
            (currentVisit.optionCButtonTexts != null && currentVisit.optionCButtonTexts.Count > 0);

        if (hasAnyButtonTexts)
        {
            // Narrative customer
            UpdateOptionButtons();
            string introText = (currentVisit.intro != null && currentVisit.intro.Count > 0) ? currentVisit.intro[0] : "Hello!";
            dialogueText.text = introText;
        }
        else
        {
            // Normal customer or fallback
            ShowNormalCustomerIntro();
        }
    }

    private void ShowNormalCustomerIntro()
    {
        optionAButton.gameObject.SetActive(false);
        optionBButton.gameObject.SetActive(false);
        optionCButton.gameObject.SetActive(true);

        string introText = (currentVisit.intro != null && currentVisit.intro.Count > 0) ? currentVisit.intro[0] : "Hello!";
        dialogueText.text = introText;

        optionCButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";

        optionCButton.onClick.RemoveAllListeners();
        optionCButton.onClick.AddListener(() =>
        {
            // Hide button after click or optionally continue to bouquet
            optionCButton.gameObject.SetActive(false);
        });
    }

    private void UpdateOptionButtons()
    {
        // Option A
        if (currentVisit.optionAButtonTexts != null && currentVisit.optionAButtonTexts.Count > 0)
        {
            optionAButton.gameObject.SetActive(true);
            optionAButton.GetComponentInChildren<TextMeshProUGUI>().text = currentVisit.optionAButtonTexts[0];
            optionAButton.onClick.RemoveAllListeners();
            optionAButton.onClick.AddListener(() => OnOptionClicked(0));
        }
        else
        {
            optionAButton.gameObject.SetActive(false);
        }

        // Option B
        if (currentVisit.optionBButtonTexts != null && currentVisit.optionBButtonTexts.Count > 0)
        {
            optionBButton.gameObject.SetActive(true);
            optionBButton.GetComponentInChildren<TextMeshProUGUI>().text = currentVisit.optionBButtonTexts[0];
            optionBButton.onClick.RemoveAllListeners();
            optionBButton.onClick.AddListener(() => OnOptionClicked(1));
        }
        else
        {
            optionBButton.gameObject.SetActive(false);
        }

        // Option C
        if (currentVisit.optionCButtonTexts != null && currentVisit.optionCButtonTexts.Count > 0)
        {
            optionCButton.gameObject.SetActive(true);
            optionCButton.GetComponentInChildren<TextMeshProUGUI>().text = currentVisit.optionCButtonTexts[0];
            optionCButton.onClick.RemoveAllListeners();
            optionCButton.onClick.AddListener(() => OnOptionClicked(2));
        }
        else
        {
            optionCButton.gameObject.SetActive(false);
        }
    }

    private void OnOptionClicked(int optionIndex)
    {
        List<string> buttonTexts = optionIndex == 0 ? currentVisit.optionAButtonTexts :
                                   optionIndex == 1 ? currentVisit.optionBButtonTexts :
                                   currentVisit.optionCButtonTexts;

        // Nested "Continue" button behavior
        if (buttonTexts != null && optionTextIndices[optionIndex] < buttonTexts.Count - 1)
        {
            optionTextIndices[optionIndex]++;
            dialogueText.text = buttonTexts[optionTextIndices[optionIndex]];

            // Show only Option C as Continue
            optionAButton.gameObject.SetActive(false);
            optionBButton.gameObject.SetActive(false);
            optionCButton.gameObject.SetActive(true);
            optionCButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonTexts[optionTextIndices[optionIndex]];
            optionCButton.onClick.RemoveAllListeners();
            optionCButton.onClick.AddListener(() => OnOptionClicked(optionIndex));
        }
        else
        {
            // Sequence finished: hide buttons
            optionAButton.gameObject.SetActive(false);
            optionBButton.gameObject.SetActive(false);
            optionCButton.gameObject.SetActive(false);

            // Start main dialogue lines
            List<string> lines = optionIndex == 0 ? currentVisit.optionA :
                                 optionIndex == 1 ? currentVisit.optionB :
                                 currentVisit.optionC;

            if (lines != null && lines.Count > 0)
                StartCoroutine(PlaySequence(lines));
        }
    }

    IEnumerator PlaySequence(List<string> lines)
    {
        foreach (string line in lines)
        {
            dialogueText.text = line;
            yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
        }
    }

    public void ShowResult(bool goodBouquet)
    {
        StopAllCoroutines();

        List<string> lines = goodBouquet ? currentVisit.goodResult : currentVisit.badResult;

        optionAButton.gameObject.SetActive(false);
        optionBButton.gameObject.SetActive(false);

        if (lines != null && lines.Count > 0)
        {
            optionCButton.gameObject.SetActive(true);
            optionCButton.GetComponentInChildren<TextMeshProUGUI>().text = lines[0];

            int currentIndex = 1;
            optionCButton.onClick.RemoveAllListeners();
            optionCButton.onClick.AddListener(() =>
            {
                if (currentIndex < lines.Count)
                {
                    optionCButton.GetComponentInChildren<TextMeshProUGUI>().text = lines[currentIndex];
                    currentIndex++;
                }
                else
                {
                    optionCButton.gameObject.SetActive(false);
                }
            });
        }
    }
}