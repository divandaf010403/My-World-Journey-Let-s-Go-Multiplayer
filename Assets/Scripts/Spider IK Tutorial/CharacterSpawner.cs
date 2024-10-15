using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(prefab, spawnPosition.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
