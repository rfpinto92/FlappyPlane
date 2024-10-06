using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Controls : MonoBehaviour
{
    [SerializeField]
    private GameMemory memory;

    public Sprite SoundOFF;
    public Sprite SoundON;
    private Button SoundButton;

    private void OnEnable()
    {
        ChangeSound();
    }

    // Start is called before the first frame update
    void Start()
    {
        #region SoundButton
        SoundButton = GameObject.Find("BtnSound").GetComponent<Button>();
        SoundButton.image.sprite = memory.SoundEnabled ? SoundON : SoundOFF;

        SoundButton.onClick.AddListener(() =>
        {
            memory.SoundEnabled = !memory.SoundEnabled;
            SoundButton.image.sprite = memory.SoundEnabled ? SoundON : SoundOFF;
            ChangeSound();
            memory.ToJSOnFile();
        });
        #endregion

        #region ExitButton
        GameObject.Find("BtnExit").GetComponent<Button>().onClick.AddListener(() =>
        {
            Application.Quit();
        });
        #endregion

        #region BtnScore
        GameObject.Find("BtnScore").GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayGamesPlatform.Activate();
            Social.Active.localUser.Authenticate(ShowLeaderboardUI);

        });
        #endregion


        GameObject.Find("BtnDonate").GetComponent<Button>().onClick.AddListener(() => Application.OpenURL("https://www.paypal.com/donate/?hosted_button_id=F5RMSGX7CCLBU"));

        #region BtnAchievement

        GameObject.Find("BtnAchievement").GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayGamesPlatform.Activate();
            Social.Active.localUser.Authenticate(ShowAchievementsUI);
        });
        #endregion

    }

    private void ChangeSound()
    {

        AudioSource[] Sounds = GameObject.Find("Player").GetComponents<AudioSource>();
        AudioSource[] SoundsMain = GameObject.Find("MainController").GetComponents<AudioSource>();

        if (Sounds != null)
            foreach (var item in Sounds)
            {
                item.mute = !memory.SoundEnabled;
            }

        if (SoundsMain != null)
            foreach (var item in SoundsMain)
            {
                item.mute = !memory.SoundEnabled;
            }

    }

    void ShowLeaderboardUI(bool success)
    {
        if (success)
        {
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_score);
        }
    }
    void ShowAchievementsUI(bool success)
    {
        if (success)
        {
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
    }

}
