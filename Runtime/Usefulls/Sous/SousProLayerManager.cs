using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SousProLayerManager : MonoBehaviour
{
    // Static
    public enum Layer
    {
        Alpha,
        Beta,
        Gamma,
        Delta,
        Epsilon,
        Zeta
    }

    private static Dictionary<Layer, SousProLayerManager> _managerDictionary;
    public static Dictionary<Layer, SousProLayerManager> managerDictionary
    {
        get
        {
            if (_managerDictionary == null)
                _managerDictionary = new Dictionary<Layer, SousProLayerManager>();
            return _managerDictionary;
        }
    }

    public static RenderTexture GetTexture(Layer layer)
    {
        if (managerDictionary.ContainsKey(layer))
            return managerDictionary[layer].renderTexture;
        else
        {
            GameObject newGO = new GameObject($"{layer} Sous Pro Layer Manager", typeof(SousProLayerManager));
            var newManager = newGO.GetComponent<SousProLayerManager>();
            newManager.layer = layer;

            managerDictionary.Add(layer, newManager);
            return newManager.renderTexture;
        }
    }


    // Instance

    public Layer layer;

    [Min(1)]
    public int width = 320;
    [Min(1)]
    public int height = 240;

    public int depth = 0;

    public bool alphaBlend = false;

    public Shader postProcessEffect;
    private Material postProcessMaterial;

    [SerializeField] bool dontDestroyOnLoad = false;


    private RenderTexture _renderTexture;
    private RenderTexture _screenTexture;
    public RenderTexture renderTexture
    {
        get
        {
            if(_renderTexture == null)
                GenerateRenderTexture();
            return _renderTexture;
        }
    }

    public void SetResolution(int width, int height)
    {
        if(width != this.width || height != this.height)
        {
            this.width = width;
            this.height = height;
            GenerateRenderTexture();
        }
    }

    private void GenerateRenderTexture()
    {
        if (_renderTexture)
            Destroy(_renderTexture);
        if (_screenTexture)
            Destroy(_screenTexture);

        _renderTexture = new RenderTexture(width, height, 24);
        _renderTexture.antiAliasing = 1;
        _renderTexture.filterMode = FilterMode.Point;

        _screenTexture = new RenderTexture(width, height, 24);
        _screenTexture.antiAliasing = 1;
        _screenTexture.filterMode = FilterMode.Point;
    }

    private void Awake()
    {

        postProcessMaterial = new Material(postProcessEffect);


        if(managerDictionary.ContainsKey(layer))
        {
            if (managerDictionary[layer] == null)
            {
                managerDictionary[layer] = this;
            }
            else
            {
                Destroy(this.gameObject);
                Debug.LogWarning($"Destroying LayerManager, as a manager for layer {layer} already exists.");
                return;
            }
        }
        else
        {
            managerDictionary.Add(layer, this);
        }

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(this.gameObject);
    }

    private void LateUpdate()
    {
        Graphics.Blit(renderTexture, _screenTexture, postProcessMaterial);
    }

    private void OnGUI()
    {
        GUI.depth = depth;

        Texture texture = _screenTexture;

        float widthScale = (float)Screen.width / (float)texture.width;
        float scaledHeight = widthScale * texture.height;
        if (scaledHeight > Screen.height)
        {
            float heightScale = (float)Screen.height / (float)texture.height;
            int halfWidth = Screen.width / 2;
            int scaledTexWidth = Mathf.FloorToInt(texture.width * heightScale);
            int texHalfWidth = scaledTexWidth / 2;

            GUI.BeginGroup(new Rect(halfWidth - texHalfWidth, 0, halfWidth - texHalfWidth + scaledTexWidth, Screen.height));
            GUI.DrawTexture(new Rect(0, 0, scaledTexWidth, Screen.height), texture, ScaleMode.StretchToFill, alphaBlend);
            GUI.EndGroup();
        }
        else
        {
            int halfHeight = Screen.height / 2;
            int scaledTexHeight = Mathf.FloorToInt(texture.height * widthScale);
            int texHalfHeight = scaledTexHeight / 2;

            GUI.BeginGroup(new Rect(0, halfHeight - texHalfHeight, Screen.width, halfHeight - texHalfHeight + scaledTexHeight));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, scaledTexHeight), texture, ScaleMode.StretchToFill, alphaBlend);
            GUI.EndGroup();
        }
    }
}
