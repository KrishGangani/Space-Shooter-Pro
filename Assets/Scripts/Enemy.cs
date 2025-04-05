using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _Explosion;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserClip;
    private AudioSource _audioSource;
    private float _fireRate = 1f;
    private float _canFire = -1f;
    private float x;
    private float top = 6.2f;
    private Player _player;
    private int _tempScore;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 6.2f, 0f);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _laserClip;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(2.0f, 4.1f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            _audioSource.Play();

            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5)
        {
            x = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(x, top, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }
            Instantiate(_Explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if(other.tag =="Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _tempScore = Random.Range(7, 12);
                _player.Score(_tempScore);
            }
            Instantiate(_Explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
