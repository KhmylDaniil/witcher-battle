# создание миграций замените Name на название вашей миграции
Add-Migration Name -Project Witcher.Storage.PostgreSql -StartupProject Witcher.MVC -verbose

# обновление дб
update-database
