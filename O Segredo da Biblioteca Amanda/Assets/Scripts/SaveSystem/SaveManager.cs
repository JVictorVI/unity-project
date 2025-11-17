using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string path = Application.persistentDataPath + "/progress.json";

    public static void SaveProgress(ProgressData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static ProgressData LoadProgress()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<ProgressData>(json);
        }

        return null;
    }

    public static bool HasSave()
    {
        return File.Exists(path);
    }

    public static void DeleteProgress()
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}
