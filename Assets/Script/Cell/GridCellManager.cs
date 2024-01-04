using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Direction
{
    Left,
    Right,
    Up,
    Down,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private List<Vector3> locations = new List<Vector3>();

    private void Start()
    {
        for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
        {
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);

                Vector3 location = tileMap.GetCellCenterWorld(localLocation);
                if (tileMap.HasTile(localLocation))
                {
                    locations.Add(location);
                }
            }
        }
    }

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void SetTileMap(Tilemap tilemap)
    {
        this.tileMap = tilemap;
    }
    public bool IsPlaceableArea(Vector3Int mouseCellPos)
    {
        if (tileMap.GetTile(mouseCellPos) == null)
        {
            return false;
        }
        return true;
    }

    #region Getters

    public List<Vector3> GetCellsPosition()
    {
        return locations;
    }

    public Vector3Int GetObjCell(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 PositonToMove(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }

    public Vector3Int GetDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return Vector3Int.left;
            case Direction.Right:
                return Vector3Int.right;
            case Direction.Up:
                return Vector3Int.up;
            case Direction.Down:
                return Vector3Int.down;
            case Direction.BottomLeft:
                return Vector3Int.left + Vector3Int.down;
            case Direction.BottomRight:
                return Vector3Int.right + Vector3Int.down;
            case Direction.TopLeft:
                return Vector3Int.left + Vector3Int.up;
            case Direction.TopRight:
                return Vector3Int.right + Vector3Int.up;
            default:
                return Vector3Int.zero;

        }
    }

    #endregion
}