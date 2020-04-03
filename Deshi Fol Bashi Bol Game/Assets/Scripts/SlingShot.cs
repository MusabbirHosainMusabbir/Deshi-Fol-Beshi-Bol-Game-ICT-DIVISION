using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;

public class SlingShot : MonoBehaviour
{

    //a vector that points in the middle between left and right parts of the slingshot
    private Vector3 SlingshotMiddleVector;

    [HideInInspector]
    public SlingshotState slingshotState;

    //the left and right parts of the slingshot
    public Transform LeftSlingshotOrigin, RightSlingshotOrigin;

    //two line renderers to simulate the "strings" of the slingshot
    public LineRenderer SlingshotLineRenderer1;
    public LineRenderer SlingshotLineRenderer2;
    
    //this linerenderer will draw the projected trajectory of the thrown Guilti
    public LineRenderer TrajectoryLineRenderer;
    
    [HideInInspector]
    //the Guilti to throw
    public GameObject GuiltiToThrow;

    //the position of the Guilti tied to the slingshot
    public Transform GuiltiWaitPosition;

    public float ThrowSpeed;

    [HideInInspector]
    public float TimeSinceThrown;

    // Use this for initialization
    void Start()
    {
      
        TrajectoryLineRenderer.sortingLayerName = "Foreground";

        slingshotState = SlingshotState.Idle;
        SlingshotLineRenderer1.SetPosition(0, LeftSlingshotOrigin.position);
        SlingshotLineRenderer2.SetPosition(0, RightSlingshotOrigin.position);

        //pointing at the middle position of the two vectors
        SlingshotMiddleVector = new Vector3((LeftSlingshotOrigin.position.x + RightSlingshotOrigin.position.x) / 2,
            (LeftSlingshotOrigin.position.y + RightSlingshotOrigin.position.y) / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (slingshotState)
        {
            case SlingshotState.Idle:
                //fix Guilti's position
                InitializeGuilti();
                //display the slingshot "strings"
                DisplaySlingshotLineRenderers();
                if (Input.GetMouseButtonDown(0))
                {
                    //get the point on screen user has tapped
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //if user has tapped onto the Guilti
                    if (GuiltiToThrow.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(location))
                    {
                        slingshotState = SlingshotState.UserPulling;
                    }
                }
                break;
            case SlingshotState.UserPulling:
                DisplaySlingshotLineRenderers();

                if (Input.GetMouseButton(0))
                {
                    //get where user is tapping
                    Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    location.z = 0;
                    //we will let the user pull the Guilti up to a maximum distance
                    if (Vector3.Distance(location, SlingshotMiddleVector) > 1.5f)
                    {
                        //basic vector maths :)
                        var maxPosition = (location - SlingshotMiddleVector).normalized * 1.5f + SlingshotMiddleVector;
                        GuiltiToThrow.transform.position = maxPosition;
                    }
                    else
                    {
                        GuiltiToThrow.transform.position = location;
                    }
                    float distance = Vector3.Distance(SlingshotMiddleVector, GuiltiToThrow.transform.position);
                    //display projected trajectory based on the distance
                    DisplayTrajectoryLineRenderer2(distance);
                }
                else//user has removed the tap 
                {
                    SetTrajectoryLineRenderesActive(false);
                    //throw the Guilti!!!
                    TimeSinceThrown = Time.time;
                    float distance = Vector3.Distance(SlingshotMiddleVector, GuiltiToThrow.transform.position);
                    if (distance > 1)
                    {
                        SetSlingshotLineRenderersActive(false);
                        slingshotState = SlingshotState.GuiltiFlying;
                        ThrowGuilti(distance);
                    }
                    else//not pulled long enough, so reinitiate it
                    {
                        //distance/10 was found with trial and error :)
                        //animate the Guilti to the wait position
                        GuiltiToThrow.transform.positionTo(distance / 10, //duration
                            GuiltiWaitPosition.transform.position). //final position
                            setOnCompleteHandler((x) =>
                        {
                            x.complete();
                            x.destroy();
                            InitializeGuilti();
                        });

                    }
                }
                break;
            case SlingshotState.GuiltiFlying:
                break;
            default:
                break;
        }

    }

    private void ThrowGuilti(float distance)
    {
        //get velocity
        Vector3 velocity = SlingshotMiddleVector - GuiltiToThrow.transform.position;
        GuiltiToThrow.GetComponent<Guilti>().OnThrow(); //make the Guilti aware of it
        //old and alternative way
        //GuiltiToThrow.GetComponent<Rigidbody2D>().AddForce
        //    (new Vector2(v2.x, v2.y) * ThrowSpeed * distance * 300 * Time.deltaTime);
        //set the velocity
        GuiltiToThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * ThrowSpeed * distance;


        //notify interested parties that the Guilti was thrown
        if (GuiltiThrown != null)
            GuiltiThrown(this, EventArgs.Empty);
    }

    public event EventHandler GuiltiThrown;

    private void InitializeGuilti()
    {
        //initialization of the ready to be thrown Guilti
        GuiltiToThrow.transform.position = GuiltiWaitPosition.position;
        slingshotState = SlingshotState.Idle;
        SetSlingshotLineRenderersActive(true);
    }

    void DisplaySlingshotLineRenderers()
    {
        SlingshotLineRenderer1.SetPosition(1, GuiltiToThrow.transform.position);
        SlingshotLineRenderer2.SetPosition(1, GuiltiToThrow.transform.position);
    }

    void SetSlingshotLineRenderersActive(bool active)
    {
        SlingshotLineRenderer1.enabled = active;
        SlingshotLineRenderer2.enabled = active;
    }

    void SetTrajectoryLineRenderesActive(bool active)
    {
        TrajectoryLineRenderer.enabled = active;
    }


  
    void DisplayTrajectoryLineRenderer2(float distance)
    {
        SetTrajectoryLineRenderesActive(true);
        Vector3 v2 = SlingshotMiddleVector - GuiltiToThrow.transform.position;
        int segmentCount = 15;
        float segmentScale = 2;
        Vector2[] segments = new Vector2[segmentCount];

        // The first line point is wherever the player's cannon, etc is
        segments[0] = GuiltiToThrow.transform.position;

        // The initial velocity
        Vector2 segVelocity = new Vector2(v2.x, v2.y) * ThrowSpeed * distance;

        float angle = Vector2.Angle(segVelocity, new Vector2(1, 0));
        float time = segmentScale / segVelocity.magnitude;
        for (int i = 1; i < segmentCount; i++)
        {
            //x axis: spaceX = initialSpaceX + velocityX * time
            //y axis: spaceY = initialSpaceY + velocityY * time + 1/2 * accelerationY * time ^ 2
            //both (vector) space = initialSpace + velocity * time + 1/2 * acceleration * time ^ 2
            float time2 = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * time2 + 0.5f * Physics2D.gravity * Mathf.Pow(time2, 2);
        }

        TrajectoryLineRenderer.SetVertexCount(segmentCount);
        for (int i = 0; i < segmentCount; i++)
            TrajectoryLineRenderer.SetPosition(i, segments[i]);
    }





}
