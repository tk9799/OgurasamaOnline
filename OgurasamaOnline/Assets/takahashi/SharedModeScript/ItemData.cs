using UnityEngine;

//ScriptableObjectはシリアライズ可能な Unity 独自のクラスで、スクリプトインスタンスからは
//独立して大量の共有データを保存できる
[CreateAssetMenu(menuName ="Item/ItemData")]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    //public Sprite icon;
}
