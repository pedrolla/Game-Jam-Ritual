using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public static PositionManager Instance;

    [SerializeField]
    private List<Transform> availablePositions = new List<Transform>();
    private string demon1Position;
    private string demon2Position;
    private string demon3Position;


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

    public Transform AssignPosition(string demonName)
    {
            int index = Random.Range(0, availablePositions.Count);
        Transform selected = availablePositions[index];
        availablePositions.RemoveAt(index);

        if (demonName == "demon1")
        {
            demon1Position = selected.name;
        }
        else if (demonName == "demon2")
        {
            demon2Position = selected.name;
        }
        else if (demonName == "demon3")
        {
            demon3Position = selected.name;
        }

        return selected;
    }

    public void ReleasePosition(Transform position)
    {
         availablePositions.Add(position);
    }

    public string GetDemonPosition(string demon)
    {
        if (demon == "demon1")
        {
            return demon1Position;
        }

        else if (demon == "demon2")
        {
            return demon2Position;
        }

        else if (demon == "demon3")
        {
            return demon3Position;
        }

        return null;
    }
}
