using UnityEngine;

internal class NumberButtonLayoutController : NumberButtonBaseController
{
    [SerializeField] private Transform multiplierStars;
    [SerializeField] private Transform smallScreenParent;

    protected override void UseNormalButtonLayout()
    {
    }

    protected override void UseCompactButtonLayout()
    {
        while (transform.childCount > 0)
        {
            transform.GetChild(0).SetParent(smallScreenParent);
        }

        if (multiplierStars)
        {
            multiplierStars.localRotation = Quaternion.Euler(0, 0, -90.0F);
        }
    }
}