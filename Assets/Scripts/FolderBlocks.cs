using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class FolderBlocks : MonoBehaviour
{
    public string path = "C:\\Users\\jstnm\\Documents";
    DirectoryInfo folder;
    List<GameObject> subfolders;
    FileInfo[] files;
    public GameObject walls;
    public GameObject subfolderPrefab;
    public long minSize = long.MaxValue;
    public long maxSize = 0;
    // Start is called before the first frame update
    void Start()
    {
        folder = new DirectoryInfo(path);
        //subfolders = folder.GetDirectories();
        files = folder.GetFiles();
        subfolders = new List<GameObject>();
        
        long i = 0;
        foreach(DirectoryInfo sfi in folder.EnumerateDirectories())
        {
            if(sfi.Attributes.HasFlag(FileAttributes.Hidden))
                continue;

            long size;
            try { size = GetDirectorySize(sfi); }
            catch { continue; }

            if(size > maxSize)
                maxSize = size;
            if(size < minSize)
                minSize = size;

            float z = 10.0f + 2*(i/4);
            float x = -6.0f + 4.0f*(i%4);
            GameObject sfObject = Instantiate(subfolderPrefab, transform);
            sfObject.transform.localPosition = new Vector3(x, 0.0f, z);
            sfObject.name = sfi.FullName;

            
            Subfolder sfComp = sfObject.GetComponent<Subfolder>();
            sfComp.folderInfo = sfi;
            sfComp.size = size;

            subfolders.Add(sfObject);

            i++;
        }

        foreach(GameObject sfObject in subfolders)
        {
            Vector3 scale = sfObject.transform.localScale;
            float y = (float)(sfObject.GetComponent<Subfolder>().size - minSize) / (float)(maxSize - minSize) * 12.0f + 2.5f;
            scale.y = y;
            sfObject.transform.localScale = scale;
        }

        Vector3 roomScale = walls.transform.localScale;
        roomScale.z = 3.5f * ((subfolders.Count-1)/4 + 1) + 10.0f;
        walls.transform.localScale = roomScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    long GetDirectorySize(DirectoryInfo dir)
    {
        long sum = 0;
        
        foreach(FileInfo file in dir.EnumerateFiles())
        {
            if(file.Attributes.HasFlag(FileAttributes.Hidden))
                continue;

            sum += file.Length;
        }

        try
        {
            foreach(DirectoryInfo folder in dir.EnumerateDirectories())
            {
                if(folder.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                sum += GetDirectorySize(folder);
            }
        }
        catch{}

        return sum;
    }
}
