using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Way : MonoBehaviour
{
    private GameObject firstNode;
    private GameObject secondNode;

    private bool isStartDetecting = false;

    public void SetNodes(GameObject firstNode, GameObject secondNode)
    {
        this.firstNode = firstNode;
        this.secondNode = secondNode;
        if(firstNode != null && secondNode != null)
        {
            isStartDetecting = true;
        }
    }

    private void Update()
    {
        if(isStartDetecting)
        {
            if(firstNode == null || secondNode == null)
            {
                transform.DOScale(0, .3f).SetEase(Ease.InOutQuad).OnComplete(() => Destroy(gameObject));
            }
        }
    }
}
