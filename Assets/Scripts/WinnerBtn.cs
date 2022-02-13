using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerBtn : MonoBehaviour
{
    public int ID;
    private Button btn;
    private GameManager gameManager;
    public GameObject[] otherWins;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(isWinner);
    }

    public void isWinner(){
        gameManager.trueWinner = ID;
        for(int i = 0; i < 3; i++){
            otherWins[i].SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}