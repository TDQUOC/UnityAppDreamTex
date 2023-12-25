using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Alta.Plugin
{
    public class RaycastExtension
    {
        //do raycast from camera
        public static GameObject  CamRayCast(Vector2 input, Camera cam, LayerMask layerMask)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(input);
            if (Physics.Raycast(ray, out hit, 10000, layerMask.value))
            {
                Debug.Log("Did Hit : " + hit.collider.gameObject.name);
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 5);
                return hit.collider.gameObject;
                
            }
            else
            {
                Debug.Log("Did not Hit");
                Debug.DrawRay(ray.origin, ray.direction * 10000, Color.yellow, 5);
                return null;
            }
        }

        //do raycast from specify transfom
        public static GameObject TransformRaycast(Transform target,Vector3 direction,LayerMask layerMask)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(target.position, target.TransformDirection(direction), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(target.position, target.TransformDirection(direction) * hit.distance, Color.green,5);
                Debug.Log("Did Hit : " + hit.collider.gameObject.name);
                return hit.collider.gameObject;
            }
            else
            {
                Debug.DrawRay(target.position, target.TransformDirection(direction) * 1000, Color.yellow,5);
                Debug.Log("Did not Hit");
                return null;
            }
        }
    }
}
