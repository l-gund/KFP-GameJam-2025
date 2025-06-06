#nullable enable

using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// Kenneth - To be honest, most of the code for spawners is from here:
// https://www.youtube.com/watch?v=SELTWo1XZ0c
public class SpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject? baseObject;
    [SerializeField] private int ticks;
    [SerializeField] private float range;
    [SerializeField] private int maxObjects;

    private readonly DeferredActionManager deferredActionManager = new();

    void Start()
    {
        if (baseObject == null)
        {
            return;
        }

        SpawnSettings spawnSettings = new SpawnSettings(ticks, maxObjects);

        deferredActionManager.Defer(
            spawnSettings.GetTicks(),
            () => Spawn(baseObject, spawnSettings, new())
        );
    }

    void FixedUpdate()
    {
        deferredActionManager.OnFixedUpdate();
    }

    private void Spawn(
        GameObject baseObject,
        SpawnSettings spawnSettings,
        HashSet<GameObject> spawnedObjects
    )
    {
        // Clean up destroyed game objects
        List<GameObject> spawnedObjectsList = spawnedObjects.ToList();
        foreach (GameObject spawnedObject in spawnedObjectsList)
        {
            if (spawnedObject.IsDestroyed())
            {
                spawnedObjects.Remove(spawnedObject);
            }
        }

        if (spawnedObjects.Count() < spawnSettings.GetMaxObjects())
        {
            // TODO: Make sure the spawn vector isn't inside a wall
            //  Maybe factor for the player too, but I can't math right now
            GameObject newSpawnedObject = Instantiate(
                baseObject,
                new Vector3(
                    Random.Range(-range, range),
                    Random.Range(-range, range),
                    0
                ),
                Quaternion.identity
            );
            newSpawnedObject.SetActive(true);

            spawnedObjects.Add(newSpawnedObject);
        }

        deferredActionManager.Defer(
            spawnSettings.GetTicks(),
            () => Spawn(baseObject, spawnSettings, spawnedObjects)
        );
    }
}