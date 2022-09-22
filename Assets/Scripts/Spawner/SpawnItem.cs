using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleItem", menuName = "ScriptableObjects/SpawnItem", order = 1)]
public class SpawnItem : ScriptableObject
{
    public GameObject prefab;
    [Range(0, 100f)] public float percentage;
}