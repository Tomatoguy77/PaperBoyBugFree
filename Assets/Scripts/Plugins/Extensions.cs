using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Extensions
{
	public static class ExtensionMethods
	{
		public static IEnumerator TypeIn(this ScoreMenuHandlers InputSpace, string Message, float StartDelay, float TypeDelay)
		{
			yield return new WaitForSeconds(StartDelay);

			for(int i = 0; i < Message.Length + 1; ++i)
			{
				InputSpace.EndScoreText.text = Message.Substring(0, i);
				yield return new WaitForSeconds(TypeDelay);
			}
		}
	}
}
