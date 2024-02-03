# MAGIC VILLA API

Api de magic villa

Para que el proyecto funcione copiar el archivo appsettings.example.json en appsettings.json y configurar las variables de la conexion a la base de datos y el secret del token jwt

Una vez que se configuro la cadena de conexión a la base de datos hay que correr dos comandos para que todo funcione bien

1- Add-Migration ( o dotnet ef migrations add ) InitialMigration
2- Update-Database ( o dotnet ef database update )