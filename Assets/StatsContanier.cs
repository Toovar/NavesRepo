using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq; //Para poder usar Json.net y estructuras de datos
using System.IO; //Usar StreamWriter y StreamReader
using System;

public class StatsContanier : MonoBehaviour
{
    Controller player;

    EnemySpawner enemies;

    public int PlayerDeaths = 0;

    public int EnemyDeaths = 0;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Controller>();

        enemies = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    public void AddPlayerDeaths()
    {
        PlayerDeaths = PlayerDeaths + player.DeathCount;
    }

    public void AddEnemyDeaths()
    {
        EnemyDeaths = EnemyDeaths + enemies.DeadEnemy;
    }

    public JObject Serialize()
    {
        string jsonString = JsonUtility.ToJson(this);
        JObject retVal = JObject.Parse(jsonString);
        return retVal;
    }

    public void Deserialize(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }
}
