using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uimanager : MonoBehaviour

{
  /*  public Button[] buttons;
  
    // Start is called before the first frame update
    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 2);
        for(int i= 0; i<buttons.Length; i++)
        {
            if(i+2>levelAt)
                buttons[i].interactable=false;
        }
    }*/

    // Update is called once per frame
    void Update()
    {
    
        
    }
    public void openscene()
    {
        SceneManager.LoadScene("GameLevel");
    }
   
    
    
     public void level1()
    {
        SceneManager.LoadScene("level1");
    }
   
    
    public void exit()
    {
        Application.Quit();
    }
   
    public void nextlevel()
    {
        GameManager.time = 60;
        scoremango.score = scoremango.score - scoremango.score;

        int currentlevel = PlayerPrefs.GetInt("ReachLevel", 1);
        PlayerPrefs.SetInt("ReachLevel", currentlevel + 1);
        SceneManager.LoadScene(currentlevel + 1);

        SceneManager.LoadScene("GameLevel");
    }
    public void level2()
    {
      
        SceneManager.LoadScene("level2");
    }
    public void level3()
    {
    SceneManager.LoadScene("level3");
    }
    public void level4()
    {

      SceneManager.LoadScene("level4");
    }

    public void level()
    {

        GameManager.time = 60;
        scoremango.score = scoremango.score - scoremango.score;

        SceneManager.LoadScene("GameLevel");
    }

}


