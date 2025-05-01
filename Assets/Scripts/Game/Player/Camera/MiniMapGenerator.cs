using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game.Player.Camera
{
    public class MiniMapGenerator : MonoBehaviour
    {
        [SerializeField] private int edgeThickness = 4;
        [SerializeField] [Range(0.0f, 1.0f)] private float edgeIntensityThreshold = 0.5f;
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
                Color miniMapFinalColor =
                    Color.Lerp(minimapColor.colorKey, minimapColor.colorValue, averageHeightMapColor.a);

                SetColorsOfBlock(x, y, outPutMiniMap,
                    terrainHeightMap, useAveragedColors ? colorBlockAverageColor : miniMapFinalColor,
                    averageHeightMapColor);
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
            for (int y = 0; y < outPutMiniMap.height; y++)
            {
                if (alreadyColoredPixels.Contains(new Vector2Int(x, y)))
                    continue;

                Color currentPixelColor = outPutMiniMap.GetPixel(x, y);

                bool setUpperPixel = TrySetEdgeColor(outPutMiniMap, x, y + 1, Direction.Up, currentPixelColor,
                    alreadyColoredPixels);
                bool setLowerPixel = TrySetEdgeColor(outPutMiniMap, x, y - 1, Direction.Down, currentPixelColor,
                    alreadyColoredPixels);
                bool setRightPixel = TrySetEdgeColor(outPutMiniMap, x + 1, y, Direction.Right, currentPixelColor,
                    alreadyColoredPixels);
                bool setLeftPixel = TrySetEdgeColor(outPutMiniMap, x - 1, y, Direction.Left, currentPixelColor,
                    alreadyColoredPixels);

                if (setLeftPixel || setRightPixel || setLowerPixel ||
                    (setUpperPixel && !alreadyColoredPixels.Contains(new Vector2Int(x, y))))
                {
                    outPutMiniMap.SetPixel(x, y,
                        edgeColor * Utilities.GetClosestColorToPixelColor(currentPixelColor, miniMapColors).colorValue);
                    alreadyColoredPixels.Add(new Vector2Int(x, y));
                }
            }
        }

        private bool TrySetEdgeColor(Texture2D outPutMiniMap, int x, int y, Direction direction,
            Color currentPixelColor,
            HashSet<Vector2Int> alreadyColoredPixels)
        {
            if (IsOutOfTextureBounds(x, y, outPutMiniMap) ||
                alreadyColoredPixels.Contains(new Vector2Int(x, y)))
                return false;

            Color pixelColor = Utilities.GetClosestColorToPixelColor(outPutMiniMap.GetPixel(x, y), miniMapColors)
                .colorValue;
            Color nearestColorToPixelColor =
                Utilities.GetClosestColorToPixelColor(currentPixelColor, miniMapColors).colorValue;

            float distance = Utilities.GetDistanceBetweenColors(nearestColorToPixelColor, pixelColor);

            if (distance > edgeIntensityThreshold)
            {
                ColorPixelsInLine(x, y, edgeThickness, direction, edgeColor * nearestColorToPixelColor,
                    outPutMiniMap, alreadyColoredPixels);
                return true;
            }

            return false;
        }

        private void ColorPixelsInLine(int currentX, int currentY, int lineThickness, Direction direction, Color color,
            Texture2D outPutMiniMap, HashSet<Vector2Int> alreadyColoredPixels)
        {
            switch (direction)
            {
                case Direction.Up:
                {
                    for (int y = 0; y < lineThickness; y++)
                    {
                        int index = currentY + y;
                        if (!IsOutOfTextureBounds(currentX, index, outPutMiniMap))
                        {
                            outPutMiniMap.SetPixel(currentX, index, color * edgeColor);
                            alreadyColoredPixels.Add(new Vector2Int(currentX, index));
                        }
                    }

                    break;
                }
                case Direction.Down:
                {
                    for (int y = 0; y < lineThickness; y++)
                    {
                        int index = currentY - y;
                        if (!IsOutOfTextureBounds(currentX, index, outPutMiniMap))
                        {
                            outPutMiniMap.SetPixel(currentX, index, color * edgeColor);
                            alreadyColoredPixels.Add(new Vector2Int(currentX, index));
                        }
                    }

                    break;
                }
                case Direction.Left:
                {
                    for (int x = 0; x < lineThickness; x++)
                    {
                        int index = currentX - x;
                        if (!IsOutOfTextureBounds(index, currentY, outPutMiniMap))
                        {
                            outPutMiniMap.SetPixel(index, currentY, color * edgeColor);
                            alreadyColoredPixels.Add(new Vector2Int(index, currentY));
                        }
                    }

                    break;
                }
                case Direction.Right:
                {
                    for (int x = 0; x < lineThickness; x++)
                    {
                        int index = currentX + x;
                        if (!IsOutOfTextureBounds(index, currentY, outPutMiniMap))
                        {
                            outPutMiniMap.SetPixel(index, currentY, color * edgeColor);
                            alreadyColoredPixels.Add(new Vector2Int(index, currentY));
                        }
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
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
                        heightMapColor = colorBlockAverageColorHeightMap;
                    else
                        heightMapColor = heightmap.GetPixel(currentX + x, currentY + y);
                }

                texture.SetPixel(currentX + x, currentY + y,
                    colorBlockAverageColor * heightMapColor);
            }
        }

        private bool IsOutOfTextureBounds(int x, int y, Texture2D texture)
        {
            return y < 0 ||
                   y >= texture.height ||
                   x < 0 ||
                   x >= texture.width;
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

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}