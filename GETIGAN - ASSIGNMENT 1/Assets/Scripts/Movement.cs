using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform targetLocation;
    public float speed = 10.0f;
    public float angle = 90;
    public int numberOfRays= 17;
    public float rayRange = 10;

    void Update() {
        var deltaPosition = Vector3.zero;
        Vector3 targetDirection = (targetLocation.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(targetLocation.rotation, transform.rotation, angle * Time.deltaTime);

        targetDirection.y = 0.0f;

        for (int i = 0; i < numberOfRays; ++i)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetLocation.position - transform.position);
            var rotationMod = Quaternion.AngleAxis((i /((float)numberOfRays - 1)) * angle * 2 - angle, this.transform.up);
            var directionToTarget = targetRotation * rotationMod * Vector3.forward;
            var ray = new Ray(this.transform.position, directionToTarget);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, rayRange)) {
                Vector3 avoidDirection = Vector3.Cross(Vector3.up, hitInfo.normal);
                avoidDirection.y = 0.0f;
                targetDirection = Vector3.Lerp(targetDirection, avoidDirection, Mathf.Clamp01(hitInfo.distance /20f));
                deltaPosition -= (3.0f / numberOfRays) * speed * directionToTarget;
            }

            else {
                deltaPosition += (3.0f / numberOfRays) * speed * directionToTarget;
            }
        }
        this.transform.position += deltaPosition * Time.deltaTime;

    }
    
}
    

