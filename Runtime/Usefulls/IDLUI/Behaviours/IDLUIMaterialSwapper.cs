using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Willow.IDLUI;

public class IDLUIMaterialSwapper : IDLUIButton.Extension
{
    Material startMaterial;
    [SerializeField] Material highlightMaterial;

    Renderer rend;
    [SerializeField] Renderer[] additionalRenderers;
    Material[] additionalStartMaterials;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if(rend)
            startMaterial = rend.sharedMaterial;
        additionalStartMaterials = new Material[additionalRenderers.Length];
        for (int i = 0; i < additionalRenderers.Length; i++)
        {
            additionalStartMaterials[i] = additionalRenderers[i].sharedMaterial;
        }
    }

    protected override void OnGainFocus()
    {
        if (highlightMaterial)
        {
            if(rend)
                rend.sharedMaterial = highlightMaterial;

            for (int i = 0; i < additionalRenderers.Length; i++)
            {
                additionalRenderers[i].sharedMaterial = highlightMaterial;
            }
        }
    }

    protected override void OnLoseFocus()
    {
        
        if(rend && startMaterial)
            rend.sharedMaterial = startMaterial;

        for (int i = 0; i < additionalRenderers.Length; i++)
        {
            additionalRenderers[i].sharedMaterial = additionalStartMaterials[i];
        }
    }
}
