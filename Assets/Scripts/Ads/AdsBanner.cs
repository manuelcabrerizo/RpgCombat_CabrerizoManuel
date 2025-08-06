using System.Collections;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Advertisements;
#endif

public class AdsBanner : MonoBehaviour
{
    [SerializeField] private string androidId = "Banner_Android";
    private string adUnitId;
    private void Start()
    {
#if UNITY_ANDROID
        adUnitId = androidId;
#elif UNITY_EDITOR
            adUnitId = androidId;
#endif
    }

    public void Show()
    {
#if UNITY_ANDROID
        BannerLoadOptions options = new BannerLoadOptions();
        options.errorCallback = OnError;
        options.loadCallback = OnLoad;

        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Load(adUnitId, options);
#endif
    }

#if UNITY_ANDROID
    private void OnLoad()
    {
        Debug.Log("Showing Banner");
        Advertisement.Banner.Show(adUnitId);
    }

    private void OnError(string message)
    {
        Debug.Log("Banner Error: " + message);
        StartCoroutine(TryToGetBannerAfterSeconds(3.0f));
    }

    IEnumerator TryToGetBannerAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        BannerLoadOptions options = new BannerLoadOptions();
        options.errorCallback = OnError;
        options.loadCallback = OnLoad;
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Load(adUnitId, options);
    }
#endif

}
