using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DemonManager : MonoBehaviour
{
    public static DemonManager Instance;

    [SerializeField]
    private Demon5 demon5Script;
    [SerializeField]
    private List<GameObject> demonList = new List<GameObject>();
    private int demonAmmount;


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
            float spawnTime = Random.Range(10, 11);
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

        GameObject newDemon = GetDemon();
        string demonName = newDemon.name;
        Transform demonPosition = PositionManager.Instance.AssignPosition(demonName);
        newDemon.transform.position = demonPosition.position;
        if (newDemon.TryGetComponent<Demon1>(out var demonScript))
        {
            demonScript.ActivateDemon();
            return;
        }
        if (newDemon.TryGetComponent<Demon2>(out var demon2Script))
        {
            demon2Script.SpawnedDemon();
            return;
        }
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

    public void ReturnDemon(GameObject demon)
    {
        demonList.Add(demon);
    }


}
