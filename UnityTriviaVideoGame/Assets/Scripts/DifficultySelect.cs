using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    [Header("Set in Inspector")]
    public TextMeshProUGUI categoryTextObj;
    public Button[] buttons;

    void Start()
    {
        buttons[0].onClick.AddListener(delegate{ DifficultySelected(0); });
        buttons[1].onClick.AddListener(delegate{ DifficultySelected(1); });
        buttons[2].onClick.AddListener(delegate{ DifficultySelected(2); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Display(string category) {  // Shows the active category
        categoryTextObj.text = category;
        switch (category)   // Set the category text color to match the color of the matching game space
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
        gameObject.SetActive(true);
    }

    void DifficultySelected(int difficulty) {   // Loads a question of selected difficulty
        Camera.main.GetComponent<GameManager>().LoadQuestion(difficulty);
    }
}
