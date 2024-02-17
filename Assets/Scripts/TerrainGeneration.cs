using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TerrainGeneration : MonoBehaviour
{
    public Terrain Terrain;
    
    [Header("Terrain Settings 3.0")]
    public int numberOfOctaves = 6;
    public float scale = 2.86f;
    public float persistence = 0.14f;
    public float lacunarity = 5.76f;
    public float baseAmplitude = 0.3f;
    public float baseFrequency = 1.7f;
    public int offsetX;
    public int offsetY;
    
    [Header("Diamond-Sqaure")]
    public int terrainSize = 129;  // Size of the terrain grid
    public float terrainHeight = 50f;  // Maximum height of the terrain
    public float roughness = 0.8f;  // Roughness factor
    private float[,] heightMap;
    
    [Header("Terrain Settings v2.0")]
    //public float scale = 1f;
    public int octaves = 3;
    //public float persistance;
    //public float lacunarity;
    public int seed;
    public Vector2 offset = new Vector2(400, 400);
    
    [Header("Terrain Settings")]
    public int width = 512;
    public int height = 512;
    
    [Header("First layer")]
    public float frequency1 = 0.03f;
    public float amplitude1 = 0.05f;
    public int offset1;

    [Header("Second layer")]
    public float frequency2 = 0.05f;
    public float amplitude2 = 0.04f;
    public int offset2;
    
    [Header("Third layer")]
    public float frequency3 = 0.02f;
    public float amplitude3 = 0.02f;
    public int offset3;

    [Header("Amplifications")]
    public float amplification1 = 1f;
    public float amplification2 = 0.25f;
    public float amplification3 = 0.01f;
    
    private void Start()
    {
        var terrainData = Terrain.terrainData;

        //var emptyHeightMap = ResetHeightmap();
        //terrainData.SetHeights(0,0, emptyHeightMap);

        //var heightmap = GenerateHeightMap2();
        //terrainData.SetHeights(0, 0, heightmap);

        //GenerateMap();
        //InvokeRepeating("DiamondStep2(terrainSize, terrainHeight, roughness)", 1f, 1f);
    }

    public void GenerateMap()
    {
        var terrainData = Terrain.terrainData;
        DiamondStep2(terrainSize, terrainHeight, roughness); 
        //GenerateTerrain2pointOh();
        //var heightmap = GenerateHeightMap2();
        //terrainData.SetHeights(0, 0, heightmap);
    }
    
    /*private void GenerateTerrainDiamond()
    {
        // Create a new terrain data object
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = terrainSize + 1; // Adjusted size
        terrainData.size = new Vector3(terrainSize, heightScale, terrainSize);

        // Generate initial corner heights with random offsets
        float[,] heights = new float[terrainSize + 1, terrainSize + 1]; // Adjusted size

        // Generate initial corner heights with random offsets
        heights[0, 0] = Random.Range(0f, 1f) * heightScale;
        heights[0, terrainSize - 1] = Random.Range(0f, 1f) * heightScale;
        heights[terrainSize - 1, 0] = Random.Range(0f, 1f) * heightScale;
        heights[terrainSize - 1, terrainSize - 1] = Random.Range(0f, 1f) * heightScale;


        int sideLength = terrainSize - 1;
        float roughness = 1f;

        // Perform the Diamond-Square algorithm
        while (sideLength > 1)
        {
            int halfSide = sideLength / 2;

            // Diamond step
            for (int x = halfSide; x < terrainSize; x += sideLength)
            {
                for (int y = halfSide; y < terrainSize; y += sideLength)
                {
                    float average = (heights[x - halfSide, y - halfSide] + heights[x - halfSide, y + halfSide] +
                                     heights[x + halfSide, y - halfSide] + heights[x + halfSide, y + halfSide]) / 4f;

                    heights[x, y] = average + Random.Range(-roughness, roughness) * heightScale;
                }
            }
            
            // Square step
            for (int x = 0; x < terrainSize - 1; x += halfSide)
            {
                for (int y = (x + halfSide) % sideLength; y < terrainSize - 1; y += sideLength)
                {
                    float average = 0f;
                    int count = 0;

                    if (x - halfSide >= 0)
                    {
                        average += heights[x - halfSide, y];
                        count++;
                    }
                    if (x + halfSide < terrainSize)
                    {
                        average += heights[x + halfSide, y];
                        count++;
                    }
                    if (y - halfSide >= 0)
                    {
                        average += heights[x, y - halfSide];
                        count++;
                    }
                    if (y + halfSide < terrainSize)
                    {
                        average += heights[x, y + halfSide];
                        count++;
                    }

                    heights[x, y] = average / count + Random.Range(-roughness, roughness) * heightScale;
                }
            }

            sideLength /= 2;
            roughness *= 0.5f;
        }

        // Normalize heights
        float minHeight = Mathf.Min(heights[0, 0], heights[0, terrainSize - 1],
                                    heights[terrainSize - 1, 0], heights[terrainSize - 1, terrainSize - 1]);
        float maxHeight = Mathf.Max(heights[0, 0], heights[0, terrainSize - 1],
                                    heights[terrainSize - 1, 0], heights[terrainSize - 1, terrainSize - 1]);
        float heightRange = maxHeight - minHeight;

        for (int x = 0; x < terrainSize; x++)
        {
            for (int y = 0; y < terrainSize; y++)
            {
                heights[x, y] = (heights[x, y] - minHeight) / heightRange;
            }
        }

        // Assign the generated heights to the terrain
        terrainData.SetHeights(0, 0, heights);

        // Apply the generated terrain data to the terrain component
        Terrain.terrainData = terrainData;
    }
    */
    
    void GenerateTerrain()
    {
        var width = Terrain.terrainData.heightmapResolution;
        var height = Terrain.terrainData.heightmapResolution;
        var heights = new float[width, height];
        
        offsetX = Random.Range(-100000, 100000);
        offsetY = Random.Range(-100000, 100000);

        for (var x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var amplitude = baseAmplitude;
                var frequency = baseFrequency;
                var noiseHeight = 0f;

                for (var i = 0; i < numberOfOctaves; i++)
                {
                    var xCoord = ((float)x + offsetX) / width * frequency * scale;
                    var yCoord = ((float)y + offsetY) / height * frequency * scale;
                    noiseHeight += Mathf.PerlinNoise(xCoord, yCoord) * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                heights[x, y] = noiseHeight;
            }
        }

        Terrain.terrainData.SetHeights(0, 0, heights);
    }
    
    void GenerateTerrain2pointOh()
    {
        var width = Terrain.terrainData.heightmapResolution;
        var height = Terrain.terrainData.heightmapResolution;
        var heights = new float[width, height];

        var minHeight = -30f;
        var maxHeight = 100f;

        offsetX = Random.Range(-100000, 100000);
        offsetY = Random.Range(-100000, 100000);

        for (var x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var amplitude = baseAmplitude;
                var frequency = baseFrequency;
                var noiseHeight = 0f;

                for (var i = 0; i < numberOfOctaves; i++)
                {
                    var xCoord = ((float)x + offsetX) / width * frequency * scale;
                    var yCoord = ((float)y + offsetY) / height * frequency * scale;

                    // Calculate the height factor based on the normalized height value
                    var normalizedHeight = heights[x, y];
                    var heightFactor = Mathf.Clamp01((normalizedHeight - minHeight) / (maxHeight - minHeight));

                    // Adjust amplitude and frequency based on the height factor
                    amplitude *= 1f - heightFactor;
                    frequency *= 1f + heightFactor;

                    noiseHeight += Mathf.PerlinNoise(xCoord, yCoord) * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                heights[x, y] = noiseHeight;
            }
        }

        Terrain.terrainData.SetHeights(0, 0, heights);
    }

    private float[,] GenerateHeightMap()
    {
        var heightmap = new float[width, height];
        offset1 = Random.Range(-9999999, 9999999);
        offset2 = Random.Range(-9999999, 9999999);
        offset3 = Random.Range(-9999999, 9999999);

        for (var x = 0; x < width; x++)
        {
            for (var z = 0; z < height; z++)
            {
                /*
                 * elevation[y][x] =    1 * noise(1 * nx, 1 * ny);
                 +  0.5 * noise(2 * nx, 2 * ny);
                 + 0.25 * noise(4 * nx, 4 * ny);
                */
                var nx = x / width - 0.5f;
                var ny = z / height - 0.5f;
                
                var noiseValue1 = Mathf.PerlinNoise(x * frequency1 /*+ offset1 */, z * frequency1 /*+ offset1*/) * amplitude1;
                var noiseValue2 = Mathf.PerlinNoise(x * frequency2 /*+ offset2 */, z * frequency2 /*+ offset2*/) * amplitude2;
                var noiseValue3 = Mathf.PerlinNoise(x * frequency3 /*+ offset3 */, z * frequency3 /*+ offset3*/) * amplitude3;

                var finalNoiseValue = noiseValue1 + noiseValue2 + noiseValue3;

                heightmap[x, z] = finalNoiseValue / (amplification1 + amplification2 + amplification3);
            }
        }

        var lol = heightmap.Cast<float>().Max(yolo => yolo);
        Debug.Log("Height == 1: " + lol + " | " + heightmap.Length);

        return heightmap;
    }

    private float[,] GenerateHeightMap2()
    {
        var heightmap = new float[width, height];

        var octaveOffsets = new Vector2[octaves];
        for (var i = 0; i < octaves; i++)
        {
            var offsetX = Random.Range(-100000, 100000) + offset.x;
            var offsetY = Random.Range(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;

        var halfWidth = width / 2f;
        var halfHeight = height / 2f;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                
                for (var i = 0; i < octaves; i++)
                {
                    var sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    var sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y; 

                    var perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                
                
                heightmap[x, y] = noiseHeight;
            } 
        }

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                heightmap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, heightmap[x, y]);
            }
        }

        return heightmap;
    }
    
    private float[,] ResetHeightmap()
    {
        var heightmap = new float[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var z = 0; z < height; z++)
            {
                heightmap[x, z] = 0;
            }
        }

        return heightmap;
    }

    private void DiamondStep2(int size, float height, float roughness)
    {
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = size;

        int randomSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(randomSeed);

        heightMap = new float[size, size];

        // Set corner points
        heightMap[0, 0] = Random.Range(0f, height);
        heightMap[0, size - 1] = Random.Range(0f, height);
        heightMap[size - 1, 0] = Random.Range(0f, height);
        heightMap[size - 1, size - 1] = Random.Range(0f, height);

        int iteration = size - 1;
        float scale = roughness * height;

        while (iteration > 1)
        {
            DiamondStep(iteration, scale);
            SquareStep(iteration, scale);

            iteration /= 2;
            scale *= Mathf.Pow(2, -roughness);
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    private void OnValidate()
    {
        if (width < 1)
        {
            width = 1;
        }

        if (height < 1)
        {
            height = 1;
        }

        if (lacunarity < 1)
        {
            lacunarity = 1;
        }

        if (octaves < 0)
        {
            octaves = 0;
        }
    }
    
    void DiamondStep(int iteration, float scale)
    {
        int step = iteration / 2;

        for (int y = 0; y < terrainSize - 1; y += iteration)
        {
            for (int x = 0; x < terrainSize - 1; x += iteration)
            {
                int halfIteration = iteration / 2;
                int x0 = x;
                int x1 = x + iteration;
                int y0 = y;
                int y1 = y + iteration;

                // Wrap the indices around for tiled terrain
                if (x1 >= terrainSize)
                    x1 = 0;
                if (y1 >= terrainSize)
                    y1 = 0;

                float average = (heightMap[x0, y0] + heightMap[x1, y0] +
                                 heightMap[x0, y1] + heightMap[x1, y1]) / 4f;

                float offset = Random.Range(-scale, scale);
                heightMap[x + step, y + step] = average + offset;
            }
        }
    }

    void SquareStep(int iteration, float scale)
    {
        int step = iteration / 2;

        for (int y = 0; y < terrainSize - 1; y += step)
        {
            for (int x = (y + step) % iteration; x < terrainSize - 1; x += iteration)
            {
                float average = 0f;
                int count = 0;

                if (x - step >= 0)
                {
                    average += heightMap[x - step, y];
                    count++;
                }

                if (x + step < terrainSize)
                {
                    average += heightMap[x + step, y];
                    count++;
                }

                if (y - step >= 0)
                {
                    average += heightMap[x, y - step];
                    count++;
                }

                if (y + step < terrainSize)
                {
                    average += heightMap[x, y + step];
                    count++;
                }

                average /= count;

                float offset = Random.Range(-scale, scale);
                heightMap[x, y] = average + offset;
            }
        }
    }
}
