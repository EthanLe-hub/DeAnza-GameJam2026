// Ethan Le (2/24/2026):
using UnityEngine; 
using UnityEngine.UI;

/** 
 * This script controls the UI of the Settings screen.
**/
public class SettingsScreenUI : MonoBehaviour 
{
    [Header("UI References:")]
    [SerializeField] private GameObject settingsPanel; // Assign GameObject component in Inspector. 
    [SerializeField] private Button closeButton; // Assign Button component in Inspector. 

    private void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePanel); // Attach listener to close panel when button is clicked. 
        }
    }

    private void HidePanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Turn off the Settings panel when Close button is clicked. 
        }
    }
}