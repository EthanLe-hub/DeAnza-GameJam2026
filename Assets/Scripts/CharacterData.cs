// Ethan Le (2/27/2026):
using System.Collections.Generic; 
using UnityEngine; 

/** 
 * Script for creating different instances of characters/customers (both narrative and normal ones).
**/
[CreateAssetMenu(fileName = "New Character", menuName = "Flower Shop/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    public int maxNumberOfVisits;
    public int numberOfRequiredVisits;

    [HideInInspector] public int visitNumber = 0;
    [HideInInspector] public bool satisfied = false;
    [HideInInspector] public bool willRevisit = true;

    [System.Serializable]
    public class VisitDialogue
    {
        public List<string> intro;

        public List<string> optionA;
        public List<string> optionB;
        public List<string> optionC;

        public List<string> goodResult;
        public List<string> badResult;

        // Per-visit hidden requirements
        public List<FlowerData> hiddenRequirements;
        public int maxBudget; // optional budget
    }

    // Index 0 = Visit 1, 1 = Visit 2, 2 = Visit 3: 
    public List<VisitDialogue> visits;
}