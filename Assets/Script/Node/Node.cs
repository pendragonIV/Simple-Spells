using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private GameObject[] linkedNodes;
    [SerializeField]
    List<GameObject> points = new List<GameObject>();

    public GameObject[] GetLinkedNodes()
    {
        return linkedNodes;
    }

    public bool IsNodeLinked(GameObject node)
    {
        return linkedNodes.Contains(node);
    }

    private void OnMouseDown()
    {
        points.Clear();
        List<GameObject> stars = new List<GameObject>();

        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Vector3Int dir = GridCellManager.instance.GetDirection(direction);
            Vector3Int startPos = GridCellManager.instance.GetObjCell(transform.position);
            GameObject star = StarDetector(startPos, dir);
            if(star != null)
            {
                stars.Add(star);
            }
        }

        StarsManager(stars);
    }

    private void StarsManager(List<GameObject> stars)
    {
        int totalStars = stars.Count;
        int totalPoints = points.Count;
        Transform pointContainer = this.transform.parent;

        if (totalStars > 0)
        {
            if(totalStars == 2 && pointContainer.childCount != 1)
            {
                return;
            }

            for (int i = 0; i < totalStars; i++)
            {
                GameObject star = stars[i];
                star.GetComponent<Collider2D>().enabled = false;
                star.transform.DOMove(transform.position, 0.5f).OnComplete(() =>
                {
                    star.GetComponent<Collider2D>().enabled = true;
                });
            }
            for (int i = 0; i < totalPoints; i++)
            {
                if (!IsStarNearBy(points[i].transform.position))
                {
                    WaysManager.instance.RemovePoint(points[i]);
                    Destroy(points[i]);
                }
            }
        }

    }

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


    public bool IsStarNearBy(Vector3 positionToCheck)
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(positionToCheck);
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Vector3Int dir = GridCellManager.instance.GetDirection(direction);
            Vector3Int checkPos = cellPos + dir;

            if(CheckLinkedNode(cellPos, checkPos))
            {
                Collider2D collider2D = Physics2D.OverlapPoint(GridCellManager.instance.PositonToMove(checkPos));
                if (collider2D != null && collider2D.gameObject.CompareTag("Star"))
                {
                    GameObject checkingPoint = WaysManager.instance.GetPlacedPoint(checkPos);
                    GameObject startPoint = WaysManager.instance.GetPlacedPoint(cellPos);
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckLinkedNode(Vector3Int startPos, Vector3Int checkPos)
    {
        GameObject checkingPoint = WaysManager.instance.GetPlacedPoint(checkPos);
        GameObject startPoint = WaysManager.instance.GetPlacedPoint(startPos);
        if(checkingPoint == null || startPoint == null)
        {
            return false;
        }

        if (linkedNodes.Contains(checkingPoint) 
            || checkingPoint.GetComponent<Node>().IsNodeLinked(startPoint))
        {
            return true;
        }
        return false;
    }

}
