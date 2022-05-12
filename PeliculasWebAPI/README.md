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

![cineOfertaEntidad](/PeliculasWebAPI/images/cineOfertaRelConv.png)

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