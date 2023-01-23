import React, { useRef } from 'react';
import logo from './logo.svg';
import './App.scss';
import { Wrapper } from '@googlemaps/react-wrapper';
import { useReviews } from './Data/searchReviews';
import {ResultsList} from './ResultsList';
import { Marker } from './Marker';
import { Map } from './Map';
import { SearchForm } from './SearchForm';
import { InitialSearchValues } from './InitialSearchValues';
import { render } from './render';
import { leedsTrainStaition } from './leedsTrainStaition';

function App() {
  const [clicks, setClicks] = React.useState<google.maps.LatLng[]>([]);
  const [zoom, setZoom] = React.useState(15); // initial zoom
  const [center, setCenter] = React.useState<google.maps.LatLngLiteral>(leedsTrainStaition);
  const[geoStatus, setGeoStatus]= React.useState<string>('');
  const[isOpen, setIsOpen] = React.useState<boolean>(false);
  const [searchForm, setSearchForm]= React.useState<any>(InitialSearchValues);

  const [data,isloading] =useReviews(searchForm);
  //@ts-ignore
const tags :string[] = data?.facets?.Tags?? [];
  function onShowInfo(review:any){

    setIsOpen(true);
  }
  return (
    <div>
      <div className='d-flex'>
        <div className='w-25'>
        <SearchForm tags={tags} onChange={(searchForm)=> {
  setSearchForm(searchForm);
  if(searchForm.location.latitude !== center.lat)
  {
    setCenter({lat:searchForm.location.latitude, lng:searchForm.location.longitude});
  }}}></SearchForm>
  </div>
      <div className='flex-grow-1'> <Wrapper apiKey={"123"} render={render}>
      <Map 
      center={center}      
      zoom={zoom}
      style={{ flexGrow: "1", height: "500px" }}
    >
      
      {    
      //@ts-ignore
      data && (data.results as any[]).map(x=><Marker key={`marker-${x.location.address.geographicPoint.longitude}-${x.location.address.geographicPoint.latitude}`} clickable={true} review={x}  position={{lat:x.location.address.geographicPoint.latitude, lng: x.location.address.geographicPoint.longitude}} >        
        </Marker>   )
      }
 
      </Map>
    
    </Wrapper></div>   </div>
    <div className='bg-info' style={{ display: "flex", height: "100%" }}>    
  


     
    
    {
     //@ts-ignore
     !isloading && data &&  <ResultsList reviews={data.results}></ResultsList>}
    
    </div>
    </div>
  );
}

export default App;


