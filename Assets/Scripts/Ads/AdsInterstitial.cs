using System.Collections;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Advertisements;
#endif

public class AdsInterstitial : MonoBehaviour
#if UNITY_ANDROID
, IUnityAdsLoadListener, IUnityAdsShowListener
#endif
{
    [SerializeField] private string androidId = "Interstitial_Android";
    private string adUnitId;
    private bool isLoaded;
    private void Start()
    {
#if UNITY_ANDROID
        adUnitId = androidId;
#elif UNITY_EDITOR
            adUnitId = androidId;
#endif
        isLoaded = false;
    }

    public void Initialize()
    {
#if UNITY_ANDROID
        Advertisement.Load(adUnitId, this);
#endif
    }

    public void Show()
    {
#if UNITY_ANDROID
        if (isLoaded)
        {
            Advertisement.Show(adUnitId, this);
        }
#endif
    }

#if UNITY_ANDROID

    public void OnUnityAdsAdLoaded(string placementId)
    {
        isLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Interstitial Error: " + message);
        StartCoroutine(TryToGetInterstitialAfterSeconds(3.0f));
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        StartCoroutine(TryToGetInterstitialAfterSeconds(3.0f));
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        // TODO: mabye pause and resume the game
    }

    IEnumerator TryToGetInterstitialAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Advertisement.Load(adUnitId, this);
    }

#endif
}
