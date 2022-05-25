# Resumen de la sección 6: Configurando Relaciones

Existen 3 tipos de manera de modificar o configurar las relaciones entre clases, y son: 

1. __Relaciones por Convenciones.__ 
1.1 __Relaciones Requeridas y Opcionales.__
2. __Relaciones por Anotación de Datos.__
3. __Relaciones por API Fluente.__
4. __División de Tablas__
5. __Entidades de Propiedad__
6. __Herencia__

### Relaciones por Convenciones

Las relaciones detectadas por convención siempre tendrán como destino la clave principal de la entidad principal. Para establecer como destino una clave alternativa, se debe realizar una configuración adicional mediante la API de Fluente.

En el siguiente caso podemos ver un ejemplo de Relaciones por Convención: 

Entidad: __Cine__

![cineEntidad](/PeliculasWebAPI/images/cineRelConv.png)

Entidad: __CineOferta__

![cineOfertaEntidad](/PeliculasWebAPI/images/cineOfertaRelCon.png)

#### 1.1 Relaciones Requeridas y Opcionales

__Relación Requerida__

Cuando se quiere eliminar un dato que tiene relación uno a uno o uno a muchos, se tiene la relación requerida, por ejemplo, se quiere eliminar un registro de la Tabla `Cines`, pero que tiene relación con la tabla `CineOfertas`.

![tablaCinesYCOfertas](/PeliculasWebAPI/images/dataOriginal.PNG)

El cual al borrar el registro `10` de la tabla Cines, desde nuestro endpoint, nos devuelve un resultado con status `200`.

![status](/PeliculasWebAPI/images/dataBorrado.PNG)

Al verificar en nuestra Base de Datos, efectivamente eliminó el registro `10` de la Tabla `Cines` y su relación en `CinesOfertas`.

![cineBorrado](/PeliculasWebAPI/images/cineBorradoAfecta.PNG)

__Relación Opcional__

Del mismo ejemplo, queremos evitar que se elimine ambos registros, solo que se deje el segundo registro pero con la relación nula. Para ello se modifica nuestra Entidad `CineOferta` en donde la propiedad que hace la relación hacia la Entidad `Cine` se usa `?` para establecer que la relación será opcional. 

![cineOfertaOpcional](/PeliculasWebAPI/images/cineOfertaOpcional.png)

Haciendo la migración para que se apliquen los cambios en nuestra Base de Datos. 

![migracionCinesOferta](/PeliculasWebAPI/images/migracionCinesOpcional.png)

Al crear otro endpoint ya que ya existe uno para eliminar pero con la _relación requerida_, en nuestro `CinesController.cs` creamos el endpoint donde con el método `include()` incluirá solo el borrado del `Cine.Id`

![cinesController](/PeliculasWebAPI/images/CinesControllerOpcional.png)

Probando las modificaciones, tenemos el registro a eliminar `11` de la Tabla `Cines` con su relación en la Tabla `CinesOfertas` en el registro `9`. 

![dataOriginal2](/PeliculasWebAPI/images/dataOriginal.PNG)

Al eliminar el id `11` desde el endpoint, recibimos el status `200`.

![dataBorrado2](/PeliculasWebAPI/images/dataBorrado2.PNG)

Al verificar en nuestra Base de Datos, se eliminó el registro de la Tabla `Cines` pero se conserva el registro que tiene la relación en a Tabla `CinesOfertas` pero en su columna `CineId` aparece como `NULL`.

![cineBorradoFinal](/PeliculasWebAPI/images/cineBorradoOpcional.PNG)

### Relaciones por Anotación de Datos

__Anotación InverseProperty__

Se usa cuando se tienen dos relaciones con la misma clase. 

En el ejemplo tenemos a la clase Persona y Mensaje, en el cual se tiene dos relaciones de Mensaje a la entidad Persona. 

Tenemos la Entidad `Mensaje` donde almacenará los mensajes entre dos personas. 

![mensajeEntidad](/PeliculasWebAPI/images/mensajeEntidad.png)

Tenemos la Entidad `Persona` donde se hará dos relaciones con la propiedad `InverseProperty`.

![personaEntidad](/PeliculasWebAPI/images/personaEntidad.png)

Al crear un data seeding para datos de prueba se hace la migración. 

![seeding](/PeliculasWebAPI/images/seedingPersona.png)

Una vez hecha la migración, se hace el endpoint en `PersonasController`, donde se tiene una petición `HttpGet`.

![PersonasController](/PeliculasWebAPI/images/PersonasController.png)

Finalmente al probar el endpoint, nos devuelve el resultado esperado:

![endpointPersonas](/PeliculasWebAPI/images/personaIdResult.PNG)

__Relación uno a uno por API Fluent__

