using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using UnityEngine.UI;

public class ShowStats : MonoBehaviour
{
    public GameObject MuertesPlayer;

    public Text TextoPlayer;

    public GameObject EnemigosMuertos;

    public Text TextoEnemies;

    public GameObject MensajeError;

    string pathCheck;

    private void Awake()
    {
        pathCheck = Application.persistentDataPath + "/NavesData.sav";

        if (System.IO.File.Exists(pathCheck))
        {
            LoadGameStats();

            MuertesPlayer.SetActive(true);

            EnemigosMuertos.SetActive(true);
        }
        else
            MensajeError.SetActive(true);
    }

    void LoadGameStats()
    {
        string saveFilePath = Application.persistentDataPath + "/NavesData.sav";

        Debug.Log("Loading from: " + saveFilePath);

        byte[] decryptedSavegame = File.ReadAllBytes(saveFilePath);

        string jsonString = Decrypt(decryptedSavegame);

        JObject jSaveGame = JObject.Parse(jsonString);

        string dataJsonString = jSaveGame.ToString();

        int Fromplayer = dataJsonString.IndexOf("\"PlayerDeaths\":") + "\"PlayerDeaths\":".Length;
        int Toplayer = dataJsonString.LastIndexOf("\"EnemyDeaths\"");

        string Playerresult = dataJsonString.Substring(Fromplayer, Toplayer - 7 - Fromplayer);

        int Fromenemies = dataJsonString.IndexOf("\"EnemyDeaths\":") + "\"EnemyDeaths\":".Length;
        int Toenemies = dataJsonString.LastIndexOf("}");

        string Enemiesresult = dataJsonString.Substring(Fromenemies, Toenemies - 3 - Fromenemies);

         TextoPlayer.text = Playerresult;

        TextoEnemies.text = Enemiesresult;
    }

    byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    byte[] _initializationVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

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
