using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Advertisements;
#endif

public class AdsManager : MonoBehaviour
#if UNITY_ANDROID
, IUnityAdsInitializationListener
#endif
{
    [SerializeField] private string androidGameId = "5917824";
    [SerializeField] private bool testMode = true;
    private string gameId;

    [SerializeField] private AdsBanner banner;
    [SerializeField] private AdsInterstitial interstitial;

    private bool isReady = false;

#if UNITY_ANDROID
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        interstitial.Initialize();
        banner.Show();
        isReady = true;
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
#endif

    private void Awake()
    {
#if UNITY_ANDROID
        gameId = androidGameId;
#elif UNITY_EDITOR
            gameId = androidGameId;
#endif


#if UNITY_ANDROID
        isReady = false;
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
#endif
    }

    public bool IsReady()
    {
        return isReady;
    }

    public void ShowBanner()
    {
        banner.Show();
    }

    public void ShowInterstitial()
    {
        interstitial.Show();
    }
}
