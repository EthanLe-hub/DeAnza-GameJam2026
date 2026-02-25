// Ethan Le (2/24/2026): 
using UnityEngine; 
using UnityEngine.UI; 

/** 
 * This script controls the buttons of the game's main screen. 
**/ 
public class MainScreenUI : MonoBehaviour
{
    [Header("Main Screen Buttons:")]
    [SerializeField] private Button play; // Attach Button component via Inspector. 
    [SerializeField] private Button settings; // Attach Button component via Inspector. 
    [SerializeField] private Button credits; // Attach Button component via Inspector. 

    [Header("Main Screen GameObject panels:")]
    [SerializeField] private GameObject mainMenuPanel; // Attach GameObject component via Inspector. 
    [SerializeField] private GameObject playPanel; // Attach GameObject component via Inspector. 
    [SerializeField] private GameObject settingsPanel; // Attach GameObject component via Inspector. 
    [SerializeField] private GameObject creditsPanel; // Attach GameObject component via Inspector. 

    void Start()
    {
        SetupOnClickListeners(); // Call function to set up each Button's listeners. 
    }

    /** 
     * Function attaches listener to the main screen's buttons. 
    **/ 
    private void SetupOnClickListeners()
    {
        if (play != null)
        {
            play.onClick.AddListener(showPlayPanel); 
        }
        else
        {
            Debug.Log("Play button is null!"); 
        }

        if (settings != null)
        {
            settings.onClick.AddListener(showSettingsPanel); 
        }
        else 
        {
            Debug.Log("Settings button is null!"); 
        }

        if (credits != null)
        {
            credits.onClick.AddListener(showCreditsPanel); 
        }
        else
        {
            Debug.Log("Credits button is null!"); 
        }
    }

    /** 
     * Function to open the Play panel.
    **/ 
    public void showPlayPanel()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false); 
        }
        if (playPanel != null) 
        {
            playPanel.SetActive(true); 
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); 
        }
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false); 
        }
    }

    /** 
     * Function to open the Settings panel.
    **/ 
    public void showSettingsPanel()
    {
        if (playPanel != null) 
        {
            playPanel.SetActive(false); 
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); 
        }
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false); 
        }
    }

    /** 
     * Function to open the Settings panel.
    **/ 
    public void showCreditsPanel()
    {
        if (playPanel != null) 
        {
            playPanel.SetActive(false); 
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); 
        }
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true); 
        }
    }
}