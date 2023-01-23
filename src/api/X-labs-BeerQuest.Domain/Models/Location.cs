namespace X_labs_BeerQuest.Domain.Models
{
	public class Location
	{
		public string Name { get; set; }
		public LocationCategory Category { get; set; }
		public string? Excerpt { get; set; }
		public string? Thumbnail { get; set; }
		public Address? Address { get; set; }
		public string? Phone { get; set; }
		public SocialPresence? SocialPresence { get; set; }
		public string? TagsString { get; set; }

		public List<string> Tags => string.IsNullOrWhiteSpace(TagsString) is false
			? TagsString?.Split(",").Select(x => x.Trim()).ToList()
			: new List<string>();
	}
}