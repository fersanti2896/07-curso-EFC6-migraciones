# Resumen de la sección 7: Comandos y Migraciones

1. __Comando `Get-Help`.__
2. __Comando `Add-Migration`.__ 
3. __Comando `Update-Database`.__ 

#### Comando `Get-Help`

Sirve para ver la documentación de los distintos comandos que podemos utilizar para las migraciones

#### Comando `Add-Migration`

Es un comando que plasma los cambios a realizar en nuestra base de datos, es decir, si agregamos una entidad o propiedad nueva que haga un cambio de configuración en la `API Fluent` se hace una migración para replicar los cambios en la base de datos. 

Por ejemplo, en nuestra entidad `Genero.cs` agregamos una nueva propiedad, después generamos una nueva migración con el comando `Add-Migration`. 

El cual la migración tendrá el nombre de `GeneroExample`.

El cual aquí tiene dos métodos, el método `Up()` que se encarga de aplicar los cambios en la base de datos y `Down()` es aquél que se ejecuta si se necesita revertir la migración a nivel de la base de datos. 

Es decir, el método `Down()` hace lo contrario al método `Up()`. 

![migracion-example](/PeliculasWebAPI/images/migracion-example.png)

#### Comando `Update-Database`

Permite tomar las migraciones y aplicarlas en nuestra base de datos destino. 

Podemos especificiar una migració o una colección de migraciones específicas, sin embargo, si solo usamos `Update-Database` va aplicar los cambios a la base de datos las migraciones que no se han ido aplicando. 

![update-database](/PeliculasWebAPI/images/update-database.PNG)

Ahora bien, si solo queremos aplicar la actualización a una migración específica, pero esta debe ser en orden cronológico, es decir, debe ser la primera migración antigüa, mas no la actual.

Consideremos dos migraciones vacías, con nombres:

`Migration-primera.cs`

![primera](/PeliculasWebAPI/images/migracion-primera.png)

`Migration-segunda.cs`

![segunda](/PeliculasWebAPI/images/migracion-segunda.png)

Al especificar la migración con `Update-Database -Migration Primera`

![migracion-especifica](/PeliculasWebAPI/images/update-database-especifico.PNG)

Si queremos asegurarnos de que solo se aplicó la migración en especifíco, comprobamos en nuestro motor de base de datos. 

![lista-migracion](/PeliculasWebAPI/images/lista-migraciones.PNG)

Podemos aplicar las demás migraciones restantes con `Update-Database`.

Aquí empuja las migraciones restantes a nuestra base de datos. 

![migraciones-restantes](/PeliculasWebAPI/images/update-database2.PNG)

Volvemos a verificar si se aplicaron las migraciones en nuestra bae de datos. 

![lista-migraciones-2](/PeliculasWebAPI/images/lista-migraciones2.PNG)