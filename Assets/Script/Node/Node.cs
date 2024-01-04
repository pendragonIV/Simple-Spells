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
        //Debug.Log("Node clicked");
        points.Clear();
        List<GameObject> nodes = new List<GameObject>();

        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Vector3Int dir = GridCellManager.instance.GetDirection(direction);
            Vector3Int startPos = GridCellManager.instance.GetObjCell(transform.position);
            GameObject node = NodeDetector(startPos, dir);
            if(node != null)
            {
                nodes.Add(node);
            }
        }

        if(nodes.Count > 0)
        {
            foreach (GameObject node in nodes)
            {
                node.transform.DOMove(transform.position, 0.5f);
            }
            int totalPoints = points.Count;
            for(int i = 0; i < totalPoints; i++)
            {
                Destroy(points[i]);
            }
        }
    }


    private GameObject NodeDetector(Vector3Int startPos, Vector3Int direction)
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
