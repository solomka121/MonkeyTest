using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveScore(Score score)
    {
        BinaryFormatter foramtter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/score.data";
        using(FileStream fileStream = new FileStream(path , FileMode.Create))
        {
            PlayerHighScore data = new PlayerHighScore(score);
            foramtter.Serialize(fileStream, data);
        }

    }

    public static PlayerHighScore LoadScore()
    {
        string path = Application.persistentDataPath + "/score.data";
        if (File.Exists(path))
        {
            BinaryFormatter foramtter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                PlayerHighScore data = foramtter.Deserialize(fileStream) as PlayerHighScore;
                return data;
            }
        }
        else
        {
            Debug.LogError("Path not found in " + path);
            return null;
        }
    }

    public static void SaveCoins(int coins)
    {
        BinaryFormatter foramtter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/coins.data";
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            PlayerCoins data = new PlayerCoins(coins);
            foramtter.Serialize(fileStream, data);
        }

    }

    public static PlayerCoins LoadCoins()
    {
        string path = Application.persistentDataPath + "/coins.data";
        if (File.Exists(path))
        {
            BinaryFormatter foramtter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                PlayerCoins data = foramtter.Deserialize(fileStream) as PlayerCoins;
                return data;
            }
        }
        else
        {
            Debug.LogError("Path not found in " + path);
            return null;
        }
    }

}
