import React from 'react';

export const ReviewResultItem: React.FC<{ review: any; }> = ({ review }) => {


  return (<div className="list-group-item d-flex" key={`rr${review.location.address.geographicPoint.latitude}-${review.location.address.geographicPoint.longitude}`}>
  
 <div className='thumbnail'><img className="img-thumbnail" src={review.location.thumbnail} /></div>
  {/* <div className="d-flex"> */}
  <div className='d-flex flex-column flex-grow-1'>
    <div className="d-flex justify-content-between align-items-start ">
     <h5>{review.location.name}</h5>
      <small>{review.dateVisited}</small>
    </div>

<div className='d-flex flex-row  justify-content-between'>
  <div> <h6>{review.location.address.addressString}</h6>
    <p>{review.location.excerpt}</p></div>
  <div className="me-4">
    <ul>
        <li>Amenities : {review.ratings.amenities}</li>
        <li>Atmosphere : {review.ratings.atmosphere}</li>
        <li>Beer : {review.ratings.beer}</li>
        <li>Value For Money : {review.ratings.valueForMoney}</li>
      </ul></div>
</div>
</div>
{/* <div className="d-flex flex-grow-1 bg-danger justify-content-between align-items-start">
  <div>
  <h6>{review.location.address.addressString}</h6>
    <p>{review.location.excerpt}</p>
  </div>
  <div>
  <ul>
        <li>Amenities : {review.ratings.amenities}</li>
        <li>Atmosphere : {review.ratings.atmosphere}</li>
        <li>Beer : {review.ratings.beer}</li>
        <li>Value For Money : {review.ratings.valueForMoney}</li>
      </ul>
  </div>
</div> */}
    
     
     {/* <div className="d-flex">
      
     <img  className="img-thumbnail" width={"150px"} src={review.location.thumbnail} />
     
     <div className=''>
     <div className="d-flex w-100 justify-content-between">
      <h5 className="mb-1">{review.location.name}</h5>
      <small>{review.dateVisited}</small>
    </div>
    <h6>{review.location.address.addressString}</h6>
    <p>{review.location.excerpt}</p>
     </div>
     */}
    {/* <div>
      <ul>
        <li>Amenities : {review.ratings.amenities}</li>
        <li>Atmosphere : {review.ratings.atmosphere}</li>
        <li>Beer : {review.ratings.beer}</li>
        <li>Value For Money : {review.ratings.valueForMoney}</li>
      </ul> */}
      {/* </div> */}
  </div>);
};
