using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PositionResetter))]
public class Ball : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public Paddle Paddle;
    public Collider2D ExitZone;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods

    public void Init(IGameStartedProvider gameStartedProvider, float maxAngle, Action<GameObject> onBounce,
                     Action onExit)
    {
        _gameStartedProvider = gameStartedProvider;
        _maxAngle = maxAngle;
        _onBounce = onBounce;
        _onExit = onExit;
    }

    public void Reinit()
    {
        _positionInitializer.ResetPosition();
        rigidbody2D.velocity = Vector2.zero;
    }

    public void Launch()
    {
        Launch(_initialPaddlePart);
    }
    
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region events
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_isPaddlePartInitialized && !_gameStartedProvider.GameStarted)
        {
            return;
        }

        if(collision.gameObject == Paddle.gameObject)
        {
            float paddlePart = 2 * (collision.contacts[0].point.x - Paddle.transform.position.x) / Paddle.Width;

            if(_gameStartedProvider.GameStarted)
            {
                Launch(paddlePart);
            }
            else if(!_isPaddlePartInitialized)
            {
                _initialPaddlePart = paddlePart;
                _isPaddlePartInitialized = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // check and fix velocity
        if(_gameStartedProvider.GameStarted)
        {
            Vector2 maxAngleTemplate = Utils.DegreesToVector2(_maxAngle);

            Vector2 velocity = rigidbody2D.velocity.normalized;
            if(Mathf.Abs(velocity.y) < maxAngleTemplate.y)
            {
                velocity.x = maxAngleTemplate.x * Mathf.Sign(velocity.x);
                velocity.y = maxAngleTemplate.y * Mathf.Sign(velocity.y);
                rigidbody2D.velocity = _speed * velocity;
            }

            Utils.InvokeAction(_onBounce, collision.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if(col == ExitZone)
        {
            Utils.InvokeAction(_onExit);
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoBehaviour

    private void Awake()
    {
        _positionInitializer = GetComponent<PositionResetter>();
    }

    private void Start()
    {
        _isPaddlePartInitialized = false;
        _initialPaddlePart = 0.0f;
        _initialShift = transform.position.x - Paddle.gameObject.transform.position.x;
    }

    private void LateUpdate()
    {
        if(!_gameStartedProvider.GameStarted)
        {
            Vector3 pos = transform.position;
            pos.x = Paddle.gameObject.transform.position.x + _initialShift;
            transform.position = pos;
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private methods
    
    private void Launch(float paddlePart)
    {
        rigidbody2D.velocity = _speed * Utils.DegreesToVector2(paddlePart * _maxAngle);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private IGameStartedProvider _gameStartedProvider;
    private Action<GameObject> _onBounce;
    private Action _onExit;
    private float _speed;
    private PositionResetter _positionInitializer;
    private bool _isPaddlePartInitialized;
    private float _initialPaddlePart;
    private float _initialShift;
    private float _maxAngle;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
