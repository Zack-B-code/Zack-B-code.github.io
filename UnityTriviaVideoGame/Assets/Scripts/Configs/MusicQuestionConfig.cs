using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicQuestion", menuName = "Question/Music Question")]
public class MusicQuestionConfig : QuestionConfig
{
    public MusicQuestionConfig() {
        questionCategory = "Music";
    }
}
