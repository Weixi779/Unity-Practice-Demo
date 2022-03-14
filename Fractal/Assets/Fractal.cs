using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    public int maxDepth; // 最大深度 防止内存溢出
    private int depth;
    public float childScale;
    // private Material[] materials;
    // private void InitializeMaterials()
    // {
    //     materials = new Material[maxDepth + 1];
    //     for (int i = 0; i <= maxDepth; i++)
    //     {
    //         materials[i] = new Material(material);
    //         Color tempColor = Color.Lerp(Color.white, Color.yellow, ((float)i) / maxDepth);
    //         //materials[i].SetColor("_Color", tempColor);
    //         materials[i].SetColor("_Color", Color.magenta);
    //     }
    //     materials[maxDepth].color = Color.magenta;
    // }
    private void Start()
    {
        // if (materials == null)
        // {
        //     InitializeMaterials();
        // }
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        // gameObject.AddComponent<MeshRenderer>().material = materials[depth];
        gameObject.AddComponent<MeshRenderer>().material = material;
        //Color tempColor = Color.Lerp(Color.white, Color.yellow, ((float)depth) / maxDepth);
        // material.SetColor("_Color", Color.magenta);
        material.SetColor("_Color", Color.black);
        // material.SetColor("_Color", tempColor);

        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }
    private static Vector3[] childDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f),
    };
    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
        }
    }
    private void Initialize(Fractal parent, int childIndex)
    {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;

        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];
    }
}
