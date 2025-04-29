using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game.Player.Camera
{
    public class MiniMapGenerator : MonoBehaviour
    {
        [SerializeField] [Min(1.0f)] private float colorIntensity;
        [SerializeField] [Range(0.0f,1.0f)] private float edgeIntensityThreshold = 0.5f;
        [SerializeField] private bool useAveragedColors;
        [SerializeField] private bool useHeightMap;
        [SerializeField] private TerrainScreenShot screenshot;
        [SerializeField] private List<ColorPair> miniMapColors = new();
        [SerializeField] private Color edgeColor = Color.black;
        [SerializeField] private Vector2Int colorBlockSize;
        
        public void GenerateMiniMap()
        {
            screenshot.gameObject.SetActive(true);
            Texture2D screenShot = screenshot.TakeScreenshot(true);
            Texture2D terrainHeightMap = screenshot.TakeTerrainScreenShot(true);
            Texture2D outPutMiniMap = new(screenshot.ScreenShotDimensions.x, screenshot.ScreenShotDimensions.y,
                TextureFormat.RGBA32, false);

            for (int x = 0; x < terrainHeightMap.width; x += colorBlockSize.x)
            for (int y = 0; y < terrainHeightMap.height; y += colorBlockSize.y)
            {
                Color colorBlockAverageColor = GetAverageColorOfBlock(x, y, screenShot);
                Color averageHeightMapColor = GetAverageColorOfBlock(x, y, terrainHeightMap);
                ColorPair minimapColor = Utilities.GetClosestColorToPixelColor(colorBlockAverageColor, miniMapColors);
                Color miniMapFinalColor = Color.Lerp(minimapColor.colorKey, minimapColor.colorValue, averageHeightMapColor.a);
                
                SetColorsOfBlock(x, y, outPutMiniMap,
                    terrainHeightMap, useAveragedColors ? colorBlockAverageColor : miniMapFinalColor, averageHeightMapColor);
            }
            
            DrawEdges(outPutMiniMap, terrainHeightMap);
            
            screenshot.gameObject.SetActive(false);

            outPutMiniMap.Apply();
            screenshot.SaveScreenshot(outPutMiniMap, "Minimap");
        }

        private void DrawEdges(Texture2D outPutMiniMap, Texture2D heightMap)
        {
            HashSet<Vector2Int> alreadyColoredPixels = new();

            for (int x = 0; x < outPutMiniMap.width; x++)
            {
                for (int y = 0; y < outPutMiniMap.height; y++)
                {
                    if(alreadyColoredPixels.Contains(new Vector2Int(x, y)))
                        continue;
                    
                    Color currentPixelColor = outPutMiniMap.GetPixel(x, y);

                    bool setUpperPixel = TrySetEdgeColor(outPutMiniMap, x, y + 1, currentPixelColor, alreadyColoredPixels);
                    bool setLowerPixel = TrySetEdgeColor(outPutMiniMap, x, y + 1, currentPixelColor, alreadyColoredPixels);
                    bool setRightPixel = TrySetEdgeColor(outPutMiniMap,x + 1  , y, currentPixelColor, alreadyColoredPixels);
                    bool setLeftPixel = TrySetEdgeColor(outPutMiniMap,x - 1, y, currentPixelColor, alreadyColoredPixels);

                    if (setLeftPixel || setRightPixel || setLowerPixel || setUpperPixel && !alreadyColoredPixels.Contains(new Vector2Int(x, y)))
                    {
                        outPutMiniMap.SetPixel(x, y, edgeColor);
                        alreadyColoredPixels.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        private bool TrySetEdgeColor(Texture2D outPutMiniMap,int x, int y, Color currentPixelColor, HashSet<Vector2Int> alreadyColoredPixels)
        {
            if (y < 0 ||
                y >= outPutMiniMap.height || 
                x < 0 ||
                x >= outPutMiniMap.width || 
                alreadyColoredPixels.Contains(new Vector2Int(x, y)))
            {
                return false;
            }
            
            Color pixelColor = outPutMiniMap.GetPixel(x, y);
            float distance = Utilities.GetDistanceBetweenColors(currentPixelColor, pixelColor);
           
            if (distance > edgeIntensityThreshold)
            {
                outPutMiniMap.SetPixel(x,y, edgeColor);
                alreadyColoredPixels.Add(new Vector2Int(x, y));
                return true;
            }

            return false;
        }

        private void SetColorsOfBlock(int currentX, int currentY, Texture2D texture, Texture2D heightmap,
            Color colorBlockAverageColor, Color colorBlockAverageColorHeightMap)
        {
            for (int x = 0; x < colorBlockSize.x; x++)
            for (int y = 0; y < colorBlockSize.y; y++)
            {
                Color heightMapColor = Color.white;

                if (useHeightMap)
                {
                    if (useAveragedColors)
                    {
                        heightMapColor = colorBlockAverageColorHeightMap;
                    }
                    else
                    {
                        heightMapColor = heightmap.GetPixel(currentX + x, currentY + y);
                    }
                }
                
                texture.SetPixel(currentX + x, currentY + y,
                    colorBlockAverageColor * Random.Range(0.9f, 1.0f));
            }
                
        }

        private Color GetAverageColorOfBlock(int currentX, int currentY, Texture2D texture)
        {
            Color average = Color.clear;

            for (int x = 0; x < colorBlockSize.x; x++)
            for (int y = 0; y < colorBlockSize.y; y++)
            {
                if (x + currentX > texture.width || y + currentY > texture.height)
                    continue;

                Color color = texture.GetPixel(currentX + x, currentY + y);
                average += color;
            }

            return average / (colorBlockSize.x * colorBlockSize.y);
        }
    }
}