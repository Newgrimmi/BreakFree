public class DamageAmplifier
{
    public enum AmplifierType
    {
        PLUS_CLICK_DAMAGE,
        CLICK_CRIT,
        PLUS_PASSIVE_DAMAGE,
        PLUS_CLICK_CRIT_DAMAGE,
        REDUCE_CREATE_TIME
    }

    public AmplifierType Type {get; private set; }

    public float Value => InitValue + IncreasePerLevel * Level;
    public float InitValue;
    public float IncreasePerLevel;

    public int Price => InitPrice + IncreasePricePerLevel * Level;
    public int InitPrice;
    public int IncreasePricePerLevel;

    public int Level;
    public int MaxLevel;
    public int Chance;

    public DamageAmplifier(AmplifierType type, float value, float increase, int initPrice, int priceIncrease, int maxLevel, int chance = 100)
    {
        Type = type;
        InitValue = value;
        IncreasePerLevel = increase;
        InitPrice = initPrice;
        IncreasePricePerLevel = priceIncrease;
        MaxLevel = maxLevel;
        Chance = chance;
    }

    public void LevelUp()
    {
        Level++;
    }
}
