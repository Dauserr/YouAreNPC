using System;
using UnityEngine;

namespace IDosGames
{
	public abstract class PopUp : MonoBehaviour
	{
		public virtual void Set(string message)
		{
		}

		public virtual void Set(string message, string ImagePath)
		{
		}

		public virtual void Set(Action callbackAction = null)
		{
		}

		// Called when popup is closed
		void OnDisable()
		{
			// Resume game when popup closes
			Time.timeScale = 1f;
		}
	}
}