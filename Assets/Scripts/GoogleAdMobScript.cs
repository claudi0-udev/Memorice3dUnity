using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdMobScript : MonoBehaviour
{
    private InterstitialAd interstitialOnMainMenu, interstitialOnExit, interstitialOnPlayAgain;
    public void InitializeAdMob()
    {
        MobileAds.Initialize(initStatus => { });
    }

    public void LoadInterstitialMainMenu(string id)
    {
        //if(id == "") id = "ca-app-pub-3940256099942544/1033173712";
        if(id == "") id = "ca-app-pub-4926019248580337/1115067433";
        // Initialize an InterstitialAd.
        this.interstitialOnMainMenu = new InterstitialAd(id);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialOnMainMenu.LoadAd(request);
    }

    public void ShowInterstitialMainMenu()
    {
        if(this.interstitialOnMainMenu.IsLoaded()) this.interstitialOnMainMenu.Show();
    }

    public void LoadInterstitialOnExit(string id)
    {
        if(id == "") id = "ca-app-pub-4926019248580337/5704037377";
        // Initialize an InterstitialAd.
        this.interstitialOnExit = new InterstitialAd(id);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialOnExit.LoadAd(request);
    }

    public void ShowInterstitialOnExit()
    {
        if(this.interstitialOnExit.IsLoaded()) this.interstitialOnExit.Show();
    }

    public void LoadInterstitialOnPlayAgain(string id)
    {
        if(id == "") id = "ca-app-pub-4926019248580337/1465409464";
        // Initialize an InterstitialAd.
        this.interstitialOnPlayAgain = new InterstitialAd(id);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialOnPlayAgain.LoadAd(request);
    }

    public void ShowInterstitialOnPlayAgain()
    {
        if(this.interstitialOnPlayAgain.IsLoaded()) this.interstitialOnPlayAgain.Show();
    }
    
}
