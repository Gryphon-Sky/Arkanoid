using System;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed

    public AudioClip BallBouncedWall;
    public AudioClip BallBouncedPaddle;
    public AudioClip BallBouncedBrick;

    public AudioClip LifeLost;

    public AudioClip LevelStarted;
    public AudioClip LevelCompleted;

    public AudioClip GameStarted;
    public AudioClip GameCompleted;
    public AudioClip GameOver;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region public methods

    public void Init(bool soundEnabled)
    {
        _soundEnabled = soundEnabled;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods
    
    public void PlayBallBouncedSound(GameplayController.BounceType bounceType, Action onPlayed)
    {
        switch(bounceType)
        {
            case GameplayController.BounceType.Wall:
                PlayClip(BallBouncedWall, onPlayed);
                break;
            case GameplayController.BounceType.Paddle:
                PlayClip(BallBouncedPaddle, onPlayed);
                break;
            case GameplayController.BounceType.Brick:
                PlayClip(BallBouncedBrick, onPlayed);
                break;
        }
    }
    
    public void PlayLifeLostSound(Action onPlayed)
    {
        PlayClip(LifeLost, onPlayed);
    }
    
    public void PlayLevelStartedSound(Action onPlayed)
    {
        PlayClip(LevelStarted, onPlayed);
    }
    
    public void PlayLevelCompletedSound(Action onPlayed)
    {
        PlayClip(LevelCompleted, onPlayed);
    }
    
    public void PlayGameStartedSound(Action onPlayed)
    {
        PlayClip(GameStarted, onPlayed);
    }
    
    public void PlayGameCompletedSound(Action onPlayed)
    {
        PlayClip(GameCompleted, onPlayed);
    }
    
    public void PlayGameOverSound(Action onPlayed)
    {
        PlayClip(GameOver, onPlayed);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private methods

    private void PlayClip(AudioClip clip, Action onPlayed)
    {
        if(_soundEnabled && (clip != null))
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
            Utils.InvokeActionWithDelay(this, clip.length, onPlayed);
        }
        else
        {
            Utils.InvokeAction(onPlayed);
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private bool _soundEnabled;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
