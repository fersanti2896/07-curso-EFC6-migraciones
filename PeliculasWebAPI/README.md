# Resumen de la sección 6: Configurando Relaciones

Existen 3 tipos de manera de modificar o configurar las relaciones entre clases, y son: 

1. __Relaciones por Convenciones.__ 
1.1 __Relaciones Requeridas y Opcionales.__
2. __Relaciones por Anotación de Datos.__
3. __Relaciones por API Fluente.__

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





