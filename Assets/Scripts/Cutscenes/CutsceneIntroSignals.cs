using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneIntroSignals : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private MessageController _messageController;
    [SerializeField] private PlayableDirector _timelineDirector;

    public void ShowFirstMessage()
    {
        _timelineDirector.Pause();

        _messageController.ShowMessage("Oh no! Looks like the door shut close!\nAnd where is the key?!\nYou must have dropped it somewhere...", delegate ()
        {
            _timelineDirector.Play();
        });
    }

    public void ShowSecondMessage()
    {
        _messageController.ShowMessage("You better be quick, you only got 5 minutes to get out of here!\nRemember to use the arrow keys to move arround.", delegate ()
        {
            _gameController.StartGame();
        });
    }
}
