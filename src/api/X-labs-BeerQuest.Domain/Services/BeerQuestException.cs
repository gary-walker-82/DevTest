using System;

namespace X_labs_BeerQuest.Domain.Services
{
	public abstract class BeerQuestException : Exception
	{
		protected BeerQuestException()
		{
		}

		protected BeerQuestException(string message) : base(message)
		{
		}

		protected BeerQuestException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}