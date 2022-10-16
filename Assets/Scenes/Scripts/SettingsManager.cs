using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{   
    string cardback;

    void Awake()
    {   
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Settings");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject); //Settings need to persist between scenes
    }

    void Start()
    {   
        SetCardback("cardback_red");
    }

    void SetCardback(string newcardback)
    {
        cardback = newcardback;
    }

    void GetCardback()
    {
        GameObject.Find("DeckManager").SendMessage("UpdateCardback", cardback);
    }
}
