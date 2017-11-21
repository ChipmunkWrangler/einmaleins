using UnityEngine;
using System.Collections;

public class ColorPickerCircle : MonoBehaviour {

    public Color TheColor = Color.cyan;

    const float MainRadius = .8f;

	[SerializeField] GameObject PointerMain = null;

    private Plane MyPlane;
    private Vector3[] RPoints;
    private Vector3 CurLocalPos;
    private Vector3 CurBary = Vector3.up;
    private Color CircleColor = Color.red;

	// Use this for initialization
	void Awake () {
        float h, s, v;
        Color.RGBToHSV(TheColor, out h, out s, out v);
//        Debug.Log("HSV = " + v.ToString() + "," + h.ToString() + "," + v.ToString() + ", color = " + TheColor.ToString());
        MyPlane = new Plane(transform.TransformDirection(Vector3.forward), transform.position);
        RPoints = new Vector3[3];
        SetNewColor(TheColor);
    }
	
	void Update () {
		if (Input.GetMouseButtonDown(0) && HasIntersection()) {
            CheckCirclePosition();
        }
    }

    private bool HasIntersection()
    {
        MyPlane = new Plane(transform.TransformDirection(Vector3.forward), transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (MyPlane.Raycast(ray, out rayDistance))
        {
            Vector3 p = ray.GetPoint(rayDistance);
            if (Vector3.Distance(p, transform.position) > MainRadius)
                return false;
            CurLocalPos = transform.worldToLocalMatrix.MultiplyPoint(p);
            return true;
        }

        return false;
    }

    public void SetNewColor(Color NewColor)
    {
        TheColor = NewColor;
        float h, s, v;
        Color.RGBToHSV(TheColor, out h, out s, out v);
        CircleColor = Color.HSVToRGB(h, 1, 1);
        PointerMain.transform.localEulerAngles = Vector3.back * (h * 360f);
        CurBary.y = 1f - v;
        CurBary.x = v * s;
        CurBary.z = 1f - CurBary.y - CurBary.x;
        CurLocalPos = RPoints[0] * CurBary.x + RPoints[1] * CurBary.y + RPoints[2] * CurBary.z;
    }

    private void CheckCirclePosition()
    {
        float a = Vector3.Angle(Vector3.left, CurLocalPos);
        if (CurLocalPos.y < 0)
            a = 360f - a;

        CircleColor = Color.HSVToRGB(a / 360f, 1f, 1f); 
        PointerMain.transform.localEulerAngles = Vector3.back * a;
        SetColor();
    }

    private void SetColor()
    {
        float h, v, s;
        Color.RGBToHSV(CircleColor, out h, out v, out s);
        Color c = (CurBary.y > .9999) ? Color.black : Color.HSVToRGB(h, CurBary.x / (1f - CurBary.y), 1f - CurBary.y);
        TheColor = c;
        TheColor.a = 1f;
    }

    private Vector3 Barycentric(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
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
        float denom = d00 * d11 - d01 * d01;
        bary.y = (d11 * d20 - d01 * d21) / denom;
        bary.z = (d00 * d21 - d01 * d20) / denom;
        bary.x = 1.0f - bary.y - bary.z;
        return bary;
    }
}
