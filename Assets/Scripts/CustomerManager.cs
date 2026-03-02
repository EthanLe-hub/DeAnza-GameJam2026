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
    public BouquetSubmissionManager bouquetSubmissionManager;

    private CharacterData currentCustomer;
    private CustomerUIController currentCustomerUI;

    void Start()
    {
        ResetAllNarrativeCustomers();
        SpawnCustomer();
    }

    /// <summary>
    /// Spawn either a narrative or normal customer randomly
    /// </summary>
    public void SpawnCustomer()
    {
        if (currentCustomerUI != null)
            Destroy(currentCustomerUI.gameObject);

        bool pickNarrative = true; //Random.value < 0.5f;

        if (pickNarrative)
            SpawnNarrativeCustomer();
        else
            SpawnNormalCustomer();
    }

    #region Narrative Customers

    /// <summary>
    /// Resets all narrative characters at the start of the game
    /// </summary>
    private void ResetAllNarrativeCustomers()
    {
        CharacterData[] narratives = {
            narrativeCharacter1, narrativeCharacter2,
            narrativeCharacter3, narrativeCharacter4,
            narrativeCharacter5
        };

        foreach (var c in narratives)
        {
            if (c != null)
            {
                c.visitNumber = 0;
                c.willRevisit = true;
            }
        }
    }

    /// <summary>
    /// Spawn a narrative customer that is still eligible to revisit
    /// </summary>
    private void SpawnNarrativeCustomer()
    {
        // Get all narrative characters that can still revisit
        List<CharacterData> availableNarratives = new List<CharacterData>();
        CharacterData[] narratives = { narrativeCharacter1, narrativeCharacter2, narrativeCharacter3, narrativeCharacter4, narrativeCharacter5 };
        foreach (var c in narratives)
            if (c != null && c.willRevisit)
                availableNarratives.Add(c);

        if (availableNarratives.Count == 0)
        {
            Debug.Log("No narrative characters available. Spawning normal customer instead.");
            SpawnNormalCustomer();
            return;
        }

        // Pick one at random
        int index = Random.Range(0, availableNarratives.Count);
        CharacterData template = availableNarratives[index];

        // Clone only for UI isolation, but preserve the original's visitNumber and willRevisit
        currentCustomer = Instantiate(template);

        // Important: store reference to the original SO so we can update visitNumber later
        bouquetSubmissionManager.originalCharacterReference = template;

        // Setup UI
        SetupCustomerUI();
    }

    #endregion

    #region Normal Customers

    private void SpawnNormalCustomer()
    {
        normalCustomerSpawner.SpawnRandomNormalCustomer();

        if (normalCustomerSpawner.genericNormalCustomer == null)
        {
            Debug.LogError("Failed to create normal customer!");
            return;
        }

        currentCustomer = Instantiate(normalCustomerSpawner.genericNormalCustomer);
        SetupCustomerUI(isNormal: true);
    }

    #endregion

    #region UI Setup

    private void SetupCustomerUI(bool isNormal = false)
    {
        customerOrderPanel.SetActive(true);
        bouquetConstructPanel.SetActive(false);

        // Instantiate UI prefab
        GameObject uiObj = Instantiate(customerUIPrefab, customerOrderPanel.transform);
        Transform cashierTable = customerOrderPanel.transform.Find("CashierTable");
        int insertIndex = cashierTable.GetSiblingIndex() + 1;
        uiObj.transform.SetSiblingIndex(insertIndex);

        currentCustomerUI = uiObj.GetComponent<CustomerUIController>();
        currentCustomerUI.SetCharacterSprite(currentCustomer.characterSprite);
        currentCustomerUI.dialogueText.text = "";

        // Clear button listeners
        currentCustomerUI.optionAButton.onClick.RemoveAllListeners();
        currentCustomerUI.optionBButton.onClick.RemoveAllListeners();
        currentCustomerUI.optionCButton.onClick.RemoveAllListeners();

        // Normal customer special handling
        if (isNormal)
        {
            currentCustomerUI.optionAButton.gameObject.SetActive(false);
            currentCustomerUI.optionBButton.gameObject.SetActive(false);

            currentCustomerUI.optionCButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Bouquet";
            currentCustomerUI.optionCButton.onClick.AddListener(() => ShowBouquetPanel());
        }

        // Assign DialogueManager references
        dialogueManager.currentCharacter = currentCustomer;
        bouquetSubmissionManager.currentCharacter = currentCustomer;
        dialogueManager.dialogueText = currentCustomerUI.dialogueText;
        dialogueManager.optionAButton = currentCustomerUI.optionAButton;
        dialogueManager.optionBButton = currentCustomerUI.optionBButton;
        dialogueManager.optionCButton = currentCustomerUI.optionCButton;

        dialogueManager.OnDialogueComplete = ShowBouquetPanel;
        dialogueManager.OnResultComplete = SpawnCustomer;

        dialogueManager.StartVisit();
    }

    #endregion

    private void ShowBouquetPanel()
    {
        customerOrderPanel.SetActive(false);
        bouquetConstructPanel.SetActive(true);
        bouquetManager.ClearBouquet();
    }
}