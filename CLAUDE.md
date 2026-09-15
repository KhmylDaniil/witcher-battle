# witcher-battle / Wastelands

## Суть проекта

Помощник ГМ для настольной ролевой игры **Wastelands** (постапокалиптический сеттинг).
Два назначения: **оцифровка листа персонажа** и **ведение боёв**.

Исторически репозиторий начинался как помощник ГМ для Witcher TRPG — весь код в
проектах `Witcher.*` относится к той, первой версии. Он не выброшен: боевой движок
(шаблоны существ, распределение попаданий по частям тела, броня, состояния,
критические эффекты, инициатива) со временем будет переработан и станет боевым
движком для Wastelands. Пока в коде встречается название "Witcher" и связанная
терминология — это наследие первой версии, а не текущая предметная область продукта.

## Состояние репозитория и веток (важно!)

Код Wastelands физически лежит НЕ в той ветке, где сейчас работает эта сессия:

- `master` и текущая ветка сессии (`claude/confident-noether-nh46ux`) — содержат
  только старый бэкенд первой версии (`Witcher.Core` / `Witcher.MVC` /
  `Witcher.Storage.Postgresql` / `Witcher.UnitTest`). Проектов `Wastelands.*` и
  фронтенда здесь ещё нет.
- `wastelands` — отдельная ветка с новыми проектами `Wastelands.*`: CRUD
  персонажа, авторизация, статы, навыки.
- `dev2-mvc` — активная интеграционная ветка. Здесь `wastelands` вливается в
  бэкенд первой версии, бэкенд обновлён до .NET 10, добавлен React SPA
  фронтенд (`witcher-frontend/`) поверх нового JSON API-слоя над старыми
  контроллерами `Witcher.MVC`. Полное состояние "до слияния" заархивировано в
  ветку `archive/witcher-full-app-2026-09-15`.
- Именно в `dev2-mvc` сейчас параллельно работает другая сессия Claude
  (задача на момент проверки: "archived witcher-full-app; analyzing wastelands
  frontend migration scope" — т.е. решает, как фронтенд должен покрыть экраны
  персонажа Wastelands в дополнение к уже реализованным боевым экранам).
- Подробный план этой миграции фронтенда упомянут в `witcher-frontend/README.md`
  как чат-артефакт `joyful-hatching-wilkinson.md` — он лежит в истории той,
  другой сессии, а не в репозитории, поэтому этот файл его не пересказывает.

Раздел "Модули" ниже описывает структуру по состоянию ветки `dev2-mvc`, а не
текущей рабочей копии — учитывай это при поиске файлов.

## Модули

### Wastelands (новая часть — лист персонажа)

- `Witcher/Core/Wastelands.Core.Contracts` — сквозные контракты: пейджинг/
  сортировка (`IPagingParams`, `IOrderParams`, `PagedRequest`), `IUserContext`,
  `ErrorCode`, иерархия исключений (`BusinessLogicException` →
  `InvalidArgumentException` / `NotFoundException`; `WebException` →
  `BadRequestException` / `UnauthorizedException`).
- `Witcher/Core/Wastelands.Core.EfDataAccess` — обобщённые EF-абстракции:
  базовая `Entity`, `IBaseRepository` / `IBaseReadRepository`, `IFilter`,
  `PagedList`.
- `Witcher/Infrastructure/Wastelands.EfDataAccess` — реализации обобщённых
  репозиториев (`BaseRepository`, `BaseReadRepository`) и extension-методы
  для DI/веб-приложения.
- `Witcher/Services/Wastelands.Service/Wastelands.Service.Domain` — доменный
  слой: сущности `Character`, `User`, enum `Skill`, контракты сервисов и
  репозиториев, DTO, фильтры, модели запросов.
- `Witcher/Services/Wastelands.Service/Wastelands.Service.Infrastructure` —
  EF-конфигурации, `WastelandsDbContext`, миграции, реализации
  `CharacterService` / `UserService` / `PasswordService`.
- `Witcher/Services/Wastelands.Service/Wastelands.Service.MVC` — ASP.NET MVC
  хост: `CharacterController`, `LoginController`, exception filter/attribute,
  Razor-вьюхи.

