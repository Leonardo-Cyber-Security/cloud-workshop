# Dockerfile

Questo Dockerfile è suddiviso in due fasi principali: la fase di build e la fase di runtime. Di seguito viene fornita una spiegazione dettagliata di ciascuna fase.

Questo Dockerfile crea un'immagine Docker che compila e pubblica un'applicazione .NET nella fase di build e poi la esegue nella fase di runtime.

## Fase 1: Build

1. **Definizione dell'immagine di base per la build**:
    ```dockerfile
    FROM mcr.microsoft.com/dotnet/sdk:8.0.403 AS build
    ```
    Utilizza l'immagine ufficiale del .NET SDK versione 8.0.403 come base per la fase di build.

2. **Impostazione della directory di lavoro**:
    ```dockerfile
    WORKDIR /src
    ```
    Imposta la directory di lavoro a `/src`.

3. **Copia dei file di progetto**:
    ```dockerfile
    COPY . .
    ```
    Copia tutti i file dal contesto di build locale alla directory di lavoro nel container.

4. **Cambio della directory di lavoro**:
    ```dockerfile
    WORKDIR /src/DotnetMSWorkshop
    ```
    Cambia la directory di lavoro a `/src/DotnetMSWorkshop`.

5. **Ripristino dei pacchetti NuGet**:
    ```dockerfile
    RUN dotnet restore --verbosity detailed
    ```
    Esegue il comando `dotnet restore` per ripristinare i pacchetti NuGet necessari, con un livello di dettaglio elevato.

6. **Pubblicazione dell'applicazione**:
    ```dockerfile
    RUN dotnet publish -c Release -o /app/publish --no-restore
    ```
    Esegue il comando `dotnet publish` per compilare l'applicazione in modalità Release e pubblicarla nella directory `/app/publish`, senza ripristinare nuovamente i pacchetti.

## Fase 2: Runtime

1. **Definizione dell'immagine di base per il runtime**:
    ```dockerfile
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
    ```
    Utilizza l'immagine ufficiale del .NET ASP.NET Core runtime versione 8.0 come base per la fase di runtime.

2. **Impostazione della directory di lavoro**:
    ```dockerfile
    WORKDIR /app
    ```
    Imposta la directory di lavoro a `/app`.

3. **Copia dei file pubblicati dalla fase di build**:
    ```dockerfile
    COPY --from=build ./app/publish .
    ```
    Copia i file pubblicati dalla fase di build nella directory di lavoro del container.

4. **Esecuzione del comando `find`**:
    ```dockerfile
    RUN find /app/
    ```
    Esegue il comando `find` per elencare i file nella directory `/app/`.

5. **Esposizione delle porte**:
    ```dockerfile
    EXPOSE 8080
    EXPOSE 443
    ```
    Espone le porte 8080 e 443 per consentire l'accesso all'applicazione.

6. **Definizione del punto di ingresso**:
    ```dockerfile
    ENTRYPOINT ["dotnet", "DotnetMSWorkshopAPI.dll"]
    ```
    Imposta il comando di avvio del container per eseguire l'applicazione .NET.

## Comandi Docker per Creare e Lanciare l'Immagine

### Creazione dell'Immagine Docker

Per creare l'immagine Docker con un tag specifico, utilizza il seguente comando:

```sh
docker build -t nome-immagine:tag .
```

Ad esempio, se vuoi creare un'immagine con il nome `dotnet-app` e il tag `v1.0`, esegui:

```sh
docker build -t dotnet-app:v1.0 .
```

### Lancio dell'Immagine Docker
Per lanciare l'immagine Docker e fare il mapping della porta 8080 del container alla porta 8080 della macchina host, utilizza il seguente comando:

```sh
docker run -p 8080:8080 nome-immagine:tag
```

Ad esempio, se hai creato l'immagine con il nome `dotnet-app` e il tag `v1.0`, esegui:

```sh
docker run -p 8080:8080 dotnet-app:v1.0
```

Questo comando avvierà il container e mapperà la porta 8080 del container alla porta 8080 del tuo host, rendendo l'applicazione accessibile tramite http://localhost:8080. 