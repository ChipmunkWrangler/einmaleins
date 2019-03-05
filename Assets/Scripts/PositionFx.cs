using System.Collections.Generic;
using UnityEngine;

internal class PositionFx : MonoBehaviour
{
    [SerializeField] private Transform referencePoint;

    private void Start()
    {
        Debug.Log("PositionFx");
        StartCoroutine(InitFxPos());
    }

    private IEnumerator<WaitForSeconds> InitFxPos()
    {
        yield return new WaitForSeconds(0.5F); // Grid or Clock buttons need to be established first HACK
        var newPos = Camera.main.ScreenToWorldPoint(referencePoint.position);
        newPos.z = gameObject.transform.position.z;
        gameObject.transform.position = newPos;
    }
}