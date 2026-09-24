import { expect, test } from '@playwright/test'

/**
 * GM flow for hex battle maps: create a map from the game page, which opens the editor in a separate
 * window; paint a hex there, save, and check the change survives a reload.
 */
test('create a battle map, paint a hex in the editor window, save', async ({ page, context }) => {
  const suffix = Date.now().toString(36)
  page.on('dialog', (dialog) => dialog.accept())

  await page.goto('/register')
  await page.getByLabel('Имя').fill('E2E Mapper')
  await page.getByLabel('Логин').fill(`e2e_map_${suffix}`)
  await page.getByLabel('Пароль').fill('password123')
  await page.getByRole('button', { name: 'Зарегистрироваться' }).click()
  await expect(page).toHaveURL('/games')

  await page.getByRole('button', { name: 'Создать игру' }).click()
  await page.getByLabel('Название игры').fill(`E2E Map Game ${suffix}`)
  const createGameResponse = page.waitForResponse((res) => res.url().includes('/api/games') && res.request().method() === 'POST')
  await page.getByRole('button', { name: 'Создать' }).click()
  const { id: gameId } = (await (await createGameResponse).json()) as { id: number }
  await page.goto(`/games/${gameId}`)

  // Карточка = <Card><div><h2>Карты боя</h2>…</div>…</Card>; "Создать" есть и в карточке боёв, поэтому скоупим.
  const mapsCard = page.getByRole('heading', { name: 'Карты боя' }).locator('xpath=../..')
  await mapsCard.getByRole('button', { name: 'Создать' }).click()
  await mapsCard.getByLabel('Название').fill('Пещера')
  await mapsCard.getByLabel('Ширина (гексов)').fill('5')
  await mapsCard.getByLabel('Высота (гексов)').fill('4')
  const editorPromise = context.waitForEvent('page')
  await mapsCard.getByRole('button', { name: 'Создать' }).click()

  const editor = await editorPromise
  await expect(editor.getByRole('heading', { name: 'Пещера' })).toBeVisible()
  await expect(editor).toHaveURL(new RegExp(`/games/${gameId}/battle-maps/\\d+$`))

  await editor.getByRole('button', { name: /Стена/ }).click()
  // Гекс (0, 0) — левый верхний; кликаем в его центр, небольшой отступ от угла SVG.
  const svg = editor.locator('main svg')
  const box = (await svg.boundingBox())!
  await editor.mouse.click(box.x + 30, box.y + 32)
  await expect(editor.getByText('Несохранённых гексов: 1')).toBeVisible()

  const saveResponse = editor.waitForResponse((res) => res.url().includes('/hexes') && res.request().method() === 'PUT')
  await editor.getByRole('button', { name: 'Сохранить' }).click()
  expect((await saveResponse).ok()).toBe(true)
  await expect(editor.getByText('Все изменения сохранены')).toBeVisible()

  await editor.reload()
  await expect(editor.getByRole('heading', { name: 'Пещера' })).toBeVisible()
  await editor.mouse.move(0, 0)
  await editor.mouse.move(box.x + 30, box.y + 32)
  await expect(editor.getByText(/Гекс \(0, 0\) · Стена/)).toBeVisible()
})
