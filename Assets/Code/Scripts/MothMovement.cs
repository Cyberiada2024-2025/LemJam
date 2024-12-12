using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(Rigidbody))]
public class MothMovement : MonoBehaviour
{
    private bool is_started = false;

    [SerializeField]
    private Animator anim;
    private bool is_dead = false;
    private float currentRotation = 0;  // obrót tylko w osi w której ćma leci - w zakresie [-1, 1]. Wpływa na prędkość ruchu w tym kierunku
    public float MaxRotation = 30;  // w stopniach, tylko graficzne (nie wpływa na mechanikę lotu)

    private float CurrentEnergy = 50;
    public float MaxEnergy = 100;
    public float RechargingSpeedFactor = 1.0f;
    public float DeathFallZone = -8;

    public float ForwardSpeed = 1;
    public float MaxHorizontalSpeed = 50;

    public float FlapForce = 2;  // jak mocno macha skrzydłami
    public float FlapCooldown = 0.75f;  // co tyle sekund może machnąć skrzydłami
    private float lastFlapTime = -1;

    public float GravityForce = 1;

    private float TimeStamp = 0;


    public float RotationForce = 0.1f;  // jak szybko obraca się lewo-prawo (w % na sekundę) (tzn 0.1 === 10%)
    public float RotationResetForce = 0.1f;  // jak szybko wraca do obrotu 0 (w % na sekundę)


    public float GlidingSpeed = -0.5f; // prędkość (pionowa) szybowania. Jeśli spada szybciej niż to, to będzie hamował do tej prędkości
    public float GlideFactor = 0.1f; // współczynnik "t" w lerpie. Jeśli spada szybciej niż GlidingSpeed, to prędkość będzie lerpowana z tym "t" do GlidingSpeed


    public float AttractionFlapForceMultiplier = 0.75f;  // machanie skrzydłami automatyczne (od bycia pod lampionem) będzie miało siłę równą FlapForce * ten parametr
    public float AttractionFlapInterval = 5;  // co tyle sekund będzie machał skrzydłami jak będzie pod atraktorem (w rzeczywistości pewnie rzadziej, bo to jest mnożone * attraction force)
    private float attractionFlapTimer = 0;
    private List<Attractor> attractors = new List<Attractor>();
    private float attractionForce;
    private Vector3 attractionVector;
    private AudioSource source;


    private float dashDescending;  // czy w danym momencie ma złożone skrzydła i leci szybko w dół (0 = nie, 1 = tak, 0.5 = trochę tak, trochę nie)

    private Rigidbody rb;

    private bool IsFalling => rb.velocity.y < 0;

    [SerializeField] private AudioClip VictorySound;
    [SerializeField] private AudioClip DieFallSound;
    [SerializeField] private AudioClip DieSound;
    [SerializeField] private AudioClip FlapSound;
    [SerializeField] private AudioClip SrobkaSound;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello~! I am a mmmmmmmoth!");
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeStamp += Time.deltaTime;
        if (!is_dead){ 
            CalculateAttractionForce();
            GameManager.Instance.SetEnergy(CurrentEnergy);

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) {
                Flap(FlapForce);
            }

            if(!is_started)
            {
                return;
            }

            
            if (Input.GetKey(KeyCode.DownArrow)) {
                dashDescending = 1;
                anim.SetBool("Descend", true); 
            } else {
                dashDescending = 0;
                anim.SetBool("Descend", false); 

            }
            
