// Редактор карты открывается в отдельном окне браузера (мастер держит карту рядом со страницей игры/боя).
// Окна не делят кэш react-query, поэтому об изменениях карты редактор сообщает через BroadcastChannel —
// карточка списка карт в исходном окне просто перезапрашивает список.

export function battleMapEditorPath(gameId: number, battleMapId: number): string {
  return `/games/${gameId}/battle-maps/${battleMapId}`
}

const WINDOW_FEATURES = 'popup=yes,width=1400,height=900'

function windowName(battleMapId: number): string {
  return `battle-map-${battleMapId}`
}

/** Открывает (или переиспользует, если уже открыто) окно редактора карты. false — окно заблокировал браузер. */
export function openBattleMapEditor(gameId: number, battleMapId: number): boolean {
  const w = window.open(battleMapEditorPath(gameId, battleMapId), windowName(battleMapId), WINDOW_FEATURES)
  w?.focus()
  return w !== null
}

/**
 * Открывает пустое окно синхронно — прямо в обработчике клика, пока браузер считает это действием
 * пользователя (из асинхронного onSuccess после создания карты window.open заблокировал бы блокировщик
 * всплывающих окон). Адрес подставляется, когда карта создана.
 */
export function reserveEditorWindow(): Window | null {
  return window.open('about:blank', '_blank', WINDOW_FEATURES)
}

export function battleMapWindowPath(gameId: number, battleId: number): string {
  return `/games/${gameId}/battles/${battleId}/map`
}

/** Открывает (или переиспользует) окно "Карта боя" для расстановки участников. false — окно заблокировал браузер. */
export function openBattleMapWindow(gameId: number, battleId: number): boolean {
  const w = window.open(battleMapWindowPath(gameId, battleId), `battle-${battleId}-map`, WINDOW_FEATURES)
  w?.focus()
  return w !== null
}

const CHANNEL_NAME = 'wastelands-battle-maps'

export function notifyBattleMapsChanged(gameId: number): void {
  if (typeof BroadcastChannel === 'undefined') return
  const channel = new BroadcastChannel(CHANNEL_NAME)
  channel.postMessage({ gameId })
  channel.close()
}

/** Подписка на изменения карт игры из других окон. Возвращает функцию отписки. */
export function subscribeBattleMapsChanged(gameId: number, onChange: () => void): () => void {
  if (typeof BroadcastChannel === 'undefined') return () => {}
  const channel = new BroadcastChannel(CHANNEL_NAME)
  channel.onmessage = (e: MessageEvent<{ gameId: number }>) => {
    if (e.data?.gameId === gameId) onChange()
  }
  return () => channel.close()
}
