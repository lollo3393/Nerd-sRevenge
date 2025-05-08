using UnityEngine;

namespace Script
{
    public class alarmController : MonoBehaviour
    {
        
        [SerializeField] private GameObject[] alarms;


        public void StartAlarm()
        {
            foreach (GameObject alarm in alarms)
            {
                alarm.BroadcastMessage("StartAlarm");
            }
        }
    }
}