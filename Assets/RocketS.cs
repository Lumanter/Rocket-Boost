using UnityEngine;

public class RocketS : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource fly;
    bool flying = false;
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
                print("Ok"); 
                break;
            case "Fuel":
                print("Fuel"); 
                break;
            default:
                print("Dead");   
                break;
        }  
    }
}
