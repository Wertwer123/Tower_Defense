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
        [SerializeField] [Min(1)] private int heightSteps = 5;
        [SerializeField] private string terrainScreenShotName;
        [SerializeField] protected LayerMask screenshotLayers;
        [SerializeField] private Terrain terrain;
        [SerializeField] private Material screenshotMaterial;
        [SerializeField] private List<ColorPair> heightMappings;

        private void CreateHeightMappings()
        {
            heightMappings.Clear();

            float stepSize = 1.0f / heightSteps;
            float alphaSteps = 1.0f / heightSteps;
            
            for (int i = 0; i < heightSteps; i++)
            {
                Color currentColor = Color.Lerp(Color.white, Color.black, i * stepSize);
                currentColor.a = Mathf.Lerp(1.0f, 0.0f,  i * alphaSteps / 1.0f);
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

            Material defaultTerrainMaterial = terrain.materialTemplate;
            terrain.materialTemplate = screenshotMaterial;

            float sampleStepX = terrain.terrainData.size.x / screenshotWidth;
            float sampleStepY = terrain.terrainData.size.z / screenshotHeight;

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

                Color lerpedHeightColor =
                    Color.Lerp(Color.black, Color.white,
                        hit.distance / (screenShotFarClip * screenShotHeightOvershoot));
                ColorPair heightColor = Utilities.GetClosestColorToPixelColor(lerpedHeightColor, heightMappings);

                screenShot.SetPixel(x, y, heightColor.colorValue);
            }

            if (saveAsImage) SaveScreenshot(screenShot, terrainScreenShotName);

            terrain.materialTemplate = defaultTerrainMaterial;
            return screenShot;
        }
    }
}