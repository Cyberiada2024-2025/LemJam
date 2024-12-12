using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LanternGenerator : MonoBehaviour
{
    public GameObject[] LanternPrefabs;
    public Vector3 offset;
    public float RandomOffsetX = 40;
    public float RandomOffsetY = 3;

    public float SpawnInterval = 1;
    private float lastSpawnTime = -1000;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.player.is_started) {
            return;
        }
        
        if (Time.time - lastSpawnTime > SpawnInterval) {
            SpawnLantern();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnLantern()
    {
        var prefab = LanternPrefabs[Random.Range(0,LanternPrefabs.Length)];
        var newPosition = GameManager.Instance.player.transform.position + offset + new Vector3(Random.Range(-RandomOffsetX, RandomOffsetX), Random.Range(-RandomOffsetY, RandomOffsetY), 0);
        // Debug.Log(newPosition);
        var lantern = Instantiate(prefab, newPosition, Quaternion.identity, transform);
        var lanternScript = lantern.GetComponent<LampionMovement>();
        lanternScript.max_height = -offset.y;
        lanternScript.distance_to_activate = 10000000f;
    }
}
