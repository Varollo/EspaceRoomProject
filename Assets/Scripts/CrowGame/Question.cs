using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "CrowGame/Question")]
public class Question : ScriptableObject
{
    [TextArea] public string QuestionTxt;
    [TextArea] public string[] AnswersTxt;
    [Space]
    [TextArea] public string CorrectResponse;
    [TextArea] public string WrongResponse;
    [Space]
    [SerializeField] private int _correctAnswerIndex;

    public int CorrectAnswer => _correctAnswerIndex;
}
