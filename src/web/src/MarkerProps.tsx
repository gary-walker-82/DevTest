export interface MarkerProps extends google.maps.MarkerOptions {
  review: any;
  onClick?: (e: google.maps.MapMouseEvent, marker: google.maps.Marker) => void;
}
