using UnityEngine;

public class SoundController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed

    public AudioClip BallBouncedWall;
    public AudioClip BallBouncedPaddle;
    public AudioClip BallBouncedBrick;

    public AudioClip LevelComplete;
    public AudioClip LevelOver;
    public AudioClip GameComplete;
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
    
    public void PlayBallBouncedSound(GameplayController.BounceType bounceType)
    {
        switch(bounceType)
        {
            case GameplayController.BounceType.Wall:
                PlayClip(BallBouncedWall);
                break;
            case GameplayController.BounceType.Paddle:
                PlayClip(BallBouncedPaddle);
                break;
            case GameplayController.BounceType.Brick:
                PlayClip(BallBouncedBrick);
                break;
        }
    }
    
    public void PlayLevelCompleteSound()
    {
        PlayClip(LevelComplete);
    }
    
    public void PlayLevelOverSound()
    {
        PlayClip(LevelOver);
    }
    
    public void PlayGameCompleteSound()
    {
        PlayClip(GameComplete);
    }
    
    public void PlayGameOverSound()
    {
        PlayClip(GameOver);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private methods

    private void PlayClip(AudioClip clip)
    {
        if(_soundEnabled && (clip != null))
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
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
