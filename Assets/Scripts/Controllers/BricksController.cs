using UnityEngine;
using System;
using System.Collections.Generic;

public class BricksController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed

    public GameObject BrickPrefab;
    public SpriteRenderer BG;

    public Transform LeftWall;
    public Transform RightWall;
    public Transform TopWall;

    public int SpacingX = 1;
    public int SpacingY = 30;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods

    public void Init(Action onBrickDestroyed, Action onLastBrickDestroyed)
    {
        _onBrickDestroyed = onBrickDestroyed;
        _onLastBrickDestroyed = onLastBrickDestroyed;
    }
    
    public void SpawnBricksForLevel(int level, int maxLevel)
    {
        Color bgMask = Color.red;
        switch((level - 1) % 3)
        {
            case 0:
                bgMask = Color.green;
                break;
            case 1:
                bgMask = Color.blue;
                break;
        }

        int zone = (level - 1) / 3;

        BG.color = GetBGColor(bgMask, zone);

        if(_bricks == null)
        {
            _bricks = new List<GameObject>();
        }
        else
        {
            foreach(GameObject brick in _bricks)
            {
                GameObject.Destroy(brick);
            }
            _bricks.Clear();
        }

        int brickSizeX = Mathf.FloorToInt(BrickPrefab.GetComponent<SpriteRenderer>().sprite.rect.width);
        int brickSizeY = Mathf.FloorToInt(BrickPrefab.GetComponent<SpriteRenderer>().sprite.rect.height);

        int leftBorder = Mathf.CeilToInt(LeftWall.localPosition.x + LeftWall.localScale.x / 2);
        int rightBorder = Mathf.FloorToInt(RightWall.localPosition.x - RightWall.localScale.x / 2);
        int topBorder = Mathf.FloorToInt(TopWall.localPosition.y - TopWall.localScale.y / 2);

        int left = leftBorder + SpacingX + (brickSizeX / 2);
        int right = rightBorder - SpacingX - (brickSizeX / 2);
        int shiftX = brickSizeX + SpacingX;
        int lineOffsetX = ((right - left) % shiftX);

        int top = topBorder - SpacingY - (brickSizeY / 2);
        int shiftY = -brickSizeY - SpacingY;
        
        int maxLines = maxLevel + 1;
        int numLines = level + 1;
        int topLine = (maxLines - numLines) / 2;

        // Inversing color mask for contrast bricks
        Color bricksMask = bgMask;
        bricksMask.r = 1.0f - bricksMask.r;
        bricksMask.g = 1.0f - bricksMask.g;
        bricksMask.b = 1.0f - bricksMask.b;

        for(int i = 0; i < numLines; ++i)
        {
            Color color = GetBrickColor(bricksMask, i, numLines);
            int line = (topLine + i);
            int y = top + line * shiftY;

            int offsetX = ((line % 2) == 0) ? 0 : lineOffsetX;
            for(int x = left; x <= right; x += shiftX)
            {
                SpawnBrick(x + offsetX, y, color);
            }
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region events
    
    private void OnBrickDestroyed(GameObject brick)
    {
        Utils.InvokeAction(_onBrickDestroyed);
        
        _bricks.Remove(brick);
        if(_bricks.Count == 0)
        {
            Utils.InvokeAction(_onLastBrickDestroyed);
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region colors
    
    private static Color GetBGColor(Color mask, int zone)
    {
        Color result = mask;
        result.r *= GetBGTint(zone);
        result.g *= GetBGTint(zone);
        result.b *= GetBGTint(zone);
        return result;
    }
    
    private static float GetBGTint(int zone)
    {
        return (3.0f + zone) / 6.0f;
    }
    
    private static Color GetBrickColor(Color mask, int zone, int zonesCont)
    {
        Color result = mask;
        result.r *= GetBrickTint(zone, zonesCont);
        result.g *= GetBrickTint(zone, zonesCont);
        result.b *= GetBrickTint(zone, zonesCont);
        return result;
    }
    
    private static float GetBrickTint(int zone, int zonesCont)
    {
        return 0.5f + 0.5f * (zone + 1.0f) / zonesCont;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    #region spawn
    
    private void SpawnBrick(float x, float y, Color color)
    {
        GameObject go = (GameObject.Instantiate(BrickPrefab) as GameObject);
        _bricks.Add(go);
        go.transform.parent = transform;
        go.transform.localPosition = new Vector3(x, y, 0);

        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;

        Brick brick = go.GetComponent<Brick>();
        brick.Init(OnBrickDestroyed);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private Action _onBrickDestroyed;
    private Action _onLastBrickDestroyed;
    private List<GameObject> _bricks;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
