### Costruzione e Avvio dell'Immagine

1. Per costruire l'immagine:

    ```bash
    docker build -t my-app .
    ```

2. Per avviare il container:

    ```bash
        docker run -d -p 5000:5000 my-app
    ```
Questo ti permetterà di avviare un'app Flask in esecuzione su Docker.