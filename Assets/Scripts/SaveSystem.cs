using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
  public static void storeData()
  {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream stream = new FileStream(cache.gameDataDir(), FileMode.Create);
    PlayerData data = new PlayerData();
    formatter.Serialize(stream, data);
    stream.Close();
  }

  public static PlayerData getSavedData()
  {
    if (File.Exists(cache.gameDataDir()))
    {
      try
      {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(cache.gameDataDir(), FileMode.Open);
        PlayerData data = formatter.Deserialize(stream) as PlayerData;
        stream.Close();
        return data;
      }
      catch (IOException)
      {
        Debug.Log("no saved data found!");
        File.Delete(cache.gameDataDir());
        cache.loadGameData();
        return null;
      }
    }
    else
    {
      Debug.Log("no saved data found!");
      return null;
    }
  }
}
