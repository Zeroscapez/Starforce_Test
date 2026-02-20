using UnityEngine;
using System.Collections;

	public abstract class BattleCardInstance
	{
	public BattleCard cardData;

    protected BattleCardInstance(BattleCard card)
	{
		this.cardData = card;
    }

	public virtual void OnUse()
	{

	}
}
