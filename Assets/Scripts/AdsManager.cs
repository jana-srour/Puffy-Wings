using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    [Header("AdMob")]
    [SerializeField]
    private string _androidInterstitialAdUnitId =
        "ca-app-pub-9050356805984869/1076709645";

    [SerializeField]
    private string _androidAppOpenAdUnitId =
        "ca-app-pub-9050356805984869/9546401618";

    [Header("Testing")]
    [SerializeField]
    private bool _useTestAd = true;

    // Google's official Android test interstitial ID.
    private const string TestInterstitialAdUnitId =
        "ca-app-pub-3940256099942544/1033173712";

    // Google's official Android test App Open ID.
    private const string TestAppOpenAdUnitId =
        "ca-app-pub-3940256099942544/9257395921";

    private InterstitialAd _interstitialAd;
    private AppOpenAd _appOpenAd;

    private bool _isInitialized = false;
    private bool _isShowingAppOpenAd = false;

    private static AdsManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeAds();
    }

    // =========================================================
    // INITIALIZE ADMOB
    // =========================================================

    private void InitializeAds()
    {
        Debug.Log("Initializing Google Mobile Ads...");

        MobileAds.Initialize((InitializationStatus initializationStatus) =>
        {
            if (initializationStatus == null)
            {
                Debug.LogError(
                    "Google Mobile Ads initialization failed."
                );

                return;
            }

            _isInitialized = true;

            Debug.Log(
                "Google Mobile Ads initialization complete."
            );

            LoadInterstitialAd();
            LoadAppOpenAd();
        });
    }

    // =========================================================
    // INTERSTITIAL ID
    // =========================================================

    private string GetInterstitialAdUnitId()
    {
#if UNITY_ANDROID
        if (_useTestAd)
            return TestInterstitialAdUnitId;

        return _androidInterstitialAdUnitId;

#elif UNITY_IOS
        return "";

#else
        return "";
#endif
    }

    // =========================================================
    // APP OPEN ID
    // =========================================================

    private string GetAppOpenAdUnitId()
    {
#if UNITY_ANDROID
        if (_useTestAd)
            return TestAppOpenAdUnitId;

        return _androidAppOpenAdUnitId;

#elif UNITY_IOS
        return "";

#else
        return "";
#endif
    }

    // =========================================================
    // INTERSTITIAL
    // =========================================================

    public void LoadAd()
    {
        LoadInterstitialAd();
    }

    private void LoadInterstitialAd()
    {
        if (!_isInitialized)
        {
            Debug.LogWarning(
                "Cannot load interstitial because AdMob is not initialized."
            );

            return;
        }

        if (_interstitialAd != null &&
            _interstitialAd.CanShowAd())
        {
            Debug.Log(
                "AdMob Interstitial is already loaded and ready."
            );

            return;
        }

        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        string adUnitId = GetInterstitialAdUnitId();

        if (string.IsNullOrEmpty(adUnitId))
        {
            Debug.LogError(
                "AdMob interstitial ad unit ID is empty."
            );

            return;
        }

        Debug.Log(
            $"Loading AdMob Interstitial: {adUnitId}"
        );

        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(
            adUnitId,
            adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError(
                        $"AdMob Interstitial failed to load: {error}"
                    );

                    _interstitialAd = null;

                    return;
                }

                Debug.Log(
                    "AdMob Interstitial loaded successfully."
                );

                _interstitialAd = ad;

                RegisterInterstitialEvents(ad);
            }
        );
    }

    public bool IsAdLoaded
    {
        get
        {
            return _interstitialAd != null &&
                   _interstitialAd.CanShowAd();
        }
    }

    // =========================================================
    // GAME OVER INTERSTITIAL
    // =========================================================

    public void ShowAd()
    {
        if (!IsAdLoaded)
        {
            Debug.Log(
                "Game Over AdMob interstitial is not ready yet."
            );

            LoadInterstitialAd();

            return;
        }

        Debug.Log(
            "Showing Game Over AdMob interstitial."
        );

        _interstitialAd.Show();
    }

    // =========================================================
    // APP OPEN
    // =========================================================

    private void LoadAppOpenAd()
    {
        if (!_isInitialized)
        {
            Debug.LogWarning(
                "Cannot load App Open ad because AdMob is not initialized."
            );

            return;
        }

        if (_appOpenAd != null &&
            _appOpenAd.CanShowAd())
        {
            Debug.Log(
                "App Open ad is already loaded and ready."
            );

            return;
        }

        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        string adUnitId = GetAppOpenAdUnitId();

        if (string.IsNullOrEmpty(adUnitId))
        {
            Debug.LogError(
                "AdMob App Open ad unit ID is empty."
            );

            return;
        }

        Debug.Log(
            $"Loading AdMob App Open ad: {adUnitId}"
        );

        AdRequest adRequest = new AdRequest();

        AppOpenAd.Load(
            adUnitId,
            adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError(
                        $"AdMob App Open ad failed to load: {error}"
                    );

                    _appOpenAd = null;

                    return;
                }

                Debug.Log(
                    "AdMob App Open ad loaded successfully."
                );

                _appOpenAd = ad;

                RegisterAppOpenEvents(ad);
            }
        );
    }

    public bool IsAppOpenAdLoaded
    {
        get
        {
            return _appOpenAd != null &&
                   _appOpenAd.CanShowAd();
        }
    }

    public void ShowAppOpenAd()
    {
        if (!IsAppOpenAdLoaded)
        {
            Debug.Log(
                "App Open ad is not ready yet."
            );

            LoadAppOpenAd();

            return;
        }

        if (_isShowingAppOpenAd)
            return;

        Debug.Log(
            "Showing AdMob App Open ad."
        );

        _isShowingAppOpenAd = true;

        _appOpenAd.Show();
    }

    // =========================================================
    // INTERSTITIAL EVENTS
    // =========================================================

    private void RegisterInterstitialEvents(InterstitialAd ad)
    {
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log(
                "AdMob Interstitial impression recorded."
            );
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log(
                "AdMob Interstitial clicked."
            );
        };

        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(
                $"AdMob Interstitial paid: {adValue.Value} {adValue.CurrencyCode}"
            );
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log(
                "AdMob Interstitial opened."
            );
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError(
                $"AdMob Interstitial failed to open: {error}"
            );

            _interstitialAd = null;

            LoadInterstitialAd();
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log(
                "AdMob Interstitial closed."
            );

            _interstitialAd = null;

            LoadInterstitialAd();
        };
    }

    // =========================================================
    // APP OPEN EVENTS
    // =========================================================

    private void RegisterAppOpenEvents(AppOpenAd ad)
    {
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log(
                "AdMob App Open impression recorded."
            );
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log(
                "AdMob App Open clicked."
            );
        };

        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(
                $"AdMob App Open paid: {adValue.Value} {adValue.CurrencyCode}"
            );
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log(
                "AdMob App Open opened."
            );
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError(
                $"AdMob App Open failed to open: {error}"
            );

            _appOpenAd = null;
            _isShowingAppOpenAd = false;

            LoadAppOpenAd();
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log(
                "AdMob App Open closed."
            );

            _appOpenAd = null;
            _isShowingAppOpenAd = false;

            LoadAppOpenAd();
        };
    }

    // =========================================================
    // CLEANUP
    // =========================================================

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;

        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }
    }
}