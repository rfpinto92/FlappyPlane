using GooglePlayGames;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishMenu : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject RewardPubCanvas;
    public GameObject RetrysPubCanvas;
    private Button DoubleButton;
    private TextMeshProUGUI FinishText;
    private Button ButtonFinish;

    public Sprite RetryImage;
    public Sprite NextLevelImage;

    [SerializeField]
    private GameMemory memory;

    private void OnEnable()
    {
        #region Check if are connected to the internet
        if (Application.internetReachability == NetworkReachability.NotReachable)
            SceneManager.LoadScene("InternetCheck");
        #endregion

        #region Button Find
        ButtonFinish = GameObject.Find("BtnFinish").GetComponent<Button>();
        DoubleButton = GameObject.Find("BtnDouble").GetComponent<Button>();
        FinishText = GameObject.Find("LevelStatus").GetComponent<TextMeshProUGUI>();
        #endregion

        #region Buttons Events

        /// Double Points
        DoubleButton.onClick.AddListener(() =>
       {
           RewardPubCanvas.GetComponent<Rewarded>().memory = memory;
           RewardPubCanvas.GetComponent<Rewarded>().MainCanvas = MainCanvas;
           Instantiate(RewardPubCanvas);
           RewardPubCanvas.SetActive(true);
           MainCanvas.SetActive(false);
       });
        ///Finish Button
        ButtonFinish.onClick.AddListener(() =>
        {
            if (memory.IsGameOver)
                memory.Retrys++;

            if (memory.Retrys >= memory.MaxRetrysToPub)
            {
                memory.Retrys = 0;
                SaveResut();
                Instantiate(RetrysPubCanvas);
                MainCanvas.SetActive(false);
            }
            else
            {
                SaveResut();
                SceneManager.LoadScene("Main");
            }
        });

        #endregion

        #region Text - Double Button
        TextMeshProUGUI text = DoubleButton.GetComponentInChildren<TextMeshProUGUI>();

        if (memory.IsToReward)
        {
            text.text = "DONE! " + memory.Pontuation + " x 2 = " + memory.Pontuation * 2;
            memory.Pontuation = memory.Pontuation * 2;
            DoubleButton.enabled = false;
        }
        else
            text.text = "AD 2x pOINTS";
        #endregion

        #region Text - Finish Title
        if (memory.Pontuation > memory.BestScore && memory.IsGameOver)
        {
            FinishText.text = "New Record!";
        }
        else
        {
            if (memory.IsGameOver)
                FinishText.text = "GAME OVER";
            else
                FinishText.text = "Level Complete";
        }
        #endregion

        #region Text - Button Finish
        text = ButtonFinish.GetComponentInChildren<TextMeshProUGUI>();

        ButtonFinish.image.sprite = memory.IsGameOver ? RetryImage : NextLevelImage;
        text.text = memory.IsGameOver ? "Retry" : "Next";
        #endregion


        SaveResut();
        memory.IsToReward = false;
    }

    internal void DoPostScore(long Score)
    {

        Social.ReportScore(
            Score,
            GPGSIds.leaderboard_score,
            (bool success) =>
            {

                if (!success)
                {
                    PlayGamesPlatform.Activate();
                    Social.Active.localUser.Authenticate((success) =>
                    {
                        if (success)
                            DoPostScore(Score);
                        else
                            SceneManager.LoadScene("InternetCheck");
                    });
                }
                else return;
            });
    }

    private void UnlockAchievement(long Points, int level, bool doublePoints = false)
    {

        if (doublePoints)
        {
            Social.ReportProgress(GPGSIds.achievement_double_points, 100.0f, (bool success) =>
            {
            });
        }

        switch (level)
        {
            case 2:
                //First Level Completed
                Social.ReportProgress(GPGSIds.achievement_first_level, 100.0f, (bool success) =>
                {
                });
                break;
            case 6:
                //5 Levels Completed
                Social.ReportProgress(GPGSIds.achievement_5_levels, 100.0f, (bool success) =>
                {
                });
                break;
            case 11:
                //5 Levels Completed
                Social.ReportProgress(GPGSIds.achievement_10_levels, 100.0f, (bool success) =>
                {
                });
                break;
            case 21:
                //5 Levels Completed
                Social.ReportProgress(GPGSIds.achievement_20_levels, 100.0f, (bool success) =>
                {
                });
                break;

            default:
                break;
        }


        if (Points >= 5)
        {
            Social.ReportProgress(GPGSIds.achievement_first_5_points, 100.0f, (bool success) =>
            {
            });
            return;
        }
        if (Points >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_100_points, 100.0f, (bool success) =>
            {
            });
            return;
        }
        if (Points >= 1000)
        {
            Social.ReportProgress(GPGSIds.achievement_1000_points, 100.0f, (bool success) =>
            {
            });
            return;
        }
        if (Points >= 9999999)
        {
            Social.ReportProgress(GPGSIds.achievement_9999999_points, 100.0f, (bool success) =>
            {
            });
            return;
        }
    }

    private void SaveResut()
    {
        #region Save Result
        DoPostScore(memory.Pontuation);
        UnlockAchievement(memory.Pontuation, memory.Level, memory.IsToReward);
        memory.ToJSOnFile();
        #endregion
    }

}