Tenemos dos tablas o entidades de nombre _Cine_ y _CineOferta_ el cual un Cine solo tiene un Cine Oferta, esta relación se puede configurar desde su API Fluent del Cine

En `CineConfig.cs` se hace la relación con el método `HasOne(c => c.CineOferta)` el cual indica que es una relación `uno-uno` de la propiedad CineOferta que está definida en la entidad `Cine.cs`.

Con el método `WithOne()` indica a que propiedad será la relación.

Con el método `HasForeignKey<CineOferta>(co => co.CineId)` indica que tomará la propiedad `CineId` de la entidad `CineOferta.cs` como llave forenea en `Cine.cs`.

![RCine-Oferta](/PeliculasWebAPI/images/Cine-CineOferta.png)

__Relación uno a muchos por API Fluent__

Tenemos dos tablas o entidades de nombre _Cine_ y _SalasCine_, en el cual un Cine puede tener muchas Salas de Cine, esta relación se puede configurar desde su API Fluent del Cine.

En `CineConfig.cs` se hace la relación con el método `HasMany(c => c.CineOferta)` el cual indica que es una relación `uno-muchos` de la propiedad SalaCine que está definida en la entidad `Cine.cs`.

Con el método `WithOne(s => s.Cine)` indica a que propiedad será la relación de la enitidad `SalaCine.cs`. 

Con el método `HasForeignKey(s => s.CineId)` indicará que tomará la propiedad `CineId` de la entidad `SalaCine.cs` como llave foranea en `Cine.cs`.

![RCine-SalaCine](/PeliculasWebAPI/images/Cine-SalaCine.png)

__Relación muchos a muchos en API Fluent con una clase intermedia__

Para indicar una relación `muchos a muchos`, se definen dos relaciones `uno a muchos`, tenemos dos tablas _Pelicula_ y _Actor_ donde una Película puede tener muchos Actores y un Actor puede estar participando o actuado en muchas Películas. 

En `PeliculaActorConfig.cs` se hace la doble relación que se hace en `uno a muchos`. 

![Pelicula-Actor](/PeliculasWebAPI/images/Pelicula-Actor.png)


__Relación muchos a muchos en API Fluent sin una clase intermedia (skipNavigation)__

Para indicar una relación `muchos a muchos`, entre dos tablas sin contar con la entidad intermedia, el ejemplo lo haremos entre las entidades `Pelicula.cs` y `Genero.cs`. 

En `PeliculaConfig.cs` se hace la relación `muchos-muchos`, donde un Película tiene muchss Géneros con el método `HasMany(p => p.Generos)` y un Género tiene muchas Películas con el método `WithMany()` y creamos la tabla intermedia con el método `UsignEntity()` y `ToTable()`.

![Pelicula-Genero](/PeliculasWebAPI/images/Pelicula-Genero.png)

__Borrado por OnDelete desde API Fluent__

__Método `OnDelete()`__

Cuando se tiene una relación entre dos entidades, está la entidad principal y la dependiente, pero cuando se borra la principal, qué puede pasar con la entidad dependiente. 

El cual al usar el método `OnDelete()` podemos usar las opciones: 

`Cascade`, el cual las entidades dependientes son borradas sí la entidad principal es borrada a nivel de base de datos aunque no todos los motores de bases de datos la soportan.

`ClientCascade`, realiza el borrado de las entidades dependientes de la aplicación, pero esta requiere que al cargar la entidad principal también se carguen las entidades dependientes. 

`NoAction`, esta no hará ninguna operación, pero tendrá problemas si borramos la entidad principal, pero su entidad dependiente apuntará a un registro que no existe. 

`Client NoAction`, similar a `NoAction`.

`Restric`, es similar a `NoAction`

`SetNull`, coloca valores nulos en la llave foranea. 

`Client SetNull`, se coloca null desde la aplicación y no en la base de datos, pero se necesitan tener las entidades dependientes cargadas al momento de borrar la entidad principal.

Por ejemplo, queremos hacer una prueba eliminando un registro de una entidad principal (`Cine.cs`), veremos que pasa si usamos `Restrict` en `CineConfig.cs`.

![UsoRestrict](/PeliculasWebAPI/images/CineConfigRestric.png)

Tenemos nuestros registros con el _id_ 12 en la tabla/entidad Cines, que se relaciona con los _ids_ 19 y 20 de la tabla/entidad SalasCines. 

![registroDB](/PeliculasWebAPI/images/Cine-SalaCineBD.PNG)

Al querer borrar el _id_ 12 con 'Cinepolis con Restric' nos arroja un error, ya que no se puede borrar el registro con la propiedad `Restrict` que definimos. 

![ErrorBorrado](/PeliculasWebAPI/images/ErrorEliminadoCine.PNG)

Pero si queremos borrar el _12_ del Cine, debemos primero borrar el registro dependiente, desde `CineController.cs`

