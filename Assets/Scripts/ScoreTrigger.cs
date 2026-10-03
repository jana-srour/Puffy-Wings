using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<Bird>() != null)
        {

            BirdAnimation birdAnimation = other.GetComponent<BirdAnimation>();
            birdAnimation.PlayPickupAnimation();

            GameManager.Instance.AddScore(1);

            Destroy(gameObject);

        }
    }

}
