#pragma warning disable 649

using UnityEngine;

namespace Game.Models.Inventory
{
    [CreateAssetMenu(menuName = "Item")]
    public class ItemModel : ScriptableObject
    {
        [SerializeField] private int itemId;
        [SerializeField] private string itemName;
        [SerializeField] private Sprite uiIcon;
        [SerializeField] private GameObject prefab;

        public int ItemId => itemId;
        public string ItemName => itemName;
        public Sprite UiIcon => uiIcon;
        public GameObject Prefab => prefab;
    }
}