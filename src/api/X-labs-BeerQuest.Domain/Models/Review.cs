namespace X_labs_BeerQuest.Domain.Models
{
	public class Review
	{
		public Location Location { get; set; }
		public Ratings Ratings { get; set; }
		public DateTimeOffset DateVisited { get; set; }
	}
}