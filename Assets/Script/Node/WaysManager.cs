using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaysManager : MonoBehaviour
{
    public static WaysManager instance;

    [SerializeField]
    private GameObject wayPrefab;
    [SerializeField]
    private Transform waysContainer;
    [SerializeField]
    private Transform pointsContainer;
    [SerializeField]
    private Transform starsContainer;

    private Dictionary<Vector3Int, GameObject> placedPoint;


    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        placedPoint = new Dictionary<Vector3Int, GameObject>();
    }

    public void SetContainer(Transform pointsContainer, Transform waysContainer, Transform starsContainer)
    {
        this.pointsContainer = pointsContainer;
        this.waysContainer = waysContainer;
        this.starsContainer = starsContainer;
        SetUp();
    }

    public void SetUp()
    {
        foreach (Transform child in pointsContainer)
        {
            child.GetComponent<CenterCell>().SetCenter();
        }

        foreach (Transform child in pointsContainer)
        {
            Node node = child.GetComponent<Node>();
            foreach (GameObject linkedNode in node.GetLinkedNodes())
            {
                SpawnWay(child.position, linkedNode.transform.position);
            }
        }
    }

    public void CheckStar(GameObject star)
    {
        if(pointsContainer.childCount <= 3)
        {
            star.GetComponent<Star>().LightStar();
        }
    }

    public int GetNumberOfStars()
    {
        return starsContainer.childCount;
    }

    public void SpawnWay(Vector2 spawnPosition, Vector2 linkedNodePosition)
    {
        GameObject newWay = Instantiate(wayPrefab, waysContainer); 
        newWay.transform.position = spawnPosition;
        newWay.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(linkedNodePosition.y - spawnPosition.y, linkedNodePosition.x - spawnPosition.x) * Mathf.Rad2Deg + 90);
        float distance = Vector2.Distance(spawnPosition, linkedNodePosition);
        float scale = distance / 2.15f;
        newWay.transform.localScale = new Vector3(1.5f, scale, 1);

        GameObject firstNode = placedPoint.ContainsKey(GridCellManager.instance.GetObjCell(spawnPosition)) ? placedPoint[GridCellManager.instance.GetObjCell(spawnPosition)] : null;
        GameObject secondNode = placedPoint.ContainsKey(GridCellManager.instance.GetObjCell(linkedNodePosition)) ? placedPoint[GridCellManager.instance.GetObjCell(linkedNodePosition)] : null;

        if(firstNode != null && secondNode != null)
        {
            newWay.GetComponent<Way>().SetNodes(firstNode, secondNode);
        }
    }

    public int GetNumberOfPoints()
    {
        return pointsContainer.childCount;
    }

    public void AddPlacedPoint(Vector3Int cellPos, GameObject point)
    {
        if (placedPoint.ContainsKey(cellPos))
        {
            placedPoint.Remove(cellPos);
        }
        placedPoint.Add(cellPos, point);
    }

    public void RemovePoint(GameObject point)
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(point.transform.position);
        placedPoint.Remove(cellPos);
    }

    public bool IsPlacedPos(Vector3Int cellPos)
    {
        return placedPoint.ContainsKey(cellPos);
    }

    public bool IsPlacedPoint(GameObject point)
    {
        return placedPoint.ContainsValue(point);
    }

    public GameObject GetPlacedPoint(Vector3Int cellPos)
    {
        return placedPoint.ContainsKey(cellPos) ? placedPoint[cellPos] : null;
    }

    public void DestroyPoints()
    {
        if(pointsContainer.gameObject.activeSelf)
        {
            pointsContainer.gameObject.SetActive(false);
        }
    }
}
