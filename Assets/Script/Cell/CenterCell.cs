using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCell : MonoBehaviour
{
    private void Start()
    {
        SetCenter();
    }
    public void SetCenter()
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(transform.position);
        Vector3 moveTo = GridCellManager.instance.PositonToMove(cellPos);
        if (this.gameObject.CompareTag("Star"))
        {
            moveTo.z = -5;
            this.transform.position = moveTo;
        }
        else
        {
            this.transform.position = moveTo;
            if (WaysManager.instance.IsPlacedPoint(this.gameObject))
            {
                WaysManager.instance.RemovePoint(this.gameObject);
            }
            WaysManager.instance.AddPlacedPoint(cellPos, this.gameObject);
        }

    }
}
