using UnityEngine;

internal class RocketMotionSolarSystem : MonoBehaviour
{
    private float minY;
    [SerializeField] private Params paramObj;
    [SerializeField] private FlashThrust thrust;

    private void Start()
    {
        minY = gameObject.transform.position.y;
    }

    private void Update()
    {
        Ascend();
    }

    private void Ascend()
    {
        var pos = gameObject.transform.position;
        pos.y = minY + thrust.Height * paramObj.HeightScale * gameObject.transform.parent.localScale.y;
        gameObject.transform.position = pos;
    }
}