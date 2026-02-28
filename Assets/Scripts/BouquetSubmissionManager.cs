using System.Collections.Generic;
using UnityEngine;

public class BouquetSubmissionManager : MonoBehaviour
{
    public BouquetManager bouquetManager;
    public CharacterData currentCharacter;
    public DialogueManager dialogueManager;

    [Header("Optional: Budget Check")]
    public bool checkCoinBudget = false;

    [HideInInspector] public bool goodBouquet;

    public void SubmitBouquet()
    {
        FlowerData[] bouquet = bouquetManager.GetBouquet();

        // Get the current visit
        if (currentCharacter.visitNumber >= currentCharacter.visits.Count)
        {
            Debug.LogWarning("No more visits defined for this character.");
            return;
        }

        var currentVisit = currentCharacter.visits[currentCharacter.visitNumber];

        // If no hidden requirements, automatically good
        if (currentVisit.hiddenRequirements == null || currentVisit.hiddenRequirements.Count == 0)
        {
            goodBouquet = true;
        }
        else
        {
            goodBouquet = CheckRequirements(bouquet, currentVisit.hiddenRequirements);
        }

        // Optional: check total coin cost
        if (checkCoinBudget && currentVisit.maxBudget > 0)
        {
            int totalCost = 0;
            foreach (FlowerData flower in bouquet)
            {
                if (flower != null)
                    totalCost += flower.cost;
            }

            if (totalCost > currentVisit.maxBudget)
            {
                goodBouquet = false; // Over budget → bad bouquet
            }
        }

        currentCharacter.satisfied = goodBouquet;

        // Show the appropriate dialogue
        dialogueManager.ShowResult(goodBouquet);

        HandleRevisitLogic();
    }

    bool CheckRequirements(FlowerData[] bouquet, List<FlowerData> requirements)
    {
        // Count bouquet flowers
        Dictionary<FlowerData, int> bouquetCounts = new Dictionary<FlowerData, int>();
        foreach (FlowerData f in bouquet)
        {
            if (f == null) continue;
            if (!bouquetCounts.ContainsKey(f)) bouquetCounts[f] = 0;
            bouquetCounts[f]++;
        }

        // Check each requirement (assume quantity = 1 for now, you can extend later)
        foreach (FlowerData req in requirements)
        {
            if (!bouquetCounts.ContainsKey(req) || bouquetCounts[req] < 1)
            {
                return false;
            }
        }

        return true;
    }

    void HandleRevisitLogic()
    {
        currentCharacter.visitNumber++;

        if (currentCharacter.visitNumber >= currentCharacter.visits.Count)
        {
            currentCharacter.willRevisit = false;
            return;
        }

        if (currentCharacter.visitNumber < currentCharacter.numberOfRequiredVisits)
        {
            currentCharacter.willRevisit = true;
        }
        else if (currentCharacter.visitNumber < currentCharacter.maxNumberOfVisits &&
                 !currentCharacter.satisfied)
        {
            currentCharacter.willRevisit = true;
        }
        else
        {
            currentCharacter.willRevisit = false;
        }
    }
}