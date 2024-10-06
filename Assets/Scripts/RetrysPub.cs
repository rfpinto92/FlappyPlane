using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class RetrysPub : MonoBehaviour
{
    private AudioSource MainSong;
    private InterstitialAd interstitial;
    private string adUnitId = "ca-app-pub-8796511797043742/5557909127";
    private void Start()
    {
        //Stop Main Song
        MainSong = GameObject.Find("MainController").GetComponent<AudioSource>();
        MainSong.Stop();

        if (interstitial != null)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
                return;
            }
        }
        RequestInterstitial();
    }

    private void RequestInterstitial()
    {

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpening;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public InterstitialAd CreateAndLoadInterstitialAd()
    {
        this.interstitial = new InterstitialAd(adUnitId);

        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        interstitial.LoadAd(request);
        return interstitial;
    }


    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        // MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: " + args.LoadAdError);
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        //   MonoBehaviour.print("HandleAdOpening event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadInterstitialAd();
        Return();
        // MonoBehaviour.print("HandleAdClosed event received");
    }

    private void Return()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Main");
    }

    private void Update()
    {

        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }
}
