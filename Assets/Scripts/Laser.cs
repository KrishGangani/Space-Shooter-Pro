using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;
    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _audioSource;
    private bool _isEnemyLaser = false;
    private Player _player;

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        _player = player.GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explosionClip;
    }
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else if(_isEnemyLaser == true) 
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        if (transform.position.y > 10f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        if (transform.position.y < -6f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _isEnemyLaser == true)
        {
            _player.Damage();
            AudioSource.PlayClipAtPoint(_audioSource.clip, transform.position);
            Destroy(this.gameObject);
        }
    }
}
