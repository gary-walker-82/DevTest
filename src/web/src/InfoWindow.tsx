import React from 'react';
import { InfoWindowProps } from "./InfoWindowProps";

const InfoWindow: React.FC<InfoWindowProps> = (options) => {

  const [infoWindow, setInfoWindow] = React.useState<google.maps.InfoWindow>();

  React.useEffect(() => {
    if (!infoWindow) {
      setInfoWindow(new google.maps.InfoWindow());

    }

    // remove infoWindow from map on unmount
    return () => {
      if (infoWindow) {
        infoWindow.setContent(null);
      }
    };
  }, [infoWindow]);

  React.useEffect(() => {
    if (infoWindow) {
      infoWindow.setOptions(options);
      infoWindow.setContent("gello");
      infoWindow.open({
        anchor: options.marker,
        map: options.map,
      });
    }
  }, [infoWindow, options]);

  return null;
};
