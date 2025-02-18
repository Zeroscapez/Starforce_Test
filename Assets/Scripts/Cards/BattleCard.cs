using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Battle Card")]
public class BattleCard : ScriptableObject
{
    [Header("Core Properties")]
    public string cardName;
    public Sprite icon;
    public CardGrade grade;
    public CardElement element;
    public int damage;

    [Header("Visual Properties")]
    public Color colorIndicator;
    public Vector2 sizeMultiplier = Vector2.one;
    public GameObject FXPrefab;

    [Header("Game Rules")]
    public bool isWhiteCard;
    public TargetPattern targetPattern;

    public enum CardGrade { Standard, Mega, Giga, White }
    public enum CardElement { Null, Aqua, Elec, Fire, Wood }
    public enum TargetPattern { Single, Column, Row, All, Custom }

    private void OnValidate()
    {
        // Update the color based on the grade.
        switch (grade)
        {
            case CardGrade.Standard:
                // Goldish yellow (approximate RGB: 255,215,0)
                colorIndicator = new Color(1f, 0.843f, 0f);
                break;
            case CardGrade.Mega:
                // Light blue (approximate RGB: 173,216,230)
                colorIndicator = new Color(173f / 255f, 216f / 255f, 230f / 255f);
                break;
            case CardGrade.Giga:
                colorIndicator = Color.red;
                break;
            case CardGrade.White:
                colorIndicator = Color.white;
                break;
        }
    }
}
