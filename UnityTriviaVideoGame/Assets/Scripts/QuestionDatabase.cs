using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionDatabase", menuName = "DB/Question Database")]
public class QuestionDatabase : ScriptableObject
{
    [SerializeField] public List<QuestionConfig> difficulty0Questions;
    [SerializeField] public List<QuestionConfig> difficulty1Questions;
    [SerializeField] public List<QuestionConfig> difficulty2Questions;
    [SerializeField] public List<QuestionConfig> difficulty3Questions;
}
