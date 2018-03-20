using System;
using System.IO;
using UnityEngine;

public class Save
{
    /// <summary>
    /// Emplacement des sauvegardes du jeu.
    /// </summary>
    public const string SavesPath = "saves/";

    /// <summary>
    /// Nom de la sauvegarde.
    /// </summary>
    public string SaveName { get; private set; }

    /// <summary>
    /// Dossier contenant la sauvegarde
    /// </summary>
    public string SaveDir { get; private set; }

    public Save(String saveName)
    {
        this.SaveName = saveName;
        this.SaveDir = SavesPath + saveName;

        if(!Directory.Exists(SaveDir))
            Directory.CreateDirectory(SaveDir);
    }

    public World LoadWorld(int dimensionID)
    {
        string[] lines = File.ReadAllLines(GetWorldDir(dimensionID) + "dim.dat");
        Vector3 playerPosition = new Vector3(Convert.ToInt32(lines[1]), Convert.ToInt32(lines[2]), Convert.ToInt32(lines[3]));

        return new World(this, dimensionID, Settings.loadDistance, playerPosition, Convert.ToInt32(lines[0]));
    }

    public World CreateWorld(int dimensionID, int seed, Vector3 playerPosition)
    {
        string path = GetWorldDir(dimensionID);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return new World(this, dimensionID, Settings.loadDistance, playerPosition, seed);
    }

    public string GetWorldsDir()
    {
        string dir =  SaveDir + "/worlds/";

        if (Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        return dir;
    }

    public string GetWorldDir(int dimensionID)
    {
        return GetWorldsDir() + "DIM_" + dimensionID + "/"; 
    }

}
