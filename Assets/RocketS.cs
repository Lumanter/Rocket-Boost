using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketS : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource fly;
    private bool flying = false; // Used for thrust fade in and out sound
    public float flyPower = 100f;
    public float rotationSpd= 100f;


    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        fly = GetComponent<AudioSource>();
        fly.volume=0;
    }

    private void ProcessInput(){
        if(Input.GetKey("up")){
            rigidBody.AddRelativeForce(Vector3.up*flyPower*Time.deltaTime);
            if(!flying){
                flying=true;
                fly.Stop();
                fly.volume=0;
            }
            if(!fly.isPlaying) 
                fly.Play();
            if(fly.volume<1)
                fly.volume+= 0.3f*Time.deltaTime;
        }else{
            flying= false;
            if(fly.isPlaying){
                if(fly.volume!=0.0f)fly.volume-= 0.78f*Time.deltaTime;
            } 
        }

        rigidBody.freezeRotation=true;
        if(Input.GetKey("left"))
            transform.Rotate(Vector3.forward*rotationSpd*Time.deltaTime);
        if(Input.GetKey("right"))
            transform.Rotate(Vector3.back*rotationSpd*Time.deltaTime);
        rigidBody.freezeRotation=false;
    }

    void Update(){
        ProcessInput();
    }

    void OnCollisionEnter(Collision collision) {
        switch(collision.gameObject.tag){
            case "Friendly":
                //print("Ok"); 
                break;
            case "Finish":
                print("Hit finish");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                break;
            default:
                print("Dead");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }  
    }
}
