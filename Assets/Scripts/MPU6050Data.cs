using System;
using System.IO.Ports;
using UnityEngine;

[System.Serializable]
public class MPU6050Data
{
    public float accelX, accelY, accelZ;
    public float gyroX, gyroY, gyroZ;
    public float roll, pitch, yaw;

    public override string ToString()
    {
        return $"Accel: ({accelX:F3}, {accelY:F3}, {accelZ:F3}), " +
               $"Gyro: ({gyroX:F3}, {gyroY:F3}, {gyroZ:F3}), " +
               $"Rot: R={roll:F1}° P={pitch:F1}° Y={yaw:F1}°";
    }
}