using UnityEngine;

public class CircularTerrainGenerator : MonoBehaviour
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
        RandomizeHeightCurve();

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
        float center = resolution / 2f;

        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                float distanceToCenter = Vector2.Distance(new Vector2(x, z), new Vector2(center, center));
                float normalizedDistance = distanceToCenter / center;

                float xCoord = (float)x / resolution * scale;
                float zCoord = (float)z / resolution * scale;

                float height = CalculateTerrainHeight(xCoord, zCoord) * Mathf.Lerp(1f, 0f, normalizedDistance);
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

    void RandomizeHeightCurve()
    {
        // Generate a random AnimationCurve
        Keyframe[] keys = new Keyframe[5];
        for (int i = 0; i < 5; i++)
        {
            float time = i / 4f;
            float value = Random.Range(0f, 1f);
            keys[i] = new Keyframe(time, value);
        }

        // Create a new AnimationCurve and assign the random keys
        heightCurve = new AnimationCurve(keys);
    }
}
