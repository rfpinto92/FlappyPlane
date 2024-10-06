using System;
using UnityEngine;
using GoogleMobileAds.Api;
public class Banner : MonoBehaviour
{
    private BannerView bannerView;

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        this.RequestBanner();
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-8796511797043742/8714593495";

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopLeft);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }
}