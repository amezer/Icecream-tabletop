using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoardElements : MonoBehaviour
{
    public int x;
    public int y;
    public int belongsTo = -1;
    public Image image;
    public Sprite defaultSprite;
    private Button btn;
    private GameManager gameManager;
    private int winner;
    private int winnerID;
    public bool isShop = false;
    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.sprite = defaultSprite;
        btn = gameObject.GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        btn.onClick.AddListener(occupy);
    }

    // Update is called once per frame
    void Update()
    {
        winner = gameManager.trueWinner;
    }

    bool canOccupy(){
        GameObject temp;
        if(x-1 >= 0){
            temp = GameObject.Find((x-1)+", " + y);
            if(temp.GetComponent<BoardElements>().belongsTo == winner){
                return true;
            }
        }

        if(x+1 <= 4){
            temp = GameObject.Find((x+1)+", " + y);
            if(temp.GetComponent<BoardElements>().belongsTo == winner){
                return true;
            }
        }

        if(y-1 >= 0){
            temp = GameObject.Find(x+", " + (y-1));
            if(temp.GetComponent<BoardElements>().belongsTo == winner){
                return true;
            }
        }

        if(y+1 <= 4){
            temp = GameObject.Find(x+", " + (y+1));
            if(temp.GetComponent<BoardElements>().belongsTo == winner){
                return true;
            }
        }

        return false;
    }

    void occupy(){
        Sprite newSprite = gameManager.players[winner].GetComponent<Players>().rabbit;
        //winnerID = gameManager.players[winner].GetComponents<Players>()[0].ID;
        //need a switch to controll ability to change block
        if(!gameManager.occupied && canOccupy() && belongsTo != winner){
            image.sprite = newSprite;
            gameManager.occupied = true;
            for(int i = 0; i < 4; i++){
                gameManager.wins[i].SetActive(false);
            }
            if(isShop){
                gameManager.outPlayers.Add(belongsTo);
                Debug.Log("player out: " + belongsTo);
  
                GameObject[] allElements = gameManager.boardElements;
                for(int i = 0; i < allElements.Length; i++){
                    Debug.Log(i + " belongs to: "+allElements[i].GetComponent<BoardElements>().belongsTo);
                    if(allElements[i].GetComponent<BoardElements>().belongsTo == belongsTo){
                        Debug.Log("called");
                        allElements[i].GetComponent<BoardElements>().image.sprite = newSprite;
                        allElements[i].GetComponent<BoardElements>().belongsTo = winner;
                    }
                }
                if(gameManager.outPlayers.Count == 3){
                    gameManager.winText.GetComponent<Button>().GetComponentInChildren<Text>().text = "Player " + winner;
                    gameManager.winText.SetActive(true);
                    gameManager.winImage.SetActive(true);
                }
                isShop = false;
            }
            belongsTo = winner;
            
        }
    }
}
