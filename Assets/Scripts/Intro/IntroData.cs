using UnityEngine;

public enum BackgroundType
{
    Normal,
    Destroyed
}

public enum CharacterType
{
    None,
    Luna,
    Sol
}
public enum CharacterName
{
    Luna,
    Sol
}

[System.Serializable]
public class IntroData
{
    public BackgroundType background;
    public CharacterType character;
    public CharacterName characterName;
    [TextArea(3, 8)]
    public string dialogue;
    public float typingSpeed = 0.03f;
    public bool playExplosion;
}