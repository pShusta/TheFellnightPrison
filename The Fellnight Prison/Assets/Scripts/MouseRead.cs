using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseRead : MonoBehaviour {

    class point
    {
        public double x;
        public double y;

        public point() { x = 0; y = 0; }
        public point(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
    }

    point last = new point();
    double? direction = null;
    List<point> points = new List<point>();

    float vectorDotProduct(point x, point y)
    {
        float temp1 = (float)x.x * (float)y.x;
        float temp2 = (float)x.y * (float)y.y;
        return (temp1 + temp2);
    }

    float vectorLength(point x)
    {
        float temp1 = (float)x.x * (float)x.x;
        float temp2 = (float)x.y * (float)x.y;
        return Mathf.Sqrt((float)temp1 + (float)temp2);
    }

    float vectorDistance(point x, point y)
    {
        //sqrt(a^2 + b^2)
        float a = Mathf.Abs((float)y.x - (float)x.x);
        float b = Mathf.Abs((float)y.y - (float)x.y);
        return Mathf.Sqrt((a * a) + (b * b));
    }

	void Start () {
	
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MouseRead.Input.GetMouseButtonDown(0)");
            last.x = Input.mousePosition.x;
            last.y = Input.mousePosition.y;
            points = new List<point>();
            //points.Add(new point(last.x, last.y));
        }

        if (Input.GetMouseButton(0))
        {
            //Debug.Log("MouseRead.Input.GetMouseButton(0)");
            float posX = Input.mousePosition.x;
            float posY = Input.mousePosition.y;
            float newDirection;
            point l = new point(posX - last.x, posY - last.y);
            Debug.Log("mousePosition == <" + posX + ", " + posY + ">");
            float dot = vectorDotProduct(new point(1, 0), l);
            Debug.Log("dot == " + dot);
            if(posY > last.y)
            {
                newDirection = Mathf.Deg2Rad * 360 - Mathf.Acos(dot / (vectorLength(l) + 1));
            } else
            {
                newDirection = Mathf.Acos(dot / (vectorLength(l) + 1));
            }
            newDirection = newDirection * 60;
            point tempPoint = null;
            if (Mathf.Abs(newDirection - (float)direction) > 30 && (newDirection - (float)direction) < 330)
            {
                Debug.Log("Direction Change");
                if (tempPoint != null) {
                    if (vectorDistance(new point(posX, posY), tempPoint) > (Screen.width / 100))
                    {
                        Debug.Log("Distance Exceeds Limit");
                        direction = newDirection;
                        last = tempPoint;
                        points.Add(new point(tempPoint.x, tempPoint.y));
                    }
                } else
                {
                    tempPoint = new point(posX, posY);
                }
            } else
            {
                tempPoint = null;
            }

            Debug.Log("direction == " + direction);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //check points for match
            Debug.Log("points contains " + points.Count + " entries.");
            if(points.Count == 2)
            {
                if (points[0].x > points[1].x)
                {
                    //left
                    if (points[0].y > points[1].y)
                    {
                        //down
                        Debug.Log("left, down");
                    } else
                    {
                        //up
                        Debug.Log("left, up");
                    }
                } else
                {
                    //right
                    if (points[0].y > points[1].y)
                    {
                        //down
                        Debug.Log("right, down");
                    }
                    else
                    {
                        //up
                        Debug.Log("right, up");
                    }
                }
            }
        }
	}
}
