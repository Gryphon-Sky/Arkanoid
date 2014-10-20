using UnityEngine;

public class GameplayController : MonoBehaviour, IGameStartedProvider
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed

    public enum BounceType
    {
        Wall,
        Paddle,
        Brick
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public Settings Settings;

    public UIController UIController;
    public InputController InputController;
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

    public void OnBallBounced(GameObject bounce)
    {
        BounceType bounceType = BounceType.Wall;

        Brick brick = bounce.GetComponent<Brick>();
        if(brick != null)
        {
            bounceType = BounceType.Brick;
            brick.OnBallBounced();
            _consecutiveBounces = 0;
        }
        else
        {
            if(bounce == Paddle.gameObject)
            {
                bounceType = BounceType.Paddle;
            }
            ++_consecutiveBounces;
        }

        SoundController.PlayBallBouncedSound(bounceType, null);
    }
    
    public void OnBrickDestroyed()
    {
        int penalty = _consecutiveBounces * Settings.BouncePenalty;
        int multiplier = Level * Settings.LevelMultiplier;
        ScoreController.AddScore(Settings.DefaultBrickScore, penalty, Settings.MinBrickScore, multiplier);
    }
    
    public void OnLastBrickDestroyed()
    {
        SoundController.PlayLevelCompletedSound(CompleteLevelOrGame);
    }

    public void OnLose()
    {
#if UNITY_EDITOR
        if(!Settings.DebugMode)
        {
            --Lives;
        }
#else
        --Lives;
#endif

        Paddle.Disappear();
        SoundController.PlayLifeLostSound(RestartLevelOrGame);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region game flow control
    
    private void InitGame()
    {
        Level = 1;
        Lives = Settings.MaxLives;
        ScoreController.ResetScore();
        UIController.HideMessage();
        
        SoundController.PlayGameStartedSound(InitLevel);
    }
    
    private void InitLevel()
    {
        Ball.SetSpeed(Settings.InitialBallSpeed + Settings.BallSpeedPerLevel * (Level - 1));
        BricksController.SpawnBricksForLevel(Level, Settings.MaxLives);

        RestartLevel();
        
        SoundController.PlayLevelStartedSound(null);
    }
    
    private void RestartLevel()
    {
        GameStarted = false;
        _consecutiveBounces = 0;

        Ball.Reinit();
        Paddle.Reinit();
    }
    
    private void RestartLevelOrGame()
    {
        if(Lives == 0)
        {
            GameOver();
        }
        else
        {
            RestartLevel();
        }
    }
    
    private void CompleteLevelOrGame()
    {
        if(Level == Settings.MaxLevel)
        {
            GameComplete();
        }
        else
        {
            LevelComplete();
        }
    }
    
    private void LevelComplete()
    {
        ++Level;
        InitLevel();
    }
    
    private void GameComplete()
    {
        UIController.ShowGameCompleteMessage(ScoreController.Score);
        SoundController.PlayGameCompletedSound(InitGame);
    }
    
    private void GameOver()
    {
        UIController.ShowGameOverMessage(ScoreController.Score);
        SoundController.PlayGameOverSound(InitGame);
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoBehaviour

    private void Awake()
    {
#if UNITY_EDITOR
        DebugShadow.SetActive(Settings.DebugMode);
#endif
        Ball.Init(this, Settings.MaxAngle, OnBallBounced, OnLose);
        Paddle.Init(InputController, Settings.PaddleSpeed);
        BricksController.Init(OnBrickDestroyed, OnLastBrickDestroyed);
        SoundController.Init(Settings.Sounds);
        ScoreController.Init(UIController.SetScore, UIController.SetHighscore);

        InitGame();
    }
    
    private void FixedUpdate()
    {
        if(!GameStarted && Input.GetButton("Fire"))
        {
            GameStarted = true;
            Ball.Launch();
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
