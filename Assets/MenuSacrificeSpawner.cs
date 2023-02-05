using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuSacrificeSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<GameObject> sacrifices;
    [SerializeField] private float spawnCd;

    private float spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0;
        SpawnSacrifice();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.fixedUnscaledTime > spawnTimer)
        {
            SpawnSacrifice();
        }
    }

    private void SpawnSacrifice()
    {
        spawnTimer += spawnCd;
        var spawnedObj = Instantiate(sacrifices[Random.Range(0, sacrifices.Count)],
            spawnPoints[Random.Range(0, spawnPoints.Count)].position, quaternion.identity).GetComponent<SacrificeController>();
        var rb = spawnedObj.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        spawnedObj.FallIntoHole();
    }
}
