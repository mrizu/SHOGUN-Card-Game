using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Card/New Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public int Value = 1;
    public int Cost = 1;
    public Color CardColor;
    public String Description = "default text";
    public String CardName = "default name";
    public Sprite CardImage;
}
