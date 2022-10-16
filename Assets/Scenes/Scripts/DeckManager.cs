using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{   
    public GameObject card;

    Vector3 drawPilePos;
    Vector3 discardPilePos;
    Vector3 playerDeckPos;
    int numSuits;
    int numDecks;
    int deckSize;
    int handSize;
    Deck drawPile;
    Deck discardPile;
    Deck playerDeck;
    Card queuedCard;
    GameObject discardPileCard;
    GameObject drawPileCard;
    GameObject[] playerCards;
    GameObject settings;
    string cardback;

    public class Card {
        public int suit;
        public int value;

        public Card(int s, int v) {
            suit = s;
            value = v;
        }
    }

    public class Deck {
        public int id;
        public Card[] cards;

        public Deck(int i, int s) {
            cards = new Card[s + 1];
            id = i;
        }
    }

    // Start is called before the first frame update
    void Start()
    {   
        drawPilePos = new Vector3(-2f,0f,0);
        discardPilePos = new Vector3(2f,0f,0);
        playerDeckPos = new Vector3(-3f,-4f,0);
        numSuits = 4;
        numDecks = 0;
        deckSize = 52;
        handSize = 7;
        settings = GameObject.Find("Settings");
        settings.SendMessage("GetCardback");
        drawPile = GenerateDeck();
        discardPile = GenerateEmptyDeck();
        discardPileCard = Instantiate(card, discardPilePos, Quaternion.identity);
        drawPileCard = Instantiate(card, drawPilePos, Quaternion.identity);
        drawPileCard.tag = "FaceDown";
        SetCardSprite(drawPileCard, cardback);
        ShuffleDeck(drawPile);
        playerDeck = new Deck(numDecks, handSize);
        playerCards = new GameObject[handSize];
        for (int i = 0; i <= 6; i++) {
            TransferCard(drawPile, playerDeck);
            playerCards[i] = Instantiate(card, playerDeckPos, Quaternion.identity);
            playerCards[i].SendMessage("SetTargetPos", playerDeckPos + new Vector3(i,0,0));
        }
    }

    void UpdateCardback(string cb)
    {
        cardback = cb;
    }

    // Update is called once per frame
    void Update()
    {   
        //PrintCard(drawPile.cards[0]);
        SetCardSprite(discardPileCard, discardPile.cards[0]);
        SetCardSprite(drawPileCard, drawPile.cards[0]);
        for (int i = 0; i <= 6; i++){
            SetCardSprite(playerCards[i], playerDeck.cards[i]);
        }
    }

    public Deck GenerateDeck() {
        Deck newdeck = new Deck(numDecks, deckSize);
        int i = 0;
        for (int j = 1; j <= 13; j++) {
            for (int k = 1; k <= numSuits; k++) {
                newdeck.cards[i] = new Card(k, j);
                i++;
            }
        }
        ++numDecks;
        return newdeck;
    }

    public Deck GenerateEmptyDeck() {
        Deck newdeck = new Deck(numDecks, deckSize);
        ++numDecks;
        return newdeck;
    }

    void PrintCard(Card card) {
        Debug.Log(card.suit+" "+card.value);
    }

    void PrintDeck(Deck deck) {
        for (int i = 0; i < deck.cards.Length; i++) {
            PrintCard(deck.cards[i]);
        }
    }

    void ShuffleDeck(Deck deck) {
        for (int i = 0; i <= deck.cards.Length * 2; i++) {
            int idx1 = Random.Range(0, deck.cards.Length);
            int idx2 = Random.Range(0, deck.cards.Length);
            Card tempcard = deck.cards[idx1];
            deck.cards[idx1] = deck.cards[idx2];
            deck.cards[idx2] = tempcard;
        }
    }

    void ExchangeCard(Deck deck1, Deck deck2, int idx1, int idx2) {
        Card tempcard = deck1.cards[idx1];
        deck1.cards[idx1] = deck2.cards[idx2];
        deck2.cards[idx2] = tempcard;
    }

    int FindSlot(Deck deck) {
        for (int i = 0; i < deck.cards.Length; i++) {
            if (deck.cards[i] == null){return i;}
        }
        return deck.cards.Length;
    }

    void TransferCard(Deck olddeck, Deck newdeck) {
        int newIndex = FindSlot(newdeck); //Finds an open spot in the new deck
        newdeck.cards[newIndex] = olddeck.cards[0];
        RemoveCard(olddeck, 0); //Removes first card from old deck
    }

    void RemoveCard(Deck deck, int index) {RemoveCard(deck, index, false);}

    void RemoveCard(Deck deck, int index, bool addToDiscard) {
        if (addToDiscard) {
            for (int i = 1; i < discardPile.cards.Length; i++){
                discardPile.cards[i] = discardPile.cards[i - 1];
            }
            discardPile.cards[0] = deck.cards[index];
        }
        deck.cards[index] = null;
        for (int i = 0; i < deck.cards.Length - 1; i++) {
            if (deck.cards[i] == null) {
                deck.cards[i] = deck.cards[i + 1];
                deck.cards[i + 1] = null;
            }
        }
    }

    void Progress() {Progress(false);}

    void Progress(bool drewDiscard) {
        GameObject.Find("InputManager").SendMessage("SwitchInputState", drewDiscard);
    }

    void DrawFromDiscard() {
        if (discardPile.cards[0] != null) {
           queuedCard = discardPile.cards[0];
           Progress(true);
        }
    }

    void DrawFromDraw() {
        queuedCard = drawPile.cards[0];
        Progress();
    }

    void GetNewCardDiscard(int cardSlot) {
        TransferCard(discardPile, playerDeck);
        RemoveCard(playerDeck, cardSlot-1, true);
        Progress();
    }

    void GetNewCardDraw(int cardSlot) {
        TransferCard(drawPile, playerDeck);
        RemoveCard(playerDeck, cardSlot-1, true);
        if (drawPile.cards[0] == null){ //Regenerate draw pile from discard pile
            drawPile.cards = discardPile.cards;
            ShuffleDeck(drawPile);
            discardPile = GenerateEmptyDeck();
        }
        Progress();
    }

    void SetCardSprite(GameObject card, string str) {
        if (card.CompareTag("FaceDown")) {
            card.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(str);
        } else {
            card.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(cardback);
        }
    }

    void SetCardSprite(GameObject card, Card cardd) {
        SetCardSprite(card, FilenameFromCard(cardd));
    }

    string FilenameFromCard(Card card) { 
        return FilenameFromCard(card, false);
    }

    string FilenameFromCard(Card card, bool debugPrint) { //Probably a better way to do this :|
        string str = "";

        if (card != null) {
            int num = card.value;
            if (num == 1) {str = "ace";}
            else if (num == 2) {str = "2";}
            else if (num == 3) {str = "3";}
            else if (num == 4) {str = "4";}
            else if (num == 5) {str = "5";}
            else if (num == 6) {str = "6";}
            else if (num == 7) {str = "7";}
            else if (num == 8) {str = "8";}
            else if (num == 9) {str = "9";}
            else if (num == 10) {str = "10";}
            else if (num == 11) {str = "jack";}
            else if (num == 12) {str = "queen";}
            else if (num == 13) {str = "king";}

            int suit = card.suit;
            if (suit == 1) {str += "_of_spades";}
            else if (suit == 2) {str += "_of_clubs";}
            else if (suit == 3) {str += "_of_hearts";}
            else if (suit == 4) {str += "_of_diamonds";}
        }
    
        if (debugPrint) {Debug.Log(str);}
        return str;
    }
}
