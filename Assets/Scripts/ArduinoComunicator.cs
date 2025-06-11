using System;
using System.IO.Ports;
using UnityEngine;

public class MPU6050Controller : MonoBehaviour
{
    [Header("Serial Port Settings")]
    public string portName = "COM5";
    public int baudRate = 115200;

    [Header("Target Object")]
    public Transform targetObject;

    [Header("Rotation Settings")]
    public bool invertX = false;
    public bool invertY = false;
    public bool invertZ = false;
    public float rotationSensitivity = 1f;
    public float smoothingFactor = 0.1f;

    [Header("Debug Options")]
    public bool enableDebugLog = true;
    public bool logRawData = false;
    public bool logParsedData = true;

    private SerialPort serialPort;
    private MPU6050Data currentData;
    private Vector3 smoothedRotation;
    private Vector3 calibrationOffset;
    private bool isCalibrated = false;

    // Public properties to access IMU data
    public MPU6050Data CurrentData => currentData;
    public float Roll => currentData?.roll ?? 0f;
    public float Pitch => currentData?.pitch ?? 0f;
    public float Yaw => currentData?.yaw ?? 0f;
    public Vector3 Acceleration => new Vector3(currentData?.accelX ?? 0f, currentData?.accelY ?? 0f, currentData?.accelZ ?? 0f);
    public Vector3 Gyroscope => new Vector3(currentData?.gyroX ?? 0f, currentData?.gyroY ?? 0f, currentData?.gyroZ ?? 0f);

    void Start()
    {
        currentData = new MPU6050Data();
        calibrationOffset = Vector3.zero;

        if (targetObject == null)
            targetObject = transform;

        ConnectToArduino();
    }

    void ConnectToArduino()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 50;
            serialPort.Open();
            Debug.Log("[MPU6050] ✓ Connected to Arduino on " + portName);
        }
        catch (Exception e)
        {
            Debug.LogError($"[MPU6050] ✗ Failed to connect to {portName}: {e.Message}");
        }
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            ReadArduinoData();
            ProcessRotation();
        }

        HandleInput();
    }

    void ReadArduinoData()
    {
        try
        {
            while (serialPort.BytesToRead > 0)
            {
                string data = serialPort.ReadLine().Trim();

                if (logRawData && enableDebugLog)
                    Debug.Log("[MPU6050] Raw Data: " + data);

                if (data.StartsWith("{") && data.EndsWith("}"))
                {
                    ParseJsonData(data);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("[MPU6050] Serial Error: " + e.Message);
        }
    }

    void ParseJsonData(string jsonData)
    {
        try
        {
            jsonData = jsonData.Replace("{", "").Replace("}", "").Replace("\"", "");
            string[] pairs = jsonData.Split(',');

            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    if (float.TryParse(keyValue[1].Trim(), out float value))
                    {
                        switch (key)
                        {
                            case "accelX": currentData.accelX = value; break;
                            case "accelY": currentData.accelY = value; break;
                            case "accelZ": currentData.accelZ = value; break;
                            case "gyroX": currentData.gyroX = value; break;
                            case "gyroY": currentData.gyroY = value; break;
                            case "gyroZ": currentData.gyroZ = value; break;
                            case "roll": currentData.roll = value; break;
                            case "pitch": currentData.pitch = value; break;
                            case "yaw": currentData.yaw = value; break;
                        }
                    }
                }
            }

            if (logParsedData && enableDebugLog)
                Debug.Log("[MPU6050] Updated Data: " + currentData);
        }
        catch (Exception e)
        {
            Debug.LogWarning("[MPU6050] Parsing failed: " + e.Message);
        }
    }

    void ProcessRotation()
    {
        Vector3 targetRotation = new Vector3(
            (invertX ? -currentData.pitch : currentData.pitch),
            (invertY ? -currentData.yaw : currentData.yaw),
            (invertZ ? -currentData.roll : currentData.roll)
        ) * rotationSensitivity;

        if (isCalibrated)
            targetRotation -= calibrationOffset;

        smoothedRotation = Vector3.Lerp(smoothedRotation, targetRotation, smoothingFactor);

        if (targetObject != null)
            targetObject.rotation = Quaternion.Euler(smoothedRotation);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CalibrateRotation();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ResetRotation();
        }
    }

    void CalibrateRotation()
    {
        calibrationOffset = new Vector3(
            (invertX ? -currentData.pitch : currentData.pitch),
            (invertY ? -currentData.yaw : currentData.yaw),
            (invertZ ? -currentData.roll : currentData.roll)
        ) * rotationSensitivity;

        isCalibrated = true;

        if (enableDebugLog)
            Debug.Log($"[MPU6050] Calibrated. Offset: {calibrationOffset}");
    }

    void ResetRotation()
    {
        calibrationOffset = Vector3.zero;
        isCalibrated = false;
        smoothedRotation = Vector3.zero;

        if (targetObject != null)
            targetObject.rotation = Quaternion.identity;

        if (enableDebugLog)
            Debug.Log("[MPU6050] Rotation reset.");
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}