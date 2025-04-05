using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20f;
    [SerializeField]
    private GameObject _Explosion;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Instantiate(_Explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}
