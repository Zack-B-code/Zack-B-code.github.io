using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] playerPrefabs;
    public TextMeshProUGUI winTxt;
    public Vector3 piecePos;
    public Button playAgain;

    [Header("Set Dynamically")]
    public string winner;

    // Start is called before the first frame update
    void Start()
    {
        winner = PlayerPrefs.GetString("Winner"); // Retrieve winner

        int winnerID = 0;
        switch (winner) {
            case "Red":
                winnerID = 0;
                break;
            case "Yellow":
                winnerID = 1;
                break;
            case "Green":
                winnerID = 2;
                break;
            case "Blue":
                winnerID = 3;
                break;
            case "Cyan":
                winnerID = 4;
                break;
            case "Purple":
                winnerID = 5;
                break;
            case "White":
                winnerID = 6;
                break;
            case "Grey":
                winnerID = 7;
                break;
            case "Black":
                winnerID = 8;
                break;
        }

        // Display the winner
        // string playerColor = "";
        // switch (winner) {
        //     case 0:
        //         playerColor = "Cyan";
        //         winTxt.color = Color.cyan;
        //         break;
        //     case 1:
        //         playerColor = "Black";
        //         winTxt.color = Color.black;
        //         break;
        //     case 2:
        //         playerColor = "White";
        //         winTxt.color = Color.white;
        //         break;
        //     case 3:
        //         playerColor = "Purple";
        //         winTxt.color = new Color32(104, 43, 193, 255);
        //         break;
        // }
        winTxt.text = winner + " wins!";


        // Displays winning piece and puts it on boat
        GameObject ocean = GameObject.FindGameObjectWithTag("OceanAnchor");
        GameObject piece = Instantiate(playerPrefabs[winnerID]);

        piece.transform.SetParent(ocean.transform);
        piece.transform.localPosition = piecePos;

        playAgain.onClick.AddListener(Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Restart(){
        SceneManager.LoadScene("StartScreen");
    }
}
