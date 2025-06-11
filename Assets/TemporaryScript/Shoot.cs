using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject crosshair;
    [SerializeField] Transform shootPoint;
    [SerializeField] string projectileTag;
    [SerializeField] float minDistance = 2.0f, maxDistance = 6.0f;
    [SerializeField] float chargeTime = 3.0f;
    [SerializeField] float bulletTravelTime = 0.5f;
    [SerializeField] LayerMask targetLayer;
    
    [Header("IMU Settings")]
    [SerializeField] MPU6050Controller imuController;
    [SerializeField] float rollSensitivity = 1.0f;
    [SerializeField] float rollDeadZone = 5.0f; // Ignore small roll values
    [SerializeField] bool useIMUControl = true;
    [SerializeField] bool flipRollDirection = false;
    
    private float holdTime = 0f;
    private float currentRange;
    private Vector2 targetPoint;
    private bool isCharging = false;
    ObjectPool objectPool;
    
    void Start()
    {
        objectPool = ObjectPool.Instance;
        if(crosshair != null) crosshair.SetActive(false);
        
        // Auto-find IMU controller if not assigned
        if(imuController == null)
        {
            imuController = FindObjectOfType<MPU6050Controller>();
            if(imuController == null)
            {
                Debug.LogWarning("[Shoot] No MPU6050Controller found! Using manual control.");
                useIMUControl = false;
            }
        }
    }
    
    void Update()
    {
        if(GameStateManager.Instance.GetCurrentState() == GameStateEnum.Play){
            ShootingMechanism();
        }
    }
   
    void ShootingMechanism(){
        GameObject target = null;
        Vector2 direction = GetShootDirection();
        
        if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.RightShift))
        {
            holdTime += Time.deltaTime;
            float t = Mathf.Clamp01(holdTime / chargeTime);
            currentRange = Mathf.Lerp(minDistance, maxDistance, t);
            isCharging = true;
            
            RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, direction, currentRange, targetLayer);
            if (hit.collider != null)
            {
                targetPoint = hit.point;
                target = hit.transform.gameObject;
            }
            else
            {
                targetPoint = (Vector2)shootPoint.position + direction * currentRange;
            }
            
            crosshair.SetActive(true);
            crosshair.transform.position = targetPoint;
        }
        
        if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.RightShift))
        {
            Fire(target);
            holdTime = 0f;
            isCharging = false;
            crosshair.SetActive(false);
        }
    }
    
    Vector2 GetShootDirection()
    {
        if (useIMUControl && imuController != null)
        {
            // Get roll data from IMU
            float rollAngle = GetRollFromIMU();
            
            // Apply dead zone
            if (Mathf.Abs(rollAngle) < rollDeadZone)
            {
                rollAngle = 0f;
            }
            
            // Apply sensitivity and flip if needed
            rollAngle *= rollSensitivity;
            if (flipRollDirection)
                rollAngle = -rollAngle;
            
            // Convert roll angle to direction vector
            // Roll 0° = right, positive roll = up, negative roll = down
            float angleInRadians = rollAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
            
            return direction.normalized;
        }
        else
        {
            // Fallback to original behavior (based on localScale)
            return transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }
    }
    
    float GetRollFromIMU()
    {
        // Now we can access roll data directly through public property
        return -imuController.Yaw;
    }
    
    void Fire(GameObject target)
    {
        if (projectileTag != null)
        {
            GameObject projectile = objectPool.SpawnFromPool(projectileTag, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTarget(targetPoint, bulletTravelTime);
            GrappleLine.Instance.SetMode(GrappleLineMode.DroneToPlayerToHook, projectile.transform);
        }
    }
    
    void OnDrawGizmos()
    {
        if (shootPoint == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(shootPoint.position, 0.1f);
        
        if (isCharging)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(shootPoint.position, targetPoint);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPoint, 0.2f);
        }
        
        // Show current shooting direction
        if (useIMUControl && imuController != null)
        {
            Vector2 direction = GetShootDirection();
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(shootPoint.position, direction * 2f);
        }
    }
}