Con el método `Include()` incuimos la Sala de Cine a eliminar y lo removemos. 

![removerSalaCineController](/PeliculasWebAPI/images/borradoSalaCineDesdeController.png)

Al borrar el _12_ del Cine, ya nos manda una respuesta `200`. 

![CineBorradoRestric](/PeliculasWebAPI/images/CineBorradoWithRestric.PNG)

Al comprobar en la Base de Datos, notamos que se eliminó tanto el registro de la tabla/entidad principal así como el registro que era dependiente del _id_ borrado. 

![Cine-SalaCineBD](/PeliculasWebAPI/images/Cines-SalaCineBD.PNG)

### División de Tablas

__Table Splitting__

Tiene la función de dividir una tabla en varias entidades, es útil cuando se tiene una tabla con muchas columnas y se quieren separar esas columnas en distintas entidades, pero para implementar el __`Table Splitting`__ es necesario que una propiedad sea requerida.

En este ejemplo, queremos agregar más campos o columnas a nuestra tabla de _Cines_.

Creamos una entidad llamada `CineDetalle.cs`

![CineDetalle](/PeliculasWebAPI/images/CineDetalle.png)

Posteriormente en `CineConfig.cs` con el `API Fluent` hacemos que las entidades `Cine.cs` y `CineDetalle.cs` apunten a la misma tabla. 

![CineDetalle-Cine](/PeliculasWebAPI/images/CineDetalle-CineAPI.png)

Para dar las configuraciones de las propiedades de la entidad `CineDetalle.cs` creamos su clase de configuración `CineDetalleConfig.cs`.

El cual con el método `ToTable("Cines")` va mapear los campos a la tabla existente `Cines`. 

![CineDetalleConfig](/PeliculasWebAPI/images/CineDetalleConfig.png)

Una vez hecho esto, se hace la migración, donde ya se encuentran las nuevas columnas a agregar, después se empujan hacia la base de datos. 

![CineDetalleMigracion](/PeliculasWebAPI/images/CineDetalleMigracion.png)

Al insertar dos registros, y verificar en nuestra base de datos, tenemos los registros ya con las columnas. 

![CinesDB](/PeliculasWebAPI/images/CinesDB.PNG)

Si queremos buscar un _id_ solo nos traerá la información que se tiene en el endpoint, pero mostrará como _CineDetalle_ como nulo

![CineIdConsulta](/PeliculasWebAPI/images/CineIdConsulta.PNG)

Para ello hacemos la configuración en `CinesController.cs`.

![CinesControllerWithCineDetalle](/PeliculasWebAPI/images/CinesControllerCineDetalle.png)

Al volver hacer la búsqueda por el _id_ ya nos mostrará la información completa. 

![CineIdConsulta2](/PeliculasWebAPI/images/CineIdConsulta2.PNG)

### Entidades de Propiedad 

Es similar a la división de tablas, las entidades de propiedad nos permiten usar las columnas de una tabla en distintas clases. La diferencia entre Table Splitting, es que las Entidades de Propiedad tiene la entidad dependiente puede ser usada en muchas entidades. 

Tenemos el ejemplo de la entidad `Direccion.cs` podemos usar como una entidad de propiedad y se puede reutilizar en otras entidades, como `Cine.cs`y `Actor.cs`

Para convertir `Direccion.cs` a una entidad de propiedad, tenemos que marcarla con la propiedad `Owned` que significa que otra entidad de va adueñar de la entidad `Direccion.cs` y una propiedad debe ser `Required`.

![DireccionEntProp](/PeliculasWebAPI/images/DireccionEntidadProp.png)

Se añade la entidad de protección en `Actor.cs` y `Cine.cs`

![cineWithEntProp](/PeliculasWebAPI/images/CineWithEntidadProp.png)

Luego se hace la migración, pero se tiene con la nomenclatura `Direccion_(nombre de la propiedad)`.

![DireccionMigracionExam](/PeliculasWebAPI/images/MigracionDireccionOwnedExam.png)

Para cambiarlo, hacemos uso del `API Fluent` en `CineConfig.cs` y cambiamos con el método `OwnsOne()` sus propiedades. 

![CineConfigOwns](/PeliculasWebAPI/images/CineConfigOwns.png)

Se hace el mismo procedimiento en `ActorConfig.cs`

![ActorConfigOwns](/PeliculasWebAPI/images/ActorConfigOwns.png)

Hacemos de nuevo la migración y vemos que ya se han aplicado los cambios correctos. 

![ActorCineMigracion](/PeliculasWebAPI/images/ActorCineMigracion.png)

Al hacer la consulta a nuestras tablas `Cine` y `Actor` se visualizan los campos agregados. 

![ActorCineDB](/PeliculasWebAPI/images/CineActorDB.PNG)

### Herencia



