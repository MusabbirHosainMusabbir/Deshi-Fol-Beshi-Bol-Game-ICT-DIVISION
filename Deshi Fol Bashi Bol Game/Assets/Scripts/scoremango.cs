using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scoremango : MonoBehaviour
{
   // public int nextlevel;
    public static int score=0;
    public Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
      ///  nextlevel = SceneManager.GetActiveScene().buildIndex + 1;

         ScoreText = GetComponent<Text>();
        ScoreText.text = "Score :  " + score;
    }

    // Update is called once per frame
    void Update()
    {
       /* if (scoremango.score == 5)
        {
            if (SceneManager.GetActiveScene().buildIndex == 7)
            {
                Debug.Log("ieui");
            }
            else
            {
                SceneManager.LoadScene(nextlevel);
                if (nextlevel >  PlayerPrefs.GetInt("levelAt")) 
                {
                    PlayerPrefs.SetInt("levelAt", nextlevel);
                }
            }*//*
        }
       */
        ScoreText.text = "Score : " + score ;
    }
  


}
