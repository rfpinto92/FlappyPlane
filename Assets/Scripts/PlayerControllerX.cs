using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerX : MonoBehaviour
{
    [SerializeField]
    private GameMemory memory;

    public float speed;
    public float rotationSpeed;
    private float actualRotationSpeed;
    public float SpinSpeed;
    public float verticalInput;

    private AudioSource[] Sounds;

    private bool Started;
    public bool isToStopCamera;
    private bool GameOver;

    public GameObject FinishMenu;
    public GameObject BtnPlayGO;

    // Start is called before the first frame update

    private void OnEnable()
    {
        FinishMenu.SetActive(false);
        isToStopCamera = false;
        Started = false;

        actualRotationSpeed = rotationSpeed;
        Sounds = gameObject.GetComponents<AudioSource>();


        BtnPlayGO.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (memory.IsGameOver)
            {
                memory.Level = 1;
                memory.Pontuation = 0;
            }
            else
                memory.Level++;
            Started = true;

            #region Set Speed by Level

            speed = speed + ((float)memory.Level / 10.0f);
            #endregion

            memory.IsGameOver = false;

            Sounds[3].Play();
            Sounds[3].mute = !memory.SoundEnabled;

            Destroy(GameObject.Find("BtnScore"));
            Destroy(GameObject.Find("StartGame"));
            Destroy(GameObject.Find("BtnDonate"));
            Destroy(GameObject.Find("BtnAchievement"));
        });
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Started)
            return;

        verticalInput = (Input.acceleration.x) * 2;


        // get the user's vertical input
        if (verticalInput == 0)
            verticalInput = Input.GetAxis("Vertical");


        // move the plane forward at a constant rate
        if (!GameOver)
        {
            transform.Translate(Vector3.forward * speed);

            // tilt the plane up/down based on up/down arrow keys
            transform.Rotate(Vector3.right * actualRotationSpeed * Time.deltaTime * verticalInput);
        }
        else
        {
            Sounds[3].Stop();
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            // tilt the plane up/down based on up/down arrow keys
            transform.Rotate(Vector3.right * actualRotationSpeed * Time.deltaTime * verticalInput);
        }

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Obstacle")
        {
            memory.IsGameOver = GameOver = true;
            isToStopCamera = true;
            Sounds[1].Play();
            Sounds[0].mute = true;
            Sounds[2].mute = true;

            #region Destroy unused Objects
            GameObject[] allPass = GameObject.FindGameObjectsWithTag("Pass");
            Destroy(GameObject.FindGameObjectWithTag("Finish"));

            foreach (var item in allPass)
            {
                Destroy(item);
            }
            #endregion

            FinishMenu.SetActive(true);
            return;
        }
        else
        {
            if (other.gameObject.tag == "Pass")
            {
                Sounds[0].Play();
                memory.Pontuation++;
                return;
            }

            if (other.gameObject.tag == "Finish")
            {
                Sounds[3].Stop();
                memory.IsGameOver = GameOver = false;
                isToStopCamera = true;
                Sounds[2].Play();
                FinishMenu.SetActive(true);
            }
        }
    }
}
