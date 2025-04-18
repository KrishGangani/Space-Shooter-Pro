using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;


    private bool _stopSpawning = false;
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnTripleShotRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawning==false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.2f, 9.2f), 6f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3f);
        } 
    }

    IEnumerator SpawnTripleShotRoutine()
    {
        yield return new WaitForSeconds(5f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.2f, 9.2f), 6f, 0f);
            int RandomPowerupID = Random.Range(0, 3);
            Instantiate(_powerups[RandomPowerupID], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5f, 7f));
        }
    }
   
    public void OnPlayerDeath()
    {
        _stopSpawning=true;
    }
}
