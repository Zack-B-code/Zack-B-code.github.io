using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MoviesTVQuestion", menuName = "Question/Movies and TV Question")]
public class MoviesTVQuestionConfig : QuestionConfig
{
    public MoviesTVQuestionConfig() {
        questionCategory = "Movies & TV";
    }
}
