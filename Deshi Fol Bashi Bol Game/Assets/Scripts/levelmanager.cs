using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelmanager : MonoBehaviour
{
    Button [] levelbuttons;
    // Start is called before the first frame update

    private void Awake()
    {
        int ReachLevel = PlayerPrefs.GetInt("ReachLevel", 1);
        levelbuttons = new Button[transform.childCount];
        for(int i=0; i<levelbuttons.Length; i++)
        {
            levelbuttons[i] = transform.GetChild(i).GetComponent<Button>();
         //   levelbuttons[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
            if (i + 1 > ReachLevel)
            {
                levelbuttons[i].interactable = false;
            }
        }
    }


}
