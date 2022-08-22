Данный проект должен представлять собой помощника ГМ для системы Witcher TRPG. Он сконструирован как back-end приложение, поскольку его основу  позаимствовал у своего ментора. В будущем проект получит отдельную фронт-часть.
На данный момент проект работает через сваггер и позволяет создавать шаблоны существ для системы Witcher TRPG и осуществлять бои между существами, основанными на шалонах, включая наложение критических эффектов. Следующая цель - создание интерфейса пользователя для замены сваггера.
Проект состоит из следующих модулей:
Sindie.ApiService.Core - хранит логику и сущности.
Sindie.ApiService.Storage.Postgresql - хранит конфигурацию базы данных.
Sindie.ApiService.WebApi - исполняемый проект, на данный момент хранит контроллеры и настройки.
Sindie.ApiService.UnitTest - юнит-тесты для обработчиков и сервисов.
