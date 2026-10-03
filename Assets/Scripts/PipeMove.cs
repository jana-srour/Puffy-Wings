using UnityEngine;

public class Move : MonoBehaviour
{

    [SerializeField] private float speed = 2f;
    [SerializeField] private float leftBound = -15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftBound)
        {
            gameObject.SetActive(false); // just deactivate instead of destroying
        }
    }
}
