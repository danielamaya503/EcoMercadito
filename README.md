# EcoMercadito API

Web API de EcoMercadito desarrollada con .NET 10.

Este documento explica paso a paso cómo preparar el entorno de desarrollo, clonar el repositorio, configurar la base de datos, compilar y ejecutar la API localmente. También describe la organización del proyecto y los archivos de configuración.

---

## Índice

1. [Requisitos previos](#1-requisitos-previos)
2. [Clonar el repositorio](#2-clonar-el-repositorio)
3. [Ingresar a la carpeta del proyecto](#3-ingresar-a-la-carpeta-del-proyecto)
4. [Configurar la base de datos](#4-configurar-la-base-de-datos)
5. [Configurar la cadena de conexión](#5-configurar-la-cadena-de-conexión)
6. [Compilar el proyecto](#6-compilar-el-proyecto)
7. [Ejecutar la API](#7-ejecutar-la-api)
8. [Probar la API](#8-probar-la-api)
9. [Detener la API](#9-detener-la-api)
10. [Estructura del proyecto](#10-estructura-del-proyecto)
11. [Comandos útiles](#11-comandos-útiles)
12. [Solución de problemas frecuentes](#12-solución-de-problemas-frecuentes)
13. [Relación con el frontend](#13-relación-con-el-frontend)
14. [Inicio rápido](#14-inicio-rápido)

---

## 1. Requisitos previos

Antes de comenzar, debes tener instaladas las siguientes herramientas:

| Herramienta                                    | Propósito                                                                                                                     |
| ---------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------- |
| .NET 10 SDK                                    | Compilar y ejecutar la API.                                                                                                   |
| Git                                            | Clonar el repositorio y gestionar los cambios del código.                                                                     |
| SQL Server o motor de base de datos compatible | Ejecutar la base de datos del proyecto.                                                                                       |
| Editor de código                               | Abrir y modificar el proyecto. Puedes utilizar Visual Studio, Visual Studio Code u otro editor.                               |
| Cliente SQL                                    | Ejecutar scripts de base de datos. Puedes utilizar SQL Server Management Studio, Azure Data Studio u otro cliente compatible. |

### 1.1. Instalar .NET 10 SDK

Puedes descargar .NET 10 desde:

https://dotnet.microsoft.com/download

Selecciona el instalador correspondiente a tu sistema operativo.

Después de instalarlo, abre una terminal y verifica la instalación:

```bash
dotnet --version
```

El comando debe mostrar una versión de .NET 10.

Si el comando muestra una versión diferente, revisa que el SDK de .NET 10 esté instalado correctamente.

### 1.2. Instalar Git

Puedes descargar Git desde:

https://git-scm.com/downloads

Después de instalarlo, verifica la instalación:

```bash
git --version
```

### 1.3. Instalar un motor de base de datos

El proyecto está orientado al uso de SQL Server.

Puedes utilizar:

- SQL Server Developer Edition.
- SQL Server Express.
- Otra edición compatible con tu entorno.

Para administrar la base de datos, puedes utilizar:

- SQL Server Management Studio (SSMS).
- Azure Data Studio.
- Otro cliente compatible.

> El nombre exacto de la instancia y la versión pueden variar según tu entorno.

### 1.4. Instalar un editor de código

Puedes utilizar:

- Visual Studio con carga de trabajo de desarrollo ASP.NET y web.
- Visual Studio Code con la extensión C# Dev Kit.

Si utilizas Visual Studio Code, instala las extensiones recomendadas para .NET.

---

## 2. Clonar el repositorio

### 2.1. Abrir una terminal

Puedes utilizar:

- La terminal integrada de Visual Studio Code.
- PowerShell.
- Símbolo del sistema.
- Git Bash.
- La terminal de Linux o macOS.

### 2.2. Ubicarte en la carpeta donde guardarás el proyecto

Por ejemplo, si utilizas una carpeta llamada `proyectos`:

```bash
cd proyectos
```

Si todavía no existe, puedes crearla:

```bash
mkdir proyectos
cd proyectos
```

> La ubicación es opcional. Puedes guardar el repositorio en cualquier carpeta donde tengas permisos de escritura.

### 2.3. Clonar EcoMercadito API

Ejecuta:

```bash
git clone <URL_DEL_REPOSITORIO_API>
```

Reemplaza `<URL_DEL_REPOSITORIO_API>` por la URL real del repositorio de la API.

> Utiliza la URL del repositorio de la API, no la del frontend.

Este comando descargará el repositorio en una carpeta con el nombre del proyecto.

Si GitHub solicita autenticación, utiliza una cuenta que tenga acceso al repositorio.

---

## 3. Ingresar a la carpeta del proyecto

Primero, entra al repositorio:

```bash
cd <NOMBRE_DEL_REPOSITORIO_API>
```

Reemplaza `<NOMBRE_DEL_REPOSITORIO_API>` por el nombre de la carpeta creada al clonar.

### 3.1. Verificar que estás en la carpeta correcta

Antes de compilar, verifica que la carpeta actual contenga el archivo de solución o el archivo de proyecto:

- Un archivo `.sln`, por ejemplo:

```text
EcomercaditoAPI.sln
```

- O un archivo `.csproj`, por ejemplo:

```text
EcomercaditoAPI.csproj
```

Para listar el contenido en Windows:

```bash
dir
```

En Git Bash, Linux o macOS:

```bash
ls
```

> Los comandos de compilación y ejecución de esta guía deben ejecutarse dentro de la carpeta que contiene el archivo de solución o el archivo de proyecto principal.

### 3.2. Abrir el proyecto en Visual Studio o Visual Studio Code

Si utilizas Visual Studio:

```text
Archivo → Abrir → Proyecto o solución → EcomercaditoAPI.sln
```

Si utilizas Visual Studio Code:

```bash
code .
```

El punto `.` indica que quieres abrir la carpeta actual.

Si el comando no está disponible, abre Visual Studio Code y selecciona:

```text
Archivo → Abrir carpeta → <NOMBRE_DEL_REPOSITORIO_API>
```

---

## 4. Configurar la base de datos

El repositorio incluye un script de base de datos para que quien utilice el proyecto pueda crear la estructura necesaria y acceder a la base de datos con fines educativos.

### 4.1. Ubicar el script de base de datos

Busca en el repositorio un archivo SQL, por ejemplo:

```text
script_db.sql
```

El nombre exacto puede variar. Consulta la documentación del repositorio o la estructura de carpetas para identificarlo.

### 4.2. Crear la base de datos

Conéctate a tu instancia de SQL Server utilizando tu cliente preferido.

Crea una base de datos para el proyecto, por ejemplo:

```sql
CREATE DATABASE EcoMercaditoDB;
```

Reemplaza `EcoMercaditoDB` por el nombre que utilizarás en la cadena de conexión.

### 4.3. Ejecutar el script

Abre el script SQL en tu cliente y ejecútalo contra la base de datos que acabas de crear.

El script debe:

- Crear las tablas necesarias.
- Insertar datos iniciales, si corresponde.
- Configurar relaciones y restricciones.

> Ejecuta el script en la base de datos que utilizará la API. No lo ejecutes en una base de datos de producción sin revisar su contenido.

### 4.4. Verificar la creación de objetos

Después de ejecutar el script, revisa que:

- Las tablas esperadas existan.
- Las columnas tengan los tipos de datos adecuados.
- Existan datos iniciales, si el script los incluye.

---

## 5. Configurar la cadena de conexión

La API utiliza un archivo de configuración para definir la cadena de conexión a la base de datos.

### 5.1. Archivo `appsettings.json`

Ubica el archivo:

```text
appsettings.json
```

Este archivo contiene una sección llamada `ConnectionStrings` con una propiedad llamada `DefaultConnection`.

Un ejemplo de configuración:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=NOMBRE_DEL_SERVIDOR;Database=EcoMercaditoDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Reemplaza los valores según tu entorno:

- `NOMBRE_DEL_SERVIDOR`: nombre de tu instancia de SQL Server.
- `EcoMercaditoDB`: nombre de la base de datos que creaste.
- Ajusta las opciones de autenticación según corresponda.

> No compartas cadenas de conexión con credenciales sensibles en repositorios públicos. Utiliza variables de entorno o archivos locales excluidos del control de versiones cuando sea necesario.

### 5.2. Variables de entorno (opcional)

Puedes definir la cadena de conexión mediante una variable de entorno, por ejemplo:

```bash
ConnectionStrings__DefaultConnection="Server=NOMBRE_DEL_SERVIDOR;Database=EcoMercaditoDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

El formato exacto puede variar según el sistema operativo y la forma de ejecución.

> La API leerá la cadena de conexión configurada en `appsettings.json` o mediante variables de entorno, según la configuración del proyecto.

---

## 6. Compilar el proyecto

Dentro de la carpeta que contiene el archivo de solución o el archivo de proyecto principal, ejecuta:

```bash
dotnet build
```

Este comando compila el proyecto y sus dependencias.

### 6.1. Verificar que la compilación fue exitosa

La terminal debe indicar que la compilación se completó sin errores.

Si aparecen errores:

1. Lee el mensaje de error.
2. Identifica el archivo y la línea afectados.
3. Corrige el problema antes de continuar.

### 6.2. Compilación en modo Release

Para compilar en modo de publicación:

```bash
dotnet build --configuration Release
```

Este modo habilita optimizaciones y deshabilita algunas características de depuración.

> Para desarrollo local, el modo Debug es suficiente.

---

## 7. Ejecutar la API

### 7.1. Ejecutar desde la terminal

Dentro de la carpeta del proyecto, ejecuta:

```bash
dotnet run
```

Este comando inicia la API y muestra la dirección en la que está escuchando.

### 7.2. Dirección de la API

La terminal mostrará direcciones similares a:

```text
Now listening on: http://localhost:5000
Now listening on: [https://localhost:5001](https://localhost:5001)
```

> Las direcciones y los puertos pueden variar según la configuración del proyecto y del sistema operativo.

Utiliza estas direcciones para probar los endpoints desde el navegador o desde herramientas como Postman.

### 7.3. Ejecutar desde Visual Studio

Si utilizas Visual Studio:

1. Abre la solución.
2. Selecciona el proyecto de la API como proyecto de inicio.
3. Presiona F5 o haz clic en el botón de inicio.

Visual Studio iniciará la API y abrirá el navegador en la dirección configurada.

### 7.4. Ejecutar desde Visual Studio Code

Si utilizas Visual Studio Code con la extensión C# Dev Kit:

1. Abre la carpeta del proyecto.
2. Utiliza el comando de ejecución de .NET o presiona F5 si está configurado.

---

## 8. Probar la API

### 8.1. Endpoint de prueba

Abre en el navegador la dirección base de la API, por ejemplo:

```text
http://localhost:5000
```

Si el proyecto incluye un endpoint de prueba, debería responder con información básica o un mensaje de estado.

### 8.2. Utilizar Swagger (si está habilitado)

Si la API tiene habilitada la documentación de Swagger, puedes acceder a:

```text
http://localhost:5000/swagger
```

o

```text
[https://localhost:5001/swagger](https://localhost:5001/swagger)
```

Desde Swagger puedes:

- Explorar los controladores disponibles.
- Probar los endpoints directamente desde el navegador.
- Observar los modelos de solicitud y respuesta.

> La disponibilidad de Swagger depende de la configuración del proyecto.

### 8.3. Probar con Postman u otra herramienta

Puedes utilizar Postman, Insomnia u otra herramienta para:

- Enviar solicitudes GET, POST, PUT, DELETE.
- Probar autenticación, si corresponde.
- Validar las respuestas de la API.

Configura la dirección base según la que muestre la terminal al ejecutar la API.

---

## 9. Detener la API

En la terminal donde está ejecutándose la API, presiona:

```text
Ctrl + C
```

Si la terminal solicita confirmar la finalización del proceso, acepta la confirmación.

Para iniciar nuevamente la API:

```bash
dotnet run
```

No necesitas volver a clonar el repositorio ni recompilar el proyecto cada vez que quieras ejecutar la API, a menos que hayas realizado cambios en el código.

---

## 10. Estructura del proyecto

La estructura principal del proyecto es:

```text
EcomercaditoAPI/
├── EcomercaditoAPI/               # Proyecto principal (controllers y configuración)
├── EcomercaditoAPI.Concrete/      # Contexto de base de datos y servicios
├── EcomercaditoAPI.Interfaces/    # Interfaces de servicios y repositorios
├── EcomercaditoAPI.Models/        # Modelos de dominio
└── EcomercaditoAPI.ViewModels/    # Modelos de vista para solicitudes y respuestas
```

Las siguientes descripciones sirven como guía de organización. La responsabilidad concreta de cada archivo debe verificarse en el código del proyecto.

### 10.1. `EcomercaditoAPI`

Proyecto principal de la API.

Contiene:

- Controladores.
- Configuración de la aplicación.
- Punto de entrada (`Program.cs` o equivalente).
- Archivos de configuración, como `appsettings.json`.

### 10.2. `EcomercaditoAPI.Concrete`

Proyecto que implementa la lógica concreta de acceso a datos y servicios.

Contiene:

- El contexto de base de datos.
- Implementaciones de servicios.
- Implementaciones de repositorios, si corresponde.

### 10.3. `EcomercaditoAPI.Interfaces`

Proyecto que define las interfaces utilizadas por la aplicación.

Contiene:

- Interfaces de servicios.
- Interfaces de repositorios.
- Contratos que deben cumplir las implementaciones.

### 10.4. `EcomercaditoAPI.Models`

Proyecto que define los modelos de dominio.

Contiene:

- Entidades que representan las tablas de la base de datos.
- Clases utilizadas por el contexto de datos.

### 10.5. `EcomercaditoAPI.ViewModels`

Proyecto que define los modelos de vista.

Contiene:

- Clases utilizadas para solicitudes HTTP.
- Clases utilizadas para respuestas de la API.
- Modelos específicos para los controladores.

> Esta organización separa responsabilidades y facilita el mantenimiento y las pruebas.

---

## 11. Comandos útiles

Ejecuta estos comandos desde la carpeta del proyecto.

| Comando            | Descripción                                                     |
| ------------------ | --------------------------------------------------------------- |
| `dotnet --version` | Muestra la versión instalada del SDK de .NET.                   |
| `dotnet build`     | Compila el proyecto y sus dependencias.                         |
| `dotnet run`       | Compila y ejecuta la API.                                       |
| `dotnet watch run` | Ejecuta la API y recompila automáticamente al detectar cambios. |
| `dotnet test`      | Ejecuta las pruebas del proyecto, si existen.                   |
| `dotnet publish`   | Publica la aplicación para su despliegue.                       |

### 11.1. Ejecutar con recarga automática

Para desarrollar con recarga automática:

```bash
dotnet watch run
```

Este comando detecta cambios en el código y recompila la aplicación.

> `dotnet watch` requiere que la herramienta esté disponible en tu entorno.

### 11.2. Publicar la aplicación

Para generar una publicación:

```bash
dotnet publish --configuration Release --output ./publish
```

El contenido de la carpeta `publish` puede desplegarse en un servidor o contenedor.

---

## 12. Solución de problemas frecuentes

### 12.1. `dotnet` no se reconoce

Posibles causas:

- .NET SDK no está instalado.
- La terminal se abrió antes de completar la instalación.
- .NET no está disponible en el `PATH` del sistema.

Qué revisar:

1. Confirma que .NET 10 SDK esté instalado.
2. Cierra y vuelve a abrir la terminal.
3. Ejecuta nuevamente:

```bash
dotnet --version
```

### 12.2. Error al compilar

Qué revisar:

1. Lee el mensaje de error completo.
2. Identifica el archivo y la línea afectados.
3. Verifica que todas las dependencias estén restauradas.
4. Ejecuta:

```bash
dotnet restore
```

5. Vuelve a compilar:

```bash
dotnet build
```

### 12.3. Error de conexión a la base de datos

Qué revisar:

1. La instancia de SQL Server esté en ejecución.
2. El nombre del servidor sea correcto.
3. El nombre de la base de datos coincida con el creado.
4. La autenticación configurada sea válida.
5. Los permisos del usuario sean adecuados.
6. La cadena de conexión en `appsettings.json` sea correcta.

Puedes probar la conexión desde tu cliente SQL antes de ejecutar la API.

### 12.4. La API inicia, pero no puede acceder a datos

Qué revisar:

1. Que el script de base de datos se haya ejecutado correctamente.
2. Que las tablas y columnas existan.
3. Que el contexto de datos esté configurado para usar `DefaultConnection`.
4. Que los modelos coincidan con la estructura de la base de datos.

### 12.5. Error de migraciones (si se utilizan)

Si el proyecto utiliza migraciones de Entity Framework Core:

1. Verifica que las migraciones estén actualizadas.
2. Ejecuta las migraciones pendientes, si corresponde.
3. Revisa que la base de datos refleje el modelo actual.

> Los pasos exactos dependen de cómo esté configurado el acceso a datos en el proyecto.

### 12.6. Swagger no está disponible

Si la dirección `/swagger` no responde:

1. Verifica que Swagger esté habilitado en la configuración.
2. Revisa que no haya errores al iniciar la API.
3. Prueba los endpoints directamente desde el navegador o con una herramienta HTTP.

---

## 13. Relación con el frontend

El frontend de EcoMercadito se encuentra en:

https://github.com/danielamaya503/EcoMercadito-frotend

Para que el frontend funcione correctamente con esta API:

1. La API debe estar ejecutándose.
2. La dirección de la API debe coincidir con la configurada en el frontend.
3. El backend debe permitir las solicitudes desde el origen del frontend (CORS), si corresponde.

> Iniciar la API no inicia automáticamente el frontend. Ambos proyectos deben ejecutarse de forma independiente.

---

## 14. Inicio rápido

Si ya tienes .NET 10 SDK, Git y SQL Server instalados:

```bash
# 1. Clonar el repositorio de la API
git clone <URL_DEL_REPOSITORIO_API>

# 2. Entrar a la carpeta del proyecto
cd <NOMBRE_DEL_REPOSITORIO_API>

# 3. Restaurar dependencias y compilar
dotnet restore
dotnet build

# 4. Configurar appsettings.json con tu cadena de conexión DefaultConnection

# 5. Ejecutar el script de base de datos en tu instancia de SQL Server

# 6. Iniciar la API
dotnet run
```

Después de iniciar la API, abre en el navegador la dirección que muestre la terminal y prueba los endpoints disponibles.

Para las siguientes ejecuciones, abre una terminal dentro de la carpeta del proyecto y ejecuta:

```bash
dotnet run
```

---

## Documentación de referencia

- .NET: https://dotnet.microsoft.com/
- ASP.NET Core: https://learn.microsoft.com/aspnet/core/
- Entity Framework Core: https://learn.microsoft.com/ef/core/
- Git: https://git-scm.com/
- SQL Server: https://learn.microsoft.com/sql/sql-server/
