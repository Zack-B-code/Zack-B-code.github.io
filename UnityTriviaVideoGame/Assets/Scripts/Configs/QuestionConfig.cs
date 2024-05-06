using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Question/Default Question")]
public class QuestionConfig : BaseConfig // This is a generic config for all answer types. Each category has its own type of Config, to be used in its own type of Database.
{
    [field: SerializeField] public string questionText;
    [field: SerializeField] public string answerAText;
    [field: SerializeField] public string answerBText;
    [field: SerializeField] public string answerCText;
    [field: SerializeField] public string answerDText;
    [field: SerializeField, Range(0, 3)] public int correctAnswer;
    [field: SerializeField, Range(0, 3)] public int difficulty;
    public string questionCategory;
}
