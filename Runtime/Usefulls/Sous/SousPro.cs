using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SousPro : MonoBehaviour
{
    public SousProLayerManager.Layer layer = SousProLayerManager.Layer.Alpha;

    private void Start() { GetComponent<Camera>().targetTexture = SousProLayerManager.GetTexture(layer); }


}
