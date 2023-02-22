using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Movement : MonoBehaviour
{
    //Originally I had the hazard call ontriggerenter and move the body but it was not working as intended
    //
    [SerializeField] public Vector2 pushDirection {get; set;}
    [SerializeField] GameObject _Pointer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,30);
        pushDirection = Random.insideUnitCircle.normalized;
        float angle = Mathf.Atan2(pushDirection.y, pushDirection.x);
        this._Pointer.transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
    }

}
