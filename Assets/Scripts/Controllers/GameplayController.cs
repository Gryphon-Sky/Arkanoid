using UnityEngine;

public class GameplayController : MonoBehaviour, IGameStartedProvider
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region exposed

    public UIController UIController;
    public SoundController SoundController;

    public Ball Ball;
    public Paddle Paddle;
    public BricksController BricksController;

    public GameObject DebugShadow;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region IGameStartedProvider

    public bool GameStarted { get; private set; }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region properties
    
    private int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            UIController.Level = _level;
        }
    }
    private int Lives
    {
        get { return _lives; }
        set
        {
            _lives = value;
            UIController.Lives = _lives;
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region events

    public void OnBallBounced(bool paddle)
    {
        ++_consecutiveBounces;
        SoundController.PlayBallBouncedSound(paddle);
    }
    
    public void OnBrickDestroyed()
    {
        int penalty = _consecutiveBounces * Utils.Settings.BouncePenalty;
        int multiplier = Level * Utils.Settings.LevelMultiplier;
        ScoreController.AddScore(Utils.Settings.DefaultBrickScore, penalty, Utils.Settings.MinBrickScore, multiplier);

        _consecutiveBounces = 0;

        SoundController.PlayBrickDestroyedSound();
    }
    
    public void OnLastBrickDestroyed()
    {
        if(Level == Utils.Settings.MaxLevel)
        {
            GameComplete();
        }
        else
        {
            LevelComplete();
        }
    }
    
    public void OnLose()
    {
#if UNITY_EDITOR
        if(!Utils.Settings.DebugMode)
        {
            --Lives;
        }
#else
        --Lives;
#endif

        if(Lives == 0)
        {
            GameOver();
        }
        else
        {
            LevelOver();
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region game flow control
    
    private void InitGame()
    {
        Level = 1;
        Lives = Utils.Settings.MaxLives;
        ScoreController.ResetScore();
        
        InitLevel();
    }
    
    private void InitLevel()
    {
        Ball.SetSpeed(Utils.Settings.InitialBallSpeed + Utils.Settings.BallSpeedPerLevel * (Level - 1));
        BricksController.SpawnBricksForLevel(Level);

        RestartLevel();
    }
    
    private void RestartLevel()
    {
        GameStarted = false;
        _consecutiveBounces = 0;

        Ball.Reinit();
        Paddle.Reinit();
    }
    
    private void LevelComplete()
    {
        ++Level;
        InitLevel();
        SoundController.PlayLevelCompleteSound();
    }
    
    private void LevelOver()
    {
        RestartLevel();
        SoundController.PlayLevelOverSound();
    }
    
    private void GameComplete()
    {
        UIController.ShowGameCompleteMessage(ScoreController.Score);
        InitGame();
        SoundController.PlayGameCompleteSound();
    }
    
    private void GameOver()
    {
        UIController.ShowGameOverMessage(ScoreController.Score);
        InitGame();
        SoundController.PlayGameOverSound();
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoBehaviour

    private void Awake()
    {
#if UNITY_EDITOR
        DebugShadow.SetActive(Utils.Settings.DebugMode);
#endif
        Ball.Init(this, OnBallBounced, OnLose);
        Paddle.Init(Utils.Settings.PaddleSpeed);
        BricksController.Init(OnBrickDestroyed, OnLastBrickDestroyed);
        ScoreController.Init(UIController.SetScore, UIController.SetHighscore);

        InitGame();
    }
    
    private void FixedUpdate()
    {
        if(!GameStarted && Input.GetButton("Fire"))
        {
            GameStarted = true;
            Ball.Launch();
            UIController.HideMessage();
        }
    }
    
    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region private members

    private int _level;
    private int _score;
    private int _lives;
    private int _consecutiveBounces;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
