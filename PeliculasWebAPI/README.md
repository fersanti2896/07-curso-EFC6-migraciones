# Resumen de la sección 6: Configurando Relaciones

Existen 3 tipos de manera de modificar o configurar las relaciones entre clases, y son: 

1. __Relaciones por Convenciones__ 
2. __Relaciones por Anotación de Datos__
3. __Relaciones por API Fluente__

### Relaciones por Convenciones

Las relaciones detectadas por convención siempre tendrán como destino la clave principal de la entidad principal. Para establecer como destino una clave alternativa, se debe realizar una configuración adicional mediante la API de Fluente.

En el siguiente caso podemos ver un ejemplo de Relaciones por Convención: 

Entidad: __Cine__

![cineEntidad](/PeliculasWebAPI/images/cineRelConv.png)

Entidad: __CineOferta__

![cineOfertaEntidad](/PeliculasWebAPI/images/cineOfertaRelConv.png)
