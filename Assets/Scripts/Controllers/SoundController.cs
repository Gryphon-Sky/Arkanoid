using UnityEngine;

public class SoundController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed

    public AudioClip BrickDestroyed;

    public AudioClip BallBounced;
    public AudioClip BallBouncedPaddle;

    public AudioClip LevelComplete;
    public AudioClip LevelOver;
    public AudioClip GameComplete;
    public AudioClip GameOver;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region public methods

    public void PlayBrickDestroyedSound()
    {
        PlayClip(BrickDestroyed);
    }
    
    public void PlayBallBouncedSound(bool paddle)
    {
        PlayClip(paddle ? BallBouncedPaddle : BallBounced);
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
        if(Utils.Settings.Sounds && (clip != null))
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
