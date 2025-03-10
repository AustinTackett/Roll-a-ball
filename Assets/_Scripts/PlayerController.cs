using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private float movementX;
    private float movementY;
    public float speed = 0;
    public float trailThreshold;
    private int count;
    private Rigidbody rb;
    
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject pickupVfxObject;
    public GameObject explosionVfxObject;
    public GameObject victoryVfxObject;
    private ParticleSystem trailParticlesSystem;
    private AudioSource pickUpAudioSource;
    public GameObject backgroundMusicObject;

    
    public void Awake()
    {
        pickUpAudioSource = GetComponent<AudioSource>();
        trailParticlesSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        count = 0;
        SetCountText();
        rb = GetComponent<Rigidbody>();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        if (rb.linearVelocity.magnitude > trailThreshold && !trailParticlesSystem.isPlaying)
        {
            //Debug.Log("Speed");
            trailParticlesSystem.Play();
        }
        else if (rb.linearVelocity.magnitude < trailThreshold && trailParticlesSystem.isPlaying)
        {
            //Debug.Log("Stop");
            trailParticlesSystem.Stop();
        }
        Debug.Log(trailParticlesSystem.isPlaying);
        
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            pickUpAudioSource.Play();
            var pickupVfx = Instantiate(pickupVfxObject, transform.position, Quaternion.identity);
            Destroy(pickupVfx, 3);
            SetCountText();
            
            if (count == 12)
            {
                Destroy(GameObject.FindGameObjectWithTag("Enemy"));
                winTextObject.SetActive(true);
                var victoryVfx = Instantiate(victoryVfxObject, transform,  worldPositionStays:false);
                Destroy(victoryVfx, 3);
                backgroundMusicObject.GetComponent<AudioSource>().Stop();
                winTextObject.gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }
    
    void SetCountText() 
   {
       countText.text =  "Count: " + count.ToString();
   }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); 
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            collision.gameObject.GetComponentInChildren<Animator>().SetFloat("speed_f", 0);
            backgroundMusicObject.GetComponent<AudioSource>().Stop();
            collision.gameObject.GetComponent<AudioSource>().Play();
            var explosionVfx = Instantiate(explosionVfxObject, transform.position, Quaternion.identity);
            Destroy(explosionVfx, 3);
        }
        if (collision.gameObject.CompareTag("Walls"))
        {
            Debug.Log("walls");
            collision.gameObject.GetComponentInParent<AudioSource>().Play();
        }
    }

}
