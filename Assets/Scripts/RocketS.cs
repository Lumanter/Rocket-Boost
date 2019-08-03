using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class RocketS : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource sfx;

    private bool flying = false; // Used for thrust fade in and out sound effect
    public float flyPower = 100f;
    public float rotationSpd= 100f;
    [SerializeField] float levelLoadDelay = 1.2f;

    [SerializeField] AudioClip engine;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    //public cameraShake camShake;
    [SerializeField] CameraShaker camShaker;

    enum State {Alive,Dying,Transcending}
    State state = State.Alive;


    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        sfx = GetComponent<AudioSource>();
        sfx.volume=0;
    }

    private void ProcessInput(){

        if(Input.GetKey("up")){
            rigidBody.AddRelativeForce(Vector3.up*flyPower*Time.deltaTime);

            engineParticles.Play();

            if (!flying){
                flying=true;
                sfx.Stop();
                sfx.volume=0;
            }
            if (!sfx.isPlaying)
                sfx.PlayOneShot(engine);
            if(sfx.volume<1)
                sfx.volume+= 0.3f*Time.deltaTime;
        }else{
            flying= false;
            if(sfx.isPlaying && state == State.Alive)
                if(sfx.volume!=0.0f)sfx.volume-= 0.78f*Time.deltaTime;

            engineParticles.Stop();

        }

        rigidBody.freezeRotation=true;
        if(Input.GetKey("left"))
            transform.Rotate(Vector3.forward*rotationSpd*Time.deltaTime);
        if(Input.GetKey("right"))
            transform.Rotate(Vector3.back*rotationSpd*Time.deltaTime);
        rigidBody.freezeRotation=false;
    }

    void Update(){
        if(state==State.Alive)
            ProcessInput();
    }

    private void LoadNextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnCollisionEnter(Collision collision) {
        //prevents unnecessary processing when not alive
        if (state != State.Alive)return;

        switch(collision.gameObject.tag){
            case "Friendly":
                //print("Ok"); 
                break;
            case "Finish":
                state = State.Transcending;
                sfx.volume = 1;
                sfx.Stop();
                sfx.PlayOneShot(success);

                camShaker.ShakeOnce(10f, 20f, .1f, 1f);

                successParticles.Play();
                Invoke("LoadNextScene", levelLoadDelay);
                break;
            default:
                print("Dead");
                state = State.Dying;
                sfx.Stop();
                sfx.volume = 1;
                sfx.PlayOneShot(crash);

                camShaker.ShakeOnce(5f,12f,.1f,1f);

                engineParticles.Stop();
                crashParticles.Play();

                GetComponent<Rigidbody>().AddTorque(transform.up*14);

                Invoke("RestartScene", 1.3f);
                break;
        }  
    }
}
