using Unity.VisualScripting;
using UnityEngine;

namespace Script
{
    public class alarmController : MonoBehaviour
    {
        
        [SerializeField] private GameObject[] alarms;
        [SerializeField] private Canvas timerCanvas;

        public void StartAlarm()
        {
            foreach (GameObject alarm in alarms)
            {
                alarm.BroadcastMessage("StartAlarm");
                
            }
            timerCanvas.gameObject.SetActive(true);
            timerCanvas.BroadcastMessage("attivaTimer");
        }
    }
}