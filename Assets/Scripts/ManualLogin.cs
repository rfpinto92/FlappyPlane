using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualLogin : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject StartMenu;
    


    void OnEnable()
    {
        GameObject.Find("BtnGoogleLogin").GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentiation);
        });
    }



    void ProcessAuthentiation(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            PlayGamesPlatform.Activate();
            StartMenu.SetActive(true);
            gameObject.SetActive(false);

        }
    }

}
