using GoogleMobileAds.Api;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;


public class MainController : MonoBehaviour
{

    [SerializeField]
    private GameMemory memory;

    public GameObject Obstacle;
    private TextMeshProUGUI PontuationTXT;
    private TextMeshProUGUI BestScoreTXT;
    private TextMeshProUGUI LevelTXT;
    private long MaxScore = 0;
    public GameObject ManualGoogleLoginGO;
    public GameObject StartGameGO;

    private void Start()
    {
        MobileAds.Initialize(initialize => { });
    }
    void OnEnable()
    {


        #region Check if are connected to the internet
        if (Application.internetReachability == NetworkReachability.NotReachable)
            SceneManager.LoadScene("InternetCheck");
        #endregion

        //To the Screen dont turn off
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //Autorotation
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;

        #region Google Login
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentiation);
        #endregion

        #region JSON File
        memory = memory.FromJsonFile();
        #endregion

        #region Add Obstacles

        GameObject FirstPosition = GameObject.Find("Obstacle");
        GameObject FinishLine = GameObject.Find("FinishLine");
        GameObject SpawnPosition = GameObject.Find("Spawn Position");

        Vector3 FirstPositionVector = FirstPosition.transform.position;
        Vector3 LastPositionVector = FinishLine.transform.position;

        //Distance Betweem Obstacles
        float z_diference = (SpawnPosition.transform.position.z - FirstPosition.transform.position.z) / 3;

        Vector3 ObstaclePosition = FirstPositionVector;

        while (ObstaclePosition.z + z_diference < LastPositionVector.z - z_diference * 2)
        {
            //Random Position to the obstacle appear down or up - Y AXIS
            float yPos = Random.Range(-51.9f, 9.9f);

            ObstaclePosition = new Vector3(0f, yPos, ObstaclePosition.z + z_diference);
            Instantiate(Obstacle, ObstaclePosition, Quaternion.identity);
        }
        #endregion


        #region Pontuation TXT
        PontuationTXT = GameObject.Find("txtPoints").GetComponent<TextMeshProUGUI>();
        BestScoreTXT = GameObject.Find("txtBestScore").GetComponent<TextMeshProUGUI>();
        LevelTXT = GameObject.Find("txtLevel").GetComponent<TextMeshProUGUI>();
        #endregion

        memory.BestScore = MaxScore;
        memory.ToJSOnFile();
    }

    private void Update()
    {
        LevelTXT.text = "Level " + memory.Level.ToString();
        PontuationTXT.text = memory.Pontuation.ToString();

        if (MaxScore != 0)
            BestScoreTXT.text = "Best Score: " + MaxScore.ToString();
    }

    /// <summary>
    /// Exit APP
    /// </summary>
    private void OnApplicationQuit()
    {
        memory.IsGameOver = true;
        memory.IsToReward = false;
        memory.Pontuation = 0;
        memory.Retrys = 0;
        memory.Level = 1;
        memory.ToJSOnFile();
    }


    /// <summary>
    ///Login
    /// </summary>
    /// <param name="status"></param>
    void ProcessAuthentiation(SignInStatus status)
    {
        if (status==SignInStatus.Success)
        {
            StartGameGO.SetActive(true);
            ManualGoogleLoginGO.SetActive(false);

            PlayGamesPlatform.Activate();
            ILeaderboard il = PlayGamesPlatform.Instance.CreateLeaderboard();
            il.id = GPGSIds.leaderboard_score;
            il.timeScope = TimeScope.AllTime;
            il.userScope = UserScope.Global;
            il.LoadScores((success) =>
            {
                if (il.scores.Length > 0)
                {
                    foreach (IScore score in il.scores)
                    {
                        if (score.userID == Social.localUser.id)
                        {
                            MaxScore = score.value;
                        }
                    }
                }

            });
        }
        else
        {
            StartGameGO.SetActive(false);
            ManualGoogleLoginGO.SetActive(true);
        }
    }
}
