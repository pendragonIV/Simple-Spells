using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class Node : MonoBehaviour
{
    [SerializeField]
    private GameObject[] linkedNodes;
    private List<GameObject> points = new List<GameObject>();
    private List<GameObject> stars = new List<GameObject>();
    [SerializeField]
    private Transform healthBar;
    [SerializeField]
    private int health;

    private void Start()
    {
        SetHealth();
    }

    private void OnMouseDown()
    {
        if (CastRay().CompareTag("Star")
            || GameManager.instance.IsGameWin()
            || GameManager.instance.IsGameLose()
            || !GameManager.instance.IsCanPress())
        {
            return;
        }
        GameManager.instance.DisablePress();
        points.Clear();
        stars.Clear();
        CheckStarAround();
        GameManager.instance.gameScene.PlayCharAnimation();
        GameManager.instance.DecreaseMoveLeft();
        StarsManager(stars);
    }

    #region Health

    public void DecreaseHealth()
    {
        health--;
        SetHealth();
    }

    public void SetHealth()
    {
        foreach (Transform child in healthBar)
        {
            child.gameObject.SetActive(false);
        }
        if (healthBar != null)
        {
            for (int i = 0; i < health; i++)
            {
                healthBar.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    #endregion

    #region Linked Nodes

    public GameObject[] GetLinkedNodes()
    {
        return linkedNodes;
    }

    public bool IsNodeLinked(GameObject node)
    {
        return linkedNodes.Contains(node);
    }

    #endregion

    private void CheckStarAround()
    {
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Vector3Int dir = GridCellManager.instance.GetDirection(direction);
            Vector3Int startPos = GridCellManager.instance.GetObjCell(transform.position);
            GameObject star = StarDetector(startPos, dir);
            if (star != null)
            {
                stars.Add(star);
            }
        }
    }

    private void StarsManager(List<GameObject> stars)
    {
        int totalStars = stars.Count;
        int totalPoints = points.Count;
        Transform pointContainer = this.transform.parent;

        if (totalStars > 0)
        {
            if(totalStars == 2 && pointContainer.childCount != totalStars + 1)
            {
                for (int i = 0; i < totalStars; i++)
                {
                    GameObject star = stars[i];
                    StarMoveBack(star);
                }
                return;
            }
            for (int i = 0; i < totalStars; i++)
            {
                GameObject star = stars[i];
                MoveStar(star);
            }
            for (int i = 0; i < totalPoints; i++)
            {
                PointHealthManager(points[i]);
            }
        }
        else
        {
            GameManager.instance.EnablePress();
        }
    }

    private void PointHealthManager(GameObject point)
    {
        if (point.GetComponent<Node>().GetHealth() <= 0 && !IsStarNearBy(point.transform.position))   
        {
            WaysManager.instance.RemovePoint(point);
            Destroy(point);
        }
        else
        {
            point.GetComponent<Node>().DecreaseHealth();
        }
        //This will be used when the game is finished
        if (WaysManager.instance.GetNumberOfPoints() == WaysManager.instance.GetNumberOfStars() + 1)
        {
            WaysManager.instance.RemovePoint(point);
            Destroy(point);
        }
    }

    #region Movement

    private void StarMoveBack(GameObject star)
    {
        Vector3 defaultPos = star.transform.position;

        star.GetComponent<Collider2D>().enabled = false;
        Vector3 moveTo = transform.position;
        moveTo.z = -5;
        defaultPos.z = -5;
        star.transform.DOMove(transform.position, 0.5f).OnComplete(() =>
        {
            star.transform.DOMove(defaultPos, 0.5f).OnComplete(() =>
            {
                star.GetComponent<Collider2D>().enabled = true;
                GameManager.instance.EnablePress();
            });
            star.transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360);
        });
        star.transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360);
    }

    private void MoveStar(GameObject star)
    {
        star.GetComponent<Collider2D>().enabled = false;
        Vector3 moveTo = transform.position;
        moveTo.z = -5;
        star.transform.DOMove(moveTo, 0.5f).OnComplete(() =>
        {
            star.GetComponent<Collider2D>().enabled = true;
            GameManager.instance.EnablePress();
        });
        star.transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360);
    }

    #endregion

    private GameObject StarDetector(Vector3Int startPos, Vector3Int direction)
    {
        List<GameObject> linkedNodes = new List<GameObject>();

        Vector3Int checkPos = startPos + direction;

        if (!CheckLinkedNode(startPos, checkPos))
        {
            return null;
        }

        Collider2D collider2D = Physics2D.OverlapPoint(GridCellManager.instance.PositonToMove(checkPos));
        while (collider2D != null)
        {
            GameObject node = WaysManager.instance.GetPlacedPoint(checkPos);
            if (node != null)
            {
                linkedNodes.Add(node);
            }

            if (collider2D.gameObject.CompareTag("Node"))
            {
                Vector3Int tempPos = checkPos + direction;

                if (!CheckLinkedNode(checkPos, tempPos))
                {
                    return null;
                }
                checkPos = tempPos;
                collider2D = Physics2D.OverlapPoint(GridCellManager.instance.PositonToMove(checkPos));
            }
            else if (collider2D.gameObject.CompareTag("Star"))
            {
                points.AddRange(linkedNodes);
                return collider2D.gameObject;
            }
        }

        return null;
    }

    #region Checking
    public bool IsStarNearBy(Vector3 positionToCheck)
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(positionToCheck);

        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Vector3Int dir = GridCellManager.instance.GetDirection(direction);
            Vector3Int checkPos = cellPos + dir;
            if(!CheckLinkedNode(cellPos, checkPos))
            {
                continue;
            }
            Collider2D collider2D = Physics2D.OverlapPoint(GridCellManager.instance.PositonToMove(checkPos));
            if (collider2D != null && collider2D.gameObject.CompareTag("Star"))
            {
                return true;
            }

        }
        return false;
    }

    public bool CheckNextNodeHealth(Vector3 positionToCheck)
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(positionToCheck);
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Vector3Int dir = GridCellManager.instance.GetDirection(direction);
            Vector3Int checkPos = cellPos;

            while (CheckLinkedNode(checkPos, checkPos + dir))
            {
                GameObject point = WaysManager.instance.GetPlacedPoint(checkPos + dir);
                if (point != null && point.GetComponent<Node>().health > 0)
                {
                    return true;
                }
                checkPos += dir;
            }
        }
        return false;
    }

    private bool CheckLinkedNode(Vector3Int startPos, Vector3Int checkPos)
    {
        GameObject checkingPoint = WaysManager.instance.GetPlacedPoint(checkPos);
        GameObject startPoint = WaysManager.instance.GetPlacedPoint(startPos);
        if (checkingPoint == null || startPoint == null)
        {
            return false;
        }
        if (startPoint.GetComponent<Node>().IsNodeLinked(checkingPoint)
            || checkingPoint.GetComponent<Node>().IsNodeLinked(startPoint))
        {
            return true;
        }
        return false;
    }

    private GameObject CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }

        return null;
    }
    #endregion
}
