using UnityEngine;

public class PipeManager : MonoBehaviour
{

    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private float spawnRateInSeconds = 2f;
    [SerializeField] private float heightRange = 2f;
    [SerializeField] private float startX = 15f;

    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRateInSeconds)
        {
            timer = 0f;
            float y = Random.Range(-heightRange, heightRange);
            Instantiate(pipePrefab, new Vector3(startX, y, 0), Quaternion.identity);
        }
    }
}
