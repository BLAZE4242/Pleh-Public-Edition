using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "folder", menuName = "BIOS/New Folder")]
public class fileExplorer_folder : ScriptableObject
{
    public fileExplorer_folder[] roots;
    public fileExplorer_file[] files;
}
