using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Transform pointPrefab;
    public Transform[] points;
    
    [Range(10,100)] 
    public int resclution = 10;

    public GraphFunctionName function;
    
    GraphFunction[] functions = {
        SineFunction,
        Sine2DFunction,
        MultiSineFunction,
        MultiSine2DFunction,
        Ripple,
        Cylinder,
        Sphere,
    };

    private void Awake() {
        float step = 2f / resclution;
        Vector3 scale = Vector3.one * step;
        points = new Transform[resclution * resclution];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    private void Update() {
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        float step = 2f / resclution;
        for (int i = 0, z = 0; z < resclution; z++){
            float v = (z+0.5f) * step - 1f;
            for (int x = 0; x < resclution; x++, i++){
                float u = (x+0.5f) * step - 1f;
                points[i].localPosition = f(u,v,t);
            }
        }
    }

    const float pi = Mathf.PI;

    static Vector3 SineFunction(float x, float z, float t) {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.z = z;
        return p;
    }

    static Vector3 Sine2DFunction(float x, float z, float t) {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x+z+t));
        p.z = z;
        return p;
    }

    static Vector3 MultiSineFunction(float x,float z,float t) {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t) );
        p.y += Mathf.Sin(2f * pi * (x + 2f*t)) * 0.5f;
        p.y *= 2f/3f;
        p.z = z;
        return p;
    }

    static Vector3 MultiSine2DFunction(float x,float z,float t) {
        Vector3 p;
        p.x = x;
        p.z = z;
        p.y = 4f * Mathf.Sin(pi * (x+z+t*0.5f));
        p.y += Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (x + 2f*t)) * 0.5f;
        p.y *= 1f/5.5f;
        return p;
    }

    static Vector3 Ripple(float x,float z,float t) {
        Vector3 p;
        p.x = x;
        p.z = z;
        float d = Mathf.Sqrt(x * x + z * z);
        p.y = Mathf.Sin(pi*(4f*d-t));
        p.y /= 1f + 10f * d;
        return p;
    }
    static Vector3 Cylinder (float u,float v,float t) {
        Vector3 p;
        float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t));
        p.x = r * Mathf.Sin(pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }
    static Vector3 Sphere (float u,float v,float t) {
        Vector3 p;
        float r = Mathf.Cos(pi * 0.5f * v + t);
        p.x = r * Mathf.Sin(pi * u);
        p.y = Mathf.Sin(pi * 0.5f * v);
        p.z = r * Mathf.Cos(pi * u);
        return p;
    }
}
