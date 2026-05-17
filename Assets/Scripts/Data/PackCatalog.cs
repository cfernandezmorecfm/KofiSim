using System;
using System.Collections.Generic; // Para List<T>
using UnityEngine;

[CreateAssetMenu(fileName = "PackCatalog", menuName = "KofiSim/PackCatalog")]
public class PackCatalog : ScriptableObject
{
    [Serializable] // Necesario para que Unity pueda serializar la clase con una List y mostrarla en el Inspector
    public struct PackData
    {
        public string name;
        public float grams;
        public float price;
    }

    public List<PackData> packs;
}   