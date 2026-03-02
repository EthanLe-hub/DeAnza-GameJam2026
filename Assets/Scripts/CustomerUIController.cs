using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomerUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image characterImage; // Root Image component. 
    public TextMeshProUGUI dialogueText;
    public Button optionAButton;
    public Button optionBButton;
    public Button optionCButton;

    public void SetCharacterSprite(Sprite sprite) // To assign the customer sprite at runtime. 
    {
        if (characterImage != null)
        {
            characterImage.sprite = sprite;
            characterImage.enabled = true; // make sure it’s visible
        }
        else
        {
            Debug.LogError("Character Image is null on CustomerUIController!");
        }
    }
}