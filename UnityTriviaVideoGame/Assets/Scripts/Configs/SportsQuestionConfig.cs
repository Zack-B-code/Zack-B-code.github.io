using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SportsQuestion", menuName = "Question/Sports Question")]
public class SportsQuestionConfig : QuestionConfig
{
    public SportsQuestionConfig() {
        questionCategory = "Sports";
    }
}
