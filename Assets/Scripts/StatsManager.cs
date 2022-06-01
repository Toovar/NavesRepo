using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System;

public class StatsManager : MonoBehaviour
{
    public StatsContanier DataContainer;

    string pathCheck;

    private void Awake()
    {
        pathCheck = Application.persistentDataPath + "/NavesData.sav";

        if (System.IO.File.Exists(pathCheck))
            LoadGameStats();
        else
            SaveGameStats();
    }

    public void SaveGameStats()
    {
        DataContainer.AddPlayerDeaths();

        DataContainer.AddEnemyDeaths();

        JObject jSaveGame = new JObject();

        JObject serializedData = DataContainer.Serialize();

        jSaveGame.Add(DataContainer.PlayerDeaths.ToString() + DataContainer.EnemyDeaths.ToString(), serializedData);

        string saveFilePath = Application.persistentDataPath + "/NavesData.sav";

        byte[] encryptSavegame = Encrypt(jSaveGame.ToString());

        File.WriteAllBytes(saveFilePath, encryptSavegame);

        Debug.Log("Saving to: " + saveFilePath);
    }

    void LoadGameStats()
    {
        string saveFilePath = Application.persistentDataPath + "/NavesData.sav";

        Debug.Log("Loading from: " + saveFilePath);

        byte[] decryptedSavegame = File.ReadAllBytes(saveFilePath);

        string jsonString = Decrypt(decryptedSavegame);

        JObject jSaveGame = JObject.Parse(jsonString);

        string dataJsonString = jSaveGame.ToString();

        DataContainer.Deserialize(dataJsonString);

        Debug.Log(dataJsonString);

        int Fromplayer = dataJsonString.IndexOf("\"PlayerDeaths\":") + "\"PlayerDeaths\":".Length;
        int Toplayer = dataJsonString.LastIndexOf("\"EnemyDeaths\"");

        string Playerresult = dataJsonString.Substring(Fromplayer, Toplayer - 7 - Fromplayer);

        int Fromenemies = dataJsonString.IndexOf("\"EnemyDeaths\":") + "\"EnemyDeaths\":".Length;
        int Toenemies = dataJsonString.LastIndexOf("}");

        string Enemiesresult = dataJsonString.Substring(Fromenemies, Toenemies - 3 - Fromenemies);

        DataContainer.PlayerDeaths = Int32.Parse(Playerresult);

        DataContainer.EnemyDeaths = Int32.Parse(Enemiesresult);

        Debug.Log(DataContainer.PlayerDeaths);

        Debug.Log(DataContainer.EnemyDeaths);
    }

    byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    byte[] _initializationVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    byte[] Encrypt(string message)
    {
        AesManaged aes = new AesManaged();

        ICryptoTransform encryptor = aes.CreateEncryptor(_key, _initializationVector);

        MemoryStream memoryStream = new MemoryStream();

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

        StreamWriter streamWriter = new StreamWriter(cryptoStream);

        streamWriter.WriteLine(message);

        streamWriter.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return memoryStream.ToArray();
    }

    string Decrypt(byte[] message)
    {
        AesManaged aes = new AesManaged();

        ICryptoTransform decrypter = aes.CreateDecryptor(_key, _initializationVector);

        MemoryStream memoryStream = new MemoryStream(message);

        CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Read);

        StreamReader streamReader = new StreamReader(cryptoStream);

        string decryptedMessage = streamReader.ReadToEnd();

        streamReader.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return decryptedMessage;
    }
}
