using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _RightEngine;
    [SerializeField]
    private GameObject _LeftEngine;
    [SerializeField]
    private int _score;
    [SerializeField]
    private AudioClip _laserClip;
    [SerializeField]
    private AudioClip _explosionClip;
    [SerializeField]
    private AudioSource _audioSource;
    private UIManager _uiManager;
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    private float _speedMultiplier = 2f;
    private float _canFire = 0f;
    private float horizontalInput;
    private float verticalInput;
    void Start()
    { 
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager==null)
        {
            Debug.Log("The Spawn Manager is NULL !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
          
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4f, 0f), 0);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        

        if (transform.position.x >= 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

       
    }

    void FireLaser()
    { 
        _canFire = Time.time + _fireRate;
        

        if (_isTripleShotActive)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        }
        _audioSource.clip = _laserClip;
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        { 
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
        }
        else
            {
                _lives--;
                _audioSource.clip = _explosionClip;
                _audioSource.Play();
                if (_lives == 2)
                {
                    _RightEngine.SetActive(true);
                }
                else if (_lives == 1)
                {
                    _LeftEngine.SetActive(true);
                }

                _uiManager.setSprite(_lives);
                if (_lives < 1)
                {
                    _audioSource.clip = _explosionClip;
                    _audioSource.Play();
                    _uiManager.setSprite(_lives);
                    _spawnManager.OnPlayerDeath();
                    Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                }
            }
    }

    public void EnableTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void EnableSpeed()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDown());
        
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _speed /= _speedMultiplier;
    }

    public void EnableShield()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void Score(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
