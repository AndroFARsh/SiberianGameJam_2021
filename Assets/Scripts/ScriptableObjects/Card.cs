using UnityEngine;

public enum ItemType {
    Engine,
    Balloon,
    Gun,
    Generator
}

public enum ActionType
{
    Build,
    Destroy
}

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class Card : ScriptableObject
{
    public GameObject PrefabItemView;

    public ItemType Type;
    public ActionType Action;

    public int Tilt;
    public int Power;
    public int Value;

    public Sprite Image;
    public string Title;
    public string Description;
    public string Parameters;
}
