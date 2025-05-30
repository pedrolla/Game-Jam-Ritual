using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DemonManager : MonoBehaviour
{
    public static DemonManager Instance;

    [SerializeField]
    private Demon5 demon5Script;
    [SerializeField]
    private List<GameObject> demonList = new List<GameObject>();
    private List<GameObject> activateDemons = new List<GameObject>();
    [SerializeField]
    private int demonAmmount;
    [SerializeField]
    private int maxDemonAmmount = 2;
    [SerializeField]
    private float demonSpawnTime;
    [SerializeField]
    private float newDemonSpawnTime;
    [SerializeField]
    private float demon5SpawnTime;
    [SerializeField]
    private bool isDemon5;


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
        if (isDemon5)
        {
            StartCoroutine(Demon5Handler());
        }
        StartCoroutine(NormalDemonsHandler());
    }

    private IEnumerator Demon5Handler()
    {
            yield return new WaitForSeconds(demon5SpawnTime);
            demon5Script.StartDemon();
            SoundManager.Instance.PlayMonster5();
    }

    public void ResetDemon5()
    {
        StartCoroutine(Demon5Handler());
    }

    private IEnumerator NormalDemonsHandler()
    {
        while (true)
        {
            yield return new WaitForSeconds(demonSpawnTime);
            SpawnNewDemon();
        }
    }

    private void SpawnNewDemon()
    {
        if (demonAmmount == maxDemonAmmount)
        {
            StartCoroutine(TryGetDemon());
            return;
        }

        GameObject newDemon = GetDemon();
        string demonName = newDemon.name;
        Transform demonPosition = PositionManager.Instance.AssignPosition(demonName);
        newDemon.transform.SetParent(demonPosition);
        newDemon.transform.localPosition = Vector2.zero;
        if (newDemon.TryGetComponent<Demon1>(out var demonScript))
        {
            string positionName = demonPosition.name;
            demonScript.ActivateDemon(positionName);
            return;
        }
        if (newDemon.TryGetComponent<Demon2> (out var demonScript2))
        {
            string positionName = demonPosition.name;
            SoundManager.Instance.PlayMonster2(positionName);
            demonScript2.SpawnedDemon(positionName);
            return;
        }
        if (newDemon.TryGetComponent<Demon3>(out var demonScript3))
        {
            string positionName = demonPosition.name;
            SoundManager.Instance.PlayMonster3(positionName);
            demonScript3.ActivateDemon(positionName);
            return;
        }
    }

    private GameObject GetDemon()
    {
        demonAmmount++;
        int index = Random.Range(0, demonList.Count);
        GameObject selectedDemon = demonList[index];
        demonList.RemoveAt(index);
        return selectedDemon;
    }

    private IEnumerator TryGetDemon()
    {

        yield return new WaitForSeconds(newDemonSpawnTime);
        SpawnNewDemon();
    }

    public void ReturnDemon(GameObject demon)
    {
        demonAmmount--;
        demonList.Add(demon);
    }

    public void AddActiveDemons(GameObject demon)
    {
        activateDemons.Add(demon);
    }

    public void RemoveActivateDemons(GameObject demon)
    {
        activateDemons.Remove(demon);
    }

    public void DeleteDemons()
    {
        foreach (var demon in activateDemons)
        {
            Destroy(demon);
        }

        StopAllCoroutines();
    }
}
