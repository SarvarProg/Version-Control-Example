using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedMultiplier = 6;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFireTime = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
 
    private bool _isTripleShotActive = false;
    private bool _isShieldsActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _lefttEngine, _rightEngine;


    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audiosource;

    private float _horizontalInput;
    private float _verticalInput;
    private Vector2 _movement;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audiosource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if ( _audiosource == null)
        {
            Debug.LogError("AudioSource on the player is NULL");
        }
        else
        {
            _audiosource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        HandleFiring();
    }

    private void HandleFiring()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Key Pressed");
            //Instantiate(laserPrefab, transform.position, Quaternion.identity);
             FireLaser();
        }
    }

   
    void CalculateMovement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _movement = new Vector2(_horizontalInput, _verticalInput);
        //transform.Translate(Vector3.right * _horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * _verticalInput * _speed * Time.deltaTime);
        transform.Translate(_movement * _speed * Time.deltaTime);
        //transform.Translate(new Vector3(1, 0, 0));


        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 9.1f)
        {
            transform.position = new Vector3(9.1f, transform.position.y, 0);
        }

        else if (transform.position.x <= -9.1f)
        {
            transform.position = new Vector3(-9.1f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (Time.time > _nextFireTime) //current time is not greater than -1;
        {
            _nextFireTime = Time.time + _fireRate; //-1 = 1.5

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

            _audiosource.Play();

        }
    }

    public void Damage()
    {
        if (_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _lefttEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {

            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);

        }
    }

    public void TrippleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TrippleShotPowerDownRoutine());
    }

    IEnumerator TrippleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
        Debug.Log("Shield is activated");
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
