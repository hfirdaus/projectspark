using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class OrbitVisualizer : MonoBehaviour
{
    public Material m;
    public float thickness;
    public Color color;
    private int segments;
    private float xradius;
    private float zradius;
    private float ypos;
    LineRenderer line;

    public OrbitVisualizer()
    {
    }

    void Start()
    {
    }

    public void drawOrbit(float radius, float y_off, int segments, float thickness)
    {
        this.segments = segments;
        line = gameObject.GetComponent<LineRenderer>();
        Color c1 = color;
        //line.material = new Material(Shader.Find("Particles/Additive"));
        line.startColor = c1;
        line.endColor = c1;
        line.startWidth = thickness;
        line.startWidth = thickness;
        line.positionCount = segments + 1;
        line.material = m;
        line.useWorldSpace = false;
        xradius = radius;
        zradius = radius;
        ypos = y_off;
        //line.useWorldSpace = false;
        CreatePoints();
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z;
        float angle = 0;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * zradius;

            line.SetPosition(i, new Vector3(x, ypos, z));
            angle += (360.5f / segments);
        }
    }
}
