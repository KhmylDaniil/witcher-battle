import { expect, test, type APIRequestContext } from '@playwright/test'

/**
 * GM adds participants to a battle that has already started: no initiative roll, they go to the end
 * of the initiative order in the order they were added, and the log records it.
 */

async function post<T>(request: APIRequestContext, url: string, data?: unknown): Promise<T> {
  const res = await request.post(url, { data })
  expect(res.ok(), `${url}: ${await res.text()}`).toBe(true)
  return (await res.json()) as T
}

test('add a creature and a character to an in-progress battle', async ({ page }) => {
  const suffix = Date.now().toString(36)
  page.on('dialog', (dialog) => dialog.accept())

  await page.goto('/register')
  await page.getByLabel('Имя').fill('E2E Late Join')
  await page.getByLabel('Логин').fill(`e2e_late_${suffix}`)
  await page.getByLabel('Пароль').fill('password123')
  await page.getByRole('button', { name: 'Зарегистрироваться' }).click()
  await expect(page).toHaveURL('/games')

  const request = page.request
  const game = await post<{ id: number }>(request, '/api/games', { name: `E2E Late ${suffix}` })
  const body = await post<{ id: number }>(request, '/api/body-templates', { gameId: game.id, name: 'Тело' })
  const stats = { int: 5, ref: 5, dex: 5, body: 5, emp: 5, cra: 5, will: 5, speed: 5, luck: 5, movement: 5 }
  await post(request, '/api/creature-templates', {
    gameId: game.id, bodyTemplateId: body.id, creatureType: 'Beast', name: 'Гуль', hp: 20, sta: 20, ...stats,
  })
  await post(request, '/api/characters', {
    gameId: game.id, name: 'Лютик', hp: 30, sta: 30, int: 5, str: 5, rea: 5, dex: 5, cra: 5, emp: 5, wil: 5, movement: 5,
  })
  const battle = await post<{ id: number }>(request, `/api/games/${game.id}/battles`, { name: 'Кладбище' })
  const templates = (await (await request.get(`/api/creature-templates?gameId=${game.id}`)).json()) as { items: { id: number }[] }
  await post(request, `/api/games/${game.id}/battles/${battle.id}/creatures`, { creatureTemplateId: templates.items[0].id, name: 'Гуль 1' })
  await post(request, `/api/games/${game.id}/battles/${battle.id}/start`)

  await page.goto(`/games/${game.id}/battles/${battle.id}`)
  await expect(page.getByText('Бой идёт')).toBeVisible()
  const addCard = page.getByRole('heading', { name: 'Добавить участников' }).locator('xpath=..')
  await expect(addCard.getByText('не бросает инициативу')).toBeVisible()

  await addCard.locator('select').first().selectOption({ label: 'Гуль (Beast)' })
  await addCard.getByLabel('Имя (необязательно)').fill('Гуль 2')
  await addCard.getByRole('button', { name: 'Добавить существо' }).click()
  await addCard.locator('select').nth(1).selectOption({ label: 'Лютик' })
  await addCard.getByRole('button', { name: 'Добавить персонажа' }).click()

  const row = (name: string) => page.locator('tbody tr', { has: page.getByText(name, { exact: true }) })
  await expect(row('Гуль 1').locator('td').nth(4)).toHaveText('1')
  await expect(row('Гуль 2').locator('td').nth(4)).toHaveText('2')
  await expect(row('Лютик').locator('td').nth(4)).toHaveText('3')
  await expect(page.getByText('Лютик вступает в бой (инициатива 3, без броска).')).toBeVisible()
})
