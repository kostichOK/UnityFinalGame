using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string objectID;    // уникальный ID
        public GameObject prefab;  // префаб ключа
        public Vector3 defaultPos; // стартовая позиция в этой сцене
    }

    public List<ItemData> itemsToSpawn = new List<ItemData>();

    void Start()
    {
        foreach (var data in itemsToSpawn)
        {
            Vector3 spawnPos;

            // Если ключ уже существует в словарях, берём сохранённую позицию
            if (Item.HasPosition(data.objectID, out Vector3 savedPos))
            {
                spawnPos = savedPos;
            }
            else
            {
                // Иначе используем дефолтную позицию в сцене
                spawnPos = data.defaultPos;
            }

            // ✅ Спавним ключ
            GameObject obj = Instantiate(data.prefab, spawnPos, Quaternion.identity);

            // ✅ Включаем физику только у ключа через корутину
            Item itemComponent = obj.GetComponent<Item>();
            if (itemComponent != null)
            {
                itemComponent.StartCoroutine(itemComponent.EnablePhysicsAfterSpawn());
            }
        }
    }
}