using System.Text.Json.Serialization;
using Azure.Core.Serialization;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Spatial;
using X_labs_BeerQuest.Domain.Models;

namespace X_labs_BeerQuest.Domain.Services.Search.SearchDocuments
{
	public class ReviewSearchDocument : ISearchDocument<Review>
	{
		[SimpleField(IsKey = true)] public string InternalId { get; set; }

		[SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
		[SimpleField(IsSortable = true, IsFilterable = true)]
		public string Name { get; set; }

		[SimpleField(IsFilterable = true, IsSortable = true)]
		public int Category { get; set; }

		[JsonConverter(typeof(MicrosoftSpatialGeoJsonConverter))]
		[SimpleField(IsFilterable = true, IsSortable = true)]
		public GeographyPoint? Location { get; set; }

		[SimpleField(IsFilterable = true, IsSortable = true)]
		public DateTimeOffset DateVisited { get; set; }


		[SimpleField(IsFilterable = true, IsFacetable = true)]
		public List<string> Tags { get; set; }

		[SimpleField(IsFilterable = true, IsSortable = true)]
		public double Atmosphere { get; set; }

		[SimpleField(IsFilterable = true, IsSortable = true)]
		public double Beer { get; set; }

		[SimpleField(IsFilterable = true, IsSortable = true)]
		public double Amenities { get; set; }

		[SimpleField(IsFilterable = true, IsSortable = true)]
		public double ValueForMoney { get; set; }

		public string? Excerpt { get; set; }
		public string? Thumbnail { get; set; }
		public string? AddressString { get; set; }
		public string? Phone { get; set; }
		public string? Website { get; set; }
		public string? TwitterHandle { get; set; }

		#region Implementation of ISearchDocument<Supplier>

		/// <inheritdoc />
		public object ConvertFrom(Review entity)
		{
			InternalId = Guid.NewGuid().ToString();
			Name = entity.Location.Name;
			Category = (int) entity.Location.Category;
			DateVisited = entity.DateVisited;
			Tags = entity.Location.Tags;
			Atmosphere = entity.Ratings.Atmosphere;
			Beer = entity.Ratings.Beer;
			Amenities = entity.Ratings.Amenities;
			ValueForMoney = entity.Ratings.ValueForMoney;

			Excerpt = entity.Location.Excerpt;
			Thumbnail = entity.Location.Thumbnail;
			AddressString = entity.Location.Address.AddressString;
			Phone = entity.Location.Phone;
			Website = entity.Location.SocialPresence.Website?.ToString();
			TwitterHandle = entity.Location.SocialPresence.TwitterHandle;
			Location = CreateLocationPoint(entity.Location.Address.GeographicPoint);
			return this;
		}

		private GeographyPoint? CreateLocationPoint(GeographicPoint? location)
		{
			if (location == null)
			{
				return null;
			}

			return GeographyPoint.Create(location.Latitude, location.Longitude);
		}

		/// <inheritdoc />
		public Review ConvertTo()
		{
			return new Review
			{
				DateVisited = DateVisited,
				Location = new Location
				{
					Name = Name,
					Excerpt = Excerpt,
					Phone = Phone,
					Thumbnail = Thumbnail,
					Category = (LocationCategory) Category,
					TagsString = string.Join(", ", Tags),
					Address = new Address
					{
						AddressString = AddressString,
						GeographicPoint = new GeographicPoint
							{Latitude = Location.Latitude, Longitude = Location.Longitude}
					}
				},
				Ratings = new Ratings
				{
					Amenities = Amenities,
					Atmosphere = Atmosphere,
					Beer = Beer,
					ValueForMoney = ValueForMoney
				}
			};
		}

		#endregion
	}
}