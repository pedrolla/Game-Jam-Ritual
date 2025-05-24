using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DemonManager : MonoBehaviour
{
    [SerializeField]
    private Demon5 demon5Script;
    private List<GameObject> demonList = new List<GameObject>();
    private int demonAmmount;

    private void Start()
    {
        StartCoroutine(Demon5Handler());
        StartCoroutine(NormalDemonsHandler());
    }

    private IEnumerator Demon5Handler()
    {
        while (true)
        {
            float spawnTime = Random.Range(10, 11);
            yield return new WaitForSeconds(spawnTime);
            demon5Script.StartDemon();
        }
    }

    private IEnumerator NormalDemonsHandler()
    {
        while (true)
        {
            float spawnTime = Random.Range(30, 50);
            yield return new WaitForSeconds(spawnTime);
            SpawnNewDemon();
        }
    }

    private void SpawnNewDemon()
    {
        if (demonAmmount == 2)
        {
            StartCoroutine(TryGetDemon());
            return;
        }

        demonAmmount++;

        Transform demonPosition = PositionManager.Instance.AssignPosition();
        GameObject newDemon = GetDemon();   
        newDemon.transform.position = demonPosition.position;
    }

    private GameObject GetDemon()
    {
        int index = Random.Range(0, demonList.Count);
        GameObject selectedDemon = demonList[0];
        demonList.RemoveAt(index);
        return selectedDemon;
    }

    private IEnumerator TryGetDemon()
    {

        yield return new WaitForSeconds(20);
        SpawnNewDemon();
    }
}
