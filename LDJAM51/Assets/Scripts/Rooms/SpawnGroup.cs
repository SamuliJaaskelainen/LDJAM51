using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnGroup : MonoBehaviour {
    [FormerlySerializedAs("easyDifficultyMin")] public float             easyPointsMin = 1.0f;
    [FormerlySerializedAs("easyDifficultyMax")] public float             easyPointsMax = 1.0f;
    [FormerlySerializedAs("hardDifficultyMin")] public float             hardPointsMin = 1.0f;
    [FormerlySerializedAs("hardDifficultyMax")] public float             hardPointsMax = 1.0f;
    private                                            List<Spawnable> spawnables;

    void Awake() {
        spawnables = new List<Spawnable>();
        FindChildSpawnables(transform);
        foreach (var spawnable in spawnables) {
            Debug.Assert(spawnable.points > 0);
            spawnable.gameObject.SetActive(false);     
        }
        
        spawnables.Sort((a, b) => a.points.CompareTo(b.points));
    }

    private void FindChildSpawnables(Transform parent) {
        foreach(Transform child in parent)
        {
            var spawnable = child.GetComponent<Spawnable>();
            if (spawnable != null) {
                spawnables.Add(spawnable);
            }
            if(child.GetComponent<SpawnGroup>() == null) {
                FindChildSpawnables(child);
            }
        }
    }

    public static void Spawn(Transform transform, float difficulty0to1) {
        var spawnGroup = transform.GetComponent<SpawnGroup>();
        if (spawnGroup == null) {
            foreach (Transform child in transform) {
                Spawn(child, difficulty0to1);
            }
        } else if (spawnGroup.enabled) {
            spawnGroup.Spawn(difficulty0to1);
        }
    }
    
    public void Spawn(float difficulty0to1) {
        float min            = Mathf.Lerp(easyPointsMin, hardPointsMin, difficulty0to1);
        float max            = Mathf.Lerp(easyPointsMax, hardPointsMax, difficulty0to1);
        float pointLeft      = Random.Range(min, max) + 1e-4f;
        float startingPoints = pointLeft;

        while (true) {
            while (spawnables.Count > 0 && spawnables[spawnables.Count - 1].points > pointLeft) {
                spawnables.RemoveAt(spawnables.Count - 1);
            }

            if (spawnables.Count == 0) {
                break;
            }
            int index = Random.Range(0, spawnables.Count);
            GameObject spawn = spawnables[index].gameObject;
            spawn.SetActive(true);
            // Debug.LogFormat("Activated {0}", spawn.name);
            Spawn(spawn.transform, difficulty0to1);
            pointLeft -= spawnables[index].points;
            spawnables.RemoveAt(index);
        }
        Debug.LogFormat("Spawned group {0} with {1}/{2} points spent ", name, startingPoints - pointLeft, startingPoints);
    }
}
