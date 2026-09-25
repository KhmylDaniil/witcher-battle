import { expect, test, type APIRequestContext } from '@playwright/test'

/**
 * GM flow for maps in battle: attach a map to a battle, place a creature and a character on hexes in
 * the separate "Карта боя" window (one participant per hex), and read a map marker via its tooltip.
 * Game/templates/battle are seeded through the API to keep the test about the map itself.
 */

async function post<T>(request: APIRequestContext, url: string, data: unknown): Promise<T> {
  const res = await request.post(url, { data })
  expect(res.ok(), `${url}: ${await res.text()}`).toBe(true)
  return (await res.json()) as T
}

test('attach a map to a battle, place participants, see marker tooltip', async ({ page, context }) => {
  const suffix = Date.now().toString(36)
  page.on('dialog', (dialog) => dialog.accept())

  await page.goto('/register')
  await page.getByLabel('Имя').fill('E2E Placer')
  await page.getByLabel('Логин').fill(`e2e_place_${suffix}`)
  await page.getByLabel('Пароль').fill('password123')
  await page.getByRole('button', { name: 'Зарегистрироваться' }).click()
  await expect(page).toHaveURL('/games')

  const request = page.request
  const game = await post<{ id: number }>(request, '/api/games', { name: `E2E Placement ${suffix}` })
  const map = await post<{ id: number }>(request, '/api/battle-maps', {
    gameId: game.id, name: 'Арена', columns: 6, rows: 5, terrainStyle: 'Sand',
  })
  const marker = await request.put(`/api/battle-maps/${map.id}/hexes/4/3/marker`, { data: { text: 'Колодец: можно спрятаться' } })
  expect(marker.ok()).toBe(true)
  const body = await post<{ id: number }>(request, '/api/body-templates', { gameId: game.id, name: 'Тело' })
  const stats = { int: 5, ref: 5, dex: 5, body: 5, emp: 5, cra: 5, will: 5, speed: 5, luck: 5, movement: 5 }
  const template = await post<{ id: number }>(request, '/api/creature-templates', {
    gameId: game.id, bodyTemplateId: body.id, creatureType: 'Beast', name: 'Волколак', hp: 20, sta: 20, ...stats,
  })
  const character = await post<{ id: number }>(request, '/api/characters', {
    gameId: game.id, name: 'Геральт', hp: 30, sta: 30, int: 5, str: 5, rea: 5, dex: 5, cra: 5, emp: 5, wil: 5, movement: 5,
  })
  const battle = await post<{ id: number }>(request, `/api/games/${game.id}/battles`, { name: 'Засада' })
  await post(request, `/api/games/${game.id}/battles/${battle.id}/creatures`, { creatureTemplateId: template.id })
  await post(request, `/api/games/${game.id}/battles/${battle.id}/characters`, { characterId: character.id })

  await page.goto(`/games/${game.id}/battles/${battle.id}`)
  await page.getByRole('button', { name: 'Подключить карту' }).click()
  await expect(page.getByText('Подключена:')).toBeVisible()

  const windowPromise = context.waitForEvent('page')
  await page.getByRole('button', { name: 'Открыть карту боя' }).click()
  const mapWindow = await windowPromise
  await expect(mapWindow.getByRole('heading', { name: 'Засада' })).toBeVisible()
  await expect(mapWindow.getByRole('heading', { name: 'Не выставлены (2)' })).toBeVisible()

  const svg = mapWindow.locator('main svg')
  const box = (await svg.boundingBox())!
  // Центр гекса (column, row) при масштабе 1: HEX_SIZE = 24, MAP_PADDING = 8, раскладка odd-r.
  const hexPoint = (column: number, row: number) => ({
    x: box.x + 8 + Math.sqrt(3) * 24 * (column + 0.5 * (row & 1)) + (Math.sqrt(3) * 24) / 2,
    y: box.y + 8 + 24 * (1.5 * row + 1),
  })

  await mapWindow.getByRole('button', { name: /Волколак/ }).click()
  let p = hexPoint(1, 1)
  await mapWindow.mouse.click(p.x, p.y)
  await expect(mapWindow.getByRole('heading', { name: 'На карте (1)' })).toBeVisible()

  // Персонаж на тот же гекс — нельзя: клик по занятому гексу выбирает стоящего там, а не ставит поверх.
  await mapWindow.getByRole('button', { name: /Геральт/ }).click()
  await mapWindow.mouse.click(p.x, p.y)
  await expect(mapWindow.getByRole('heading', { name: 'На карте (1)' })).toBeVisible()

  await mapWindow.getByRole('button', { name: /Геральт/ }).click()
  p = hexPoint(2, 1)
  await mapWindow.mouse.click(p.x, p.y)
  await expect(mapWindow.getByRole('heading', { name: 'На карте (2)' })).toBeVisible()

  p = hexPoint(4, 3)
  await mapWindow.mouse.move(p.x, p.y)
  await expect(mapWindow.getByRole('tooltip')).toContainText('Колодец: можно спрятаться')

  // Позиции видны и на странице боя (карточка "Карта боя" обновилась по SignalR).
  await expect(page.getByText('на карте 2 из 2 участников')).toBeVisible()
})
