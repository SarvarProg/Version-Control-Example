using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update

    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            {
                switch(powerupID)
                {
                    case 0:
                         player.TrippleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        Debug.Log("Power speed");
                        break;
                    case 2:
                        player.ShieldsActive();
                        Debug.Log("Collected Shields");
                        break;
                    default:
                        break;
                }
            }
            Destroy (this.gameObject);
        }
    }
}
