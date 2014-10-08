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
    
    public void Init(float speed)
    {
        _speed = speed;

        _leftBorder = LeftWall.localPosition.x + (LeftWall.localScale.x + Width) / 2;
        _rightBorder = RightWall.localPosition.x - (RightWall.localScale.x + Width) / 2;
    }

    public void Reinit()
    {
        _positionInitializer.ResetPosition();
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

    private void FixedUpdate()
    {
        float input = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
        if(Utils.IsEqual0(input))
        {
            input = Input.GetAxis("Horizontal");
        }

        if(!Utils.IsEqual0(input))
        {
            Vector3 pos = transform.localPosition;
            pos.x = Mathf.Clamp(pos.x + input * _speed, _leftBorder, _rightBorder);
            transform.localPosition = pos;
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private float _speed;
    private float _leftBorder;
    private float _rightBorder;
    private PositionResetter _positionInitializer;

    #endregion

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
