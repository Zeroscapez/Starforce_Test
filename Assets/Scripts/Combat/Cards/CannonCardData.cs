using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Battle Card/Standard/Cannon")]
public class CannonCardData: BattleCard
    {

		public override BattleCardInstance CreateInstance()
		{
			return new CannonInstance(this);
		  }



	}
		
