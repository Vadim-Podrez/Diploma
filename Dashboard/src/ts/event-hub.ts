import * as signalR from '@microsoft/signalr'

import { addEvent } from './state'
import { showToast } from './toast'
import { addMarker } from './map'

export type DroneEvent = {
  id: number;
  sensorType: 'RF' | 'AUDIO' | 'VIDEO';
  threatLevel: 'INFO' | 'WARN' | 'ALERT';
  title: string;
  lat?: number;
  lng?: number;
  createdAt: string;
}

const hub = new signalR.HubConnectionBuilder()
  .withUrl('/eventHub')
  .withAutomaticReconnect()
  .build()

hub.on('NewEvent', (e: DroneEvent) => {
  addEvent(e)
  // оновлюємо журнал (таблиця)
  if (e.lat && e.lng) {
    addMarker(e)
  }

  // крапка на мапі
  if (e.threatLevel === 'ALERT') {
    // передаємо ТІЛЬКИ текст, а не весь обʼєкт
    showToast(`${e.sensorType}: ${e.title}`, 'danger')
  }
  // pop-up + звук
})

export async function startHub() {
  // eslint-disable-next-line no-console
  return hub.start().catch(console.error)
}
