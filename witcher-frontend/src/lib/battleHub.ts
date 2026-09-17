// Один общий HubConnection на всё приложение — переиспользуется всеми экранами боя, которые могут
// монтироваться/размонтироваться независимо. Хаб не передаёт данные боя, только голое событие
// "BattleUpdated" (см. Wastelands.Service.MVC/Hubs/BattleHub.cs) — по нему инвалидируем React Query.
import { useEffect, useRef } from 'react'
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

let connection: HubConnection | null = null
let startPromise: Promise<void> | null = null

function getConnection(): HubConnection {
  if (!connection) {
    connection = new HubConnectionBuilder()
      .withUrl('/api/hubs/battle')
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build()
  }
  return connection
}

async function ensureStarted(): Promise<HubConnection> {
  const conn = getConnection()
  if (conn.state === 'Disconnected') {
    startPromise = conn.start()
  }
  if (startPromise) {
    await startPromise
  }
  return conn
}

/** Подписывается на обновления конкретного боя; вызывает onUpdate при каждом событии "BattleUpdated". */
export function useBattleUpdates(battleId: number, onUpdate: () => void) {
  const onUpdateRef = useRef(onUpdate)
  onUpdateRef.current = onUpdate

  useEffect(() => {
    let cancelled = false
    let joinedConn: HubConnection | null = null

    const handler = () => onUpdateRef.current()

    ensureStarted()
      .then((conn) => {
        if (cancelled) return
        joinedConn = conn
        conn.on('BattleUpdated', handler)
        // При автопереподключении группа теряется — нужно переприсоединиться.
        conn.onreconnected(() => {
          conn.invoke('JoinBattle', battleId).catch(() => {})
        })
        return conn.invoke('JoinBattle', battleId)
      })
      .catch(() => {
        // Соединение не удалось — экран боя продолжит работать без live-обновлений
        // (запасной refetchInterval на странице подхватит изменения позже).
      })

    return () => {
      cancelled = true
      if (joinedConn) {
        joinedConn.off('BattleUpdated', handler)
        joinedConn.invoke('LeaveBattle', battleId).catch(() => {})
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [battleId])
}
