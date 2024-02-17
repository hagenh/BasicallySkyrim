using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class SlopeTexturePainter : MonoBehaviour
{
    public Terrain terrain;
    public Texture2D grassTexture;
    public Texture2D stoneTexture;
    public float slopeThreshold = 45f;

    private TerrainData terrainData;

    private void Start()
    {
        terrainData = terrain.terrainData;
        ApplySlopeTextures();
    }

    private void Update()
    {
        terrainData = terrain.terrainData;
        ApplySlopeTextures();
    }

    private void ApplySlopeTextures()
    {
        int textureResolution = terrainData.alphamapResolution;
        float[,] slopes = CalculateSlopes(textureResolution);

        float[, ,] splatMap = new float[textureResolution, textureResolution, 2];

        for (int i = 0; i < textureResolution; i++)
        {
            for (int j = 0; j < textureResolution; j++)
            {
                float slope = slopes[i, j];

                if (slope < slopeThreshold)
                {
                    splatMap[i, j, 0] = 1f;
                    splatMap[i, j, 1] = 0f;
                }
                else
                {
                    splatMap[i, j, 0] = 0f;
                    splatMap[i, j, 1] = 1f;
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatMap);
    }

    private float[,] CalculateSlopes(int resolution)
    {
        int heightResolution = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, heightResolution, heightResolution);

        float[,] slopes = new float[resolution, resolution];

        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                int x = Mathf.RoundToInt((float)i / resolution * heightResolution);
                int y = Mathf.RoundToInt((float)j / resolution * heightResolution);

                float height = heights[x, y];

                float heightDiff = 0f;
                if (x > 0 && y > 0)
                {
                    heightDiff = Mathf.Abs(height - heights[x - 1, y - 1]);
                }

                float slope = Mathf.Rad2Deg * Mathf.Atan(heightDiff / (1f / heightResolution));
                slopes[i, j] = slope;
            }
        }

        return slopes;
    }

}
