// Ethan Le (2/24/2026):
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 

/**
 * Script to add a flower whenever player clicks a specific Flower button during bouquet construction. 
**/ 
public class FlowerButton : MonoBehaviour
{
    // Each Flower Button will have these variables:
    public FlowerData flower; // Assign the SPECIFIC Flower object (specific Data Asset) in the Inspector. 
    public BouquetManager bouquet; // Assign BouquetManager script so the button is hooked to the current bouquet the player is constructing. 
    private Image flowerImage; // Retrieve it from the specific Flower Data Asset.   
    private TextMeshProUGUI flowerText; // Retrieve it from the specific Flower Data Asset. 
    private TextMeshProUGUI costText; // Retrieve it from the specific Flower Data Asset. 

    void Start()
    {
        flowerText = transform.Find("NameText").GetComponent<TextMeshProUGUI>(); // Get the name TextMeshProUGUI component of the FlowerButton. 
        costText = transform.Find("CostText").GetComponent<TextMeshProUGUI>(); // Get the cost TextMeshProUGUI component of the FlowerButton. 
        flowerImage = GetComponent<Image>(); // Get the Image component of the FlowerButton. 

        /* Safety checks to ensure we have the Flower Object (for data), the Button's name text component, the Button's cost text component, and the Button's image component. */ 
        if (flower != null && flowerText != null)
        {
            flowerText.text = flower.flowerName; // Get the flower name from the specific Flower Data Asset and assign to Button text. 
        }

        if (flower != null && costText != null)
        {
            costText.text = flower.cost.ToString() + " coins"; // Get the flower cost from the specific Flower Data Asset and assign to Button text. 
        }

        if (flower != null && flowerImage != null) 
        {
            flowerImage.sprite = flower.sprite; // Get the sprite from the specific Flower Data Asset and assign to Button image. 
        } 
    }

    /** 
     * Each Flower Button is assigned this script in its individual inspector. 
     * Then assign the SPECIFIC Flower Data Asset and BouquetManager GameObject holding BouquetManager.cs script.
     * Add an "On Click ()" event in the Button's inspector; drag the "Flower Button (Script)" from the same inspector into it.
     * Function -> "FlowerButton" -> "OnClickFlower ()". 
    **/ 
    public void OnClickFlower()
    {
        bouquet.AddFlower(flower); // Add this specific type of flower to the current bouquet. 
    }
}