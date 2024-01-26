using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int terrainSize = 100;
    public float scale = 1f;
    public float terrainHeightMultiplier = 10f;
    public AnimationCurve heightCurve;
    public int iterations = 4;
    public float variation = 2.0f;
    public float roughness = 0.5f;

    void Start()
    {
        GenerateTerrain();
    }

    void OnValidate()
    {
        if (enabled)
        {
            GenerateTerrain();
        }
    }

    void Update()
    {
        // Add Update logic here if needed
    }

    public void SetTerrainSize(int newSize)
    {
        terrainSize = newSize;
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
    }

    TerrainData GenerateTerrainData(TerrainData terrainData)
    {
        terrainData.heightmapResolution = terrainSize + 1;
        terrainData.size = new Vector3(terrainSize, terrainHeightMultiplier, terrainSize);

        terrainData.SetHeights(0, 0, GenerateHeights(terrainData.heightmapResolution));

        return terrainData;
    }

    float[,] GenerateHeights(int resolution)
    {
        float[,] heights = new float[resolution, resolution];

        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                float xCoord = (float)x / resolution * scale;
                float zCoord = (float)z / resolution * scale;

                float height = CalculateTerrainHeight(xCoord, zCoord);
                heights[x, z] = height;
            }
        }

        return heights;
    }

    float CalculateTerrainHeight(float x, float z)
    {
        float height = 0;

        for (int i = 0; i < iterations; i++)
        {
            float xCoord = x * scale * Mathf.Pow(variation, i);
            float zCoord = z * scale * Mathf.Pow(variation, i);

            height += heightCurve.Evaluate(Mathf.PerlinNoise(xCoord, zCoord)) * Mathf.Pow(roughness, i);
        }

        return height;
    }
}
