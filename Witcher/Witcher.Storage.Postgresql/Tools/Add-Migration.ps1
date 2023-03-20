# создание миграций замените Name на название вашей миграции
Add-Migration Name -Project Sindie.ApiService.Storage.PostgreSql -StartupProject Sindie.ApiService.WebApi -verbose

# обновление дб
update-database
