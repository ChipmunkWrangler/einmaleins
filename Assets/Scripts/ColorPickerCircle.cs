using UnityEngine;

internal class ColorPickerCircle : MonoBehaviour
{
    private Color circleColor = Color.red;
    private Vector3 curBary = Vector3.up;

    private Vector3 curLocalPos;
    [SerializeField] private GameObject pointerLocation;
    [SerializeField] private Collider raycastTarget;

    public Color TheColor { get; private set; } = Color.cyan;

    public void SetNewColor(Color newColor)
    {
        TheColor = newColor;
        float h, s, v;
        Color.RGBToHSV(TheColor, out h, out s, out v);
        circleColor = Color.HSVToRGB(h, 1, 1);
        pointerLocation.transform.localEulerAngles = Vector3.back * (h * 360F);
        curBary.y = 1F - v;
        curBary.x = v * s;
        curBary.z = 1F - curBary.y - curBary.x;
        curLocalPos = Vector3.zero;
    }

    private void Awake()
    {
        float h, s, v;
        Color.RGBToHSV(TheColor, out h, out s, out v);
        SetNewColor(TheColor);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && UpdateCurLocalPos()) CheckCirclePosition();
    }

    private bool UpdateCurLocalPos()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider == raycastTarget)
        {
            curLocalPos = transform.worldToLocalMatrix.MultiplyPoint(hit.point);
            return true;
        }

        return false;
    }

    private void CheckCirclePosition()
    {
        var a = Vector3.Angle(Vector3.left, curLocalPos);
        if (curLocalPos.y < 0) a = 360F - a;
        circleColor = Color.HSVToRGB(a / 360F, 1F, 1F);
        pointerLocation.transform.localEulerAngles = Vector3.back * a;
        SetColor();
    }

    private void SetColor()
    {
        float h, v, s;
        Color.RGBToHSV(circleColor, out h, out v, out s);
        var c = curBary.y > .9999 ? Color.black : Color.HSVToRGB(h, curBary.x / (1F - curBary.y), 1F - curBary.y);
        c.a = 1F;
        TheColor = c;
    }

    private Vector3 Barycentric(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        var bary = Vector3.zero;
        var v0 = b - a;
        var v1 = c - a;
        var v2 = p - a;
        var d00 = Vector3.Dot(v0, v0);
        var d01 = Vector3.Dot(v0, v1);
        var d11 = Vector3.Dot(v1, v1);
        var d20 = Vector3.Dot(v2, v0);
        var d21 = Vector3.Dot(v2, v1);
        var denom = d00 * d11 - d01 * d01;
        bary.y = (d11 * d20 - d01 * d21) / denom;
        bary.z = (d00 * d21 - d01 * d20) / denom;
        bary.x = 1.0F - bary.y - bary.z;
        return bary;
    }
}