using UnityEngine;

namespace Script
{
    public class AlarmScript : MonoBehaviour
    {
        private AudioSource alarmSound;
        private bool alarm;
        [SerializeField] public float speed= 90f;
        void Start()
        {
            alarmSound = GetComponent<AudioSource>();
        }
        void Update()
        {
            if(alarm)
            {
                transform.Rotate(0f, 0f, speed * Time.deltaTime);
            }
        }

        void StartAlarm()
        {
            alarmSound.Play();
            alarm = true;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        void StopAlarm()
        {
            alarmSound.Stop();
            alarm = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
