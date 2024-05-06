using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    [Header("Set in Inspector")]
    public TextMeshProUGUI categoryTextObj;
    public TextMeshProUGUI questionTextObj;
    public TextMeshProUGUI resultTextObj;
    public AudioSource correctAudioSource;
    public AudioSource incorrectAudioSource;
    public AudioSource piecewinAudioSource;
    public Button[] buttons = new Button[4]; // This value is properly set in Inspector. Set it to 4.
    public QuestionDatabase historyQuestionDatabase;
    public QuestionDatabase miscellaneousQuestionDatabase;
    public QuestionDatabase moviesTVQuestionDatabase;
    public QuestionDatabase musicQuestionDatabase;
    public QuestionDatabase sportsQuestionDatabase;
    public QuestionDatabase videoGamesQuestionDatabase;

    [Header("Set Dynamically")]
    public string questionCategory;
    public string questionText;
    public string answerAText;
    public string answerBText;
    public string answerCText;
    public string answerDText;
    public int correctAnswer; // 0 = A, 1 = B, 2 = C, 3 = D
    public string correctAnswerStr;
    public string[] answers = new string[4];
    public int difficulty;  // A value from 0 to 3, 0 being the easiest, 3 being the hardest
    public List<List<QuestionConfig>> historyUnusedQ;
    public List<List<QuestionConfig>> miscUnusedQ;
    public List<List<QuestionConfig>> moviesTVUnusedQ;
    public List<List<QuestionConfig>> musicUnusedQ;
    public List<List<QuestionConfig>> sportsUnusedQ;
    public List<List<QuestionConfig>> videoGamesUnusedQ;

    void Awake()
    {   
        questionText = questionTextObj.gameObject.GetComponent<TextMeshProUGUI>().text;
        answerAText = buttons[0].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        answerBText = buttons[1].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        answerCText = buttons[2].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        answerDText = buttons[3].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        resultTextObj.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //QuestionDisplayTest();

        //print(questionText);
        //print(answerAText);
        //print(answerBText);
        //print(answerCText);
        //print(answerDText);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public QuestionConfig GetQuestion(string category, int difficulty)  // Selects a question of given category and difficulty from the database
    { // Should this class go in a class that handles the question selection, and not just the question displaying?
        List<QuestionConfig> questionList = new List<QuestionConfig>();
        int listSize;
        int rand = 0;
        
        switch (category)   // Find the correct list to use
        {
            case "History":
                questionList.AddRange(historyUnusedQ[difficulty]);

                listSize = questionList.Count;
                rand = Random.Range(0, listSize);

                // Remove used Question, refill if empty
                historyUnusedQ[difficulty].RemoveAt(rand);
                if (historyUnusedQ[difficulty].Count == 0) {
                    RefillQuestionSet(category, difficulty);
                }
                break;
            case "Miscellaneous":
                questionList.AddRange(miscUnusedQ[difficulty]);

                listSize = questionList.Count;
                rand = Random.Range(0, listSize);

                miscUnusedQ[difficulty].RemoveAt(rand);
                if (miscUnusedQ[difficulty].Count == 0) {
                    RefillQuestionSet(category, difficulty);
                }
                break;
            case "Movies & TV":
                questionList.AddRange(moviesTVUnusedQ[difficulty]);

                listSize = questionList.Count;
                rand = Random.Range(0, listSize);

                moviesTVUnusedQ[difficulty].RemoveAt(rand);
                if (moviesTVUnusedQ[difficulty].Count == 0) {
                    RefillQuestionSet(category, difficulty);
                }
                break;
            case "Music":
                questionList.AddRange(musicUnusedQ[difficulty]);

                listSize = questionList.Count;
                rand = Random.Range(0, listSize);

                musicUnusedQ[difficulty].RemoveAt(rand);
                if (musicUnusedQ[difficulty].Count == 0) {
                    RefillQuestionSet(category, difficulty);
                }
                break;
            case "Sports":
                questionList.AddRange(sportsUnusedQ[difficulty]);

                listSize = questionList.Count;
                rand = Random.Range(0, listSize);

                sportsUnusedQ[difficulty].RemoveAt(rand);
                if (sportsUnusedQ[difficulty].Count == 0) {
                    RefillQuestionSet(category, difficulty);
                }
                break;
            case "Video Games":
                questionList.AddRange(videoGamesUnusedQ[difficulty]);

                listSize = questionList.Count;
                rand = Random.Range(0, listSize);

                videoGamesUnusedQ[difficulty].RemoveAt(rand);
                if (videoGamesUnusedQ[difficulty].Count == 0) {
                    RefillQuestionSet(category, difficulty);
                }
                break;
        }

        return questionList[rand]; // Select a random question within the range of the list
    }

    public void SetQuestion(QuestionConfig questionConfig)  // Sets the question up on the Question handler, and configures all values
    {
        // Set all class values to the values from the question config
        questionText = questionConfig.questionText;
        // answerAText = questionConfig.answerAText;
        // answerBText = questionConfig.answerBText;
        // answerCText = questionConfig.answerCText;
        // answerDText = questionConfig.answerDText;
        correctAnswer = questionConfig.correctAnswer;
        List<string> answerTemp = new List<string>();
        answers[0] = questionConfig.answerAText;
        answers[1] = questionConfig.answerBText;
        answers[2] = questionConfig.answerCText;
        answers[3] = questionConfig.answerDText;
        for (int i = 0; i < 4; i++) {
            answerTemp.Add(answers[i]);
        }
        correctAnswerStr = answers[correctAnswer];
        difficulty = questionConfig.difficulty;
        questionCategory = questionConfig.questionCategory;


        // Show all values on the Question handler
        switch (questionCategory)   // Set the category text color to match the color of the matching game space
        {
            case "History":
                categoryTextObj.gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(39, 89, 201, 255);
                break;
            case "Miscellaneous":
                categoryTextObj.gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(86, 120, 47, 255);
                break;
            case "Movies & TV":
                categoryTextObj.gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(169, 50, 51, 255);
                break;
            case "Music":
                categoryTextObj.gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(228, 192, 64, 255);
                break;
            case "Sports":
                categoryTextObj.gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(139, 110, 84, 255);
                break;
            case "Video Games":
                categoryTextObj.gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(160, 160, 160, 255);
                break;
        }
        categoryTextObj.gameObject.GetComponent<TextMeshProUGUI>().text = questionCategory;
        questionTextObj.gameObject.GetComponent<TextMeshProUGUI>().text = questionText;
        // buttons[0].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answerAText;
        // buttons[1].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answerBText;
        // buttons[2].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answerCText;
        // buttons[3].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answerDText;

        for (int i = 0; i < 4; i++) {
            int rand = Random.Range(0, answerTemp.Count);
            buttons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answerTemp[rand];
            answers[i] = answerTemp[rand];
            answerTemp.RemoveAt(rand);
        }
        // buttons[0].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answers[0];
        // buttons[1].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answers[1];
        // buttons[2].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answers[2];
        // buttons[3].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answers[3];

        // Add listeners to all answer buttons
        for (int i = 0; i < 4; i++) {
            if (answers[i] == correctAnswerStr) {
            //if (i == correctAnswer) {
                buttons[i].onClick.AddListener(CorrectClicked);
                correctAnswer = i;
            } else {
                buttons[i].onClick.AddListener(IncorrectClicked);
            }
        }

        //print(questionText);

        gameObject.SetActive(true);
    }

    void QuestionDisplayTest()
    {
        string category = null;
        int randomCategory = Random.Range(0, 6); // Select a random category from the six possible
        switch (randomCategory)
        {
            case 0:
                category = "History";
                break;
            case 1:
                category = "Miscellaneous";
                break;
            case 2:
                category = "Movies & TV";
                break;
            case 3:
                category = "Music";
                break;
            case 4:
                category = "Sports";
                break;
            case 5:
                category = "Video Games";
                break;
        }
        print("The category selected is " + category);
        SetQuestion(GetQuestion(category, 0));
    }

    void CorrectClicked() { // Shows the correct answer selected
        //print("Correct Answer");
        if (difficulty == 3) {
            piecewinAudioSource.Play();
        } else {
            correctAudioSource.Play();
        }
        buttons[correctAnswer].GetComponent<Image>().color = Color.green;
        resultTextObj.enabled = true;
        resultTextObj.color = Color.green;
        resultTextObj.text = "Correct!";

        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
        buttons[2].onClick.RemoveAllListeners();
        buttons[3].onClick.RemoveAllListeners();

        StartCoroutine(QuestionDelayCorrect());
    }

    void IncorrectClicked() { // Shows the incorrect answer selected and the correct answer
        //print("Incorrect Answer");

        for (int i = 0; i < 4; i++) {
            //if (answers[i] == correctAnswerStr) {
            if (i == correctAnswer) {
                buttons[i].GetComponent<Image>().color = Color.green;
            } else {
                buttons[i].GetComponent<Image>().color = Color.red;
            }
        }
        incorrectAudioSource.Play();
        resultTextObj.enabled = true;
        resultTextObj.color = Color.red;
        resultTextObj.text = "Incorrect!";

        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
        buttons[2].onClick.RemoveAllListeners();
        buttons[3].onClick.RemoveAllListeners();

        StartCoroutine(QuestionDelayIncorrect());
    }

    IEnumerator QuestionDelayCorrect() {   // Forces the game to wait so the answers can be viewed
        yield return new WaitForSecondsRealtime(3); // Wait 5 seconds

        ResetQuestion();
        Camera.main.GetComponent<GameManager>().PlayerCorrect(difficulty);
    }

    IEnumerator QuestionDelayIncorrect() {   // Forces the game to wait so the answers can be viewed
        yield return new WaitForSecondsRealtime(3); // Wait 5 seconds

        ResetQuestion();
        Camera.main.GetComponent<GameManager>().PlayerIncorrect();
    }

    void ResetQuestion() {  // Resets all question colors to white, and disables the result text
        buttons[0].GetComponent<Image>().color = Color.white;
        buttons[1].GetComponent<Image>().color = Color.white;
        buttons[2].GetComponent<Image>().color = Color.white;
        buttons[3].GetComponent<Image>().color = Color.white;

        categoryTextObj.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        resultTextObj.color = Color.white;
        resultTextObj.enabled = false;
    }

    public void RefillQuestionSet(string category, int difficulty) {  //Fill lists with questions from database
        QuestionDatabase database = null;
        List<QuestionConfig> questionList = null;

        switch (category)   // Find the correct Database to use
        {
            case "History":
                database = historyQuestionDatabase;
                break;
            case "Miscellaneous":
                database = miscellaneousQuestionDatabase;
                break;
            case "Movies & TV":
                database = moviesTVQuestionDatabase;
                break;
            case "Music":
                database = musicQuestionDatabase;
                break;
            case "Sports":
                database = sportsQuestionDatabase;
                break;
            case "Video Games":
                database = videoGamesQuestionDatabase;
                break;
        }
        
        switch (difficulty) // Find the correct question list to use within the Database
        {
            case 0:
                questionList = database.difficulty0Questions;
                break;
            case 1:
                questionList = database.difficulty1Questions;
                break;
            case 2:
                questionList = database.difficulty2Questions;
                break;
            case 3:
                questionList = database.difficulty3Questions;
                break;
        }

        switch (category)   // Find the correct Database to use
        {
            case "History":
                historyUnusedQ[difficulty].AddRange(questionList);
                break;
            case "Miscellaneous":
                miscUnusedQ[difficulty].AddRange(questionList);
                break;
            case "Movies & TV":
                moviesTVUnusedQ[difficulty].AddRange(questionList);
                break;
            case "Music":
                musicUnusedQ[difficulty].AddRange(questionList);
                break;
            case "Sports":
                sportsUnusedQ[difficulty].AddRange(questionList);
                break;
            case "Video Games":
                videoGamesUnusedQ[difficulty].AddRange(questionList);
                break;
        }
    }
}
