import type { DroneEvent } from './event-hub'

export const events: DroneEvent[] = []

export function addEvent(e: DroneEvent) {
  events.unshift(e)

  // оновлюємо DOM-таблицю
  const tbody = document.querySelector('#events-body')!
  const row = document.createElement('tr')
  row.innerHTML = `
    <td>${e.sensorType}</td>
    <td>${e.title}</td>
    <td>${new Date(e.createdAt).toLocaleTimeString()}</td>`
  if (e.threatLevel === 'ALERT') {
    row.classList.add('table-danger')
  }

  tbody.prepend(row)
}
