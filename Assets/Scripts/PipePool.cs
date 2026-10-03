using System.Collections.Generic;
using UnityEngine;

public class PipePool : MonoBehaviour
{
    public GameObject pipePrefab;
    public int poolSize = 5;

    private List<GameObject> pipes;
    private float startX = 15f;
    private float heightRange = 2f;

    private float timer = 0f;
    public float spawnRateInSeconds = 2f;

    void Start()
    {
        // Create pool
        pipes = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject pipe = Instantiate(pipePrefab, new Vector3(-1000, -1000, 0), Quaternion.identity);
            pipe.SetActive(false);
            pipes.Add(pipe);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRateInSeconds)
        {
            timer = 0f;
            SpawnPipe();
        }
    }

    void SpawnPipe()
    {
        GameObject pipe = GetInactivePipe();
        if (pipe != null)
        {
            float y = Random.Range(-heightRange, heightRange);
            pipe.transform.position = new Vector3(startX, y, 0);
            pipe.SetActive(true);
        }
    }

    GameObject GetInactivePipe()
    {
        foreach (var pipe in pipes)
        {
            if (!pipe.activeInHierarchy)
                return pipe;
        }
        return null; // all pipes are active
    }
}
