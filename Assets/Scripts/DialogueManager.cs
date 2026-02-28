// Ethan Le (2/27/2026):
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using TMPro; 
using UnityEngine.InputSystem; 
/** 
 * Script to handle the appropriate set of dialogue that the character/customer will say.
**/ 
public class DialogueManager : MonoBehaviour
{
    public CharacterData currentCharacter;
    public TextMeshProUGUI dialogueText;

    private CharacterData.VisitDialogue currentVisit; // Current set of character dialogues for this visit. 
    private bool waitingForClick;

    public void StartVisit()
    {
        // Safety check to ensure that a customer does not visit beyond their max defined visit: 
        if (currentCharacter.visitNumber >= currentCharacter.visits.Count)
        {
            Debug.LogWarning("No more visits defined.");
            return;
        }

        currentVisit = currentCharacter.visits[currentCharacter.visitNumber];
        StartCoroutine(PlaySequence(currentVisit.intro));
    }

    public void ChooseOption(int optionIndex)
    {
        StopAllCoroutines();

        switch (optionIndex) // Character's next dialogue depends on player option choice. 
        {
            case 0:
                StartCoroutine(PlaySequence(currentVisit.optionA));
                break;
            case 1:
                StartCoroutine(PlaySequence(currentVisit.optionB));
                break;
            case 2:
                StartCoroutine(PlaySequence(currentVisit.optionC));
                break;
        }
    }

    public void ShowResult(bool goodBouquet) // Character's good/bad dialogue depends on if all FlowerData requirements are satisfied. 
    {
        StopAllCoroutines();

        if (goodBouquet)
            StartCoroutine(PlaySequence(currentVisit.goodResult));
        else
            StartCoroutine(PlaySequence(currentVisit.badResult));
    }

    IEnumerator PlaySequence(List<string> lines) // Play the dialogue, waiting for mouse-click when necessary. 
    {
        foreach (string line in lines)
        {
            dialogueText.text = line;
            waitingForClick = true;

            yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
            waitingForClick = false;
        }
    }
}