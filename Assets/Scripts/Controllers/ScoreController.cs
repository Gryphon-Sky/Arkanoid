using UnityEngine;
using System;

public static class ScoreController
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region properties
    
    public static int Score
    {
        get { return _score; }
        private set
        {
            _score = value;
            Utils.InvokeAction(_onScoreChanged, _score);

            if(_score > Highscore)
            {
                Highscore = _score;
            }
        }
    }

    private static int Highscore
    {
        get { return _highscore; }
        set
        {
            _highscore = value;
            PlayerPrefs.SetInt(HIGHSCORE_KEY, _highscore);
            Utils.InvokeAction(_onHighscoreChanged, _highscore);
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods

    public static void Init(Action<int> onScoreChanged, Action<int> onHighscoreChanged)
    {
        _onScoreChanged = onScoreChanged;
        _onHighscoreChanged = onHighscoreChanged;
        Highscore = PlayerPrefs.GetInt(HIGHSCORE_KEY, 0);
    }

    public static void AddScore(int baseScore, int penalty, int min, int multiplier)
    {
        Score += Mathf.Max(baseScore - penalty, min) * multiplier;
    }
    
    public static void ResetScore()
    {
        Score = 0;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region private members

    private static int _score;
    private static int _highscore;

    private static Action<int> _onScoreChanged;
    private static Action<int> _onHighscoreChanged;

    private const string HIGHSCORE_KEY = "Highscore";

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
