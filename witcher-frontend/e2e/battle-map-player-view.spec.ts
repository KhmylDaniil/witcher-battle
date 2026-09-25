import { expect, test, type APIRequestContext, type Browser } from '@playwright/test'

/**
 * A player whose character is in a battle sees the battle map only once the battle is running, and only
 * read-only: positions, tokens and markers the GM marked "видно игрокам" yes; GM-only markers and
 * placement controls no.
 */

async function send<T>(request: APIRequestContext, method: 'post' | 'put', url: string, data?: unknown): Promise<T> {
  const res = await request[method](url, { data })
  expect(res.ok(), `${method.toUpperCase()} ${url}: ${await res.text()}`).toBe(true)
  const text = await res.text()
  return (text ? JSON.parse(text) : undefined) as T
}

async function registeredContext(browser: Browser, baseURL: string, login: string) {
  const context = await browser.newContext({ baseURL })
  await send(context.request, 'post', '/api/auth/register', { name: login, login, password: 'password123' })
  await send(context.request, 'post', '/api/auth/login', { login, password: 'password123' })
  return context
}

test('player sees the battle map read-only while the battle is in progress', async ({ browser, baseURL }) => {
  const suffix = Date.now().toString(36)
  const gm = await registeredContext(browser, baseURL!, `e2e_gm_${suffix}`)
  const player = await registeredContext(browser, baseURL!, `e2e_pl_${suffix}`)
  const gmApi = gm.request
  const playerApi = player.request

  const game = await send<{ id: number }>(gmApi, 'post', '/api/games', { name: `E2E Player View ${suffix}` })
  const join = await send<{ id: number }>(playerApi, 'post', `/api/games/${game.id}/join-requests`)
  await send(gmApi, 'post', `/api/games/${game.id}/join-requests/${join.id}/accept`)
  const character = await send<{ id: number }>(playerApi, 'post', '/api/characters', {
    gameId: game.id, name: 'Цири', hp: 30, sta: 30, int: 5, str: 5, rea: 5, dex: 5, cra: 5, emp: 5, wil: 5, movement: 5,
  })

  const map = await send<{ id: number }>(gmApi, 'post', '/api/battle-maps', { gameId: game.id, name: 'Руины', columns: 5, rows: 4, terrainStyle: 'AncientStreet' })
  await send(gmApi, 'put', `/api/battle-maps/${map.id}/hexes/3/2/marker`, { text: 'Засада мастера' })
  await send(gmApi, 'put', `/api/battle-maps/${map.id}/hexes/0/2/marker`, { text: 'Старый колодец', visibleToPlayers: true })
  const battle = await send<{ id: number }>(gmApi, 'post', `/api/games/${game.id}/battles`, { name: 'Налёт' })
  await send(gmApi, 'post', `/api/games/${game.id}/battles/${battle.id}/characters`, { characterId: character.id })
  await send(gmApi, 'put', `/api/games/${game.id}/battles/${battle.id}/map`, { battleMapId: map.id })
  await send(gmApi, 'put', `/api/games/${game.id}/battles/${battle.id}/map/participants/Character/${character.id}`, { column: 1, row: 1 })

  // Бой ещё готовится — игроку ни бой, ни карта не видны.
  const mapUrl = `/api/games/${game.id}/battles/${battle.id}/map`
  expect((await playerApi.get(mapUrl)).status()).toBe(404)

  await send(gmApi, 'post', `/api/games/${game.id}/battles/${battle.id}/start`)

  const view = (await (await playerApi.get(mapUrl)).json()) as { canEdit: boolean; map: { hexes: { markerText: string | null }[] } }
  expect(view.canEdit).toBe(false)
  expect(view.map.hexes.filter((h) => h.markerText !== null).map((h) => h.markerText)).toEqual(['Старый колодец'])
  // Менять расстановку игрок не может.
  const place = await playerApi.put(`${mapUrl}/participants/Character/${character.id}`, { data: { column: 2, row: 1 } })
  expect(place.ok()).toBe(false)

  const page = await player.newPage()
  await page.goto(`/games/${game.id}/battles/${battle.id}`)
  const mapWindowPromise = player.waitForEvent('page')
  await page.getByRole('button', { name: 'Карта боя' }).click()
  const mapWindow = await mapWindowPromise

  await expect(mapWindow.getByRole('heading', { name: 'Налёт' })).toBeVisible()
  await expect(mapWindow.getByText('Просмотр карты.')).toBeVisible()
  await expect(mapWindow.getByRole('heading', { name: 'На карте (1)' })).toBeVisible()
  await expect(mapWindow.getByText('(вы)')).toBeVisible()
  await expect(mapWindow.getByRole('button', { name: /Цири/ })).toHaveCount(0)

  const box = (await mapWindow.locator('main svg').boundingBox())!
  const hexPoint = (column: number, row: number) => ({
    x: box.x + 8 + Math.sqrt(3) * 24 * (column + 0.5 * (row & 1)) + (Math.sqrt(3) * 24) / 2,
    y: box.y + 8 + 24 * (1.5 * row + 1),
  })
  const token = hexPoint(1, 1)
  await mapWindow.mouse.move(token.x, token.y)
  await expect(mapWindow.getByRole('tooltip')).toContainText('Цири')

  const openMarker = hexPoint(0, 2)
  await mapWindow.mouse.move(openMarker.x, openMarker.y)
  await expect(mapWindow.getByRole('tooltip')).toContainText('Старый колодец')
  await expect(mapWindow.getByRole('tooltip')).not.toContainText('Видно игрокам')
  const hiddenMarker = hexPoint(3, 2)
  await mapWindow.mouse.move(hiddenMarker.x, hiddenMarker.y)
  await expect(mapWindow.getByRole('tooltip')).toHaveCount(0)

  await gm.close()
  await player.close()
})