            bool arrowsPressed = false;
            if (Input.GetKey(KeyCode.RightArrow)) {
                //Debug.Log("right.");
                arrowsPressed = true;
                currentRotation -= RotationForce * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftArrow)) {
                //Debug.Log("left.");
                arrowsPressed = true;
                currentRotation += RotationForce * Time.deltaTime;
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

            currentRotation -= attractionVector.x * Time.deltaTime;
            if (currentRotation > 1) {
                currentRotation = 1;
            } else if (currentRotation < -1) {
                currentRotation = -1;
            }
            if (attractionVector.y < 0) {
                dashDescending -= attractionVector.y;
                dashDescending = Mathf.Max(dashDescending, 0);
            } else if (attractionVector.y > 0) {
                attractionFlapTimer += attractionVector.y * Time.deltaTime;
                if (attractionFlapTimer >= AttractionFlapInterval) {
                    attractionFlapTimer = 0;
                    Flap(FlapForce * AttractionFlapForceMultiplier);
                }
            }
            
            /*if (transform.position.y< DeathFallZone+ GameManager.Instance.cameraBox.GetY())
            {
                source.PlayOneShot(DieFallSound, AudioListener.volume);
                Death();
            }*/
        }

        if (is_dead){
            
            var step = 1000 * Time.deltaTime;
            var camera = GameObject.Find("Main Camera").GetComponent<Transform>();
            var rotation = Quaternion.LookRotation(transform.position - camera.position);
            camera.rotation = Quaternion.Slerp(camera.rotation, rotation, Time.deltaTime * 6);
        }

        AddGravity();

        if (IsFalling) {
            float newVelocity = Mathf.Lerp(rb.velocity.y, GlidingSpeed, GlideFactor * (1-dashDescending) * Time.deltaTime);
            rb.velocity = new Vector3(rb.velocity.x, newVelocity, 0);
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
        
        CalculateLightRecharge();
            
    }


    void Flap(float force) {
        if (Time.time - lastFlapTime > FlapCooldown && CurrentEnergy >= 0) {
            //Debug.Log("flap");
            
            anim.SetTrigger("flap");
            rb.velocity = new Vector3(rb.velocity.x, force, 0);
            lastFlapTime = Time.time;
            CurrentEnergy -= 3;
            source.PlayOneShot(FlapSound, AudioListener.volume);
            if (TimeStamp > 3)
            {
                source.PlayOneShot(SrobkaSound, AudioListener.volume);
                Debug.Log(DeathFallZone + GameManager.Instance.cameraBox.GetY());
                TimeStamp = 0;
            }
            
        }
        else {
            //Debug.Log("NO FLAP :c");
        }
    }


        void AddGravity() {
        rb.AddForce(-GravityForce * Vector3.up);
    }


    void CalculateLightRecharge()
    {
        Vector3 combinedVector = Vector3.zero;

        foreach (var attractor in attractors)
        {
            float distance = Vector3.Distance(transform.position, attractor.transform.position);
            float radius = attractor.Radius;

            var force = (1 - distance / radius) * attractor.AttractionForce;
            var forceVector = attractor.transform.position - transform.position;

            combinedVector += forceVector * force / (distance);
        }

        var finalForce = combinedVector.magnitude;

        CurrentEnergy += finalForce*RechargingSpeedFactor;
        if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
    }


    void CalculateAttractionForce()
    {
        Vector3 combinedVector = Vector3.zero;

        foreach (var attractor in attractors) {
            if (attractor.transform.position.z < transform.position.z) {
                continue;  // pominięcie atraktorów które już są za nami
            }

            float distance = Vector3.Distance(transform.position, attractor.transform.position);
            float radius = attractor.Radius;

            distance = Mathf.Min(distance, radius);

            var force = (1 - distance / radius) * attractor.AttractionForce;
            var forceVector = attractor.transform.position - transform.position;

            combinedVector += forceVector*force / (distance);
        }
        
        //var finalForce = combinedVector.magnitude;
        //combinedVector.Normalize();

        attractionVector = combinedVector;

    }

    public void Death()
    {
        is_dead = true;
        StartCoroutine(Fade());
        ScoreTextSetter.scoreToSet = GameManager.Instance.GetScore(transform.position.z);
    }

    public void SafeDeath()
    {        
        StartCoroutine(Fade());
        ScoreTextSetter.scoreToSet = GameManager.Instance.GetScore(transform.position.z);
    }

    private IEnumerator Fade(){
		CanvasGroup canvasGroup = GameObject.Find("FadeToBlack").GetComponent<CanvasGroup>();
		while (canvasGroup.alpha < 1){
			canvasGroup.alpha += Time.deltaTime/3;
			yield return null;
		}
        
         GameManager.Restart();
		canvasGroup.interactable = false;	
    } 
    public void TurnOn()
    {
        is_started = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LanternDeathZone"))
        {
            source.PlayOneShot(DieSound, AudioListener.volume);
            Death();
            Debug.Log("Entered Death Zone");
            Debug.Log("Score: " + GameManager.Instance.GetScore(transform.position.z));
        }
        else if (other.CompareTag("Attractor")) {
            var attractor = other.GetComponent<Attractor>();
            if (attractor != null) {
                attractors.Add(attractor);
            }
        }
        //if (other.CompareTag("LanternEnergyZone"))
        //{
        //    CurrentEnergy += 10;
        //}

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Attractor")) {
            var attractor = other.GetComponent<Attractor>();
            if (attractor != null) {
                attractors.Remove(attractor);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + attractionVector);
    }

    public float GetCurrentEnergy()
    {
        return CurrentEnergy;
    }
}
