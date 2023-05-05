using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UIElements;

public class FoldersAndFiles : MonoBehaviour
{
    public string path = "C:\\Users\\jstnm\\Documents";
    DirectoryInfo folder;
    List<GameObject> subfolders;
    List<GameObject> shelves;
    List<GameObject> files;
    public GameObject walls;
    public GameObject subfolderPrefab;
    public GameObject shelfPrefab;
    public GameObject filePrefab;
    public long minSize = long.MaxValue;
    public long maxSize = 0;
    public UIDocument ui;
    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = ui.rootVisualElement;
        Label label = root.Q<Label>("Label");
        label.text = path;

        folder = new DirectoryInfo(path);
        subfolders = new List<GameObject>();
        shelves = new List<GameObject>();
        files = new List<GameObject>();
        
        {
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

                float x = -6.0f + 4.0f*(i%4);
                float z = 10.0f + 2.0f*(i/4);
                GameObject sfObject = Instantiate(subfolderPrefab, transform);
                sfObject.transform.localPosition = new Vector3(x, 0.0f, z);
                sfObject.name = sfi.Name;

                
                Subfolder sfComp = sfObject.GetComponent<Subfolder>();
                sfComp.folderInfo = sfi;
                sfComp.size = size;

                subfolders.Add(sfObject);
                sfComp.UpdateMaterial();

                i++;
            }
        }

        foreach(GameObject sfObject in subfolders)
        {
            Vector3 scale = sfObject.transform.localScale;
            float y = (float)(sfObject.GetComponent<Subfolder>().size - minSize) / (float)(maxSize - minSize) * 7.0f + 2.5f;
            scale.y = y;
            sfObject.transform.localScale = scale;
        }

        foreach(FileInfo fi in folder.EnumerateFiles())
        {
            if(fi.Attributes.HasFlag(FileAttributes.Hidden))
                continue;

            long size;
            try { size = fi.Length; }
            catch { continue; }

            GameObject fObject = Instantiate(filePrefab, transform);
            File fComp = fObject.GetComponent<File>();
            fComp.fileInfo = fi;
            fComp.size = fi.Length;
            fObject.name = fi.Name;

            files.Add(fObject);
        }

        float filesStartZ = 2.0f * ((subfolders.Count-1)/4 + 1) + 14.0f;
        for(int i = 0; i*10 < files.Count; i++)
        {
            float z = filesStartZ + 4.0f * (i/2) ;
            float x = i%2==0 ? -6.0f : 6.0f;
            GameObject shelf = Instantiate(shelfPrefab, transform);
            shelf.transform.localPosition = new Vector3(x, 0.0f, z);
            shelf.name = "Shelf_" + i.ToString();

            shelves.Add(shelf);
        }

        for(int i = 0; i < files.Count; i++)
        {
            Vector3 pos = new Vector3();
            pos.z = i/20 * 4.0f + filesStartZ;
            pos.x = ((i/10)%2==0 ? -9.0f : 3.0f) + i%5 * 1.5f;
            pos.y = (i/5)%2==0 ? 1.125f : 1.875f;

            files[i].transform.localPosition = pos;
        }

        Vector3 roomScale = walls.transform.localScale;
        roomScale.z = 2.0f * ((subfolders.Count-1)/4 + 1) + 4.0f * (shelves.Count/2 + shelves.Count%2) + 15.0f;
        walls.transform.localScale = roomScale;

        Vector3 exitPos = new Vector3(0.0f, 1.33f, roomScale.z);
        transform.Find("Exit").localPosition = exitPos;

        Transform next = transform.Find("Room");
        if(next)
        {
            Vector3 nextPos = next.localPosition;
            nextPos.z = roomScale.z;
            next.localPosition = nextPos;
        }
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
