using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using CardIdentity;

public class Players : MonoBehaviour
{
    public int ID; //4 table 0 red 1 yellow 2 blue 3 green
    
    public GameObject[] cards;
    public cardIdentity[] cardIdentities = {new cardIdentity(), new cardIdentity(), new cardIdentity(), new cardIdentity(), new cardIdentity()};
    private Button[] cardBtn;
    private bool[] isGrey = {true, true, true, true, true};

    public GameObject propose;
    private Button proposeBtn;

    public GameObject swap;
    private Button swapBtn;

    public float playerPts;
    public GameObject playerPtsTxt;
    public bool cardDealed = false;
    public bool canGoNext = false;

    private int[] conesPts = {5, 4, 3, 4, 3, 2, 3};
    private int[] icecreamPts = {6, 4, 4, 3, 4, 4, 3, 5, 3, 5, 5, 3, 3};
    private int[] toppingsPts = {3, 2, 3, 1, 1, 2, 7, 2, 1};

    private GameManager gameManager;

    public int numOfCones = 0;
    public int numOfIcecreams = 0;
    public int[] chosed = {10, 10, 10};
    public bool needSwap = false;
    public bool end = false;
    public bool hasProposed = false;
    public List<GameObject> occupiedBlocks = new List<GameObject>();
    public Sprite rabbit;
    // Start is called before the first frame update
    void Start()
    {
        proposeBtn = propose.GetComponent<Button>();
        proposeBtn.onClick.AddListener(proposeCards);

        swapBtn = swap.GetComponent<Button>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        cards[0].GetComponent<Button>().onClick.AddListener(delegate{ chooseCard(0); });
        cards[1].GetComponent<Button>().onClick.AddListener(delegate{ chooseCard(1); });
        cards[2].GetComponent<Button>().onClick.AddListener(delegate{ chooseCard(2); });
        cards[3].GetComponent<Button>().onClick.AddListener(delegate{ chooseCard(3); });
        cards[4].GetComponent<Button>().onClick.AddListener(delegate{ chooseCard(4); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int countPickedCards(){
        int count = 0;
        for (int i = 0; i < isGrey.Length; i++){
            if (!isGrey[i]){
                count++;
            }
        }
        return count;
    }
    
    void proposeCards(){
        playerPts = 0;
        for (int i = 0; i < isGrey.Length; i++){
            if (!isGrey[i]){
                cardIdentity currentCard = cardIdentities[i];
                switch(currentCard.type){
                    case 0:
                        playerPts += conesPts[currentCard.ID]*cardIdentities[i].getMultiplier(gameManager.currentWeather);
                        numOfCones--;
                        break;
                    case 1:
                        playerPts += icecreamPts[currentCard.ID]*cardIdentities[i].getMultiplier(gameManager.currentWeather);
                        numOfIcecreams--;
                        break;
                    case 2:
                        playerPts += toppingsPts[currentCard.ID]*cardIdentities[i].getMultiplier(gameManager.currentWeather);
                        break;
                }
            }
        }
        playerPtsTxt.GetComponent<Text>().text = "Points: " + playerPts;
        canGoNext = true;
        end = true;
        hasProposed = true;
    }

    void swapCards(int idx1, int idx2, int idx3){
        cardIdentity identity1;
        Sprite sprite1;
        gameManager.getCard(out sprite1, out identity1);

        cardIdentities[idx1] = identity1;

        cards[idx1].GetComponents<Image>()[0].sprite = sprite1;

        cardIdentity identity2;
        Sprite sprite2;
        gameManager.getCard(out sprite2, out identity2);

        cardIdentities[idx2] = identity2;

        cards[idx2].GetComponents<Image>()[0].sprite = sprite2;

        cardIdentity identity3;
        Sprite sprite3;
        gameManager.getCard(out sprite3, out identity3);

        cardIdentities[idx3] = identity3;

        cards[idx3].GetComponents<Image>()[0].sprite = sprite3;
        swap.SetActive(false);
        canGoNext = true;
        needSwap = false;
        end = true;
        hasProposed = false;
    }

    public void newRound(){
        //everything grey again
        for(int i = 0; i < 5; i++){
            isGrey[i] = true;
            cards[i].GetComponent<Image>().color = new Color (150f/225f, 150f/225f, 150f/225f);
        }

        needSwap = false;
        hasProposed = false;
        canGoNext = false;
        end = false;
        playerPtsTxt.GetComponent<Text>().text = "Points: " + 0;
        playerPts = 0;
        propose.SetActive(false);
    }

    void chooseCard(int i){
        if (!isGrey[i]){
            cards[i].GetComponent<Image>().color = new Color (150f/225f, 150f/225f, 150f/225f);
            isGrey[i] = true;
        }else{
            if(countPickedCards() < 3){
                cards[i].GetComponent<Image>().color = Color.white;
                isGrey[i] = false;
            }
        }

        bool hasCone = false;
        bool hasIcecream = false;
        
        int idx = 0;
        numOfCones = 0;
        numOfIcecreams = 0;
        for(int j = 0; j < 5; j++){
            switch(cardIdentities[j].type){
                case 0:
                    numOfCones++;
                    break;
                case 1:
                    numOfIcecreams++;
                    break;
            }
            if(!isGrey[j]){
                chosed[idx] = j;
                idx++;
                if(cardIdentities[j].type == 0){
                    hasCone = true;
                }
                if(cardIdentities[j].type == 1){
                    hasIcecream = true;
                }
            }
        }

        if ((numOfCones == 0 || numOfIcecreams == 0) && countPickedCards() == 3 && !canGoNext){
            swap.SetActive(true);
            swapBtn.onClick.RemoveAllListeners();
            swapBtn.onClick.AddListener(delegate{ swapCards(chosed[0], chosed[1], chosed[2]); });
        }else if (countPickedCards() == 3 && hasCone && hasIcecream && !end){
            propose.SetActive(true);
        }else{
            if(!end){
                canGoNext = false;
            }
            propose.SetActive(false);
        }
    }
}
