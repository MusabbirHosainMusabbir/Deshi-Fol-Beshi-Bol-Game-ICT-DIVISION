using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Mango : MonoBehaviour
{
  
    public float Health = 150f;
    public Sprite SpriteShownWhenHurt;
    private float ChangeSpriteHealth;
    Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {
        ChangeSpriteHealth = Health - 30f;
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        //if we are hit by a 



        if (col.gameObject.tag == "Guilti")
        {
            GetComponent<AudioSource>().Play();
            //Destroy(gameObject);
            rb.gravityScale = 1;

        }
        else //we're hit by something else
        {
            //calculate the damage via the hit object velocity
            float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            Health -= damage;
            //don't play sound for small damage
            if (damage >= 10)
                GetComponent<AudioSource>().Play();
            rb.gravityScale = 1;
            Destroy(this.gameObject, 4f);
            scoremango.score += 1;
            

            if (Health < ChangeSpriteHealth)
            {//change the shown sprite

                GetComponent<SpriteRenderer>().sprite = SpriteShownWhenHurt;
            }
            if (Health <= 0) Destroy(this.gameObject);
        }
    }
    /* void Update()
    {
        if (scoremango.score == 4 )

        {
            SceneManager.LoadScene("Demo_Scene_Mobile");
        }
    }*/


}
