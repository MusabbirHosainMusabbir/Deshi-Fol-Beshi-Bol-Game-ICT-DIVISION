using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [HideInInspector]
    public Vector3 StartingPosition;


    private const float minCameraX = 0;
    private const float maxCameraX = 13;
    [HideInInspector]
    public bool IsFollowing;
    [HideInInspector]
    public Transform GuiltiToFollow;

    // Use this for initialization
    void Start()
    {
        StartingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFollowing)
        {
            if (GuiltiToFollow != null) //Guilti will be destroyed if it goes out of the scene
            {
                var GuiltiPosition = GuiltiToFollow.transform.position;
                float x = Mathf.Clamp(GuiltiPosition.x, minCameraX, maxCameraX);
                //camera follows Guilti's x position
                transform.position = new Vector3(x, StartingPosition.y, StartingPosition.z);
            }
            else
                IsFollowing = false;
        }
    }

   
}
