using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed

    public GUIStyle Style;

    public List<GameObject> LivesObjects;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region publiс setters

    public int Level
    {
        set
        {
            _level = "Level " + value;
        }
    }

    public int Lives
    {
        set
        {
            for(int i = 0; i < LivesObjects.Count; ++i)
            {
                LivesObjects[i].SetActive(i < value);
            }
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods

    public void ShowMessage(string message, float width)
    {
        _message = message;
        _messageRect = new Rect(500.0f - width / 2.0f, 200.0f, width, 70.0f);
        _isMessageShown = true;
    }

    public void HideMessage()
    {
        _isMessageShown = false;
    }

    public void SetScore(int score)
    {
        _score = "Score\n" + score;
    }
    
    public void SetHighscore(int highscore)
    {
        _highscore = "Highscore\n" + highscore;
    }

    public void ShowGameCompleteMessage(int score)
    {
        ShowMessage(string.Format("Last level completed!\n{0} Score", score), 300.0f);
    }

    public void ShowGameOverMessage(int score)
    {
        ShowMessage(string.Format("Game over!\n{0} Score", score), 300.0f);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoBehaviour

    private void OnGUI()
    {
        GUI.Box(new Rect(25, 50, 150, 40), _level, Style);
        GUI.Box(new Rect(25, 150, 150, 40), "Lives", Style);
        GUI.Box(new Rect(25, 400, 150, 80), _score, Style);
        GUI.Box(new Rect(25, 500, 150, 80), _highscore, Style);

        if(_isMessageShown)
        {
            GUI.Box(_messageRect, _message, Style);
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoBehaviour

    private string _level;
    private string _score;
    private string _highscore;

    private string _message;
    private Rect _messageRect;
    private bool _isMessageShown;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
