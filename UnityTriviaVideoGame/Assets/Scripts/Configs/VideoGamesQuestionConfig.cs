using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "VideoGamesQuestion", menuName = "Question/Video Games Question")]
public class VideoGamesQuestionConfig : QuestionConfig
{
    public VideoGamesQuestionConfig() {
        questionCategory = "Video Games";
    }
}
