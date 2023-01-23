import React, { useEffect, useState } from 'react';
import { leedsTrainStaition } from "./leedsTrainStaition";
import { InitialSearchValues } from "./InitialSearchValues";

export const SearchForm: React.FC<{ tags: string[]; onChange: (searchForm: any) => void; }> = ({ tags, onChange }) => {
  const [searchFormValues, setSearchFormValues] = useState<any>(InitialSearchValues);
  const [searchfromLocation, setSearchFromLocation] = useState<"trainstaition" | "mylocation">("trainstaition");
  function updateSearch(newSearchValue: any) {
    setSearchFormValues(newSearchValue);
    onChange(newSearchValue);
  }
  useEffect(() => {
    if (searchfromLocation === "trainstaition") {
      updateSearch({ ...searchFormValues, location: { ...searchFormValues.location, longitude: leedsTrainStaition.lng, latitude: leedsTrainStaition.lat } });
    } else {
      if (!navigator.geolocation) {
        alert('Geolocation is not supported by your browser');
        setSearchFromLocation("trainstaition");
      } else {
        navigator.geolocation.getCurrentPosition((position: any) => {
          const latitude = position.coords.latitude;
          const longitude = position.coords.longitude;
          updateSearch({ ...searchFormValues, location: { ...searchFormValues.location, longitude: longitude, latitude: latitude } });

        }, () => {
          alert('Unable to retrieve your location');
          setSearchFromLocation("trainstaition");
        }
        );
      }
    }

  }, [searchfromLocation]);

  return (<div><form className='mt-3 me-3'> <div className="mb-3 row">
    <label className="col-sm col-form-label">Search distance From:</label>
    <div className="col-sm"><div className="form-check">
      <input className="form-check-input" type="radio" value="trainstaition" checked={searchfromLocation === 'trainstaition'}
        onChange={() => setSearchFromLocation("trainstaition")} />
      <label className="form-check-label">
        Train Station
      </label>
    </div>
      <div className="form-check">
        <input className="form-check-input" type="radio" value="mylocation" checked={searchfromLocation === 'mylocation'}
          onChange={() => setSearchFromLocation("mylocation")} />
        <label className="form-check-label">
          My Location
        </label>
      </div></div></div>

    <div className="mb-3 row">
      <label className="col-sm col-form-label">Max Distance from Station (m):</label>
      <div className="col-sm">
        <input className="form-control" type="number" name="distance" step="0.1" min="0" max="20" onChange={(e) => {
          updateSearch({ ...searchFormValues, location: { ...searchFormValues.location, maxDistanceInMiles: parseFloat(e.target.value) } });

        }} />
      </div>
    </div>
    <div className="mb-3 row">
      <label className="col-sm col-form-label">Min Atmosphere Rating:</label>
      <div className="col-sm">
        <input className="form-control" type="number" name="atmosphere" step="0.5" min="0" max="5" onChange={(e) => {
          updateSearch({ ...searchFormValues, minRatings: { ...searchFormValues.minRatings, atmosphere: parseFloat(e.target.value) } });
        }} />
      </div>
    </div>
    <div className="mb-3 row">
      <label className="col-sm col-form-label">Min Amenities Rating:</label>
      <div className="col-sm">
        <input className="form-control" type="number" name="amenities" step="0.5" min="0" max="5" onChange={(e) => {
          updateSearch({ ...searchFormValues, minRatings: { ...searchFormValues.minRatings, amenities: parseFloat(e.target.value) } });
        }} />
      </div>
    </div>
    <div className="mb-3 row">
      <label className="col-sm col-form-label">Min Beer Rating:</label>
      <div className="col-sm">
        <input className="form-control" type="number" name="beer" step="0.5" min="0" max="5" onChange={(e) => {
          updateSearch({ ...searchFormValues, minRatings: { ...searchFormValues.minRatings, beer: parseFloat(e.target.value) } });
        }} />
      </div>
    </div>
    <div className="mb-3 row">
      <label className="col-sm col-form-label">Min Value For Money Rating:</label>
      <div className="col-sm">
        <input className="form-control" type="number" name="valueForMoney" step="0.5" min="0" max="5" onChange={(e) => {
          updateSearch({ ...searchFormValues, minRatings: { ...searchFormValues.minRatings, valueForMoney: parseFloat(e.target.value) } });
        }} />
      </div>
    </div>
    <div className="mb-3 row">
      <label className="col-sm col-form-label">Amenities:</label>
      <div className="col-sm">
        <select className="form-select" multiple onChange={(e) => {
          let selectedOptions = Array.from(e.target.selectedOptions, option => option.value);
          updateSearch({ ...searchFormValues, tags: selectedOptions });}}>
          {tags.map(x => <option key={`Tag-${x}`} value={x}>{x}</option>)}
        </select>
      </div>
    </div>


  </form></div>);
};
