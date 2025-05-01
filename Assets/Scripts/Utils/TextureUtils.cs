using UnityEngine;

namespace Utils
{
    public static class TextureUtils
    {
        /// <summary>
        ///     Input is expected to be a 3 times 3 matrix
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="kernel"></param>
        /// <param name="sharpeningStrength"></param>
        /// <returns></returns>
        public static Texture2D SharpenTexture(Texture2D texture, float[,] kernel, float sharpeningStrength)
        {
            int width = texture.width;
            int height = texture.height;

            Color[] originalPixels = texture.GetPixels();
            Color[] resultPixels = new Color[originalPixels.Length];

            // Loop through each pixel (ignoring edges for simplicity)
            for (int y = 1; y < height - 1; y++)
            for (int x = 1; x < width - 1; x++)
            {
                Color finalColor = new(0, 0, 0, 0);

                // Apply kernel to the 3x3 neighborhood
                for (int ky = -1; ky <= 1; ky++)
                for (int kx = -1; kx <= 1; kx++)
                {
                    int pixelX = Mathf.Clamp(x + kx, 0, width - 1);
                    int pixelY = Mathf.Clamp(y + ky, 0, height - 1);

                    Color neighborColor = originalPixels[pixelY * width + pixelX];
                    finalColor += neighborColor * kernel[ky + 1, kx + 1];
                }

                // Clamp the color to valid range [0, 1]
                finalColor.r = Mathf.Clamp01(finalColor.r);
                finalColor.g = Mathf.Clamp01(finalColor.g);
                finalColor.b = Mathf.Clamp01(finalColor.b);
                finalColor.a = 1.0f; // Preserve alpha.

                int currentIndex = y * width + x;
                resultPixels[currentIndex] = Color.Lerp(originalPixels[currentIndex], finalColor, sharpeningStrength);
            }

            texture.SetPixels(resultPixels);

            return texture;
        }

        public static void StylizeTexture(Texture2D source, int levels, float midPointBias, float contrastMultiplier)
        {
            // Get the pixels from the source texture
            Color[] sourcePixels = source.GetPixels();
            Color[] resultPixels = new Color[sourcePixels.Length];

            // Process each pixel
            for (int i = 0; i < sourcePixels.Length; i++)
            {
                Color originalColor = sourcePixels[i];

                // Adjust contrast
                Color contrastedColor = new(
                    Mathf.Clamp01((originalColor.r - midPointBias) * contrastMultiplier + midPointBias),
                    Mathf.Clamp01((originalColor.g - midPointBias) * contrastMultiplier + midPointBias),
                    Mathf.Clamp01((originalColor.b - midPointBias) * contrastMultiplier + midPointBias)
                );

                // Quantize the colors
                Color quantizedColor = new(
                    Mathf.Floor(contrastedColor.r * levels) / levels,
                    Mathf.Floor(contrastedColor.g * levels) / levels,
                    Mathf.Floor(contrastedColor.b * levels) / levels
                );

                resultPixels[i] = quantizedColor;
            }

            // Apply the modified pixels to the new texture
            source.SetPixels(resultPixels);
            source.Apply();
        }

        public static void EnbrightenTexture(Texture2D source, float brightness)
        {
            Color[] sourcePixels = source.GetPixels();
            Color[] resultPixels = new Color[sourcePixels.Length];

            for (int x = 0; x < source.width; x++)
            for (int y = 0; y < source.height; y++)
            {
                Color originalColor = sourcePixels[x + y * source.width];
                Color newColor = originalColor * brightness;
                resultPixels[x + y * source.width] = newColor;
            }

            source.SetPixels(resultPixels);
            source.Apply();
        }
    }
}