### Witcher (первая версия — боевой движок, будет переработан)

- `Witcher/Witcher.Core` — домен шаблонов существ/боя и обработчики в
  CQRS-стиле (Requests/Handlers/Validators, MediatR-подобный паттерн).
- `Witcher/Witcher.Storage.Postgresql` — EF-конфигурация и миграции старой схемы.
- `Witcher/Witcher.MVC` — старые Razor-контроллеры/вьюхи + новый JSON-слой
  `Controllers/Api/*` (добавлен под SPA) + SignalR-хаб для боя в реальном времени.
- `Witcher/Witcher.UnitTest` — юнит-тесты обработчиков/сервисов первой версии.

### Фронтенд

- `witcher-frontend/` — React + TypeScript + Vite SPA, ходит в
  `Witcher.MVC/Controllers/Api/*` по JSON, SignalR — для экрана боя в реальном
  времени. Реализовано: аутентификация, список/вступление в игры, полный CRUD
  шаблонов существ, CRUD боёв, интерактивный экран боя. Экранов для персонажа
  Wastelands пока нет — это как раз то, что сейчас оценивает параллельная сессия.

## Статус

Готово:
- Первая версия (боевой движок): шаблоны существ, распределение попаданий по
  частям тела, броня, состояния, критические эффекты, инициатива и порядок
  действий в бою.
- Wastelands: CRUD персонажа, логин/регистрация, блок из 7 характеристик
  (Int/Str/Rea/Dex/Cra/Emp/Wil) и список навыков по характеристикам, обработка
  исключений через фильтр.

В работе (ветка `dev2-mvc`, другая сессия): слияние Wastelands в бэкенд первой
версии, апгрейд до .NET 10, оценка объёма работ по фронтенду персонажа Wastelands.

Не начато: единого плана переноса домена боевого движка (BodyTemplate/
CreatureTemplate/Effects) на правила Wastelands пока нет — по словам автора,
весь код `Witcher.*` со временем будет переделан.

## Технологии

.NET (в `dev2-mvc` — .NET 10), EF Core + PostgreSQL, паттерн запрос/обработчик
в стиле MediatR + FluentValidation (первая версия), обобщённый repository
pattern (Wastelands), ASP.NET MVC + Razor + JSON API поверх тех же контроллеров,
SignalR для live-обновлений, React + TypeScript + Vite на фронтенде.

## Доменный глоссарий

Черновик — требует комментариев автора. Ниже классы/сущности, смысл которых
не считывается однозначно из кода.

### Wastelands
- `Character` (`Wastelands.Service.Domain/Entities/Character.cs`) — статы
  Int/Str/Rea/Dex/Cra/Emp/Wil и словарь `Skills`.
- `Skill` (`Wastelands.Service.Domain/Enums/Skill.cs`) — навыки, сгруппированные
  по управляющей характеристике.
- `User` (`Wastelands.Service.Domain/Entities/User.cs`) — соотносится ли с
  `Witcher.Core/DAL/Entities/User.cs` / `UserAccount` / `UserGame` /
  `UserGameCharacter` из первой версии, или это независимая сущность.

### Witcher (первая версия — на будущее, если этот домен переносится в Wastelands)
- `BodyTemplate` / `BodyTemplatePart` vs `CreatureTemplate` /
  `CreatureTemplatePart` vs `Creature` / `CreaturePart` — трёхуровневая схема
  шаблон тела → шаблон существа → существо в бою.
- `AttackProcess` / `AttackData` (`Witcher.Core/Logic`) — ядро разрешения атаки.
- `CritEffect` и наследники (`Simple/Difficult/Deadly/Complex` × часть тела) —
  таблица тяжести критических эффектов.
- `Effect` и наследники (`BleedEffect`, `PoisonEffect`, `FreezeEffect`,
  `StunEffect` и т.д.) — система состояний/статусов.
- `Ability` / `DefensiveSkill` — система способностей; как соотносится с
  `Skill` из Wastelands.
- `Game` / `UserGame` / `GameRole` — управление игровой сессией/столом.

<!-- Комментарии автора вставить сюда по мере поступления -->
