# Deploy su Minikube

Questa guida ti spiegherà come deployare una Secret, un ConfigMap, un Deployment, un Service su un cluster Minikube utilizzando i file YAML forniti.

## Passi per il deployment

### 0. Creare il Namespace

Il Namespace nel file YAML contiene l'ambiente in cui verranno creati gli oggetti.

Applica il Namespace al cluster con il seguente comando:

```bash
kubectl apply -f manifests/namespace.yaml
```

### 1. Creare la Secret

La Secret nel file YAML contiene dati sensibili (puoi aggiungere i tuoi dati codificati in base64).

Applica la Secret al cluster con il seguente comando:

```bash
kubectl apply -f manifests/secret.yaml
```

### 2. Creare il ConfigMap

Il ConfigMap nel file YAML contiene le variabili d'ambiente e/o le configurazioni necessarie affinchè l'applicazione possa partire.
Il ConfigMap viene utilizzato per memorizzare le informazioni di configurazione non sensibili.

Applica il ConfigMap al cluster con il seguente comando:

```bash
kubectl apply -f manifests/configmap.yaml
```

### 3. Creare il Deployment

Il Deployment nel file YAML contiene le specifiche per il pod e il container.

Il Deployment creerà tre repliche del container `seminario-container`, che esegue l'immagine `seminario-dotnet-ms:latest`.

Applica il Deployment al cluster con il seguente comando:

```bash
kubectl apply -f manifests/deployment.yaml
```

### 4. Creare il Service

Il Service nel file YAML espone il Deployment al mondo esterno.

Il Service esporrà il Deployment per permettere la comunicazione interna nel cluster sui porti 8080 e 8443.

Applica il Service al cluster con il seguente comando:

```bash
kubectl apply -f manifests/service.yaml
```

### 5. Creare l'Ingress (opzionale fuori da Minikube)

L'Ingress nel file YAML espone il Service al mondo esterno.

L'Ingress esporrà il Service per permettere la comunicazione esterna al cluster.

Applica l'Ingress al cluster con il seguente comando:

```bash
kubectl apply -f manifests/ingress.yaml
```

### 6. Verificare il deployment

Puoi controllare se gli oggetti sono stati creati correttamente con i seguenti comandi:

```bash
kubectl get secrets
kubectl get configmaps
kubectl get deployments
kubectl get pods
kubectl get services
kubectl get ingress
```

### 7. Verificare l'applicazione

Se stai utilizzando Minikube, puoi ottenere l'URL per accedere al servizio con il seguente comando:

```bash
minikube service seminario-service
```

### 8. Eseguire rollouts e rollbacks

Puoi eseguire un rollout per aggiornare l'applicazione con il seguente comando:

```bash
kubectl set image deployment/seminario-deployment seminario-container=seminario-dotnet-ms:latest
```

Puoi eseguire un rollback per tornare alla versione precedente dell'applicazione con il seguente comando:

```bash
kubectl rollout undo deployment/seminario-deployment
```

### 9. Riavviare il Deployment

Se hai bisogno di riavviare il Deployment per applicare modifiche alla configurazione o per altri motivi, puoi utilizzare il seguente comando:

```bash
kubectl rollout restart deployment/seminario-deployment
```

Questo comando riavvierà tutti i pod del Deployment senza downtime significativo.

### 10. Debug del Pod (opzionale)

Se hai bisogno di debuggare il pod, puoi accedere al pod con il seguente comando:

```bash
kubectl get pods -n <namespace>
kubectl exec -it <nome-pod> -- /bin/bash
```
Sostituisci `<namespace>` con il nome del namespace in cui si trova il pod.

Sostituisci `<nome-pod>` con il nome del pod che vuoi debuggare.