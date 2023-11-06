using SO.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    const string SPAWN_POINT = "EnemySpawnPoint";

    //singleton
    [SerializeField] private FloatReference playerHealth;

    [SerializeField] private IntReference currentScore;

    private PlayerController playerController;

    [SerializeField]private List<Transform> spawnPoints = new();

    [SerializeField] private List<EnemyController> enemies;


    // stack-based ObjectPool available with Unity 2021 and above
    private IObjectPool<EnemyController> cyanPool;
    private IObjectPool<EnemyController> magentaPool;
    private IObjectPool<EnemyController> yellowPool;

    // throw an exception if we try to return an existing item, already in the pool
    [SerializeField] private bool collectionCheck = true;

    // extra options to control the pool capacity and maximum size
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private void Awake()
    {
        cyanPool = new ObjectPool<EnemyController>(CreateCyan,
               OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
               collectionCheck, defaultCapacity, maxSize);

        magentaPool = new ObjectPool<EnemyController>(CreateMagenta,
               OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
               collectionCheck, defaultCapacity, maxSize);

        yellowPool = new ObjectPool<EnemyController>(CreateYellow,
               OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
               collectionCheck, defaultCapacity, maxSize);

        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnDestroyPooledObject(EnemyController controller)
    {
        Destroy(controller.gameObject);
    }

    private void OnReleaseToPool(EnemyController controller)
    {
        controller.gameObject.SetActive(false);
    }

    private void OnGetFromPool(EnemyController controller)
    {
        controller.gameObject.SetActive(true);  
    }

    private EnemyController CreateCyan()
    {
        EnemyController enemyInstance = Instantiate(enemies[0], enemies[0].transform.position, Quaternion.identity, null);
        enemyInstance.EnemyPool = cyanPool;
        return enemyInstance;
    }

    private EnemyController CreateMagenta()
    {
        EnemyController enemyInstance = Instantiate(enemies[1], enemies[1].transform.position, Quaternion.identity, null);
        enemyInstance.EnemyPool = magentaPool;
        return enemyInstance;
    }
    private EnemyController CreateYellow()
    {
        EnemyController enemyInstance = Instantiate(enemies[2], enemies[2].transform.position, Quaternion.identity, null);
        enemyInstance.EnemyPool = yellowPool;
        return enemyInstance;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        Transform spawnPoints = GameObject.FindGameObjectWithTag(SPAWN_POINT).transform;

        this.spawnPoints.Clear();

        foreach (Transform transform in spawnPoints)
            if (transform != spawnPoints)
                this.spawnPoints.Add(transform);
    }

    public void SpawnEnemy(float cyanChance, float magentaChance, float yellowChance)
    {
        EnemyController enemyObject;

        float chance = Random.value;

        if (chance < cyanChance)
        {
            enemyObject = cyanPool.Get();
            enemyObject.SetupEnemy(GetPosition(), playerController.Body);
        }

        if (chance < magentaChance)
        {
            enemyObject = magentaPool.Get();


            //Debug.Log(enemyObject.transform.position);

            enemyObject.SetupEnemy(GetMagentaPos(), playerController.Body);

            //Debug.Log(enemyObject.transform.position);
        }

        if (chance < yellowChance)
        {
            enemyObject = yellowPool.Get();
            enemyObject.transform.SetPositionAndRotation(GetPosition(), Quaternion.identity);
            enemyObject.SetupEnemy(GetPosition(), playerController.Body);
        }

    }

    private Vector3 GetPosition()
    {
        int[] spawnPos = new int[3];

        spawnPos[0] = Random.Range(0, 3);
        spawnPos[1] = Random.Range(3, 6);
        spawnPos[2] = Random.Range(6, 10);

        Vector3 position = spawnPoints[spawnPos[Random.Range(0, 3)]].position;
        position.y = 1.5f;

        //Debug.Log("get position: " + position);

        return position;
    }

    private Vector3 GetMagentaPos()
    {
        Vector3 position = spawnPoints[Random.Range(5, 10)].position;
        
        position.y = 1.5f;

        //Debug.Log("get position: " + position);

        return position;
    }

    public void StartSpawn() => StartCoroutine(SpawnCycle());

    private IEnumerator SpawnCycle()
    {
        yield return new WaitForSeconds(2);

        float spawnSpeed = 2;
        float currentSpawnSpeed = 2;

        while(playerHealth > 0)
        {
            switch (currentScore)
            {
                case < 50:
                    SpawnEnemy(0, 1, 0);
                    //magenta
                    break;
                case < 150:
                    SpawnEnemy(0, 1, 0.8f);
                    //magenta + yellow
                    break;
                case < 300:
                    SpawnEnemy(0.6f, 1, 0.6f);
                    //magenta + yellow + cyan
                    break;
                case < 500:
                    SpawnEnemy(0.7f, 1, 0.7f);
                    currentSpawnSpeed = spawnSpeed / 2f;
                    break;
                case < 700:
                    SpawnEnemy(0.8f, 1, 0.8f);
                    currentSpawnSpeed = spawnSpeed / 2.5f;
                    break;
                case < 1000:
                    SpawnEnemy(0.9f, 1, 0.9f);
                    currentSpawnSpeed = spawnSpeed / 3f;
                    break;
                default:
                    SpawnEnemy(1, 1, 1);
                    currentSpawnSpeed = spawnSpeed / 5f;
                    break;
            }

            yield return new WaitForSeconds(currentSpawnSpeed);
        }
    }

}
