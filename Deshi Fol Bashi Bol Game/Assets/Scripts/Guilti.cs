 using UnityEngine;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody2D))]
public class Guilti : MonoBehaviour
{
    public ParticleSystem Gultibanga;

    // Use this for initialization
    void Start()
    {
        //trailrenderer is not visible until we throw the 



        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<TrailRenderer>().sortingLayerName = "Foreground";
        //no gravity at first
        GetComponent<Rigidbody2D>().isKinematic = true;
        //make the collider bigger to allow for easy touching
        GetComponent<CircleCollider2D>().radius = Constants.GuiltiColliderRadiusBig;
        State = GuiltiState.BeforeThrown;
    }



    void FixedUpdate()
    {
        //if we've thrown the Guilti
        //and its speed is very small
        if (State == GuiltiState.Thrown &&
            GetComponent<Rigidbody2D>().velocity.sqrMagnitude <= Constants.MinVelocity)
        {
            //destroy the Guilti after 2 seconds
            StartCoroutine(DestroyAfter(2));
        }
    }

    public void OnThrow()
    {
        //play the sound
        GetComponent<AudioSource>().Play();
        //show the trail renderer
        GetComponent<TrailRenderer>().enabled = true;
        //allow for gravity forces
        GetComponent<Rigidbody2D>().isKinematic = false;
        //make the collider normal size
        GetComponent<CircleCollider2D>().radius = Constants.GuiltiColliderRadiusNormal;
        State = GuiltiState.Thrown;
    }

    IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    public GuiltiState State
    {
        get;
        private set;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "mango")
        {
            Instantiate(Gultibanga, transform.position, Quaternion.identity);
         
            Destroy(this.gameObject);
           
           
        }
    }
}
