using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PositionResetter))]
public class Paddle : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region exposed

    public Transform LeftWall;
    public Transform RightWall;

    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region properties

    public float Width { get; private set; }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods
    
    public void Init(IInputProvider iInputProvider, float speed)
    {
        _iInputProvider = iInputProvider;
        _speed = speed;

        _leftBorder = LeftWall.localPosition.x + (LeftWall.localScale.x + Width) / 2;
        _rightBorder = RightWall.localPosition.x - (RightWall.localScale.x + Width) / 2;
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }

    public void Reinit()
    {
        _positionInitializer.ResetPosition();
        gameObject.SetActive(true);
    }
    
    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region MonoDevelop

    private void Awake()
    {
        _positionInitializer = GetComponent<PositionResetter>();
        Width = GetComponent<SpriteRenderer>().sprite.rect.width;
    }

    private void Update()
    {
        Vector3 pos = transform.localPosition;
        pos.x = Mathf.Clamp(pos.x + _iInputProvider.InputX * _speed, _leftBorder, _rightBorder);
        transform.localPosition = pos;
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private IInputProvider _iInputProvider;
    private float _speed;
    private float _leftBorder;
    private float _rightBorder;
    private PositionResetter _positionInitializer;

    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
