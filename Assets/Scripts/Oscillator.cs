using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] float delay = 0f;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    [Range(0,1)]
    [SerializeField] float movementFactor; // 0 for not moved, 1 for fully moved

    Vector3 startingPos;

    void Start(){
        startingPos = transform.position;
    }

    void Update(){
        if (period <= Mathf.Epsilon) return; // compares to smallest float

        float cycles = (Time.time/period)+delay;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);// varies [-1,1]
        Vector3 offset = movementFactor * movementVector;

        transform.position = startingPos + offset;
        movementFactor = (rawSinWave / 2f) + 0.5f;
    }
}
