# 3D-File-Explorer
The 3D file explorer will allow a user to explore their file system in 3D with similar functionality to the 2D desktop file explorer. 

## Concepts
---
### Folder
A folder is a directory in the file system that can contain files and other folders. Folders themselves do not have any data and only exist as a concept for presenting files in an organized way for the user. They are based of the real life concept of folders. \
In this application, the currently open folder is the room that the user is inside of and the blocks inside the room are the folders that exist within that folder. The files in the folder will be represented as objects on shelves inside the room.
![folder room](https://user-images.githubusercontent.com/54608301/223931534-1977b5af-82a3-4cea-b291-2cfd870cff32.png)

#### Folders exist within folders
Folders exist within folders. To maintain this concept moving from 2D to 3D, folders can be entered through a doorway that appears on the face of a folder block. This doorway leads into a new room representing the inside of that folder. The doorway at the front of the room will lead back out of the doorway on the folder block inside the parent folder room. Unlike the back button in the desktop file explorer, exiting through the doorway only leads to the parent folder, and cannot lead to the previous folder if the current folder was "jumped" to through some kind of shortcut.

#### Folder size
Folders have a size that is the sum of the sizes of all the files contained within it and its subfolders. Calling it "size" is already an abstraction as data does not take up any physical space on disk. This concept is easily mapped to 3D as the folder blocks can have their physical size scaled with their size on disk. Each folder block will have a different height representing its size relative to other folders. The physical sizes will be based on the minimum and maximum sizes within the current folder and will not be comparable between folders.

---

### File
A files represent pieces of data on the disk with certain properties and permissions and exist inside folders. Unlike folders, files are associated with actual data and take up space on the disk. Files can have different extensions indicating the type of data they contain and what programs should be used to open them. Files also commonly have different visual representations based on their extension such as notepad icon for text files, or a piece of a film reel for a video. Files generally cannot be broken down into smaller pieces with some exceptions (.zip, .docx). \
In this application, files will be represented as objects on shelves within the room representing the current folder. The files will have different representations based on the type (paper for a document, disc or tape for video/audio).

---

### Operations
In a desktop file explorer there are many operations that can be performed on files and folders such as copy, cut, paste, move, and delete. What these operations do will not change in the 3D file explorer.

#### Cut, copy, paste, and the clipboard
When copying or cutting files to later be pasted, files and folders are copied to a clipboard to see what you currently have ready to paste and what you have copied and pasted before. The clipboard will be represented as a physical clipboard that the user can interact with.

#### Cut/move
Cutting and moving are actually the same operation. A file or folder is removed from one location and placed in another. The difference is that moving involves dragging the folder or file to its new location with the mouse, while cutting is done via shortcuts or menu buttons. \
In the 3D file explorer the move operation makes sense for files since they are represented by small objects. The move operation could also be implemented for folders by shrinking them down if the player grabs them.

---

### Todo
Trash, multiple drives, hidden files and folders, tabs, shortcuts, network, opening files...
