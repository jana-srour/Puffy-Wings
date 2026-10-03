using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 1f; // slow it down, adjust as needed
    private float width;

    void Start()
    {
        // Automatically get the sprite width
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Move left
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

        // Loop the background
        if (transform.position.x <= -width)
        {
            // Jump exactly 2 widths to the right
            transform.position += new Vector3(width * 2f, 0, 0);
        }
    }
}
