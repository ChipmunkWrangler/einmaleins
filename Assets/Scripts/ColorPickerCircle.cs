using UnityEngine;

class ColorPickerCircle : MonoBehaviour
{
    [SerializeField] GameObject pointerLocation = null;
    [SerializeField] Collider raycastTarget = null;

    Vector3 curLocalPos;
    Vector3 curBary = Vector3.up;
    Color circleColor = Color.red;

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

    void Awake()
    {
        float h, s, v;
        Color.RGBToHSV(TheColor, out h, out s, out v);
        SetNewColor(TheColor);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && UpdateCurLocalPos())
        {
            CheckCirclePosition();
        }
    }

    bool UpdateCurLocalPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider == raycastTarget)
        {
            curLocalPos = transform.worldToLocalMatrix.MultiplyPoint(hit.point);
            return true;
        }
        return false;
    }

    void CheckCirclePosition()
    {
        float a = Vector3.Angle(Vector3.left, curLocalPos);
        if (curLocalPos.y < 0)
        {
            a = 360F - a;
        }
        circleColor = Color.HSVToRGB(a / 360F, 1F, 1F);
        pointerLocation.transform.localEulerAngles = Vector3.back * a;
        SetColor();
    }

    void SetColor()
    {
        float h, v, s;
        Color.RGBToHSV(circleColor, out h, out v, out s);
        Color c = (curBary.y > .9999) ? Color.black : Color.HSVToRGB(h, curBary.x / (1F - curBary.y), 1F - curBary.y);
        c.a = 1F;
        TheColor = c;
    }

    Vector3 Barycentric(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 bary = Vector3.zero;
        Vector3 v0 = b - a;
        Vector3 v1 = c - a;
        Vector3 v2 = p - a;
        float d00 = Vector3.Dot(v0, v0);
        float d01 = Vector3.Dot(v0, v1);
        float d11 = Vector3.Dot(v1, v1);
        float d20 = Vector3.Dot(v2, v0);
        float d21 = Vector3.Dot(v2, v1);
        float denom = (d00 * d11) - (d01 * d01);
        bary.y = ((d11 * d20) - (d01 * d21)) / denom;
        bary.z = ((d00 * d21) - (d01 * d20)) / denom;
        bary.x = 1.0F - bary.y - bary.z;
        return bary;
    }
}
