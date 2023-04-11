using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Subfolder : MonoBehaviour
{
    public DirectoryInfo folderInfo;

    public long size;

    public Material materialReference;
    public Texture2D regular;
    public Texture2D thick;
    public Texture2D lightTex;
    public GameObject model;
    Material material;

    public void UpdateMaterial()
    {   
        long itemCount = folderInfo.GetDirectories().Length + folderInfo.GetFiles().Length;
        float scale = 1.0f;

        material = Instantiate(materialReference);

        if(itemCount < 5)
            scale = 0.5f;
        else if(itemCount < 10)
            scale = 1.0f;
        else if(itemCount < 20)
            scale = 2.5f;
        else if(itemCount < 40)
            scale = 5.0f;
        else if(itemCount < 80)
            scale = 6.5f;
        else if(itemCount < 160)
            scale = 8.0f;

        if(scale >= 6.5f)
            material.SetTexture("_Texture2D", thick);
        else if(scale <= 1.0f)
            material.SetTexture("_Texture2D", lightTex);
        else
            material.SetTexture("_Texture2D", regular);

        material.SetFloat("_Scale", scale);
        

        model.GetComponent<Renderer>().material = material;
    }
}
