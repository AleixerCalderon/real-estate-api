# Pantallas
## Detalle de cada propiedad 
![DetallePropiedad](./Z-img/DetallePropiedad.png)
## Vista de los propietarios. 
![listPropietarios](./Z-img/listPropietarios.png)
## Panel principal
![pnel](./Z-img/pnel.png)
## Formulario para crear una propiedad. 
![propiedad](./Z-img/propiedad.png)
## Vista de propiedades en oferta 
![propiedades](./Z-img/propiedades.png)
## Formulario para crear nuevos propietarios. 
![propietario](./Z-img/propietario.png)

# pantalla del API en swagger
## /swagger/v1/swagger.json
![propietario](./Z-img/swagger.png)


# real-estate-api
Full-stack real estate property management API with .NET 9 and Next.js

## para ejecutar el back
# Ir a la carpeta del backend
cd backend

# Restaurar paquetes NuGet (primera vez)
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar la API
dotnet run --project RealEstate.API

# O despues de tener todo:
cd RealEstate.API
dotnet run

# http://localhost:5107/swagger


## para ejecutar el front
# Ir a la carpeta del frontend
cd frontend

# Instalar dependencias (primera vez)
npm install

# Ejecutar en modo desarrollo
npm run dev

# http://localhost:3000



#  Real Estate API

Sistema completo de gestión de propiedades inmobiliarias desarrollado con .NET 9 y Next.js.

##  Descripción

Esta aplicación permite gestionar propiedades inmobiliarias y sus propietarios a través de una API REST moderna y una interfaz web responsive. Incluye funcionalidades de búsqueda avanzada, filtros y operaciones CRUD completas.

##  Tecnologías Utilizadas

### Backend
- **.NET 9** - Framework principal
- **C#** - Lenguaje de programación
- **MongoDB** - Base de datos NoSQL
- **NUnit** - Framework de pruebas unitarias

### Frontend
- **Next.js 15** - Framework de React
- **TypeScript** - Tipado estático
- **Tailwind CSS** - Framework de CSS
- **Axios** - Cliente HTTP

##  Arquitectura

El proyecto sigue los principios de **Clean Architecture** con las siguientes capas:

- **Domain** - Entidades del negocio
- **Application** - Lógica de aplicación y DTOs
- **Infrastructure** - Acceso a datos y servicios externos
- **API** - Controladores y configuración web

## Estructura del Proyecto

```
real-estate-api/
├── backend/
│   ├── RealEstate.Domain/          # Entidades de dominio
│   ├── RealEstate.Application/     # Servicios y DTOs
│   ├── RealEstate.Infrastructure/  # Repositorios y MongoDB
│   ├── RealEstate.API/            # Controllers y configuración
│   └── RealEstate.Tests/          # Pruebas unitarias
└── frontend/
    ├── src/
    │   ├── app/                   # Páginas de Next.js
    │   ├── components/            # Componentes reutilizables
    │   ├── services/              # Servicios de API
    │   └── types/                 # Tipos de TypeScript
    └── public/                    # Archivos estáticos
```

##  Instalación y Configuración

### Prerequisitos
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [MongoDB](https://www.mongodb.com/) (local o Atlas)

### 1. Clonar el Repositorio
```bash
git clone https://github.com/tu-usuario/real-estate-api.git
cd real-estate-api
```

### 2. Configurar Backend

```bash
cd backend

# Restaurar dependencias
dotnet restore

# Configurar MongoDB
# Edita appsettings.Development.json con tu cadena de conexión
```

**Configuración de MongoDB (appsettings.Development.json):**
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb+srv://usuario:password@cluster.mongodb.net/",
    "DatabaseName": "RealEstateDB"
  }
}
```

### 3. Configurar Frontend

```bash
cd frontend

# Instalar dependencias
npm install

# Configurar variables de entorno
# Crear archivo .env.local
echo "NEXT_PUBLIC_API_URL=http://localhost:5107/api" > .env.local
```

##  Ejecución

### Ejecutar Backend
```bash
cd backend
dotnet run --project RealEstate.API
```
La API estará disponible en: `http://localhost:5107`
Swagger UI: `http://localhost:5107/swagger`

### Ejecutar Frontend
```bash
cd frontend
npm run dev
```
La aplicación web estará disponible en: `http://localhost:3000`

##  Pruebas

### Ejecutar Pruebas Unitarias
```bash
cd backend
dotnet test RealEstate.Tests
```

