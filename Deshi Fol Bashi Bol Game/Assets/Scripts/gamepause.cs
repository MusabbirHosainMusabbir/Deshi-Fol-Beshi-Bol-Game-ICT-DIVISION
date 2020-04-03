using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamepause : MonoBehaviour
{

    private Image image;
   
    private Sprite start, pause;
    private bool isPaused = false;


    // Start is called before the first frame update

    void Start()

    {
        image = GetComponent<Image>();

        start = Resources.Load<Sprite>("Pause(4)");


        pause = Resources.Load<Sprite>("Play");

        image.sprite = pause;
    }


    // Update is called once per frame

    void Update()
    {


    }
    public void pausescene()
    {
        /* for (int i = 0; i < Input.touchCount; ++i)
         {
             if (Input.GetTouch(i).phase == TouchPhase.Began)*/
       /* if (Input.GetMouseButtonDown(1))
        {*/
            if (isPaused)
            {
            
                ///PAUSED
                Time.timeScale = 1;
                isPaused = false;
                if (image.sprite == start)
                    image.sprite = pause;
               
            }
            else
            {
           
            //NOT PAUSED
            Time.timeScale = 0;
                isPaused = true;

                if (image.sprite == pause)
                    image.sprite = start;
            }
        }


  
    }


    


