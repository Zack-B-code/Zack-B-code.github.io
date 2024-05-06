using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "HistoryQuestion", menuName = "Question/History Question")]
public class HistoryQuestionConfig : QuestionConfig
{
    public HistoryQuestionConfig() {
        questionCategory = "History";
    }
}
