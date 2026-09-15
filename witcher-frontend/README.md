# Wastelands — фронтенд

React + TypeScript SPA поверх JSON API `Wastelands.Service.MVC/Controllers/Api/*`.

> Изначально этот проект был SPA для Witcher (`Witcher.MVC`) — полная история и обоснование стека в первом
> плане реализации. Проект переведён на Wastelands (вторая, более чистая попытка того же приложения) —
> полный снапшот версии для Witcher сохранён в ветке `archive/witcher-full-app-2026-09-15`.

Реализовано: аутентификация (регистрация/логин/логаут), игры (создание, список с индикацией статуса
участия, заявки на присоединение — отправка игроком, приём/отклонение мастером), CRUD персонажей
(привязаны к конкретной игре, 7 характеристик), добавление/изменение/удаление навыков. У Wastelands пока
нет концепции боёв/шаблонов существ — по мере переноса логики Witcher в Wastelands сюда добавятся
соответствующие разделы фронтенда.

## Разработка

Нужны два параллельных процесса — бэкенд и dev-сервер фронтенда:

```bash
# терминал 1 — бэкенд (из Witcher/Services/Wastelands.Service/Wastelands.Service.MVC)
dotnet run

# терминал 2 — фронтенд (из witcher-frontend/)
npm install
npm run dev
```

Vite dev-сервер (обычно `http://localhost:5173`) проксирует `/api/**` на бэкенд (`https://localhost:7114` —
см. `vite.config.ts` и `Wastelands.Service.MVC/Properties/launchSettings.json`), поэтому cookie-аутентификация
работает без какой-либо настройки CORS — браузер видит всё как один origin. Открывать нужно именно адрес
Vite (5173), а не адрес бэкенда. Прокси нацелен на HTTPS-порт намеренно — на HTTP-порту бэкенд обязательно
редиректит на HTTPS (`app.UseHttpsRedirection()`), и если прокси целится в HTTP, браузер сам идёт по этому
редиректу как по настоящему кросс-origin запросу — упирается в CORS, которого специально нет.

Бэкенду нужен Postgres по адресу из `Wastelands.Service.MVC/appsettings.json`
(`Host=localhost;Port=5431;Database=wastelands;Username=postgres;Password=admin`) — либо подними такой
кластер, либо переопредели `ConnectionStrings__Postgres` через переменную окружения под то, что есть локально.

## Продакшен-сборка

Единый деплой без CORS: собранный SPA копируется в `wwwroot/app` бэкенда и раздаётся тем же процессом.

```bash
npm run build
rm -rf ../Witcher/Services/Wastelands.Service/Wastelands.Service.MVC/wwwroot/app
cp -r dist ../Witcher/Services/Wastelands.Service/Wastelands.Service.MVC/wwwroot/app
```

После этого `dotnet run`/деплой `Wastelands.Service.MVC` — SPA доступен на `/app`, API на том же origin
(см. `app.MapFallbackToFile` в `Program.cs`). Шаг пока ручной — не завязан на MSBuild, чтобы не усложнять
.NET-сборку зависимостью от Node.
