using UnityEngine;

public class Bird : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] public float forceUp;
    [SerializeField] private AudioClip jumpSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem jumpParticle;

    private Animator animator;
    private AudioSource audioSource;
    private bool jumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool tap = false;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            tap = true;
        }

        if (tap)
        {
            if (!GameManager.Instance.HasStarted)
            {
                GameManager.Instance.StartGame();
                rb.simulated = true;
                return;
            }

            jumping = true;

            if (PlayerPrefs.GetInt("MuteSFX", 0) == 0)
            {
                audioSource.PlayOneShot(jumpSound);
            }

            jumpParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            jumpParticle.Play(true);
        }
    }

    private void FixedUpdate()
    {
        if (jumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * forceUp, ForceMode2D.Impulse);
            jumping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (animator != null)
            animator.SetBool("isHit", true);

        GameManager.Instance.GameOver();
    }
}
