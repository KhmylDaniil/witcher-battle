import { expect, test } from '@playwright/test'

/**
 * One smoke test through the app's spine: register (which logs the user in), create a game,
 * create a character in it, see it listed, then delete the game as cleanup. Doesn't try to cover
 * every feature (battles, items, conditions, …) — those are exercised by the backend integration
 * and unit-test phases; this is here to catch routing/auth/API-wiring regressions a build alone
 * wouldn't.
 */
test('register, create a game, add a character, delete the game', async ({ page }) => {
  const suffix = Date.now().toString(36)
  const login = `e2e_${suffix}`
  const gameName = `E2E Game ${suffix}`
  const characterName = `E2E Hero ${suffix}`

  page.on('dialog', (dialog) => dialog.accept())

  await page.goto('/register')
  await page.getByLabel('Имя').fill('E2E Tester')
  await page.getByLabel('Логин').fill(login)
  await page.getByLabel('Пароль').fill('password123')
  await page.getByRole('button', { name: 'Зарегистрироваться' }).click()

  await expect(page).toHaveURL('/games')

  await page.getByRole('button', { name: 'Создать игру' }).click()
  await page.getByLabel('Название игры').fill(gameName)
  const createGameResponse = page.waitForResponse((res) => res.url().includes('/api/games') && res.request().method() === 'POST')
  await page.getByRole('button', { name: 'Создать' }).click()
  const { id: gameId } = (await (await createGameResponse).json()) as { id: number }

  // Navigate directly by id rather than clicking the list entry — the games list is paginated and,
  // on a long-lived dev database with lots of prior test data, a freshly created game isn't
  // guaranteed to land on the first page.
  await page.goto(`/games/${gameId}`)
  await expect(page.getByRole('heading', { name: gameName })).toBeVisible()

  await page.getByRole('link', { name: 'Создать персонажа' }).click()
  await page.getByLabel('Имя').fill(characterName)
  await page.getByRole('button', { name: 'Сохранить' }).click()

  await expect(page.getByRole('heading', { name: characterName })).toBeVisible()

  await page.goto(`/games/${gameId}`)
  // Создатель игры — её мастер, поэтому его персонаж есть и в мастерской карточке "Персонажи игроков",
  // и в общей "Персонажи" (карточка = <Card><div><h2/>…</div>…</Card>). Проверяем общий список, иначе
  // локатор находит две ссылки, как только догрузится мастерская карточка.
  const charactersCard = page.getByRole('heading', { name: 'Персонажи', exact: true }).locator('xpath=../..')
  await expect(charactersCard.getByRole('link', { name: characterName })).toBeVisible()

  await page.getByRole('button', { name: 'Удалить игру' }).click()
  await expect(page).toHaveURL('/games')
})
