using Game.CameraComponents;
using UnityEngine;
using Utils;

namespace Game.Player.Camera
{
    public class MiniMapGenerator : MonoBehaviour
    {
        [SerializeField] private string miniMapOutputPath;
        [SerializeField] private float centerKernelSharpening;
        [SerializeField] [Range(0.1f, 1.0f)] private float sharpeningStrength;
        [SerializeField] [Range(1, 100)] private int colorLevels;
        [SerializeField] [Range(1, 20)] private float contrastMultiplier;
        [SerializeField] [Range(0.1f, 1.0f)] private float midPointBias;
        [SerializeField] [Range(1.0f, 5.0f)] private float brightness;
        [SerializeField] private bool sharpenTexture;
        [SerializeField] private bool stylizeTexture;
        [SerializeField] private Screenshot screenshot;

        [SerializeField] [Tooltip("How pixelized the resulting minimap will look")]
        private Vector2Int colorBlockSize;

        public void GenerateMiniMap()
        {
            screenshot.gameObject.SetActive(true);
            Texture2D screenShot = screenshot.TakeScreenshot(true);
            Texture2D outPutMiniMap = new(screenshot.ScreenShotDimensions.x, screenshot.ScreenShotDimensions.y,
                TextureFormat.RGBA32, false);

            for (int x = 0; x < outPutMiniMap.width; x += colorBlockSize.x)
            for (int y = 0; y < outPutMiniMap.height; y += colorBlockSize.y)
            {
                Color colorBlockAverageColor = GetAverageColorOfBlock(x, y, screenShot);
                SetColorsOfBlock(x, y, outPutMiniMap, colorBlockAverageColor);
            }

            TextureUtils.EnbrightenTexture(outPutMiniMap, brightness);

            float neighborKernelValue = (centerKernelSharpening - 1) / 4;

            float[,] kernel = new float[3, 3]
            {
                { 0, -neighborKernelValue, 0 },
                { -neighborKernelValue, centerKernelSharpening, -neighborKernelValue },
                { 0, -neighborKernelValue, 0 }
            };

            if (stylizeTexture)
                TextureUtils.StylizeTexture(outPutMiniMap, colorLevels, midPointBias, contrastMultiplier);
            if (sharpenTexture) TextureUtils.SharpenTexture(outPutMiniMap, kernel, sharpeningStrength);


            screenshot.gameObject.SetActive(false);

            outPutMiniMap.Apply();

            Utilities.FileUtils.SaveTexture(outPutMiniMap, miniMapOutputPath, "Minimap");
        }

        private void SetColorsOfBlock(int currentX, int currentY, Texture2D texture,
            Color colorBlockAverageColor)
        {
            for (int x = 0; x < colorBlockSize.x; x++)
            for (int y = 0; y < colorBlockSize.y; y++)
                texture.SetPixel(currentX + x, currentY + y,
                    colorBlockAverageColor);
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
    }
}