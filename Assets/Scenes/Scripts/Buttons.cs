using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{   
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); 
    }

    public void RulesButton()
    {
        SceneManager.LoadScene("RulesMenu", LoadSceneMode.Single); 
    }

    public void SettingsButton()
    {
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Single); 
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Rummy", LoadSceneMode.Single); 
    }

    public void SetRedCardback()
    {
        UpdateCardback("cardback_red");
    }
    
    public void SetGreenCardback()
    {
        UpdateCardback("cardback_green");
    }
    
    public void SetBlueCardback()
    {
        UpdateCardback("cardback_blue");
    }

    public void UpdateCardback(string cardback)
    {
        GameObject settings = GameObject.FindGameObjectsWithTag("Settings")[0];
        settings.SendMessage("SetCardback", cardback);
    }
}
