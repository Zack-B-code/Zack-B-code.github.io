using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] spaces;
    public List<GameObject> players;
    public GameObject question;
    public GameObject difficultySelect;
    public TextMeshProUGUI turnTxt;
    public TextMeshProUGUI playerNameTxt;
    public GameObject[] playerPrefabs;
    public Vector3[] initialPlayerPos;
    public AudioSource piecemoveAudioSource;

    [Header("Set Dynamically")]
    public int numPlayers;
    public int[] playerPositions = { 0, 0, 0, 0 };
    public List<string> playerColors;
    public int activePlayer;
    public string activeCategory;

    void Awake()
    {
        numPlayers = PlayerPrefs.GetInt("numPlayers"); // Attempt at getting player count from StartGameClick.cs

        for (int i = 0; i < numPlayers; i++)
        {
            //players[i].SetActive(true);
            switch (PlayerPrefs.GetString("Player"+i)) {
                case "Red":
                    GameObject player = Instantiate(playerPrefabs[0]);
                    player.transform.position = initialPlayerPos[i];
                    Vector3 rot = new Vector3();
                    rot.x = 270;
                    player.transform.rotation = Quaternion.Euler(rot);
                    players.Add(player);
                    break;
                case "Yellow":
                    player = Instantiate(playerPrefabs[1]);
                    player.transform.position = initialPlayerPos[i];
                    rot = new Vector3();
                    rot.x = 270;
                    player.transform.rotation = Quaternion.Euler(rot);
                    players.Add(player);
                    break;
                case "Green":
                    player = Instantiate(playerPrefabs[2]);
                    player.transform.position = initialPlayerPos[i];
                    rot = new Vector3();
                    rot.x = 270;
                    player.transform.rotation = Quaternion.Euler(rot);
                    players.Add(player);
                    break;
                case "Blue":
                    player = Instantiate(playerPrefabs[3]);
                    player.transform.position = initialPlayerPos[i];
                    rot = new Vector3();
                    rot.x = 270;
                    player.transform.rotation = Quaternion.Euler(rot);
                    players.Add(player);
                    break;
                case "Cyan":
                    player = Instantiate(playerPrefabs[4]);
                    player.transform.position = initialPlayerPos[i];
                    rot = new Vector3();
                    rot.x = 270;
                    player.transform.rotation = Quaternion.Euler(rot);
                    players.Add(player);
                    break;
                case "Purple":
                    player = Instantiate(playerPrefabs[5]);
                    player.transform.position = initialPlayerPos[i];
                    rot = new Vector3();
                    rot.x = 270;
                    player.transform.rotation = Quaternion.Euler(rot);
                    players.Add(player);
                    break;
                case "White":
                    player = Instantiate(playerPrefabs[6]);
                    player.transform.position = initialPlayerPos[i];
                    rot = new Vector3();
                    rot.x = 270;
                    player.transform.rotation = Quaternion.Euler(rot);
                    players.Add(player);
                    break;
                case "Grey":
                    player = Instantiate(playerPrefabs[7]);
                    player.transform.position = initialPlayerPos[i];
                    rot = new Vector3();
                    rot.x = 270;
                    player.transform.rotation = Quaternion.Euler(rot);
                    players.Add(player);
                    break;
                case "Black":
                    player = Instantiate(playerPrefabs[8]);
                    player.transform.position = initialPlayerPos[i];
                    rot = new Vector3();
                    rot.x = 270;
                    player.transform.rotation = Quaternion.Euler(rot);
                    players.Add(player);
                    break;
            }
        }
        question.SetActive(false);
        activePlayer = 0;


        //Initial fill question lists
        Question questionScript = question.GetComponent<Question>();

        questionScript.historyUnusedQ = new List<List<QuestionConfig>>();
        questionScript.moviesTVUnusedQ = new List<List<QuestionConfig>>();
        questionScript.musicUnusedQ = new List<List<QuestionConfig>>();
        questionScript.miscUnusedQ = new List<List<QuestionConfig>>();
        questionScript.sportsUnusedQ = new List<List<QuestionConfig>>();
        questionScript.videoGamesUnusedQ = new List<List<QuestionConfig>>();
        for (int i = 0; i < 4; i++)
        {
            questionScript.historyUnusedQ.Add(new List<QuestionConfig>());
            questionScript.miscUnusedQ.Add(new List<QuestionConfig>());
            questionScript.moviesTVUnusedQ.Add(new List<QuestionConfig>());
            questionScript.musicUnusedQ.Add(new List<QuestionConfig>());
            questionScript.sportsUnusedQ.Add(new List<QuestionConfig>());
            questionScript.videoGamesUnusedQ.Add(new List<QuestionConfig>());
            questionScript.RefillQuestionSet("History", i);
            questionScript.RefillQuestionSet("Miscellaneous", i);
            questionScript.RefillQuestionSet("Movies & TV", i);
            questionScript.RefillQuestionSet("Music", i);
            questionScript.RefillQuestionSet("Sports", i);
            questionScript.RefillQuestionSet("Video Games", i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Should we wait a few seconds before starting first turn?
        // I say yes, at least a few seconds. - Lukas
        StartCoroutine(WaitToStart());
    }

    IEnumerator WaitToStart()
    { // Forces the game to wait before the game starts
        yield return new WaitForSecondsRealtime(2); // Wait 2 seconds
        PlayerStartTurn(0);
        turnTxt.gameObject.SetActive(true);
        playerNameTxt.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayerStartTurn(int playerID)
    {    // Handles all starting operations for a player's turn
        //print("Player " + playerID);

        //Display text showing whose turn it is
        playerNameTxt.text = players[playerID].GetComponent<Player>().color;
        playerNameTxt.color = players[playerID].GetComponent<Player>().GetPlayerColor32();

        int category = Random.Range(0, 6); // Random category, will be changed if not on start or end

        //Set category to category of space if not on start or end
        if (playerPositions[playerID] != 0 && playerPositions[playerID] != spaces.Length + 1)
        {
            activeCategory = spaces[playerPositions[playerID] - 1].GetComponent<GameSpace>().category;
        }
        else
        {
            switch (category)
            {
                case 0:
                    activeCategory = "History";
                    break;
                case 1:
                    activeCategory = "Miscellaneous";
                    break;
                case 2:
                    activeCategory = "Movies & TV";
                    break;
                case 3:
                    activeCategory = "Music";
                    break;
                case 4:
                    activeCategory = "Sports";
                    break;
                case 5:
                    activeCategory = "Video Games";
                    break;
            }
        }

        //Ask for difficulty unless on last space
        if (playerPositions[playerID] == spaces.Length + 1)
        {
            LoadQuestion(3);
        }
        else
        {
            difficultySelect.GetComponent<DifficultySelect>().Display(activeCategory);
        }
    }

    public void LoadQuestion(int difficulty)
    {  // Loads a question using the Question class
        difficultySelect.SetActive(false);
        Question questionScript = question.GetComponent<Question>();
        questionScript.SetQuestion(questionScript.GetQuestion(activeCategory, difficulty));
    }

    public void PlayerCorrect(int difficulty)
    { // Advances the player when they get a question correct, then selects the next player
        question.SetActive(false);

        if (difficulty == 3)
        {
            PlayerPrefs.SetString("Winner", players[activePlayer].GetComponent<Player>().color);
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            for (int i = 0; i < difficulty + 1; i++)    // A for-loop is used to move the piece one space at a time, as many times as needed
            {
                // Update Position
                int newPos = playerPositions[activePlayer] + 1;

                if (newPos > spaces.Length + 1)
                {
                    newPos = spaces.Length + 1;
                }
                playerPositions[activePlayer] = newPos;

                // Adjusts pieces on space moved from
                if (players[activePlayer].transform.parent != null)
                {
                    players[activePlayer].transform.parent.GetComponent<GameSpace>().playersOnSpace.Remove(players[activePlayer]);
                    players[activePlayer].transform.parent.GetComponent<GameSpace>().MovePiece();
                }

                if (playerPositions[activePlayer] == spaces.Length + 1)
                {
                    FinalSpace spaceScript = FindObjectOfType<FinalSpace>().GetComponent<FinalSpace>();
                    players[activePlayer].transform.SetParent(spaceScript.gameObject.transform);
                    spaceScript.playersOnSpace.Add(players[activePlayer]);
                    spaceScript.MovePiece();
                }
                else
                { // Move to new space
                    players[activePlayer].transform.SetParent(spaces[playerPositions[activePlayer] - 1].transform);
                    spaces[playerPositions[activePlayer] - 1].GetComponent<GameSpace>().playersOnSpace.Add(players[activePlayer]);
                    spaces[playerPositions[activePlayer] - 1].GetComponent<GameSpace>().MovePiece();


                    for (int a = 0; a < difficulty+1; a++)
                    {
                        //piecemoveAudioSource.Play();
                    }
                    
                }
            }

            //Next turn
            StartCoroutine(WaitForNextTurn());
        }
    }

    public void PlayerIncorrect()
    { // Selects the next player when a player gets a question incorrect
        question.SetActive(false);

        //Next turn
        StartCoroutine(WaitForNextTurn());
    }

    IEnumerator WaitForNextTurn()
    { // Waits before starting next turn
        yield return new WaitForSecondsRealtime(2); // Wait 2 seconds

        activePlayer++;
        activePlayer = activePlayer % numPlayers;
        PlayerStartTurn(activePlayer);
    }
}
