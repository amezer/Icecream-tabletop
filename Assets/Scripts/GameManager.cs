using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using CardIdentity;

namespace CardIdentity
{
    public class cardIdentity{
        public int type;
        public int ID;
        
        public cardIdentity(){

        }
        public cardIdentity (int type, int ID){
            this.type = type;
            this.ID = ID;
        }

        public string printIdentity(){
            return "Type: " + type + " ID: " + ID;
        }

        public int getWeather(){ 
            switch(type){
                case 0:
                    switch(ID){
                        case 0:
                            return 1;
                        case 1: 
                            return 3;
                        case 2: 
                            return 0;
                        case 3: 
                            return 5;
                        case 4: 
                            return 2;
                        case 5:
                        case 6: 
                            return 4;
                    }
                    break;
                case 1:
                    switch(ID){
                        case 0:
                        case 4:
                            return 1;
                        case 1:
                        case 12:
                        case 11:
                        case 9:
                            return 4;
                        case 2:
                        case 5:
                            return 3;
                        case 3:
                        case 6:
                            return 0;
                        case 8:
                            return 2;
                        case 7:
                        case 10:
                            return 5;
                    }
                    break;
                case 2:
                    switch(ID){//0 cloudy 1 hail 2 rainy 3 snow 4 sunny 5 thunder
                        case 0:
                        case 8:
                        case 3:
                            return 4;
                        case 4:
                            return 0;
                        case 7:
                            return 5;
                        case 1:
                        case 2:
                            return 3;
                        case 5:
                            return 2;
                        case 6: 
                            return 1;
                    }
                    break;
            }
            return 1;
        }

        public float getMultiplier(int currentWeather){
            int w = getWeather();
            if(w == currentWeather){
                switch(w){
                    case 0:
                        return 1.4f;
                    case 1:
                        return 2;
                    case 2:
                        return 1.8f;
                    case 3:
                        return 1.4f;
                    case 4:
                        return 1.2f;
                    case 5: 
                        return 1.6f;
                }
            }
            return 1;
        }
    }
}
public class GameManager : MonoBehaviour
{
    public GameObject nextPlayer;
    private Button nextPlayerBtn;
    public GameObject[] players;
    private int currentPlayerID;

    public Sprite[] conesSprite; // 0 black 1 caramel 2 choco 3 cinnamon 4 matcha 5 plain 6 strawberry
    public Sprite[] icecreamsSprite; // 0 black 1 blueberry 2 caramel 3 choco 4 mint 5 coffee 6 cookie 7 cotton 8 matcha 9 sakura 10 soda 11 strawberry 12 vanilla
    public Sprite[] toppingsSprite; // 0 berrySauce 1 brownies 2 caramel 3 cherry 4 chocoSauce 5 cornflakes 6 magic 7 pocky 8 w&w

    private int[] conesPts = {5, 4, 3, 4, 3, 2, 3};
    private int[] icecreamPts = {6, 4, 4, 3, 4, 4, 3, 5, 3, 5, 5, 3, 3};
    private int[] toppingsPts = {3, 2, 3, 1, 1, 2, 7, 2, 1};

    private int[] cones = {
        5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 
        2, 2, 2, 
        4, 4, 4, 
        6, 6, 6, 
        3, 3, 
        1, 1, 
        0
    };
    private int[] icecreams = {
        12, 12, 12, 12, 12, 12,
        3, 3, 3, 3, 3, 3, 
        11, 11, 11, 11, 11, 11,
        8, 8, 8, 8, 8, 8,
        6, 6, 6, 6, 6, 6,
        1, 1, 1, 1,
        4, 4, 4, 4, 
        5, 5, 5, 5,
        2, 2, 2, 2, 
        7, 7, 7,
        9, 9, 9, 
        10, 10, 10,
        0
    };
    private int[] toppings = {
        8, 8, 8, 8, 
        3, 3, 3, 3,
        4, 4, 4, 4, 
        7, 7, 7, 
        1, 1, 1, 
        5, 5, 5, 
        2, 2, 
        0, 0, 
        6
    };

