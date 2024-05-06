using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameClick : MonoBehaviour
{
    public Vector3 initialPos;
    public Vector3 initialRot;
    public Vector3 pieceSelectPos;
    public Vector3 pieceSelectRot;
    public GameObject startMenu;
    public GameObject startMenuBG;
    public GameObject pieceSelectMenu;
    public GameObject pieceExamples;
    public GameObject infoMenu;
    public TextMeshProUGUI playerSelectText;
    public Button[] pieceButtons;
    public Button infoButton;
    public Button backButton;

    public int playerID;
    public List<string> selectedColors = new List<string>();

    void Awake() {
        Camera.main.transform.position = initialPos;
        Camera.main.transform.rotation = Quaternion.Euler(initialRot);

        pieceButtons[0].onClick.AddListener(() => { SelectedPiece("Red"); });
        pieceButtons[1].onClick.AddListener(() => { SelectedPiece("Yellow"); });
        pieceButtons[2].onClick.AddListener(() => { SelectedPiece("Green"); });
        pieceButtons[3].onClick.AddListener(() => { SelectedPiece("Blue"); });
        pieceButtons[4].onClick.AddListener(() => { SelectedPiece("Cyan"); });
        pieceButtons[5].onClick.AddListener(() => { SelectedPiece("Purple"); });
        pieceButtons[6].onClick.AddListener(() => { SelectedPiece("White"); });
        pieceButtons[7].onClick.AddListener(() => { SelectedPiece("Grey"); });
        pieceButtons[8].onClick.AddListener(() => { SelectedPiece("Black"); });

        infoButton.onClick.AddListener(LoadInfo);
        backButton.onClick.AddListener(LoadTitle);

        startMenu.SetActive(true);
        startMenuBG.SetActive(true);
        pieceSelectMenu.SetActive(false);
        pieceExamples.SetActive(false);
        infoMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void ChangeScene1P()
    {
        // if 1 player button clicked
        // store data of 1 player
        PlayerPrefs.SetInt("numPlayers", 1);
        //SceneManager.LoadScene("GameplayScene");
        LoadPieceSelectMenu();
    }
    public void ChangeScene2P()
    {     
        // if 2 player button clicked
        // store data of 2 players
        PlayerPrefs.SetInt("numPlayers", 2);
        //SceneManager.LoadScene("GameplayScene");
        LoadPieceSelectMenu();
    }
    public void ChangeScene3P()
    {     
        // if 2 player button clicked
        // store data of 3 players
        PlayerPrefs.SetInt("numPlayers", 3);
        //SceneManager.LoadScene("GameplayScene");
        LoadPieceSelectMenu();
    }
    public void ChangeScene4P()
    {     
        // if 2 player button clicked
        // store data of 4 players
        PlayerPrefs.SetInt("numPlayers", 4);
        //SceneManager.LoadScene("GameplayScene");
        playerID = 0;
        LoadPieceSelectMenu();
    }

    void LoadPieceSelectMenu() {
        startMenu.SetActive(false);
        startMenuBG.SetActive(false);
        pieceSelectMenu.SetActive(true);
        pieceExamples.SetActive(true);

        Camera.main.transform.position = pieceSelectPos;
        Camera.main.transform.rotation = Quaternion.Euler(pieceSelectRot);

        foreach (Button button in pieceButtons) {
            if (selectedColors.Contains(button.name)) {
                button.GetComponent<Image>().color = Color.gray;
                button.onClick.RemoveAllListeners();
            }
        }

        playerSelectText.text = "Player " + (playerID+1) + " Select Your Piece";
    }

    public void SelectedPiece(string color) {
        PlayerPrefs.SetString("Player" + playerID, color);
        selectedColors.Add(color);

        playerID++;
        if (playerID >= PlayerPrefs.GetInt("numPlayers")) {
            SceneManager.LoadScene("GameplayScene");
        } else {
            LoadPieceSelectMenu();
        }
    }

    void LoadInfo() {
        startMenu.SetActive(false);
        startMenuBG.SetActive(false);
        infoMenu.SetActive(true);
    }

    void LoadTitle() {
        startMenu.SetActive(true);
        startMenuBG.SetActive(true);
        infoMenu.SetActive(false);
    }
}
