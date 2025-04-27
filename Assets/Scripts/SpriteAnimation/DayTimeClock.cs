using Game;
using UnityEngine;
using Utils;

public class DayTimeClock : MonoBehaviour
{
   [SerializeField, Min(0.1f)] private float clockRadius = 1.0f;
   [SerializeField] private Transform rotationRoot;
   [SerializeField] private Transform morningDisplay;
   [SerializeField] private Transform afternoonDisplay;
   [SerializeField] private Transform eveningDisplay;
   [SerializeField] private Transform nightDisplay;

   public void UpdateDayClock(float passedDayPercentage)
   {
      rotationRoot.eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0,0,360), passedDayPercentage);
   }
   
   /// <summary>
   /// Initializes the daytime displays to be at the correct positions
   /// </summary>
   /// <param name="morningPercentage"></param>
   /// <param name="afternoonPercentage"></param>
   /// <param name="eveningPercentage"></param>
   public void Init(float morningPercentage, float afternoonPercentage, float eveningPercentage)
   {
      float fullRotationAmount = 360.0f;

      float angleOfMorning = 0.0f;
      float angleOfAfternoon = fullRotationAmount * morningPercentage;
      float angleOfEvening = angleOfAfternoon + fullRotationAmount * afternoonPercentage;
      float angleOfNight = angleOfEvening + fullRotationAmount * eveningPercentage;
     
      Vector3 center = transform.position;
      
      Vector3 startPositionMorningDisplay = Utilities.GetPositionOnCircleByAngle(angleOfMorning, clockRadius, center);
      Vector3 startPositionAfternoonDisplay = Utilities.GetPositionOnCircleByAngle(angleOfAfternoon, clockRadius, center);
      Vector3 startPositionEveningDisplay = Utilities.GetPositionOnCircleByAngle(angleOfEvening, clockRadius, center);
      Vector3 startPositionNightDisplay = Utilities.GetPositionOnCircleByAngle(angleOfNight, clockRadius, center);
      
      morningDisplay.position = startPositionMorningDisplay;
      afternoonDisplay.position = startPositionAfternoonDisplay;
      eveningDisplay.position = startPositionEveningDisplay;
      nightDisplay.position = startPositionNightDisplay;
   }
}
