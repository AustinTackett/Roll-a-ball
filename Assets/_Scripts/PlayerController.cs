using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private float movementX;
    private float movementY;
    public float speed = 0;
    private int count;
    private Rigidbody rb;
    
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject pickupVfxObject;
    public GameObject explosionVfxObject;
    private AudioSource pickUpAudioSource;
    public GameObject backgroundMusicObject;

    
    public void Awake()
    {
        pickUpAudioSource = GetComponent<AudioSource>();

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

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp")) 
       {
            other.gameObject.SetActive(false);
            count = count + 1;
            pickUpAudioSource.Play();
            Instantiate(pickupVfxObject, transform.position, Quaternion.identity);
            SetCountText();
            
            if (count == 12)
            {
                Destroy(GameObject.FindGameObjectWithTag("Enemy"));
                winTextObject.SetActive(true);
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
            backgroundMusicObject.GetComponent<AudioSource>().Stop();
            collision.gameObject.GetComponent<AudioSource>().Play();
            var explosionVfx = Instantiate(explosionVfxObject, transform.position, Quaternion.identity);
            Destroy(explosionVfx);
        }
        if (collision.gameObject.CompareTag("Walls"))
        {
            Debug.Log("walls");
            collision.gameObject.GetComponentInParent<AudioSource>().Play();
        }
    }

}
