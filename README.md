# 3D-File-Explorer
The 3D file explorer will allow a user to explore their file system in 3D with similar functionality to the 2D desktop file explorer. 

![overview](https://user-images.githubusercontent.com/54608301/236376024-7767b48a-072a-4c05-88ba-c34dc930cc7d.jpg)

## Concepts

---
### Current Folder
In a 2D file explorer, the contents of the current folder are displayed in a large area of the window as a list or grid. In the 3D file explorer this area is replaced with a 3D environment that can be navigated using WASD and mouse controls. Folders are represented by blocks, and files represented by small spheres. All are labeled as they would be in the 2D file explorer. The subfolders and files are seperated as they would be in the 2D file explorer.

### Subfolders
Subfolders are represented by blocks layed out in a grid. Each subfolder's height represents its size relative to other subfolders within the current folder. Larger folders are taller, and smaller folders are shorter. Each folder also has a grid overlayed on its surface. This grid represents the number of items contained immediately within. A denser grid means a folder has more items, and a sparser grid means it has less items. This is not relative to other folders and is determined by values set in code. Each folder has its name hovering over it that always faces the user.

### Files
![File](https://user-images.githubusercontent.com/54608301/236376045-af3e5bb4-3ca8-4e4a-bfba-138cebf7a04a.jpg)

Files within the current folder are represented by spheres sitting on shelves. Each has its name hovering over it just like the subfolders.

### Item Highlight!
[highlight](https://user-images.githubusercontent.com/54608301/236376067-03805da4-ca43-46b7-a98e-e1f3d0cf4933.jpg)

When looking at an item, a blue highlight will appear around it, and the item's size will be displayed along the bottom of the screen.

### 2D Application with 3D Elements
![desktop](https://user-images.githubusercontent.com/54608301/236375929-e05b433b-cd59-4860-8e9c-a63b29706c7b.jpg)

Many applications using 3D are first and foremost 3D, with 2D UI elements on top. This file explorer is much like the 2D file explorer, but with a 3D element added to it. The 3D environement replaces the area where the current folder's contents usually are. This keeps most of the application simple and familiar. Adding the usual buttons and menus to a 3D world does not provide any benefit here. Showing the files and folders and 3D objects does, however.

## Issues
The controls as they are may not be intuitive to a lot of people. Many people, especially those who are older, may not be familiar with exploring 3D environments, especially in first-person. This application would likely be easier to use for most people if the controls were mouse only. Controls similar to something like Blender would be best. Scrolling to zoom in and out, middle click to pan, right click to rotate. This would also allow the user to move between interaction with the 3D and 2D portions of the application more easily because a key like ESC would not need to be pressed to release control from the first person camera.
