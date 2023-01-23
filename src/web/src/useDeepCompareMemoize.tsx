import React from 'react';
import { MapProps } from './MapProps';

function useDeepCompareMemoize(value: MapProps) {
  const ref = React.useRef<MapProps>();
  if (!value)
    return ref.current;
  if (!ref.current) {
    ref.current = value;
  }
  if (value.center?.lat !== ref.current?.center?.lat ||
    value.center?.lng !== ref.current?.center?.lng) {
    ref.current = value;
  }

  return ref.current;
}

export function useDeepCompareEffectForMaps(
  callback: React.EffectCallback,
  dependencies: any[]
) {
  React.useEffect(callback, dependencies.map(useDeepCompareMemoize));
}
