using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RocketMotionSolarSystem : MonoBehaviour
{
    [SerializeField] FlashThrust thrust = null;
    [SerializeField] Params paramObj = null;
    float minY;

    void Start()
    {
        minY = gameObject.transform.position.y;
    }

    void Update()
    {
        Ascend();
    }

    void Ascend()
    {
        Vector3 pos = gameObject.transform.position;
        pos.y = minY + (thrust.Height * paramObj.HeightScale * gameObject.transform.parent.localScale.y);
        gameObject.transform.position = pos;
    }
}
