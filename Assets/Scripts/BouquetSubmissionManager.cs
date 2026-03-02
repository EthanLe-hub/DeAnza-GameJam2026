using System.Collections.Generic;
using UnityEngine;

public class BouquetSubmissionManager : MonoBehaviour
{
    public BouquetManager bouquetManager;
    public CharacterData currentCharacter;
    public DialogueManager dialogueManager;

    public GameObject customerOrderPanel;
    public GameObject bouquetConstructPanel;
    public CustomerManager customerManager;

    [Header("Optional: Budget Check")]
    public bool checkCoinBudget = false;

    [HideInInspector] public bool goodBouquet;

    public void SubmitBouquet()
    {
        FlowerData[] bouquet = bouquetManager.GetBouquet();

        if (currentCharacter.visitNumber >= currentCharacter.visits.Count)
        {
            Debug.LogWarning("No more visits defined for this character.");
            return;
        }

        var currentVisit = currentCharacter.visits[currentCharacter.visitNumber];

        // Check requirements
        goodBouquet = (currentVisit.hiddenRequirements == null || currentVisit.hiddenRequirements.Count == 0)
            ? true
            : CheckRequirements(bouquet, currentVisit.hiddenRequirements);

        // Optional: budget
        if (checkCoinBudget && currentVisit.maxBudget > 0)
        {
            int totalCost = 0;
            foreach (FlowerData flower in bouquet)
                if (flower != null) totalCost += flower.cost;

            if (totalCost > currentVisit.maxBudget) goodBouquet = false;
        }

        currentCharacter.satisfied = goodBouquet;

        bouquetConstructPanel.SetActive(false);
        customerOrderPanel.SetActive(true);

        dialogueManager.ShowResult(goodBouquet);

        HandleRevisitLogic();
    }

    private bool CheckRequirements(FlowerData[] bouquet, List<FlowerData> requirements)
    {
        Dictionary<FlowerData, int> bouquetCounts = new Dictionary<FlowerData, int>();
        foreach (FlowerData f in bouquet)
        {
            if (f == null) continue;
            if (!bouquetCounts.ContainsKey(f)) bouquetCounts[f] = 0;
            bouquetCounts[f]++;
        }

        foreach (FlowerData req in requirements)
        {
            if (!bouquetCounts.ContainsKey(req) || bouquetCounts[req] < 1) return false;
        }

        return true;
    }

    private void HandleRevisitLogic()
    {
        // ALWAYS increment visit
        currentCharacter.visitNumber++;

        // Will revisit until maxNumberOfVisits is reached
        currentCharacter.willRevisit = currentCharacter.visitNumber < currentCharacter.maxNumberOfVisits;
    }
}