    public GameObject[] playerRedCards;
    public GameObject[] playerYellowCards;
    public GameObject[] playerGreenCards;
    public GameObject[] playerBlueCards;
    public GameObject[] wins;
    public int trueWinner;
    public bool occupied = false;
    public List<int> outPlayers = new List<int>();
    public GameObject[] boardElements;
    public GameObject winImage;
    public GameObject winText;
    public Sprite[] weathers; //0 cloudy 1 hail 2 rainy 3 snow 4 sunny 5 thunder
    public GameObject weatherImg;
    public int currentWeather;
    // Start is called before the first frame update
    void Start()
    {
        nextPlayerBtn = nextPlayer.GetComponent<Button>();
        currentPlayerID = 4;
        players[4].SetActive(true);
        nextPlayerBtn.onClick.AddListener(goNextPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getCard(out Sprite rstCard, out cardIdentity identity){
        int type = Random.Range(0, 3); //0 = cone 1 = icecream 2 = toppings
        Sprite card;
        int ID;
        if (type == 0){
            ID = cones[Random.Range(0, cones.Length)];
            card = conesSprite[ID];
        }else if(type == 1){
            ID = icecreams[Random.Range(0, icecreams.Length)];
            card = icecreamsSprite[ID];
        }else{
            ID = toppings[Random.Range(0, toppings.Length)];
            card = toppingsSprite[ID];
        }
        rstCard = card;
        identity = new cardIdentity(type, ID);
    }

    void clearRound(){
        GameObject[] temp = playerRedCards;
        float highestPts = 0;
        List<int> winner = new List<int>();
        for(int i = 0; i < 4; i++){
            switch(i){
                case 0:
                    temp = playerRedCards;
                    break;
                case 1:
                    temp = playerYellowCards;
                    break;
                case 2:
                    temp = playerGreenCards;
                    break;
                case 3:
                    temp = playerBlueCards;
                    break;
            }

            Players playerScript = players[i].GetComponents<Players>()[0];
            if(playerScript.hasProposed){
                for(int j = 0; j < 3; j++){
                    temp[j].SetActive(true);
                    cardIdentity c = playerScript.cardIdentities[playerScript.chosed[j]];
                    if(c.type == 0){
                        temp[j].GetComponent<Image>().sprite = conesSprite[c.ID];
                    }else if(c.type == 1){
                        temp[j].GetComponent<Image>().sprite = icecreamsSprite[c.ID];
                    }else{
                        temp[j].GetComponent<Image>().sprite = toppingsSprite[c.ID];
                    }
                }
            }else{
                for(int j = 0; j < 3; j++){
                    temp[j].SetActive(false);
                }
            }

            if(highestPts == playerScript.playerPts){
                winner.Add(i);
            }else if(highestPts < playerScript.playerPts){
                winner.Clear();
                winner.Add(i);
                highestPts = playerScript.playerPts;
            }
        }

        Debug.Log("num of winners: " + winner.Count);
        Debug.Log("highest pts: " + highestPts);
        if(highestPts != 0){
            for(int k = 0; k < winner.Count; k++){
                Debug.Log(k+": "+winner[k]);
                wins[winner[k]].SetActive(true);
                if(winner.Count == 1){
                    Debug.Log("only 1 winner");
                    trueWinner = winner[k];
                }
            }
        }
    }
    

    private void goNextPlayer(){ 

        Players playerScript = players[currentPlayerID].GetComponents<Players>()[0];
        
        if(playerScript.canGoNext){
            players[currentPlayerID].SetActive(false);
            if(currentPlayerID == 4){
                currentWeather = Random.Range(0, 6);
                weatherImg.GetComponent<Image>().sprite = weathers[currentWeather];
                currentPlayerID = 0;
                for(int i = 0; i < 4; i++){
                    wins[i].SetActive(false);
                }
                occupied = false;
            }else{
                currentPlayerID++;
            }

            while(outPlayers.Contains(currentPlayerID)){
                players[currentPlayerID].GetComponent<Players>().playerPts = 0;
                currentPlayerID++;
            }

            players[currentPlayerID].SetActive(true);
            playerScript = players[currentPlayerID].GetComponents<Players>()[0];
            if (currentPlayerID != 4 && !playerScript.cardDealed){
                for (int i = 0; i < 5; i++){
                    int type = Random.Range(0, 3); //0 = cone 1 = icecream 2 = toppings
                    Sprite card;
                    int ID;
                    if (type == 0){
                        ID = cones[Random.Range(0, cones.Length)];
                        card = conesSprite[ID];
                        playerScript.numOfCones++;
                    }else if(type == 1){
                        ID = icecreams[Random.Range(0, icecreams.Length)];
                        card = icecreamsSprite[ID];
                        playerScript.numOfIcecreams++;
                    }else{
                        ID = toppings[Random.Range(0, toppings.Length)];
                        card = toppingsSprite[ID];
                    }
                    playerScript.cards[i].GetComponents<Image>()[0].sprite = card;
                    playerScript.cardIdentities[i] = new cardIdentity(type, ID);
                }

                playerScript.cardDealed = true;
            }else if(playerScript.hasProposed &&  currentPlayerID!=4){
                playerScript.newRound();
                for (int i = 0; i < 3; i++){
                    cardIdentity identity;
                    Sprite sprite;
                    int cardIdx = playerScript.chosed[i];
                    getCard(out sprite, out identity);
                    playerScript.cardIdentities[cardIdx] = identity;
                    if(playerScript.cardIdentities[cardIdx].type == 0){
                        playerScript.numOfCones++;
                    }else if(playerScript.cardIdentities[cardIdx].type == 1){
                        playerScript.numOfIcecreams++;
                    }
                    playerScript.cards[cardIdx].GetComponents<Image>()[0].sprite = sprite;
                }

            }else if(!playerScript.hasProposed && currentPlayerID != 4){
                playerScript.newRound();
                if(playerScript.numOfCones == 0 || playerScript.numOfIcecreams == 0){
                    playerScript.needSwap = true;
                }
            }else if(currentPlayerID == 4){
                clearRound();
                //interact with board elements
            }
        }
    }
}
