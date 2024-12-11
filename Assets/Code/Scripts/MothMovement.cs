using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(Rigidbody))]
public class MothMovement : MonoBehaviour
{
    private float currentRotation = 0;  // obrót tylko w osi w której ćma leci - w zakresie [-1, 1]. Wpływa na prędkość ruchu w tym kierunku
    public float MaxRotation = 30;  // w stopniach, tylko graficzne (nie wpływa na mechanikę lotu)


    public float ForwardSpeed = 1;
    public float MaxHorizontalSpeed = 50;

    public float FlapForce = 0;  // jak mocno macha skrzydłami

    public float GravityForce = 1;



    public float RotationForce = 0.1f;  // jak szybko obraca się lewo-prawo (w % na sekundę) (tzn 0.1 === 10%)
    public float RotationResetForce = 0.1f;  // jak szybko wraca do obrotu 0 (w % na sekundę)


    public float GlidingSpeed = -0.5f; // prędkość (pionowa) szybowania. Jeśli spada szybciej niż to, to będzie hamował do tej prędkości
    public float GlideFactor = 0.1f; // współczynnik "t" w lerpie. Jeśli spada szybciej niż GlidingSpeed, to prędkość będzie lerpowana z tym "t" do GlidingSpeed


    private bool dashDescending;  // czy w danym momencie ma złożone skrzydła i leci szybko w dół

    private Rigidbody rb;

    private bool IsFalling => rb.velocity.y < 0;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Hello~! I am a mmmmmmmoth!");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            //Debug.Log("flap.");
            Flap();
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            dashDescending = true;
        } else {
            dashDescending = false;
        }
        
        bool arrowsPressed = false;
        if (Input.GetKey(KeyCode.RightArrow)) {
            //Debug.Log("right.");
            arrowsPressed = true;
            currentRotation -= RotationForce * Time.deltaTime;

            if (currentRotation < -1) {
                currentRotation = -1;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            //Debug.Log("left.");
            arrowsPressed = true;
            currentRotation += RotationForce * Time.deltaTime;

            if (currentRotation > 1) {
                currentRotation = 1;
            }
        }

        if (!arrowsPressed) {
            if (currentRotation > 0) {
                currentRotation -= RotationResetForce * Time.deltaTime;
                currentRotation = Mathf.Max(currentRotation, 0);
            }
            else {
                currentRotation += RotationResetForce * Time.deltaTime;
                currentRotation = Mathf.Min(currentRotation, 0);
            }
        }


        AddGravity();

        if (!dashDescending && IsFalling && rb.velocity.y < GlidingSpeed) {
            //Debug.Log("LERP");
            float newVelocity = Mathf.Lerp(rb.velocity.y, GlidingSpeed, GlideFactor);
            rb.velocity = new Vector3(rb.velocity.x, newVelocity, rb.velocity.z);
        }

        // ustawianie ładnego obrotu ćmy
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, currentRotation * MaxRotation);

        var horizontalSpeed = -currentRotation * MaxHorizontalSpeed;
        var forwardSpeed = ForwardSpeed;

        transform.position = new Vector3(
            transform.position.x + horizontalSpeed * Time.deltaTime,
            transform.position.y,
            transform.position.z + forwardSpeed * Time.deltaTime
        );
    }


    void Flap() {
        rb.velocity = new Vector3(rb.velocity.x, FlapForce, rb.velocity.z);
    }

    void AddGravity() {
        rb.AddForce(-GravityForce * Vector3.up);
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LanternDeathZone"))
        {
            Debug.Log("Entered Death Zone");
        }
    }

}
