// Ethan Le (2/28/2026): 
using System.Collections.Generic;
using UnityEngine;

/**
 * Dynamically spawns a normal customer and assigns their dialogue + requirements.
 * Works with your existing CharacterData, BouquetManager, and DialogueManager.
**/
public class NormalCustomerSpawner : MonoBehaviour
{
    public CharacterData genericNormalCustomer; // Drag in your generic CharacterData asset
    public BouquetManager bouquetManager;
    public DialogueManager dialogueManager;

    [System.Serializable]
    public class NormalCustomerData
    {
        public string customerName;
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

    void Start()
    {
        SpawnRandomNormalCustomer();
    }

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

        // Assign the generic CharacterData for this spawn
        genericNormalCustomer.characterName = selected.customerName;
        genericNormalCustomer.maxNumberOfVisits = 1; // Normal customers only visit once
        genericNormalCustomer.numberOfRequiredVisits = 1;
        genericNormalCustomer.visitNumber = 0;
        genericNormalCustomer.satisfied = false;
        genericNormalCustomer.willRevisit = false;

        // Create a single visit dynamically
        CharacterData.VisitDialogue visit = new CharacterData.VisitDialogue();
        visit.intro = selected.dialogues; // assign all lines as intro
        visit.optionA = new List<string>(); // leave empty if no options
        visit.optionB = new List<string>();
        visit.optionC = new List<string>();
        visit.goodResult = new List<string>() { "Thank you! This is perfect." };
        visit.badResult = new List<string>() { "Hmm… this isn’t quite right." };

        // Assign hidden requirements with quantities
        visit.hiddenRequirements = new List<FlowerData>();
        foreach (var req in selected.hiddenRequirements)
        {
            for (int i = 0; i < req.quantity; i++)
            {
                visit.hiddenRequirements.Add(req.flower);
            }
        }

        // Overwrite visits
        genericNormalCustomer.visits = new List<CharacterData.VisitDialogue>() { visit };

        // Start dialogue
        dialogueManager.currentCharacter = genericNormalCustomer;
        dialogueManager.StartVisit();
    }

    /*** Example Setup ***/
    // You can define normal customers in the inspector:
    // normalCustomers[0].customerName = "Customer A"
    // normalCustomers[0].dialogues = [ "Just a Red Rose, please.", "Thanks!" ]
    // normalCustomers[0].hiddenRequirements = [ FlowerRequirement { flower = RedRose, quantity=1 } ]
}