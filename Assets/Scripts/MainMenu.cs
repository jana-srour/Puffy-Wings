using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Ads")]
    [SerializeField] private AdsManager adsManager;

    private bool openingAdShown = false;

    private void Start()
    {
        if (adsManager == null)
        {
            adsManager = FindFirstObjectByType<AdsManager>();
        }

        if (adsManager == null)
        {
            Debug.LogWarning(
                "AdsManager could not be found in MainMenu."
            );

            return;
        }

        Debug.Log(
            "MainMenu: AdsManager found."
        );

        InvokeRepeating(
            nameof(TryShowOpeningAd),
            0.2f,
            0.2f
        );
    }

    private void TryShowOpeningAd()
    {
        if (openingAdShown)
            return;

        if (adsManager == null)
        {
            adsManager = FindFirstObjectByType<AdsManager>();

            if (adsManager == null)
                return;
        }

        if (!adsManager.IsAppOpenAdLoaded)
            return;

        openingAdShown = true;

        CancelInvoke(
            nameof(TryShowOpeningAd)
        );

        Debug.Log(
            "MainMenu: Showing App Open ad."
        );

        adsManager.ShowAppOpenAd();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleMusic()
    {
        int current = PlayerPrefs.GetInt("MuteMusic", 0);

        if (current == 0)
            PlayerPrefs.SetInt("MuteMusic", 1);
        else
            PlayerPrefs.SetInt("MuteMusic", 0);
    }

    public void ToggleSFX()
    {
        int current = PlayerPrefs.GetInt("MuteSFX", 0);

        if (current == 0)
            PlayerPrefs.SetInt("MuteSFX", 1);
        else
            PlayerPrefs.SetInt("MuteSFX", 0);
    }
}