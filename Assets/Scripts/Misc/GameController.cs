using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _timelineDirector;
    [SerializeField] private Timer _timer;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Fade _fade;
    [SerializeField] private Fade _youWonFade;
    [SerializeField] private Fade _youLostFade;
    [SerializeField] private AudioController _audioController;


    private void Start()
    {
        _fade.FadeIn();
    }

    public void StartGame()
    {
        _audioController.Play("Music");
        _timelineDirector.Stop();
        _timer.StartTimer();
        _inventory.ShowHud();
        PlayerMovement.CanMove = true;
        InteractableManager.CanInteract = true;
    }

    public void WinGame()
    {
        _audioController.Play("Right");
        _audioController.Stop("Music");
        _audioController.Stop("Background");

        _timer.Stop = true;
        _fade.FadeOut();
        _youWonFade.FadeOut();
    }

    public void LoseGame()
    {
        _audioController.Play("Wrong");
        _audioController.Stop("Music");
        _audioController.Stop("Background");

        _fade.FadeOut();
        _youLostFade.FadeOut();
    }
}
