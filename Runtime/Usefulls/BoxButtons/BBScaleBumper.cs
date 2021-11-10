using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBScaleBumper : BoxButton.Extension
{
    
    [SerializeField] private float scaler = 1f;

    private Vector3 startingLocalScale;

    private void Start()
    {
        startingLocalScale = transform.localScale;
    }

    protected override void OnGainHover()
    {
        transform.localScale = startingLocalScale * scaler;
    }

    protected override void OnLoseHover()
    {
        transform.localScale = startingLocalScale;
    }
}
