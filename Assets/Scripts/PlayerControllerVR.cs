using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerVR : MonoBehaviour
{
    public Text accelerationInfo;
    private UnityEngine.XR.InputDevice device;
    void Start()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        if (inputDevices.Count > 0)
        {
            device = inputDevices[0];
        }

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}'", device.name));
        }
    }

    void Update()
    {
        Vector3 acceleration;
        if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.centerEyeAcceleration, out acceleration))
        {
            Debug.Log(acceleration.ToString());
            accelerationInfo.text = acceleration.ToString();
        }

        // Quaternion rotation;
        // if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.centerEyeRotation, out rotation))
        // {
        //     accelerationInfo.text = rotation.ToString();
        // }
    }
}
