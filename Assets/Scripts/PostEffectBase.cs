using UnityEngine;
using System.Collections;

//The effect is also triggered when not running
[ExecuteInEditMode]
//The post-screen processing special effects generally need to be bound to the camera
[RequireComponent(typeof(Camera))]
//Provide a base class for post-processing, the main function is to drag the shader directly through the Inspector panel to generate the material corresponding to the shader
public class PostEffectBase : MonoBehaviour
{

    //Drag directly into the Inspector panel
    public Shader shader = null;
    private Material _material = null;
    public Material _Material
    {
        get
        {
            if (_material == null)
                _material = GenerateMaterial(shader);
            return _material;
        }
    }

    //Create materials for screen effects based on shader
    protected Material GenerateMaterial(Shader shader)
    {
        if (shader == null)
            return null;
        //Need to judge whether shader supports
        if (shader.isSupported == false)
            return null;
        Material material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        if (material)
            return material;
        return null;
    }

}