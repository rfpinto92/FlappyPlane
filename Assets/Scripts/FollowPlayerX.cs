using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{
    private GameObject plane;
    private Vector3 offset;

    [SerializeField]
    private GameMemory memory;
    private PlayerControllerX PlayerControllerX;

    // Start is called before the first frame update
    void Start()
    {
        plane = GameObject.Find("Player");
        PlayerControllerX = plane.GetComponent<PlayerControllerX>();

        offset = new Vector3(35.71f, 1f, 0.56f);
    }

    // Update is called once per frame
    void Update()
    {
        if (plane != null)
            if (!PlayerControllerX.isToStopCamera)
                transform.position = plane.transform.position + offset;
    }
}