### Ejecutar con Cobertura
```bash
dotnet test RealEstate.Tests --collect:"XPlat Code Coverage"
```

##  API Endpoints

### Propiedades
- `GET /api/properties` - Obtener todas las propiedades
- `GET /api/properties/{id}` - Obtener propiedad por ID
- `GET /api/properties/search` - Buscar con filtros
- `POST /api/properties` - Crear nueva propiedad
- `PUT /api/properties/{id}` - Actualizar propiedad
- `DELETE /api/properties/{id}` - Eliminar propiedad

### Propietarios
- `GET /api/owners` - Obtener todos los propietarios
- `GET /api/owners/{id}` - Obtener propietario por ID
- `POST /api/owners` - Crear nuevo propietario
- `PUT /api/owners/{id}` - Actualizar propietario
- `DELETE /api/owners/{id}` - Eliminar propietario

##  Filtros de Búsqueda

La API soporta los siguientes filtros para propiedades:

- **name** - Buscar por nombre de propiedad
- **address** - Buscar por dirección
- **minPrice** - Precio mínimo
- **maxPrice** - Precio máximo
- **page** - Número de página (paginación)
- **pageSize** - Elementos por página

**Ejemplo:**
```
GET /api/properties/search?name=apartamento&minPrice=100000&maxPrice=500000&page=1&pageSize=10
```

##  Modelo de Datos

### Propiedad (Property)
```csharp
{
  "id": "string",
  "name": "string",
  "address": "string", 
  "price": "decimal",
  "codeInternal": "int",
  "year": "int",
  "idOwner": "string",
  "image": "string"
}
```

### Propietario (Owner)
```csharp
{
  "id": "string",
  "name": "string",
  "address": "string",
  "phone": "string",
  "birthday": "DateTime"
}
```

##  Características

### Backend
-  API REST completa con Swagger
-  Arquitectura limpia y escalable
-  Validaciones de datos robustas
-  Manejo de errores centralizado
-  Filtros y paginación avanzados
-  Pruebas unitarias completas
-  Documentación automática con Swagger

### Frontend
-  Interfaz moderna y responsive
-  Búsqueda y filtros en tiempo real
-  Gestión de estados optimizada
-  Componentes reutilizables
-  Manejo de errores elegante
-  Carga de datos optimizada

##  Funcionalidades de la Aplicación

1. **Listado de Propiedades** - Ver todas las propiedades con paginación
2. **Búsqueda Avanzada** - Filtrar por nombre, dirección y rango de precios
3. **Detalles de Propiedad** - Vista completa de información
4. **Gestión de Propiedades** - Crear, editar y eliminar propiedades
5. **Gestión de Propietarios** - CRUD completo de propietarios
6. **Dashboard** - Estadísticas y métricas en tiempo real

##  Configuración Adicional

### CORS
El backend está configurado para permitir conexiones desde:
- `http://localhost:3000` (desarrollo frontend)

### Base de Datos
El sistema incluye datos de ejemplo que se crean automáticamente al iniciar la aplicación.

### Logs
Los logs de aplicación se configuran en `appsettings.json` y muestran información detallada en desarrollo.

##  Solución de Problemas

### Error de Conexión MongoDB
- Verificar que la cadena de conexión sea correcta
- Asegurar que MongoDB esté ejecutándose
- Verificar permisos de acceso de red en MongoDB Atlas

### Error CORS
- Verificar que el frontend esté en `http://localhost:3000`
- Revisar configuración de CORS en `Program.cs`

### Error de Compilación Frontend
```bash
# Limpiar cache y reinstalar
rm -rf node_modules package-lock.json
npm install
```

## Mejoras Futuras

- [ ] Autenticación y autorización (JWT)
- [ ] Carga de imágenes con almacenamiento en la nube
- [ ] Notificaciones en tiempo real
- [ ] Dashboard de administración avanzado
- [ ] Integración con servicios de mapas
- [ ] Exportación de datos a PDF/Excel

##  Desarrollo

Este proyecto fue desarrollado siguiendo las mejores prácticas de la industria:

- **Clean Architecture** para mantenibilidad
- **SOLID Principles** para código escalable
- **Test-Driven Development** para calidad
- **Responsive Design** para accesibilidad
- **Type Safety** con TypeScript

##  Licencia

Este proyecto es de uso educativo y profesional.

---

**Desarrollado por:** Aleixer Alvarado Bernal
**Fecha:** Agosto 2025  
**Tecnologías:** .NET 9, Next.js, MongoDB, TypeScript