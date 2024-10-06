using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InternetTest : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        GameObject.Find("BtnRetryInternet").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("InternetCheck"));
        GameObject.Find("BtnExit").GetComponent<Button>().onClick.AddListener(() => Application.Quit());

        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                SceneManager.LoadScene("Main");
                break;
            default:
                break;
        }
    }
}
