# Witcher Battle — фронтенд

React + TypeScript SPA поверх нового JSON API (`Witcher.MVC/Controllers/Api/*`). Полное описание архитектуры и
причин выбора этого стека — в плане реализации (сообщение в истории чата / `joyful-hatching-wilkinson.md`).

Реализовано в этой итерации: аутентификация, список/вступление в игры, полный CRUD шаблонов существ (включая
части тела/навыки/модификаторы урона), CRUD боёв, и интерактивный экран проведения боя в реальном времени
(SignalR вместо `location.reload()`). Остальные CRUD-разделы (BodyTemplate, WeaponTemplate, ArmorTemplate,
Ability, Character, Item, Notification) — по тому же паттерну, что и `features/creatureTemplates/`, следующими
итерациями.

## Разработка

Нужны два параллельных процесса — бэкенд и dev-сервер фронтенда:

```bash
# терминал 1 — бэкенд (из Witcher/Witcher.MVC)
dotnet run

# терминал 2 — фронтенд (из witcher-frontend/)
npm install
npm run dev
```

Vite dev-сервер (обычно `http://localhost:5173`) проксирует `/api/**` и `/messageHub` на бэкенд
(`http://localhost:5277` — см. `vite.config.ts` и `Witcher.MVC/Properties/launchSettings.json`), поэтому
cookie-аутентификация работает без какой-либо настройки CORS — браузер видит всё как один origin. Открывать
нужно именно адрес Vite (5173), а не адрес бэкенда.

## Продакшен-сборка

Единый деплой без CORS: собранный SPA копируется в `wwwroot/app` бэкенда и раздаётся тем же процессом.

```bash
npm run build
rm -rf ../Witcher/Witcher.MVC/wwwroot/app
cp -r dist ../Witcher/Witcher.MVC/wwwroot/app
```

После этого `dotnet run`/деплой `Witcher.MVC` — SPA доступен на `/app`, API и SignalR-хаб на том же origin
(см. `app.MapFallbackToFile` в `Program.cs`). Шаг пока ручной (см. план) — не завязан на MSBuild, чтобы не
усложнять .NET-сборку зависимостью от Node.
