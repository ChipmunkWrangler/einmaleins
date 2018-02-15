using UnityEngine;
using System.Collections;

public class ColorPickerCircle : MonoBehaviour {

    public Color TheColor = Color.cyan;
	[SerializeField] GameObject PointerMain = null;
	[SerializeField] Collider raycastTarget = null;

    private Vector3[] RPoints;
    private Vector3 CurLocalPos;
    private Vector3 CurBary = Vector3.up;
    private Color CircleColor = Color.red;

	// Use this for initialization
	void Awake () {
        float h, s, v;
        Color.RGBToHSV(TheColor, out h, out s, out v);
//        Debug.Log("HSV = " + v.ToString() + "," + h.ToString() + "," + v.ToString() + ", color = " + TheColor.ToString());
        RPoints = new Vector3[3];
        SetNewColor(TheColor);
    }
	
	void Update () {
		if (Input.GetMouseButton (0) && UpdateCurLocalPos()) {
			CheckCirclePosition ();
        }
    }

	private bool UpdateCurLocalPos()
    {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit) && hit.collider == raycastTarget) {
			CurLocalPos = transform.worldToLocalMatrix.MultiplyPoint (hit.point);
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
        PointerMain.transform.localEulerAngles = Vector3.back * (h * 360F);
        CurBary.y = 1F - v;
        CurBary.x = v * s;
        CurBary.z = 1F - CurBary.y - CurBary.x;
        CurLocalPos = RPoints[0] * CurBary.x + RPoints[1] * CurBary.y + RPoints[2] * CurBary.z;
    }

    private void CheckCirclePosition()
    {
        float a = Vector3.Angle(Vector3.left, CurLocalPos);
        if (CurLocalPos.y < 0)
            a = 360F - a;

        CircleColor = Color.HSVToRGB(a / 360F, 1F, 1F); 
        PointerMain.transform.localEulerAngles = Vector3.back * a;
        SetColor();
    }

    private void SetColor()
    {
        float h, v, s;
        Color.RGBToHSV(CircleColor, out h, out v, out s);
        Color c = (CurBary.y > .9999) ? Color.black : Color.HSVToRGB(h, CurBary.x / (1F - CurBary.y), 1F - CurBary.y);
        TheColor = c;
        TheColor.a = 1F;
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
        bary.x = 1.0F - bary.y - bary.z;
        return bary;
    }
}
