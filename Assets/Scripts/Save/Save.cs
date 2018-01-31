using System;
using System.Collections.Generic;
using System.IO;

class Save
{
    /// <summary>
    /// Emplacement des sauvegardes du jeu.
    /// </summary>
    public static string SavesPath = "/saves/";

    /// <summary>
    /// Nom de la sauvegarde.
    /// </summary>
    public string SaveName { get; private set; }

    /// <summary>
    /// Dossier contenant la sauvegarde
    /// </summary>
    public string SaveDir { get; private set; }

    /// <summary>
    /// Liste des mondes sauvegardés.
    /// </summary>
    public string[] Dimensions { get; private set; }

    public Save(String saveName)
    {
        this.SaveName = saveName;
        this.SaveDir = SavesPath + saveName;

        if(!Directory.Exists(SaveDir))
        {
            Directory.CreateDirectory(SaveDir);
        }
        else
        {
            Dimensions = Directory.GetFiles(GetWorldsDir());
        }
    }

    public string GetWorldsDir()
    {
        string dir =  SaveDir + "/worlds/";

        if (Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        return dir;
    }

}
