import type { DroneEvent } from './event-hub'

import * as L from 'leaflet'
import 'leaflet/dist/leaflet.css'

const map = L.map('map').setView([50.45, 30.52], 13)
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
  attribution: '© OSM'
}).addTo(map)

export function addMarker(e: DroneEvent) {
  L.marker([e.lat!, e.lng!]).addTo(map)
    .bindPopup(`<b>${e.title}</b><br>${e.sensorType}`)
    .openPopup()
}
