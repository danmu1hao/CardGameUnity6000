using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow: MonoBehaviour
{
    public Vector2 startPoint;
    RectTransform arrow;
    public Vector2 endPoint;

    public Vector2 offsetVector;

    float arrowLength;
    Vector2 arrowPosition;
    float theta;

    public float offset = 0.98f;
    // Start is called before the first frame update
    public RectTransform targetRect;
    void Start()
    {
        arrow = transform.GetComponent<RectTransform>();
        /*startPoint =
            Camera.main.WorldToScreenPoint(transform.parent.transform.position);*/
        startPoint = targetRect.position;

        /*
         LogCenter.Log(screenPos);*/
    }


    // Update is called once per frame
    void Update()
    {
        //缓兵之计，这个问题待解决，因为箭头太过于贴近鼠标，导致点击到箭头会被认为是在点击物体，太扯了
        /*
        startPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, GetComponent<RectTransform>().position);
        */


        endPoint = new Vector2(Input.mousePosition.x,Input.mousePosition.y)/* - offsetVector*/;

        arrowLength = Mathf.Sqrt((endPoint.x - startPoint.x) * (endPoint.x - startPoint.x) + (endPoint.y - startPoint.y) * (endPoint.y - startPoint.y));

        theta = Mathf.Atan2((endPoint.y - startPoint.y), (endPoint.x - startPoint.x));

        arrowPosition = new Vector2((endPoint.x - startPoint.x) / 2, (endPoint.y - startPoint.y) / 2);
        arrow.localPosition = offset*arrowPosition;
        arrow.sizeDelta = new Vector2(offset*arrowLength, arrow.sizeDelta.y);
        arrow.localEulerAngles = new Vector3(0.0f, 0.0f, theta * 180 / Mathf.PI);
    }
    public void SetStartPoint(Vector2 _startPoint)
    {
        startPoint = new Vector2(0,0);
        offsetVector = _startPoint;
    }
}
