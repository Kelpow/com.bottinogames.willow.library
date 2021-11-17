using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Willow.IDLUI;

public class IDLUIMaterialSwapper : IDLUIButton.Extension
{
    Material startMaterial;
    [SerializeField] Material highlightMaterial;

    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startMaterial = rend.sharedMaterial;
    }

    protected override void OnGainFocus()
    {
        if (highlightMaterial && rend)
            rend.sharedMaterial = highlightMaterial;

    }

    protected override void OnLoseFocus()
    {
        if (startMaterial && rend)
            rend.sharedMaterial = startMaterial;
    }
}
