using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject customerOrderPanel;   // Panel with dialogue
    public GameObject bouquetConstructPanel; // Panel with BouquetManager + buttons

    [Header("Customer UI Elements")]
    public TextMeshProUGUI customerNameText;
    public TextMeshProUGUI dialogueText;
    public Button optionAButton;
    public Button optionBButton;
    public Button optionCButton;

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
    private bool isNarrativeCustomer = false;

    void Start()
    {
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        // Decide randomly: 50% normal vs. narrative (adjust as you like)
        bool pickNarrative = Random.value > 0.5f;

        if (pickNarrative)
        {
            isNarrativeCustomer = true;

            // Randomly pick one of the 5 narrative characters
            int index = Random.Range(0, 5);
            switch (index)
            {
                case 0: currentCustomer = narrativeCharacter1; break;
                case 1: currentCustomer = narrativeCharacter2; break;
                case 2: currentCustomer = narrativeCharacter3; break;
                case 3: currentCustomer = narrativeCharacter4; break;
                case 4: currentCustomer = narrativeCharacter5; break;
            }

            SetupNarrativeCustomer();
        }
        else
        {
            isNarrativeCustomer = false;
            normalCustomerSpawner.SpawnRandomNormalCustomer();
            currentCustomer = normalCustomerSpawner.genericNormalCustomer;

            SetupNormalCustomer();
        }
    }

    #region Narrative Customer Setup
    void SetupNarrativeCustomer()
    {
        customerOrderPanel.SetActive(true);
        bouquetConstructPanel.SetActive(false);

        customerNameText.text = currentCustomer.characterName;

        // Enable all 3 options
        optionAButton.gameObject.SetActive(true);
        optionBButton.gameObject.SetActive(true);
        optionCButton.gameObject.SetActive(true);

        // Remove old listeners
        optionAButton.onClick.RemoveAllListeners();
        optionBButton.onClick.RemoveAllListeners();
        optionCButton.onClick.RemoveAllListeners();

        // Assign listeners
        optionAButton.onClick.AddListener(() => dialogueManager.ChooseOption(0));
        optionBButton.onClick.AddListener(() => dialogueManager.ChooseOption(1));
        optionCButton.onClick.AddListener(() => {
            // Option C leads to bouquet construction
            ShowBouquetPanel();
        });

        dialogueManager.currentCharacter = currentCustomer;
        dialogueManager.StartVisit();
    }
    #endregion

    #region Normal Customer Setup
    void SetupNormalCustomer()
    {
        customerOrderPanel.SetActive(true);
        bouquetConstructPanel.SetActive(false);

        customerNameText.text = currentCustomer.characterName;

        // Only one option: use Option C button
        optionAButton.gameObject.SetActive(false);
        optionBButton.gameObject.SetActive(false);
        optionCButton.gameObject.SetActive(true);

        optionCButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Bouquet"; // change label

        // Remove old listeners
        optionCButton.onClick.RemoveAllListeners();

        // Clicking takes you to bouquet panel
        optionCButton.onClick.AddListener(() => {
            ShowBouquetPanel();
        });
    }
    #endregion

    void ShowBouquetPanel()
    {
        customerOrderPanel.SetActive(false);
        bouquetConstructPanel.SetActive(true);

        // Reset bouquet manager for new bouquet
        bouquetManager.ClearBouquet();
    }
}