# Resumen de la sección 7: Comandos y Migraciones
___

1. __Comando `Get-Help`.__
2. __Comando `Add-Migration`.__ 
3. __Comando `Update-Database`.__ 
4. __Comando `Remove-Migration`.__
5. __Comando `Get-Migration`.__
6. __Comando `Drop-Database`.__
7. __Modificando una migración manualmente.__
8. __Migration Bundles.__
9. __Comando `Script-Migration`__

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

Volvemos a verificar si se aplicaron las migraciones en nuestra base de datos. 

![lista-migraciones-2](/PeliculasWebAPI/images/lista-migraciones2.PNG)

#### Comando `Remove-Migration`

Podemos remover migraciones, se puede hacer en dos escenarios: 

_Remover migración que no ha sido aplicada a la base de datos._  

El cual para este ejemplo agregaremos otro campo a nuestra entidad `Genero.cs`, al agregar la migración, veremos la forma de revertirla.

Con `Remove-Migration` removemos la migración que se había agregado, esto no afectará, puesto que aún no ha sido agregada a la base de datos. 

![remove-migration](/PeliculasWebAPI/images/remove-migration.PNG)

_Remover migración que si ha sido aplicado a la base de datos._

Ahora bien, si aplicamos la migración hacia la base de datos, vemos la forma de revertirlo. Por lo cual agregamos la migración y empujamos los cambios hacia la base de datos. 

![update-database-remove](/PeliculasWebAPI/images/remove-migration-desdeBD.PNG)

Vemos las migraciones que se han aplicado en la base de datos. 

![lista-migraciones-3](/PeliculasWebAPI/images/lista-migraciones3.PNG)

Si aplicamos `Remove-Migration` nos devolverá un error, puesto que ya la migración se aplicó a la base de datos, por lo cual este comando ya no nos funciona. 

Pero si aplicamos `Remove-Migration -Force`, lo que hará es aplicar el método `Down()` de la migración que se hizo, el cual revierte dicha migración.

![remove-migration-force](/PeliculasWebAPI/images/remove-migration-force.PNG)

Si verificamos desde nuestra base de datos la lista de migraciones, notamos que ya no existe dicha migración. 

![migracion-removida](/PeliculasWebAPI/images/lista-migraciones4.PNG)

Ahora bien, si queremos solo remover una migración parcialmente, puesto que en una migración puede tener muchos cambios, veamos el ejemplo de la migración `GeneroExample.cs`

Para ello no usamos el comando de `Remove-Migration` sino que hacemos una nueva migración anulando lo que queremos resetear.

Para ello modificamos la entidad `Genero.cs` para hacer los cambios pertinentes, al generar la nueva migración, ya se eliminan los campos que queríamos que se quitaran. 

![campos-removidos](/PeliculasWebAPI/images/eliminar-campos-generos.png)

Al ejecutar los cambios y agregarlos a la base de datos, esta se aplica 

![aplicacion-migracion](/PeliculasWebAPI/images/nueva-migracion.PNG)

#### Comando `Get-Migration`

Con este comando se pueden visualizar las migraciones aplicadas y pendientes que ha tenido nuestra aplicación, desde la primera hasta la actual. 

En la columna `Applied` vemos si tiene un status `True` o `False` el cual menciona la migración si ha sido aplicada o no en la base de datos. 

![Get-Migration](/PeliculasWebAPI/images/Get-Migration.PNG)

Si hacemos `Get-Migration -NoConnect` esta enlistará todas las migraciones pero sin información en la columna `Applied` puesto que solo lista las migraciones pero sin conectarse a la base de datos. 

![Get-Migration-NoConnect](/PeliculasWebAPI/images/Get-Migration-NoConnect.PNG)

#### Comando `Drop-Database`

Sirve para borrar una base de datos, este comando se debe usar con suma precaución


#### Modificando una migración manualmente

Una migración permite configurar nuestra base de datos, también las migraciones provienen en base a las modificaciones que se hacen en nuestros modelos. 

Por lo que podemos configurar una migración de manera manual, por ejemplo, de una migración vacía llamada `VistaPeliculaConteo.cs`

![migracion-manual](/PeliculasWebAPI/images/migracion-manual.png)

Otro caso de migración manual, fue en `EjemploPersona.cs`, donde modificamos una propiedad `OnDelete` en modo `Restrict`. 

![migracion-manual2](/PeliculasWebAPI/images/migracion-manual2.png)

#### Migration Bundles o Empaquetado de Migraciones

Tenemos que con el comando `Update-Database` podemos empujar los cambios a nuestra base de datos, pero no siempre esta forma es la más conveniente. 

Si tenemos una base de datos en docker o en un servidor el cual no tiene un empaquetado de .NET, la solución para esto es hacer un `Migration Bundles` el cual crea un ejecutable el cual se puede correr en una base de datos y esta va a ejecutar las migraciones pendientes por aplicarse. 

Es decir, es un pequeño programa con las migraciones configuradas. 

Por ejemplo, si queremos empaquetar nuestras migraciones, en el directorio raíz de nuestro proyecto, ejecutamos una terminal de _PowerShell_ y damos el siguiente comando:

    dotnet ef migrations bundle --configuration Bundle

![migration-bundle](/PeliculasWebAPI/images/migration-bundle.PNG)

El cual nos va crear el ejecutable el cual contiene todas las migraciones de nuestra aplicación. 

![migration-bundle-ejecutable](/PeliculasWebAPI/images/migration-bundle-ejecutable.PNG)

Si queremos ejecutar nuestro bundle en cualquier base de datos, lo hacemos con el siguiente comando en _PowerShell_:

    .\efbundle.exe --connection "Connection_String"

Si no existe la base de datos, la crea como si ejecutaramos `Update-Database` como lo hemos venido trabajando. 

Si existe la base de datos, solo actualiza los cambios o migraciones pendientes. 

Si tenemos una nueva migración, podemos generar o actualizar nuestro Bundle existente, con el comando: 

    dotnet ef migrations bundle --configuration Bundle --force

#### Comando `Script-Migration`

Además de `Update-Database` y del comando de Bundle Migration, otra forma de actualizar nuestra base de datos es por medio `Script-Migration`, el cual se encarga de generar un script de SQL para llevar acabo la actualización de la base de datos. 

![script-migration](/PeliculasWebAPI/images/script-migration.PNG)

Pero hay una desventaja de usar `Script-Migration` y es cuando tenemos código personalizado en una migración el no tiene un `idempotent`, ya que si lo hacemos solo con `Script-Migration` al empujar los cambios a la base de datos, esta nos devolverá un error ya que hay migraciones que tienen el mismo contenido SQL y daría un error de duplicado.

![script-migration-dup](/PeliculasWebAPI/images/script-migration2.PNG)

Para solucionarlo, se aplica:

    Script-Migration --Idempotent

![script-idem](/PeliculasWebAPI/images/script-migration-idem.PNG)

Como resultado tendremos ya las migraciones y sus diferencias. 

![script-idem-2](/PeliculasWebAPI/images/script-migration-idem2.PNG)

Pero como se mencionó antes, la desventaja de usar `Script-Migration -Idempotent` es que si tenemos en una migración código personalizado, no será posible ejecutar el código SQL hacia la base de datos, ya que dará un error de sintaxis, por el cual en un proceso continúo de cambios, se recomienda usar mejor `Migration Bundle` ya que este proceso empaquetará mejor los cambios que queremos ejecutar hacia la base de datos. 

