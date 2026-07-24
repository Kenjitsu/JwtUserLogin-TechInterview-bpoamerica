# Login user tech - Evaluación Técnica - BPO de las américas

## Resumen de lo desarrollado

Este proyecto es la implementación de un sistema sencillo de inicio de sesión aplicando
el patrón arquitectónico **BFF (Backend for Frontend)**.

La solución está compuesta por:

-   **Web API RESTful (Backend):** Encargada de la lógica de negocio,
    validación de credenciales y generación de json web token (JWT).
-   **Razor Pages (Frontend):** Aplicación web que actúa como cliente
    seguro. Consume la API, obtiene el JWT y lo guarda en una **Cookie
    cifrada e HttpOnly**. Esto garantiza que el navegador gestione la
    sesión de forma segura y mitiga vulnerabilidades como ataques XSS.

Todo el diseño de interfaz se desarrolló de manera responsiva y moderna
utilizando **Bootstrap 5**.

## Tecnologías Utilizadas

-   **Backend:** ASP.NET Core Web API (C#), EF Core, SQL Lite, JWT, Cookies
-   **Frontend:** ASP.NET Core Razor Pages (C#), Bootstrap 5, HTML

## Paso a paso para la ejecución

### 1. Inicializar secretos y generar JWT

Para garantizar la seguridad de la aplicación, las claves sensibles no
se exponen en el código fuente. Se utiliza el administrador de secretos
de .NET.

Abrir una terminal en el directorio de tu proyecto API y ejecutar el
siguiente comando para inicializar el almacén de secretos:

``` bash
dotnet user-secrets init
```

Despues, se debe generar un "token key" de 64 bytes para firmar los JWT.
Se puede generar este token abriendo PowerShell y ejecutando el comando:

``` powershell
[Convert]::ToBase64String((1..64 | ForEach-Object { Get-Random -Maximum 256 }))
```

Ahora con la cadena generada, copiarla y anexarla al siguiente comando:

``` bash
dotnet user-secrets set "Jwt:Key" "...aqui va el token generado con el comando de powershell..."
```

### 2. Habilitar Swagger en la API (Opcional)

Si se requiere visualizar y probar los endpoints de la API directamente
desde el navegador, se puede habilitar la interfaz de Swagger.

Dirigirse al archivo `Properties/launchSettings.json` dentro del proyecto
de la API. Localizar el perfil de ejecución que se utilizando y
modificar el parámetro `launcBrowser` y dejarlo en true, como se ve acontinuación:

``` json
{
  "profiles": {
    "TuProyectoApi": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### 3. Ejecutar ambos proyectos simultáneamente

Para que el flujo de inicio de sesión funcione, el Backend (API) y el
Frontend (Razor Pages) deben ejecutarse al mismo tiempo.

#### Usando Visual Studio

1.  Hacer clic derecho sobre la **Solución** en el Explorador de
    soluciones y seleccionar **Configurar proyectos de inicio...**.
2.  Marcar la opción **Proyectos de inicio múltiples**.
3.  Cambiar la acción de los proyectos **API** y **Frontend** a
    **Iniciar**.
4.  Presionar **F5** o hacer clic en **Iniciar**.

#### Usando la CLI de .NET

Abrir dos ventanas de terminal independientes. En la primera, navegar al
directorio de la API; en la segunda, navegar al directorio del Frontend.
En ambas ejecuta:

``` bash
dotnet run
```

> **Nota:** Asegurar de que el puerto en el que se levanta la API
> coincida con la URL base configurada en el cliente HTTP dentro del
> archivo `Program.cs` del proyecto Frontend.

### 4. Comprobar el funcionamiento de la api via POSTMAN

Se incluye dentro del comprimido un archivo de colección de Postman llamado `UserLogin-bpoamericas.postman_collection.json`. Este archivo contiene el endpoint de la API para probar el inicio de sesión. En este endpoint se encuentra un pre-request script, que es el que se encarga de codificar el usuario y contraseña que se envian en la petición, si es necesario cambiar el usuario y contraseña, se debe modificar el pre-request script con los datos a probar.

### 5. Usuarios de prueba

La aplicación cuenta con un conjunto de usuarios de prueba que se insertan una vez que la API se ejecuta por primera vez. Esta lista de usuarios se encuentra en el archivo `Data/seedData.json`. Sin embargo, los usuarios de prueba son:

``` json
  {
    "userName": "user@test.com",
    "password": "Pa$$w0rd",
    "userProfile": "lowlevel"
  }

  {
    "userName": "user2@test.com",
    "password": "Pa$$w0rd123",
    "userProfile": "midlevel"
  }


  {
    "userName": "user3@test.com",
    "password": "Pa$$w0rd456",
    "userProfile": "highlevel"
  }
```
------------------------------------------------------------------------

 2026 - Login user tech - **Autor:** Ricardo Corredor
