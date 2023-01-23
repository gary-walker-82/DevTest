import React from 'react';
import { ReviewResultItem } from "./ReviewResultItem";

export const ResultsList: React.FC<{ reviews: any[]; }> = ({ reviews }) => {
  return (<div className='list-group flex-fill'>
    {
      //@ts-ignore
      reviews.map(x => <ReviewResultItem key={`reviewitem-${x.location.address.geographicPoint.longitude}-${x.location.address.geographicPoint.latitude}`} review={x} />)}
  </div>);
};
