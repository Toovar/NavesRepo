using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase base para la pool de objetos

[System.Serializable]
public class PoolItem
{
    //Objeto que queremos añadir a la pool

    public GameObject prefab;

    //Cantidad de instancias de ese objeto que se van a almacenar

    public int Amount;
}

//Singleton con el que accedemos a la pool de objetos

public class Pool : MonoBehaviour
{
    //Acceso a el script

    public static Pool singleton;

    //Lista de objetos de la pool

    public List<PoolItem> items;

    //Lista de objetos instanciados

    public List<GameObject> pooledItems = new List<GameObject>();

    //Comprobamos que no exista ninguna instancia anterior del singleton

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
    }

    void Start()
    {
        //Por cada objeto presente en la pool de objetos, instancia la cantidad que se haya especificado y se añade a la lista de objetos instanciados

        foreach (PoolItem item in items)
        {
            for (int i = 0; i < item.Amount; i++)
            {
                GameObject obj = Instantiate(item.prefab);

                pooledItems.Add(obj);
            }
        }
    }

    //Metodo de acceso a los objetos de la pool mediante el tag de estos

    public GameObject Get(string tag)
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            if(!pooledItems[i].activeInHierarchy && pooledItems[i].tag == tag)
            {
                return pooledItems[i];
            }
        }

        //Si el tag no coincide, no retorna nada

        return null;
    }
}
