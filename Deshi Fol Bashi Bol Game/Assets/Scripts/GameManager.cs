using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    //public GameObject pannel;
    public static float time = 60f;
    public Text timerText;


    public CameraFollow cameraFollow;
    int currentGuiltiIndex;
    public SlingShot slingshot;
    [HideInInspector]
    public static GameState CurrentGameState = GameState.Start;
    private List<GameObject> Bricks;
    private List<GameObject> Guiltis;
    private List<GameObject> Mangos;

    // Use this for initialization
    void Start()
    {
       // pannel.SetActive(false);
        timerText.text = "Time : " + time.ToString("F2");
       

      
       
        CurrentGameState = GameState.Start;
        slingshot.enabled = false;
        //find all relevant game objects
        Bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Brick"));
        Guiltis = new List<GameObject>(GameObject.FindGameObjectsWithTag("guilti"));
        Mangos = new List<GameObject>(GameObject.FindGameObjectsWithTag("mango"));
        //unsubscribe and resubscribe from the event
        //this ensures that we subscribe only once
        slingshot.GuiltiThrown -= Slingshot_GuiltiThrown; slingshot.GuiltiThrown += Slingshot_GuiltiThrown;
    }


    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        time -= Time.deltaTime;
        timerText.text = "Time :  " + Mathf.Round(time).ToString() + " sce";
        if (time <=0)
        {
            SceneManager.LoadScene("GameLose");
            // pannel.SetActive(true);
        }



        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Application.Quit();
        }
        switch (CurrentGameState)
        {
            case GameState.Start:
                //if player taps, begin animating the Guilti 
                //to the slingshot
                if (Input.GetMouseButtonUp(0))
                {
                    AnimateGuiltiToSlingshot();
                }
                break;
            case GameState.GuiltiMovingToSlingshot:
                //do nothing
                break;
            case GameState.playing:
                //if we have thrown a Guilti
                //and either everything has stopped moving
                //or there has been 5 seconds since we threw the Guilti
                //animate the camera to the start position
                if (slingshot.slingshotState == SlingshotState.GuiltiFlying &&
                    (BricksGuiltisMangosStoppedMoving() || Time.time - slingshot.TimeSinceThrown > 5f))
                {
                    slingshot.enabled = false;
                    AnimateCameraToStartPosition();
                    CurrentGameState = GameState.GuiltiMovingToSlingshot;
                }
                break;
            //if we have won or lost, we will restart the level
            //in a normal game, we would show the "Won" screen 
            //and on tap the user would go to the next level
            case GameState.Won:


            case GameState.Lost:
                if (Input.GetMouseButtonUp(0))
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
                break;
            default:
                break;
        }
    }



    private bool AllMangosDestroyed()
    {
        return Mangos.All(x => x == null);
    }

   
    private void AnimateCameraToStartPosition()
    {
        float duration = Vector2.Distance(Camera.main.transform.position, cameraFollow.StartingPosition) / 10f;
        if (duration == 0.0f) duration = 0.1f;
        //animate the camera to start
        Camera.main.transform.positionTo
            (duration,
            cameraFollow.StartingPosition). //end position
            setOnCompleteHandler((x) =>
                        {
                            cameraFollow.IsFollowing = false;
                            /*   if ((scoremango.score >= 4||scoremango.score <= 15) && currentGuiltiIndex == Guiltis.Count - 1)*/
                            if (scoremango.score >= 4 && (currentGuiltiIndex == Guiltis.Count - 1))
                            {
                                CurrentGameState = GameState.Won;
                            }
                            //animate the next Guilti, if available
                            else if ( currentGuiltiIndex == Guiltis.Count - 1)
                            {
                                //no more Guiltis, go to finished
                                CurrentGameState = GameState.Lost;
                            }
                            else
                            {
                                slingshot.slingshotState = SlingshotState.Idle;
                                //Guilti to throw is the next on the list
                                currentGuiltiIndex++;
                                AnimateGuiltiToSlingshot();
                            }
                        });
    }

    /// <summary>
    /// Animates the Guilti from the waiting position to the slingshot
    /// </summary>
    void AnimateGuiltiToSlingshot()
    {
        CurrentGameState = GameState.GuiltiMovingToSlingshot;
        Guiltis[currentGuiltiIndex].transform.positionTo
            (Vector2.Distance(Guiltis[currentGuiltiIndex].transform.position / 10,
            slingshot.GuiltiWaitPosition.transform.position) / 10, //duration
            slingshot.GuiltiWaitPosition.transform.position). //final position
                setOnCompleteHandler((x) =>
                        {
                            x.complete();
                            x.destroy(); //destroy the animation
                            CurrentGameState = GameState.playing;
                            slingshot.enabled = true; //enable slingshot
                            //current Guilti is the current in the list
                            slingshot.GuiltiToThrow = Guiltis[currentGuiltiIndex];
                        });
    }


/*    /// <param name="sender"></param>
    /// <param name="e"></param>*/
    private void Slingshot_GuiltiThrown(object sender, System.EventArgs e)
    {
        cameraFollow.GuiltiToFollow = Guiltis[currentGuiltiIndex].transform;
        cameraFollow.IsFollowing = true;
    }

    /// <summary>
    /// Check if all Guiltis, Mangos and bricks have stopped moving
    /// </summary>
    /// <returns></returns>
    bool BricksGuiltisMangosStoppedMoving()
    {
        foreach (var item in Bricks.Union(Guiltis).Union(Mangos))
        {
            if (item != null && item.GetComponent<Rigidbody2D>().velocity.sqrMagnitude > Constants.MinVelocity)
            {
                return false;
            }
        }

        return true;
    }

    
  /* ///<param name="screenWidth"></param>
  /// <param name="screenHeight"></param>*/
    public static void AutoResize(int screenWidth, int screenHeight)
    {
        Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
    }

    /// <summary>
    /// Shows relevant GUI depending on the current game state
    /// </summary>
    void OnGUI()
    {
        AutoResize(1280, 720);
        switch (CurrentGameState)
        {
            case GameState.Start:
              
      
                break;
            case GameState.Won:

           
                SceneManager.LoadScene("win scene");
                
             
                break;

            case GameState.Lost:
               
                SceneManager.LoadScene("GameLose");

         
                break;
            default:
                break;
        }
    }


}
