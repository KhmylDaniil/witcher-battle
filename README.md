Данный проект представляет собой помощника ГМ для ролевой системы Witcher TRPG. Он сконструирован как MVC приложение.
На данный момент проект позволяет создавать шаблоны существ с использованием разных моделей распеределения попаданий (BodyTemplate) для системы Witcher TRPG и осуществлять бои между существами, основанными на шаблонах, включая распределение ударов по частям тела, работу с броней, наложение и снятие состояний, наложение и стабилизацию критических эффектов.

Проект состоит из следующих модулей (ветка Master):
Witcher.Core - хранит логику и сущности.
Witcher.Postgresql - хранит конфигурацию базы данных.
Witcher.MVC - исполняемый проект, на данный момент хранит контроллеры, представления и настройки.
Witcher.UnitTest - юнит-тесты для обработчиков и сервисов.
