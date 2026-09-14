import * as signalR from '@microsoft/signalr'
import { useEffect } from 'react'

// Тот же хаб, что и в Razor-версии (Witcher.MVC/Hubs/MessageHub.cs): сервер таргетированно шлёт
// "UpdateBattleLog" без payload участникам конкретного боя (Clients.Users(ids)). Раньше клиент на это
// событие делал location.reload() — здесь вместо этого просто уведомляем подписчиков, и страница боя
// сама решает, что перезапросить через TanStack Query (см. useBattleUpdates в features/runBattle).
let connection: signalR.HubConnection | null = null
const listeners = new Set<() => void>()

function ensureConnection(): signalR.HubConnection {
  if (connection) return connection

  connection = new signalR.HubConnectionBuilder()
    .withUrl('/messageHub')
    .withAutomaticReconnect()
    .build()

  connection.on('UpdateBattleLog', () => {
    for (const listener of listeners) listener()
  })

  connection.start().catch((err) => {
    // Соединение не критично для базовой работы страницы (данные всё равно можно перезапросить вручную/по фокусу),
    // поэтому просто логируем, не роняя UI.
    console.error('SignalR connection failed', err)
  })

  return connection
}

/** React-хук: вызывает onUpdate при каждом "UpdateBattleLog" от сервера, пока компонент смонтирован */
export function useBattleUpdates(onUpdate: () => void): void {
  useEffect(() => {
    ensureConnection()
    listeners.add(onUpdate)
    return () => {
      listeners.delete(onUpdate)
    }
  }, [onUpdate])
}
