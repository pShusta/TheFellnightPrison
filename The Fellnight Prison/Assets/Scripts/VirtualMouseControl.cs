using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System;

public class MouseOperations
{
    [Flags]
    public enum MouseEventFlags
    {
        LeftDown = 0x00000002,
        LeftUp = 0x00000004,
        MiddleDown = 0x00000020,
        MiddleUp = 0x00000040,
        Move = 0x00000001,
        Absolute = 0x00008000,
        RightDown = 0x00000008,
        RightUp = 0x00000010
    }

    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out MousePoint lpMousePoint);

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    public static void SetCursorPosition(int X, int Y)
    {
        SetCursorPos(X, Y);
    }

    public static void SetCursorPosition(MousePoint point)
    {
        SetCursorPos(point.X, point.Y);
    }

    public static MousePoint GetCursorPosition()
    {
        MousePoint currentMousePoint;
        var gotPoint = GetCursorPos(out currentMousePoint);
        if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
        return currentMousePoint;
    }

    public static void MouseEvent(MouseEventFlags value)
    {
        MousePoint position = GetCursorPosition();
        //Debug.Log("MouseEvent");
        mouse_event
            ((int)value,
             position.X,
             position.Y,
             0,
             0)
            ;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

    }

}

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int X;
    public int Y;

    public POINT(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    //public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) { }

    /*
    public static implicit operator System.Drawing.Point(POINT p)
    {
        return new System.Drawing.Point(p.X, p.Y);
    }

    public static implicit operator POINT(System.Drawing.Point p)
    {
        return new POINT(p.X, p.Y);
    }
    */
}

public class VirtualMouseControl : MonoBehaviour {

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT pos);

    float distancex = 0;
    float distancey = 0;
    bool moving = false;
    List<POINT> points = new List<POINT>();

    // Use this for initialization
    void Start () {
        
    }    

    void populatePoints()
    {
        points.Clear();
        points.Add(new POINT(600, 300));
        points.Add(new POINT(500, 300));
        //points.Add(new POINT(500, 300));
        //points.Add(new POINT(500, 300));
        //points.Add(new POINT(500, 400));
        //points.Add(new POINT(500, 300));
        
    }

    void reachedPoint(bool begin)
    {
        if(!begin) points.RemoveAt(0);
        if (points.Count > 1)
        {
            distancex = points[1].X - points[0].X;
            distancey = points[1].Y - points[0].Y;
        } else
        {
            points.Clear();
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
        }
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("VirtualMouseControl.Input.GetMouseButton(0)");
        } else if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("VirtualMouseControl.Input.GetMouseButtonDown(0)");
        } else if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("VirtualMouseControl.Input.GetMouseButtonUp(0)");
        }
        //if(points.Count != 0) Debug.Log("points[0] == <" + points[0].X + ", " + points[0].Y + ">");
        if (Input.GetKeyUp(KeyCode.Space)) {
            populatePoints();
            SetCursorPos(points[0].X, points[0].Y);
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            reachedPoint(true);
        }
        if (points.Count != 0)
        {
            POINT temp;
            GetCursorPos(out temp);
            int x1, x2;
            int y1, y2;
            int xdist;
            if (points[0].X < points[1].X)
            {
                x1 = points[0].X;
                x2 = points[1].X;
                xdist = (int)Mathf.Ceil(distancex * Time.deltaTime);
            } else
            {
                x2 = points[0].X;
                x1 = points[1].X;
                xdist = (int)Mathf.Floor(distancex * Time.deltaTime);
            }
            int ydist;
            if(points[0].Y < points[1].Y)
            {
                y1 = points[0].Y;
                y2 = points[1].Y;
                ydist = (int)Mathf.Ceil(distancey * Time.deltaTime);
            } else
            {
                y2 = points[0].Y;
                y1 = points[1].Y;
                ydist = (int)Mathf.Floor(distancey * Time.deltaTime);
            }
            SetCursorPos(Mathf.Clamp(temp.X + xdist, x1, x2), Mathf.Clamp(temp.Y + ydist, y1, y2));
            GetCursorPos(out temp);
            //Debug.Log("currentPos == <" + temp.X + ", " + temp.Y + ">");
            //Debug.Log("distancex == " + distancex + " distancey == " + distancey);
            if(temp.X == points[1].X && temp.Y == points[1].Y)
            {
                reachedPoint(false);
            }
        }
    }
}
