import React from 'react';
import { Status } from '@googlemaps/react-wrapper';

export const render = (status: Status) => {
  switch (status) {
    case Status.LOADING:
      return <h1>loading</h1>;
    case Status.FAILURE:
      return <h1>error</h1>;
    case Status.SUCCESS:
      return <h1>were goog</h1>;
  }
};
