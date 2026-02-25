// Ethan Le (2/24/2026):
using UnityEngine; 
using UnityEngine.UI; 

/**
 * Script to add a flower whenever player clicks a specific Flower button during bouquet construction. 
**/ 
public class FlowerButton : MonoBehaviour
{
    // Each Flower Button will have these variables:
    public FlowerData flower; // Assign the SPECIFIC Flower object (specific Data Asset) in the Inspector. 
    public BouquetManager bouquet; // Assign BouquetManager script so the button is hooked to the current bouquet the player is constructing. 
    private Image flowerImage; // Retrieve it from the specific Flower Data Asset.   

    void Start()
    {
        flowerImage = GetComponent<Image>(); // Get the Image component of the FlowerButton. 

        if (flower != null && flowerImage != null) // Safety check to ensure we have both the Flower Object (for data) and the Button's image component. 
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