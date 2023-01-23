import React from 'react';
import ReactDOMServer from "react-dom/server";
import { ReviewOverview } from "./ReviewOverview";
import { MarkerProps } from "./MarkerProps";

export const Marker: React.FC<MarkerProps> = (options) => {
  const [marker, setMarker] = React.useState<google.maps.Marker>();

  React.useEffect(() => {
    if (!marker) {
      setMarker(new google.maps.Marker());

    }

    // remove marker from map on unmount
    return () => {
      if (marker) {
        marker.setMap(null);
        marker.unbindAll();
      }
    };
  }, [marker]);

  React.useEffect(() => {
    if (marker) {
      marker.setOptions(options);
      marker.setClickable(true);

      var infoWindow = new google.maps.InfoWindow({
        content: "",
      });
      infoWindow.addListener("click", () => infoWindow.close());

      //@ts-ignore
      marker.addListener("click", (e) => {
        const content = ReactDOMServer.renderToString((<ReviewOverview review={options.review} />));
        infoWindow.setContent(content);
        infoWindow.open({ anchor: marker, map: options.map });

      });
    }
  }, [marker, options]);

  return null;
};
