using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Game.Player.Camera
{
    public class MiniMapGenerator : MonoBehaviour
    {
        [SerializeField] private TerrainScreenShot screenshot;
        [SerializeField] private List<ColorPair> miniMapColors = new();
        [SerializeField] private Vector2Int colorBlockSize;
        [SerializeField] private bool useAveragedColors;
        [SerializeField] [Min(1.0f)] private float colorIntensity;

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
                Color minimapColor = Utilities.GetClosestColorToPixelColor(colorBlockAverageColor, miniMapColors);

                SetColorsOfBlock(x, y, outPutMiniMap, terrainHeightMap,
                    useAveragedColors ? colorBlockAverageColor : minimapColor);
            }

            screenshot.gameObject.SetActive(false);

            outPutMiniMap.Apply();
            screenshot.SaveScreenshot(outPutMiniMap, "Minimap");
        }

        private void SetColorsOfBlock(int currentX, int currentY, Texture2D texture, Texture2D heightMap,
            Color colorBlockAverageColor)
        {
            for (int x = 0; x < colorBlockSize.x; x++)
            for (int y = 0; y < colorBlockSize.y; y++)
                texture.SetPixel(currentX + x, currentY + y,
                    colorBlockAverageColor * heightMap.GetPixel(currentX + x, currentY + y) * colorIntensity);
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