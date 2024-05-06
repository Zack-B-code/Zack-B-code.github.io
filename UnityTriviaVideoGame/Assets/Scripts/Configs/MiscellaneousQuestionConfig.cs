using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MiscellaneousQuestion", menuName = "Question/Miscellaneous Question")]
public class MiscellaneousQuestionConfig : QuestionConfig
{
    public MiscellaneousQuestionConfig() {
        questionCategory = "Miscellaneous";
    }
}
