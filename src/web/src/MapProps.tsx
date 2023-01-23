import React from 'react';


export interface MapProps extends google.maps.MapOptions {
  style: { [key: string]: string; };
  onClick?: (e: google.maps.MapMouseEvent) => void;
  onIdle?: (map: google.maps.Map) => void;
  children?: React.ReactNode;
}
