using UnityEngine;
using System;
public class Clock : MonoBehaviour {
    const float degressPerHour = 30f;
    public Transform hoursTransform;
    public Transform minuteTransform;
    public Transform secondsTransform;
    public bool continucus;
    void Update() {
        if (continucus) {
            UpdateContinucus();
        } else {
            DisupdateContinucus();
        }
    }

    void UpdateContinucus() {
        TimeSpan time = DateTime.Now.TimeOfDay;
            hoursTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalHours * degressPerHour, 0f);
            minuteTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalMinutes * degressPerHour, 0f);
            secondsTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalSeconds * degressPerHour, 0f);
    }

    void DisupdateContinucus() {
        DateTime time = DateTime.Now;
            hoursTransform.localRotation = Quaternion.Euler(0f, time.Hour * degressPerHour, 0f);
            minuteTransform.localRotation = Quaternion.Euler(0f, time.Minute * degressPerHour, 0f);
            secondsTransform.localRotation = Quaternion.Euler(0f, time.Second * degressPerHour, 0f);
    }
}