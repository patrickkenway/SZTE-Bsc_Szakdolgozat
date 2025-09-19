using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawner : MonoBehaviour
{
    public enum ObjectType { SmallGem, BigGem , Gem, Enemy};

    public Tilemap tilemap;
    public GameObject[] objectPrefabs; //0=smallgem 1=gem 2=biggem 3=enemy
    public float bigGemProbability = 0.2f;
    public float smallGemProbability = 0.3f;
    public float enemyProbability = 0.4f;
    public int maxObjects = 6;
    public float gemLifeTime = 10f; //only for gems
    public float spawnInterval = 0.5f;

    private List<Vector3> validSpawnPositions = new List<Vector3>();
    private List<GameObject> spawnObjects = new List<GameObject>();
    private bool isSpawning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GatherValidPositions();
        StartCoroutine(SpawnObjectsIfNeeded());
        GameController.OnReset += LevelChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (!tilemap.gameObject.activeInHierarchy)
        {
            LevelChange();
        }
        if(!isSpawning && ActiveObjectCount() < maxObjects)
        {
            StartCoroutine(SpawnObjectsIfNeeded());
        }
    }

    private void LevelChange()
    { 
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        GatherValidPositions();
        DestroyAllSpawnedObjects();
    }

    private int ActiveObjectCount()
    {
        spawnObjects.RemoveAll(item => item == null);
        return spawnObjects.Count;
    }
    private IEnumerator SpawnObjectsIfNeeded()
    {
        isSpawning = true;
        while (ActiveObjectCount() < maxObjects)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
        isSpawning = false;
    }

    private bool PositionHasObject(Vector3 positionToCheck)
    {
        return spawnObjects.Any(checkObj => checkObj && Vector3.Distance(checkObj.transform.position, positionToCheck) < 1.0f);
    }
    private ObjectType RandomObjectType()
    {
        float randomChoice = Random.value;
        if(randomChoice <= enemyProbability)
        {
            return ObjectType.Enemy;
        }
        else if(randomChoice > enemyProbability && randomChoice <= (enemyProbability+bigGemProbability))
        {
            return ObjectType.BigGem;
        }
        else if(randomChoice > (enemyProbability+bigGemProbability) && randomChoice <= (enemyProbability + bigGemProbability + smallGemProbability))
        {
            return ObjectType.SmallGem;
        }
        else
        {
            return ObjectType.Gem;
        }
    }

    private void SpawnObject()
    {
        if (validSpawnPositions.Count == 0) return;
        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        while(!validPositionFound && validSpawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            Vector3 potentialPosition = validSpawnPositions[randomIndex];
            Vector3 leftPosition = potentialPosition + Vector3.left;
            Vector3 rightPosition = potentialPosition + Vector3.right;

            if(!PositionHasObject(leftPosition) && !PositionHasObject(rightPosition))
            {
                spawnPosition = potentialPosition;
                validPositionFound = true;
            }
            validSpawnPositions.RemoveAt(randomIndex);
        }
        if (validPositionFound)
        {
            ObjectType objectType = RandomObjectType();
            GameObject gameObject = Instantiate(objectPrefabs[(int) objectType],spawnPosition,Quaternion.identity);
            spawnObjects.Add(gameObject);

            //destroy gems only after time
            if(objectType != ObjectType.Enemy)
            {
                StartCoroutine(DestroyObjectAfterTime(gameObject, gemLifeTime));
            }
        }
    
    }
    private IEnumerator DestroyObjectAfterTime(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        if (gameObject)
        {
            spawnObjects.Remove(gameObject);
            validSpawnPositions.Add(gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    private void DestroyAllSpawnedObjects()
    {
        foreach (GameObject gameObject in spawnObjects)
        {
            if(gameObject != null)
            {
                Destroy(gameObject);
            }
            
        }
        spawnObjects.Clear();
    }

    private void GatherValidPositions()
    {
        validSpawnPositions.Clear();
        BoundsInt boundsInt = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt);
        Vector3 start = tilemap.CellToWorld(new Vector3Int (boundsInt.xMin, boundsInt.yMin, 0));

        for (int x = 0; x < boundsInt.size.x; x++) {
            for (int y = 0; y < boundsInt.size.y; y++)
            {
                TileBase tile = allTiles[x + y * boundsInt.size.x];
                if(tile != null)
                {
                    Vector3 place = start + new Vector3(x + 0.5f, y + 2f, 0);
                    validSpawnPositions.Add(place);
                }
            }
        }

    }
}
