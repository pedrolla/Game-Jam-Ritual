using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public static PositionManager Instance;

    [SerializeField]
    private Transform[] demonPositions;
    private List<Transform> availablePositions = new List<Transform>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform AssignPosition()
    {
        int index = Random.Range(0, availablePositions.Count);
        Transform selected = availablePositions[index];
        availablePositions.RemoveAt(index); 
        return selected;
    }

    public void ReleasePosition(Transform position)
    {
         availablePositions.Add(position);
    }
}
