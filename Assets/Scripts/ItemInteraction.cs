using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemInteraction : MonoBehaviour
{

    Transform selected;
    public UIDocument ui;

    private void Update()
    {
        RaycastHit hitInfo;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 100.0f))
        {
            Transform selection = hitInfo.transform;
            if(selected == selection)
                return;
            else
            {
                if(selected && (selected.GetComponentInParent<Subfolder>() || selected.GetComponentInParent<File>()))
                    selected.parent.Find("Highlight").gameObject.SetActive(false);

                selected = selection;
            }

            if(!selected)
                return;

            VisualElement itemInfo = ui.rootVisualElement.Q<VisualElement>("ItemInfo");
            Label label = itemInfo.Q<Label>("Size");

            Subfolder subfolder = selected.GetComponentInParent<Subfolder>();
            File file = selected.GetComponentInParent<File>();
            string unit = " B";
            if(subfolder)
            {
                selected.parent.Find("Highlight").gameObject.SetActive(true);
                float size = (float)subfolder.size;
                if(size > 1024*1024*1024)
                {
                    size /= 1024*1024*1024;
                    unit = " GiB";
                }
                else if(size > 1024*1024)
                {
                    size /= 1024*1024;
                    unit = " MiB";
                }
                else if(size > 1024)
                {
                    size /= 1024;
                    unit = " KiB";
                }


                label.text = "Size: " + size.ToString("0.00") + unit;
            }
            else if(file)
            {
                selected.parent.Find("Highlight").gameObject.SetActive(true);
                float size = (float)file.size;
                if(size > 1024*1024*1024)
                {
                    size /= 1024*1024*1024;
                    unit = " GiB";
                }
                else if(size > 1024*1024)
                {
                    size /= 1024*1024;
                    unit = " MiB";
                }
                else if(size > 1024)
                {
                    size /= 1024;
                    unit = " KiB";
                }


                label.text = "Size: " + size.ToString("0.00") + unit;
            }
            else
                label.text = "Size: ";

        }
    }
}
