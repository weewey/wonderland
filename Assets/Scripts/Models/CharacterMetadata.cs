using System;

[Serializable]
public class CharacterMetadata
{
    public string name;
    public string symbol;
    public string image;
    public int social_index;
    public Attributes[] attributes;
}

[Serializable]
public class Attributes
{
    public string trait_type;
    public int value;
}