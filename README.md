# Bonsai

A family wiki and photoalbum engine (in Russian).

## Features

* Pages with Markdown text
* Media files: photos, videos (PDF documents will be supported later)
* Person tags on photos
* Relations: validation, inferrence
* Fact storage (birthday, gender, blood type, languages, hobbies, etc.)
* Access control: editor, reader and guest roles
* Changesets: browse changes to any page/media, see diffs, easily revert if necessary

## Screenshots

#### Front-end:

<a href="https://user-images.githubusercontent.com/604496/46574247-037d4f00-c9a9-11e8-8585-0d574dda2600.png"><img src="https://user-images.githubusercontent.com/604496/46574252-1859e280-c9a9-11e8-821f-daeaaac7de3f.png" /></a>
<a href="https://user-images.githubusercontent.com/604496/46574259-2c054900-c9a9-11e8-8ecc-ca542053f665.png"><img src="https://user-images.githubusercontent.com/604496/46574288-9a4a0b80-c9a9-11e8-8373-2a7d3e00289c.png" /></a>
<a href="https://user-images.githubusercontent.com/604496/46574262-31629380-c9a9-11e8-9ea6-18fbe63f239f.png"><img src="https://user-images.githubusercontent.com/604496/46574291-9f0ebf80-c9a9-11e8-8656-8a54dd2f2be7.png" /></a>

#### Admin panel:

<a href="https://user-images.githubusercontent.com/604496/46574266-3f181900-c9a9-11e8-828d-9d9a5db25acb.png"><img src="https://user-images.githubusercontent.com/604496/46574292-a209b000-c9a9-11e8-8193-cd99fc1f5f91.png" /></a>
<a href="https://user-images.githubusercontent.com/604496/46574268-43443680-c9a9-11e8-974f-f8a60fbeaa74.png"><img src="https://user-images.githubusercontent.com/604496/46574297-a504a080-c9a9-11e8-8612-d3e5cd1592a4.png" /></a>

## Installation

For development, you will need the following:

* [.NET Core 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1): the main runtime for Bonsai
* [NodeJS 10+](https://nodejs.org/en/): required for building scripts and styles
* [Java 8+](https://java.com/en/download/windows-64bit.jsp): required for ElasticSearch

### Windows
1. Install [PostgreSQL server](https://www.openscg.com/bigsql/postgresql/installers.jsp/) (9.6+)
2. Install [ElasticSearch 5.6.x](https://www.elastic.co/downloads/past-releases) (6.0 is not supported yet)
3. Install [Russian Morphology](https://github.com/imotov/elasticsearch-analysis-morphology) for ElasticSearch.
   
   If you're getting a "Syntax of the command is incorrect" error during this step, make sure you have a `JAVA_HOME` environment variable defined.
4. Download [ffmpeg shared binaries](https://ffmpeg.zeranoe.com/builds/) for your system and extract the archive's contents into `External/ffmpeg` folder in the solution root (must contain both `ffmpeg` and `ffprobe` executables).
5. Create a [Facebook Authorization App](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-2.1&tabs=aspnetcore2x)
6. Create a [Google Authorization App](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins) or comment out the `UseGoogle` block in `Startup.cs`
7. Create a file called `appsettings.Development.json` and save the auth secrets/connection strings/etc in it:

    ```
    {
      "ConnectionStrings": {
        "Database": "Server=127.0.0.1;Port=5432;Database=bonsai;User Id=<login>;Password=<password>;Persist Security Info=true"
      },
      "Auth": {
        "Facebook": {
          "AppId": "...",
          "AppSecret": "..." 
        },
        "Google": {
          "ClientId": "...",
          "ClientSecret": "..." 
        } 
      } 
    }
    ```
    
8. Create the database:

    ```
    dotnet ef database update
    ```
9. Build the styles and scripts:

    ```
    npm install
    npm run build
    ```
10. Run the app (as Visual Studio project or using `dotnet run`).

## Linux + Docker
1. Modify your `vm.max_map_count` to at least 262,144 for ElasticSearch to start:

    ```
    sysctl -w vm.max_map_count=262144
    ```

2. Create a [Facebook Authorization App](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-2.1&tabs=aspnetcore2x)
3. Create a [Google Authorization App](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins)
4. Download the [docker-compose](docker-compose.yml) file and fill in the credentials in environment variable section where applicable:

   ```
   environment:
      - Auth__Facebook__AppId=
      - Auth__Facebook__AppSecret=
      - Auth__Google__ClientId=
      - Auth__Google__ClientSecret=
      - WebServer__RequireHttps=false
      - ASPNETCORE_ENVIRONMENT=Production
   ```
   
5. Bring everything up using `docker compose`:
   ```
   docker-compose up -d
   ```
6. After everything is brought up Bonsai will listen on port 80. It may be a good idea to hide it behind some kind of reverse proxy.