using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputManager : MonoBehaviour
{   
    public GameObject text;
    GameObject deckManager;
    int inputCooldown;
    int baseCooldown;
    string actionText;
    string cardText;
    bool selectingCard;
    bool recycling;

    void Start() {   
        actionText = "Press 1 to draw the top card from the discard pile (if possible). \nPress 2 to draw the top card from the draw pile.";
        cardText = "Select which card you will discard using the number keys 1-7.";
        deckManager = GameObject.Find("DeckManager");
        baseCooldown = 90;
        inputCooldown = baseCooldown;
        text.GetComponent<TextMeshProUGUI>().text = actionText;
        selectingCard = false;
    }

    void SwitchInputState(bool drewDiscard) {
        if (selectingCard) {
            text.GetComponent<TextMeshProUGUI>().text = actionText;
        } else {
            text.GetComponent<TextMeshProUGUI>().text = cardText;
        }
        recycling = drewDiscard;
        selectingCard = !selectingCard;
        inputCooldown = baseCooldown;
    }

    void Update() {   
        --inputCooldown;
        if (inputCooldown <= 0) {
            if (selectingCard) {
                string message;
                if (recycling) {message = "GetNewCardDiscard";} 
                else {message = "GetNewCardDraw";}
    
                if (Input.GetKeyDown(KeyCode.Alpha1)) {deckManager.SendMessage(message, 1);}
                else if (Input.GetKeyDown(KeyCode.Alpha2)) {deckManager.SendMessage(message, 2);}
                else if (Input.GetKeyDown(KeyCode.Alpha3)) {deckManager.SendMessage(message, 3);}
                else if (Input.GetKeyDown(KeyCode.Alpha4)) {deckManager.SendMessage(message, 4);}
                else if (Input.GetKeyDown(KeyCode.Alpha5)) {deckManager.SendMessage(message, 5);}
                else if (Input.GetKeyDown(KeyCode.Alpha6)) {deckManager.SendMessage(message, 6);}
                else if (Input.GetKeyDown(KeyCode.Alpha7)) {deckManager.SendMessage(message, 7);}
            } else {
                if (Input.GetKeyDown(KeyCode.Alpha1)) {deckManager.SendMessage("DrawFromDiscard");}
                else if (Input.GetKeyDown(KeyCode.Alpha2)) {deckManager.SendMessage("DrawFromDraw");}
            }
        }
    }
}
