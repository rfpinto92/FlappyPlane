using System;
using UnityEngine;
using GoogleMobileAds.Api;
public class Rewarded : MonoBehaviour
{
    private RewardedAd rewardedAd;

    [SerializeField]
    public GameMemory memory;

    public GameObject MainCanvas;

    private AudioSource MainSong;
    private string adUnitId = "ca-app-pub-8796511797043742/8375644154";

    private void Start()
    {
        //Stop Main Song
        MainSong = GameObject.Find("MainController").GetComponent<AudioSource>();
        MainSong.Stop();

        memory.IsToReward = false;

        if (rewardedAd != null)
        {
            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
                return;
            }
        }

        RequestRewardedVideo();

    }

    public void RequestRewardedVideo()
    {
        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Return();
        // MonoBehaviour.print(
        //     "HandleRewardedAdFailedToLoad event received with message: "
        //                      + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.AdError);
    }



    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadRewardedAd();
        Return();
        // MonoBehaviour.print("HandleRewardedAdClosed event received");
    }


    public RewardedAd CreateAndLoadRewardedAd()
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
        return rewardedAd;
    }


    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;

        memory.IsToReward = true;
        Return();
       //MonoBehaviour.print(
       //    "HandleRewardedAdRewarded event received for "
       //                + amount.ToString() + " " + type);
    }

    private void Return()
    {
        //Continue Main Song
        MainSong.Play();

        MainCanvas.SetActive(true);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (rewardedAd.IsLoaded())
            rewardedAd.Show();
    }
}
