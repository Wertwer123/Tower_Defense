using Extensions;
using ScriptableObjects.Buildings;
using SpriteAnimation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    ///     Base class for showing general building information can be inherited to show specified versions of this panel
    /// </summary>
    public class BuildingInformationPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buildingNameText;
        [SerializeField] private TextMeshProUGUI buildingDescription;
        [SerializeField] private Image buildingIcon;
        [SerializeField] private Button closeButton;
        [SerializeField] private SpriteAnimTemplate openInformationPanelAnimation;

        private BuildingData _currentlyShownBuildingData;
        private bool _isDisablingItsSelf;

        private void Start()
        {
            closeButton.onClick.AddListener(Disable);
        }

        public virtual void Enable(BuildingData buildingDataToShow, Vector3 position)
        {
            StopAllCoroutines();

            gameObject.SetActive(true);
            _isDisablingItsSelf = false;

            transform.position = position;
            _currentlyShownBuildingData = buildingDataToShow;

            buildingNameText.text = _currentlyShownBuildingData.BuildingName;
            buildingIcon.sprite = _currentlyShownBuildingData.UISprite;
            buildingDescription.text = _currentlyShownBuildingData.Description;

            openInformationPanelAnimation.GetCopy<SpriteAnimTemplate>().PlayAnimation(this);
        }

        public void Disable()
        {
            if (!gameObject.activeSelf || _isDisablingItsSelf) return;

            StopAllCoroutines();
            _isDisablingItsSelf = true;

            SpriteAnimTemplate animationCopy = openInformationPanelAnimation.GetCopy<SpriteAnimTemplate>();
            animationCopy.OnAnimationFinished += () =>
            {
                _isDisablingItsSelf = false;
                gameObject.SetActive(false);
            };
            animationCopy.PlayAnimation(this, true);

            _currentlyShownBuildingData = null;
        }
    }
}