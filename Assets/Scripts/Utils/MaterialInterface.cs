using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class MaterialInterface
    {
        [SerializeField] Material material;

        public void SetMaterialColor(string materialParameterName, Color color)
        {
            material.SetColor(materialParameterName, color);
        }

        public void SetFloat(string materialParameterName, float value)
        {
            material.SetFloat(materialParameterName, value);
        }
    }
}