// Ethan Le (2/27/2026):
using UnityEngine; 

/** 
 * Script to check if the player's constructed bouquet matches the customer's requirements.
**/ 
public class BouquetSubmissionManager : MonoBehaviour
{
    public BouquetManager bouquetManager;
    public CharacterData currentCharacter;
    public DialogueManager dialogueManager; 

    public bool goodBouquet;

    public void SubmitBouquet()
    {
        FlowerData[] bouquet = bouquetManager.GetBouquet(); // Retrieve player's constructed bouquet that was submitted. 

        var currentVisit = currentCharacter.visits[currentCharacter.visitNumber]; // Get the appropriate set of customer overall details (dialogue and requirements) based on current visit. 

        int matches = 0; // Checking if the requirements are met. 

        foreach (FlowerData requirement in currentVisit.hiddenRequirements) // Loop through the per-visit hidden requirements. 
        {
            foreach (FlowerData flower in bouquet)
            {
                if (flower == requirement)
                {
                    matches++; // Increment completion status if the current FlowerData matches a customer's requirement. 
                    break;
                }
            }
        }

        goodBouquet = matches == currentVisit.hiddenRequirements.Count; // goodBouquet flag is set to true if ALL hidden requirements for the current visit are met. 

        currentCharacter.satisfied = goodBouquet;

        dialogueManager.ShowResult(goodBouquet); // Show the appropriate customer response after submitting bouquet. 

        HandleRevisitLogic();
    }

    void HandleRevisitLogic()
    {
        currentCharacter.visitNumber++; // Increment the customer's visit number. 

        // Prevent going beyond defined visits -- ensure the customer does not visit beyond max defined visits: 
        if (currentCharacter.visitNumber >= currentCharacter.visits.Count)
        {
            currentCharacter.willRevisit = false;
            return;
        }

        // If the customer has not completed all REQUIRED visits yet, they will revisit again soon. 
        if (currentCharacter.visitNumber < currentCharacter.numberOfRequiredVisits)
        {
            currentCharacter.willRevisit = true;
        }

        // Otherwise, if the customer is not satisfied and is still not at the max visit number for their character, they will revisit. 
        else if (currentCharacter.visitNumber < currentCharacter.maxNumberOfVisits &&
                 !currentCharacter.satisfied)
        {
            currentCharacter.willRevisit = true;
        }

        // Otherwise, if they are either satisfied and do not return upon satisfaction, or if they are at the max number of visits, then they will not visit again. 
        else
        {
            currentCharacter.willRevisit = false;
        }
    }
}