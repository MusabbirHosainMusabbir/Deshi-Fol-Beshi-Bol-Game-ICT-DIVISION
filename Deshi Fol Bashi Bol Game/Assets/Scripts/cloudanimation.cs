using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudanimation : MonoBehaviour
{
    float scrollspeed = -2f;
    Vector2 startpos;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollspeed, 40f);
        transform.position = startpos + Vector2.right * newPos;
    }
}
