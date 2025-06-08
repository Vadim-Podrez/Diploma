// import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

// const connection = new HubConnectionBuilder()
//     .withUrl('http://127.0.0.1:8080/eventHub')
//     .configureLogging(LogLevel.Information)
//     .withAutomaticReconnect()
//     .build()

// // Слухаємо нові події
// connection.on('newEvent', ev => {
//   // eslint-disable-next-line no-console
//   console.log('New event:', ev)
//   // Тут твій код додавання точки на карту/оновлення UI
// })

// export async function startRealtime() {
//   try {
//     await connection.start()
//     // eslint-disable-next-line no-console
//     console.log('SignalR connected!')
//   } catch (error) {
//     // eslint-disable-next-line no-console
//     console.error('SignalR error:', error)
//   }
// }

import { HubConnectionBuilder } from '@microsoft/signalr'

const connection = new HubConnectionBuilder()
    .withUrl('http://localhost:5000/eventHub')
    .build()

connection.on('ReceiveEvent', eventData => {
  // Обробка нової події: намалювати на мапі, оновити список і т.д.
  // eslint-disable-next-line no-console
  console.log(eventData)
})

await connection.start()
