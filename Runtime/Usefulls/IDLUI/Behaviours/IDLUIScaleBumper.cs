using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Willow.IDLUI;
public class IDLUIScaleBumper : IDLUIButton.Extension
{

    [SerializeField] private float scaler = 1f;

    private Vector3 startingLocalScale;

    private void Start()
    {
        startingLocalScale = transform.localScale;
    }

    protected override void OnGainFocus()
    {
        transform.localScale = startingLocalScale * scaler;
    }

    protected override void OnLoseFocus()
    {
        transform.localScale = startingLocalScale;
    }
}
