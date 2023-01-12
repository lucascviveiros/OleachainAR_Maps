using UnityEngine;
using System.Collections;

//Inherited from PostEffectBase
public class ColorAdjustEffect : PostEffectBase
{

    //Range controls the range of parameters that can be entered
    [Range(0.0f, 3.0f)]
    public float brightness = 1.0f;//Brightness
    [Range(0.0f, 3.0f)]
    public float contrast = 1.0f;//Contrast
    [Range(0.0f, 3.0f)]
    public float saturation = 1.0f;//Saturation

    //Overwrite OnRenderImage function
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //Post-processing is only done when there is a material, if _Material is empty, no post-processing
        if (_Material)
        {
            //You can set the parameter value in shader through Material.SetXXX ("name", value)
            _Material.SetFloat("_ Brightness", brightness);
            _Material.SetFloat("_ Saturation", saturation);
            _Material.SetFloat("_ Contrast", contrast);
            //Use Material to process Texture, dest is not necessarily the screen, post-processing effects can be superimposed!
            Graphics.Blit(src, dest, _Material);
        }
        else
        {
            //Draw directly
            Graphics.Blit(src, dest);
        }
    }
}