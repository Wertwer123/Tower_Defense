using UnityEngine;
using Utils;

namespace Game.CameraComponents
{
    public class Screenshot : MonoBehaviour
    {
        [SerializeField] protected Camera screenshotCamera;
        [SerializeField] protected int screenshotWidth = 300;
        [SerializeField] protected int screenshotHeight = 300;
        [SerializeField] private string screenshotPath;
        [SerializeField] private string screenshotName;

        public Vector2Int ScreenShotDimensions => new(screenshotWidth, screenshotHeight);

        /// <summary>
        ///     Takes a screenshot and saves it at the specified location
        /// </summary>
        /// <returns></returns>
        public Texture2D TakeScreenshot(bool saveAsImage = false)
        {
            RenderTexture screenshotRenderTexture = CreateScreenshotRenderTexture();
            int width = screenshotWidth;
            int height = screenshotHeight;
            Texture2D screenshot = new(width, height,
                TextureFormat.RGBA32, false);
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshot.Apply();

            //Release the render texture from memory
            screenshotRenderTexture.Release();

            if (saveAsImage) Utilities.FileUtils.SaveTexture(screenshot, screenshotPath, screenshotName);

            return screenshot;
        }


        private RenderTexture CreateScreenshotRenderTexture(RenderTextureFormat format = RenderTextureFormat.ARGB32)
        {
            RenderTexture renderedTexture = new(screenshotWidth, screenshotHeight, 24, format);
            RenderTexture.active = renderedTexture;
            screenshotCamera.targetTexture = renderedTexture;
            screenshotCamera.Render();
            screenshotCamera.targetTexture = null;
            return renderedTexture;
        }
    }
}