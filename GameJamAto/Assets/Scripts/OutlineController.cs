using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor;
    [SerializeField] private Color outlineColor;
    private Renderer outlineRenderer;

    private void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
        //outlineRenderer.enabled = true;
    }

    public void EnableOutline(bool enabled)
    {
        if (outlineRenderer)
            outlineRenderer.enabled = enabled;
    }

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        GameObject outlineObject = Instantiate(gameObject, transform.position, transform.rotation, transform);
        outlineObject.transform.localScale = Vector3.one;
        Renderer rend = outlineObject.GetComponent<Renderer>();

        Material[] newMats = new Material[rend.materials.Length];
        for(int i=0;i<newMats.Length;i++)
        {
            newMats[i] = outlineMat;
            newMats[i].SetColor("_OutlinColor", color);
            newMats[i].SetFloat("_Scale", scaleFactor);
        }

        rend.materials = newMats;
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        outlineObject.GetComponent<OutlineController>().enabled = false;
        //outlineObject.GetComponent<Collider>().enabled = false;

        rend.enabled = false;

        return rend;
    }
}
