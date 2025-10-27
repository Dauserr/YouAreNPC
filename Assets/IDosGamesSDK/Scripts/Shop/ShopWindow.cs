using UnityEngine;

namespace IDosGames
{
	public class ShopWindow : MonoBehaviour
	{
		private void OnDisable()
		{
			// Resume game when shop window is disabled
			Time.timeScale = 1f;
		}
		
		private void OnEnable()
		{
			// Freeze game when shop window is enabled
			Time.timeScale = 0f;
		}
	}
}