using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class File : MonoBehaviour
{
    public FileInfo fileInfo;
    public TMP_Text hoverLabel;

    public void Start()
    {
        hoverLabel.text = fileInfo.Name;
    }

    public long size;
}
