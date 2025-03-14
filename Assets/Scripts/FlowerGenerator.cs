using UnityEngine;
using System.Collections.Generic;

public class FlowerGenerator : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject prefab;       
    public Transform parent;       
    public int spawnCount = 10;     
    
    [Header("Materials")]
    [Tooltip("Array of materials to choose from (ideally 2).")]
    public Material[] materials; 

    [Header("Spawn Area Settings")]
    public Vector2 spawnArea = new Vector2(10f, 10f);
    public int maxAttempts = 20;    

    private List<Vector3> spawnedPositions = new List<Vector3>();

    private float checkRadius = 0.5f;

    void Start()
    {
        Collider prefabCollider = prefab.GetComponent<Collider>();
        if (prefabCollider != null)
        {
            checkRadius = prefabCollider.bounds.extents.magnitude;
        }
        else
        {
            Debug.LogWarning("Prefab does not have a collider. Using default check radius.");
        }

        SpawnPrefabs();
    }
    
    void SpawnPrefabs()
    {
        int spawned = 0;
        int attempts = 0;
        
        while (spawned < spawnCount && attempts < spawnCount * maxAttempts)
        {
            float x = Random.Range(-spawnArea.x, spawnArea.x);
            float z = Random.Range(-spawnArea.y, spawnArea.y);
            Vector3 candidatePos = new Vector3(x, 0f, z);  // assuming Y=0 for ground placement

            Collider[] hitColliders = Physics.OverlapSphere(candidatePos, checkRadius);
            if (hitColliders.Length == 0)
            {
                GameObject instance = Instantiate(prefab, candidatePos, Quaternion.identity, parent);
                spawnedPositions.Add(candidatePos);

                Renderer rend = instance.GetComponent<Renderer>();
                if (rend != null)
                {
                    // Choose one of the provided materials randomly if available
                    if (materials != null && materials.Length > 0)
                    {
                        int randomMatIndex = Random.Range(0, materials.Length);
                        rend.material = materials[randomMatIndex];
                    }

                    // Create a MaterialPropertyBlock to assign random shader parameters per instance
                    MaterialPropertyBlock block = new MaterialPropertyBlock();
                    
                    block.SetFloat("_HueShift", Random.Range(0f, 360f));      // HueShift in degrees (0 to 360)
                    block.SetFloat("_Saturation", Random.Range(0f, 2f));       // Saturation (0 to 2)
                    block.SetFloat("_Brightness", Random.Range(0f, 5f));       // Brightness (0 to 5)
                    block.SetFloat("_CellShading", Random.Range(0f, 2f));      // CellShading (0 to 2)
                    block.SetFloat("_CellShadingAlpha", Random.Range(0f, 1f));   // CellShadingAlpha (0 to 1)
                    block.SetFloat("_RimLightPower", Random.Range(0f, 1f));      // RimLightPower (0 to 1)
                    block.SetFloat("_RimLightDistance", Random.Range(0f, 3f));   // RimLightDistance (0 to 3)
                    block.SetFloat("_RimLightAlpha", Random.Range(0f, 1f));      // RimLightAlpha (0 to 1)
                    
                    Color randomRimColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    block.SetColor("_RimLightColor", randomRimColor);
                    
                    rend.SetPropertyBlock(block);
                }
                
                spawned++;
            }
            attempts++;
        }
        
        if (spawned < spawnCount)
        {
            Debug.LogWarning("Could not spawn all prefabs without overlapping after maximum attempts.");
        }
    }
}
