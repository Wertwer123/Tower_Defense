using UnityEngine;

namespace Game
{
    public class BuildingPreview : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;

        public bool IsActive => gameObject.activeSelf;

        public void Init(Mesh buildingMesh)
        {
            meshFilter.mesh = buildingMesh;
            Debug.Log(meshFilter.mesh);
            Debug.Log(buildingMesh);
            gameObject.SetActive(true);
        }

        public void ChangeSpriteState(bool canPlaceBuilding)
        {
            meshRenderer.material.color = canPlaceBuilding ? Color.green : Color.red;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}