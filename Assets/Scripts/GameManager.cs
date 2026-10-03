using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Audio")]
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private MusicPlayer musicPlayer;

    [Header("Audio Settings")]
    [SerializeField] private bool muteMusic = false;
    [SerializeField] private bool muteSFX = false;


    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    [Header("Ads")]
    [SerializeField] private AdsManager adsManager;

    private int score = 0;
    private bool hasStarted = false;

    void Start()
    {
        Time.timeScale = 0f;

        muteMusic = PlayerPrefs.GetInt("MuteMusic", 0) == 1; // 1 = muted
        muteSFX = PlayerPrefs.GetInt("MuteSFX", 0) == 1;     // 1 = muted

        if (musicPlayer != null)
            musicPlayer.SetMute(muteMusic);

        if (audioSource != null)
            audioSource.mute = muteSFX;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }
    }

    void Update()
    {
        if (!hasStarted)
        {
            bool tap = false;

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                tap = true;
            }

            if (tap)
            {
                StartGame();
            }
        }
    }

    public void SetMusicMute(bool mute)
    {
        muteMusic = mute;

        if (musicPlayer != null)
            musicPlayer.SetMute(mute);
    }
    public void SetSFXMute(bool mute)
    {
        muteSFX = mute;

        if (audioSource != null)
            audioSource.mute = mute;
    }

    public void StartGame()
    {
        hasStarted = true;
        Time.timeScale = 1f;
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
        {
            if (!muteSFX)
                audioSource.PlayOneShot(coinSound);
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver called");

        if (musicPlayer != null && !muteMusic)
            musicPlayer.StopMusic();

        if (!muteSFX)
            audioSource.PlayOneShot(gameOverSound);

        gameOverScoreText.text = "Score: " + score.ToString();
        gameOverPanel.SetActive(true);

        if (adsManager != null)
        {
            adsManager.ShowAd();
        }

        Time.timeScale = 0f;
    }


    public void Restart()
    {
        Time.timeScale = 1f;

        gameOverPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public bool HasStarted => hasStarted;
}
