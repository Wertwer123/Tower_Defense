using System.Collections.Generic;
using Game.CameraComponents;
using UnityEngine;
using Utils;

namespace Game.Player.Camera
{
    public class TerrainScreenShot : Screenshot
    {
        [SerializeField] [Min(10.0f)] private float screenShotFarClip;
        [SerializeField] [Range(1.1f, 2f)] private float screenShotHeightOvershoot;
        [SerializeField] private int terrainHeight = 2000;
        [SerializeField] private int terrainWidth = 2000;
        [SerializeField] [Min(1)] private int heightSteps = 5;
        [SerializeField] private string terrainScreenShotName;
        [SerializeField] protected LayerMask screenshotLayers;
        [SerializeField] private Transform terrain;
        [SerializeField] private List<ColorPair> heightMappings;

        private void CreateHeightMappings()
        {
            heightMappings.Clear();

            float stepSize = 1.0f / heightSteps;

            for (int i = 0; i < heightSteps; i++)
            {
                Color currentColor = Color.Lerp(Color.gray, Color.white, i * stepSize);
                heightMappings.Add(new ColorPair(currentColor, currentColor));
            }
        }

        /// <summary>
        ///     for this to work the terrain needs to be sized as a quad this should fullfill most usecases
        /// </summary>
        /// <returns></returns>
        public Texture2D TakeTerrainScreenShot(bool saveAsImage = false)
        {
            CreateHeightMappings();

            float sampleStepX = (float)terrainWidth / screenshotWidth;
            float sampleStepY = (float)terrainHeight / screenshotHeight;

            Texture2D screenShot = new(screenshotWidth, screenshotHeight, TextureFormat.RGBA32, false);
            for (int x = 0; x < screenshotWidth; x++)
            for (int y = 0; y < screenshotHeight; y++)
            {
                Vector3 position = terrain.transform.position +
                                   new Vector3(x * sampleStepX, screenShotFarClip, y * sampleStepY);
                Physics.Raycast(position, Vector3.down,
                    out RaycastHit hit,
                    screenShotFarClip * screenShotHeightOvershoot,
                    screenshotLayers);
                Debug.DrawLine(position, Vector3.down * 1000, Color.green, 20.0f);

                Color lerpedHeightColor =
                    Color.Lerp(Color.black, Color.white,
                        hit.distance / (screenShotFarClip * screenShotHeightOvershoot));
                ColorPair heightColor = Utilities.GetClosestColorToPixelColor(lerpedHeightColor, heightMappings);

                screenShot.SetPixel(x, y, heightColor.colorValue);
            }

            if (saveAsImage) SaveScreenshot(screenShot, terrainScreenShotName);

            return screenShot;
        }
    }
}