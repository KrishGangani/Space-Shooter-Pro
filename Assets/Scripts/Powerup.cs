using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int powerupId;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-9.5f,9.5f),6.2f,0f);
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * 3 * Time.deltaTime);
        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_audioSource.clip, transform.position);
            if (player != null) 
            {
                switch(powerupId)
                {
                    case 0:
                        Destroy(this.gameObject);
                        player.EnableTripleShot();
                        break;
                    case 1:
                        Destroy(this.gameObject);
                        player.EnableSpeed();
                        break;
                    case 2:
                        Destroy(this.gameObject);
                        player.EnableShield();
                        break;
                    default:
                        Debug.Log("Default Occured !");
                        break;
                }
                
            }
            
        }
    }
}
