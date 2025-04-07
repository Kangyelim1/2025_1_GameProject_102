using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefabs;
    public GameObject MissilePrfabs;

    [Header("스폰 타이밍 설정")]
    public float minSpawnlnterval = 0.5f;
    public float maxSpawnlnterval = 2.0f;

    [Header("동전 스폰 확률 설정")]
    [Range(0, 100)]
    public int coinSpawnChance = 50;

    public float timer = 0.0f;
    public float nextSpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        SetNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= nextSpawnTime)
        {
            SpawnObject();
            timer = 0.0f;
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(maxSpawnlnterval, minSpawnlnterval);
    }

    void SpawnObject()
    {
        Transform spawnTransfrom = transform;

        int randomValue = Random.Range(0, 100);
        if(randomValue < coinSpawnChance)
        {
            Instantiate(coinPrefabs, spawnTransfrom.position, spawnTransfrom.rotation);
        }
        else
        {
            Instantiate(MissilePrfabs, spawnTransfrom.position, spawnTransfrom.rotation);
        }
    }
}
