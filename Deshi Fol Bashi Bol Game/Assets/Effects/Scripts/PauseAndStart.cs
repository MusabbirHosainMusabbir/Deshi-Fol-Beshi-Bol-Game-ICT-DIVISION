using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAndStart : MonoBehaviour
{
    public SpriteRenderer rend;
    private Sprite start, pause;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        start = Resources.Load<Sprite>("start");
        pause = Resources.Load<Sprite>("Button");
        rend.sprite = start;
    }

    // Update is called once per frame
    void Update()
    {

        pausescene();

    }
    public void pausescene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                ///PAUSED
                Time.timeScale = 1;
                isPaused = false;
                if (rend.sprite == start)
                    rend.sprite = pause;

            }
            else
            {
                //NOT PAUSED
                Time.timeScale = 0;
                isPaused = true;

                if (rend.sprite == pause)
                    rend.sprite = start;
            }
        }
    }
}



