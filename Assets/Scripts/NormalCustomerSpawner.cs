// Ethan Le (2/28/2026): 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Dynamically spawns a normal customer and assigns their dialogue + requirements.
 * Works with your existing CharacterData, BouquetManager, and DialogueManager.
**/
public class NormalCustomerSpawner : MonoBehaviour
{
    public CharacterData genericNormalCustomer; // Drag in your generic CharacterData asset
    public BouquetManager bouquetManager;
    public DialogueManager dialogueManager;

    public GameObject currentCustomerUI; // The UI GameObject displaying the customer.
    public Image customerImage;          // The Image component in the UI for the sprite.
    
    [System.Serializable]
    public class NormalCustomerData
    {
        public string customerName;
        public Sprite customerSprite; 
        public List<string> dialogues; // The "normal" dialogue lines
        public List<FlowerRequirement> hiddenRequirements; // What flowers they want
    }

    [System.Serializable]
    public class FlowerRequirement
    {
        public FlowerData flower; // The FlowerData asset
        public int quantity;      // How many of this flower
    }

    [Header("Normal Customer Examples")]
    public List<NormalCustomerData> normalCustomers = new List<NormalCustomerData>();

/*
    void Start()
    {
        SpawnRandomNormalCustomer();
    }
*/

    public void SpawnRandomNormalCustomer()
    {
        if (normalCustomers.Count == 0)
        {
            Debug.LogWarning("No normal customers defined!");
            return;
        }

        // Pick a random normal customer
        int index = Random.Range(0, normalCustomers.Count);
        NormalCustomerData selected = normalCustomers[index];

        // Assign generic CharacterData for dialogue + bouquet
        genericNormalCustomer.characterName = selected.customerName;
        genericNormalCustomer.characterSprite = selected.customerSprite; // <-- ADD THIS
        genericNormalCustomer.maxNumberOfVisits = 1;
        genericNormalCustomer.numberOfRequiredVisits = 1;
        genericNormalCustomer.visitNumber = 0;
        genericNormalCustomer.satisfied = false;
        genericNormalCustomer.willRevisit = false;

        // Create a single visit dynamically
        CharacterData.VisitDialogue visit = new CharacterData.VisitDialogue();
        visit.intro = selected.dialogues;
        visit.optionA = new List<string>();
        visit.optionB = new List<string>();
        visit.optionC = new List<string>();
        visit.goodResult = new List<string>() { "Thank you! This is perfect." };
        visit.badResult = new List<string>() { "Hmm… this isn't quite right." };

        // Assign hidden requirements
        visit.hiddenRequirements = new List<FlowerData>();
        foreach (var req in selected.hiddenRequirements)
        {
            for (int i = 0; i < req.quantity; i++)
            {
                visit.hiddenRequirements.Add(req.flower);
            }
        }

        genericNormalCustomer.visits = new List<CharacterData.VisitDialogue>() { visit };
    }
}