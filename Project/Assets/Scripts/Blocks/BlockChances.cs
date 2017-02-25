using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockChances
{
	public static Dictionary<int, Dictionary<string, float>> Chances = new Dictionary<int, Dictionary<string, float>> 
	{ 
		{0, new Dictionary<string, float> 
			{
				{ "air", 0.050f },
				{ "dirt", 0.950f },
				{ "coal", 0.970f },
				{ "bronze", 0.980f },
				{ "iron", 0.990f },
				{ "gold", 0.998f },
				{ "diamond", 1.000f }
			}
		},
		{1, new Dictionary<string, float> 
			{
				{ "air", 0.050f },
				{ "dirt", 0.950f },
				{ "coal", 0.970f },
				{ "bronze", 0.980f },
				{ "iron", 0.990f },
				{ "gold", 0.998f },
				{ "diamond", 0.999f }
			}
		},
		{2, new Dictionary<string, float> 
			{
				{ "air", 0.050f },
				{ "dirt", 0.950f },
				{ "coal", 0.970f },
				{ "bronze", 0.980f },
				{ "iron", 0.990f },
				{ "gold", 0.998f },
				{ "diamond", 0.999f }
			}
		},
		{3, new Dictionary<string, float> 
			{
				{ "air", 0.050f },
				{ "dirt", 0.950f },
				{ "coal", 0.970f },
				{ "bronze", 0.980f },
				{ "iron", 0.990f },
				{ "gold", 0.998f },
				{ "diamond", 0.999f }
			}
		}
	};
}
