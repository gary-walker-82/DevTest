import React from 'react';

export const ReviewOverview: React.FC<{ review: any; }> = (options) => {

  return (<div>
    <h3>{options.review.location.name}</h3>
    <h6>{options.review.location.address.addressString}</h6>
  </div>);
};
