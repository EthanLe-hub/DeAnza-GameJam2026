using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject customerOrderPanel;   // Parent panel for instantiated customer UI
    public GameObject bouquetConstructPanel; // Panel with BouquetManager + buttons

    [Header("Prefab")]
    public GameObject customerUIPrefab;     // Assign your CustomerUI prefab here

    [Header("References")]
    public CharacterData narrativeCharacter1;
    public CharacterData narrativeCharacter2;
    public CharacterData narrativeCharacter3;
    public CharacterData narrativeCharacter4;
    public CharacterData narrativeCharacter5;
    public NormalCustomerSpawner normalCustomerSpawner;
    public DialogueManager dialogueManager;
    public BouquetManager bouquetManager;

    private CharacterData currentCustomer;
    private CustomerUIController currentCustomerUI;
    //private bool isNarrativeCustomer = false;

    void Start()
    {
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        // For testing: true = narrative, false = normal
        bool pickNarrative = false; // <- Change this for testing  
        // Destroy old UI if it exists
        if (currentCustomerUI != null)
            Destroy(currentCustomerUI.gameObject);
        if (pickNarrative)
        {
            Debug.Log("Spawning Narrative character!");
            
            int index = Random.Range(0, 5);
            switch (index)
            {
                case 0: currentCustomer = narrativeCharacter1; break;
                case 1: currentCustomer = narrativeCharacter2; break;
                case 2: currentCustomer = narrativeCharacter3; break;
                case 3: currentCustomer = narrativeCharacter4; break;
                case 4: currentCustomer = narrativeCharacter5; break;
            }
            // Make sure narrative character has valid data
            if (currentCustomer == null)
            {
                Debug.LogError($"Narrative character at index {index} is null!");
                return;
            }
            SetupNarrativeCustomer();
        }
        else
        {
            Debug.Log("Spawning Normal character!");
            
            // First spawn the random normal customer data
            normalCustomerSpawner.SpawnRandomNormalCustomer();
            
            // Then use that data
            currentCustomer = normalCustomerSpawner.genericNormalCustomer;
            
            if (currentCustomer == null)
            {
                Debug.LogError("Failed to create normal customer!");
                return;
            }
            
            SetupNormalCustomer();
        }
    }

    #region Narrative Customer Setup
    void SetupNarrativeCustomer()
    {
        customerOrderPanel.SetActive(true);
        bouquetConstructPanel.SetActive(false);

        // Instantiate prefab dynamically
        GameObject uiObj = Instantiate(customerUIPrefab, customerOrderPanel.transform);
        currentCustomerUI = uiObj.GetComponent<CustomerUIController>();

        currentCustomerUI.dialogueText.text = "";

        // Assign DialogueManager references
        dialogueManager.currentCharacter = currentCustomer;
        dialogueManager.dialogueText = currentCustomerUI.dialogueText;
        dialogueManager.optionAButton = currentCustomerUI.optionAButton;
        dialogueManager.optionBButton = currentCustomerUI.optionBButton;
        dialogueManager.optionCButton = currentCustomerUI.optionCButton;

        if (dialogueManager.dialogueText == null)
            Debug.LogError("Dialogue Text is null! Did you assign it in CustomerUIController?");

        dialogueManager.OnDialogueComplete = ShowBouquetPanel;
        dialogueManager.StartVisit();
    }
    #endregion

    #region Normal Customer Setup
    void SetupNormalCustomer()
    {
        customerOrderPanel.SetActive(true);
        bouquetConstructPanel.SetActive(false);

        // Instantiate prefab dynamically
        GameObject uiObj = Instantiate(customerUIPrefab, customerOrderPanel.transform);
        currentCustomerUI = uiObj.GetComponent<CustomerUIController>();

        currentCustomerUI.dialogueText.text = "";

        currentCustomerUI.optionAButton.gameObject.SetActive(false);
        currentCustomerUI.optionBButton.gameObject.SetActive(false);

        currentCustomerUI.optionCButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Bouquet";
        currentCustomerUI.optionCButton.onClick.RemoveAllListeners();
        currentCustomerUI.optionCButton.onClick.AddListener(() => ShowBouquetPanel());

        dialogueManager.currentCharacter = currentCustomer;
        dialogueManager.dialogueText = currentCustomerUI.dialogueText;
        dialogueManager.optionAButton = currentCustomerUI.optionAButton;
        dialogueManager.optionBButton = currentCustomerUI.optionBButton;
        dialogueManager.optionCButton = currentCustomerUI.optionCButton;

        if (dialogueManager.dialogueText == null)
            Debug.LogError("Dialogue Text is null! Did you assign it in CustomerUIController?");
    
        dialogueManager.OnDialogueComplete = ShowBouquetPanel;
        dialogueManager.StartVisit();
    }
    #endregion

    void ShowBouquetPanel()
    {
        customerOrderPanel.SetActive(false);
        bouquetConstructPanel.SetActive(true);

        bouquetManager.ClearBouquet();
    }
}