# **Docker & Kubernetes Hands-on Workshop**
 
## **Obiettivi**
Durante questo workshop imparerai:
- Come creare e gestire container Docker
- Come creare immagini Docker personalizzate
- Come lavorare con Kubernetes utilizzando Minikube
- Come deployare, scalare e gestire applicazioni in un cluster Kubernetes
 
---
 
## **Prerequisiti**
Assicurati di avere accesso a Google Cloud Shell, che ha già installati Docker e Minikube. Verificheremo l’ambiente nella sezione seguente.
 
---
 
## **Indice del Workshop**
1. [Setup e Verifica dell'Ambiente](#setup)
2. [Creazione e Gestione dei Container Docker](#docker-basics)
3. [Introduzione a Kubernetes con Minikube](#kubernetes-minikube)
4. [Deployment e Gestione di Applicazioni su Kubernetes](#kubernetes-deployment)
5. [Scaling e Aggiornamento di un Deployment](#kubernetes-scaling)
 
---
<a name="setup"></a>

## **1. Setup e Verifica dell'Ambiente**
Inizializza una cloud shell con minikube e kubectl.

Puoi utilizzare [Google Cloud Shell](https://ide.cloud.google.com) per avere una shell con minikube e kubectl già installati.

Potrebbe volerci qualche minuto per inizializzare la shell.
1. **Verifica della versione di Docker:**
   ```bash
   docker --version
   ```
 
2. **Avviare Minikube:**
   ```bash
   minikube start
   ```
 
3. **Verifica della versione di `kubectl`:**
   ```bash
   kubectl version --client
   ```

---

<a name="docker-basics"></a>
## **2. Creazione e Gestione dei Container Docker**
 
### **Esercizio 1: Avviare un container "Hello World"**
1. Avvia un container di esempio per assicurarti che Docker funzioni:
   ```bash
   docker run hello-world
   ```
 
### **Esercizio 2: Creare un container con un web server**
1. Avvia un container Nginx:
   ```bash
   docker run --name my-nginx -d -p 8080:80 nginx
   ```
2. Visualizza i container in esecuzione:
   ```bash
   docker ps
   ```

3. Prova a pingare il web server:
   ```bash
   curl http://localhost:8080
   ```

4. Clicca su "Web Preview" in alto a destra e seleziona "Preview on port 8080" per visualizzare il web server in un browser.
 
### **Esercizio 3: Gestione dei container**
1. Visualizza i container in esecuzione:
  ```bash
  docker ps
  ```
2. Ferma il container creato precedentemente:
  ```bash
  docker stop my-nginx
  ```
3. Rimuovi un container:
  ```bash
  docker rm my-nginx
  ``` 
  Nota: Non rimuovere il container di minikube altrimenti dovrai riavviarlo.

---
 
<a name="kubernetes-minikube"></a>
## **3. Introduzione a Kubernetes con Minikube**
 
### **Esercizio 4: Avvia Minikube**
1. Assicurati che Minikube sia avviato:
   ```bash
   minikube start
   ```
 
2. Verifica che Minikube funzioni correttamente:
   ```bash
   kubectl get nodes
   ```
 
---
 
<a name="kubernetes-deployment"></a>
## **4. Deployment e Gestione di Applicazioni su Kubernetes**
 
### **Esercizio 5: Creare un Deployment**
1. Crea un deployment con un container di esempio:
   ```bash
   kubectl create deployment hello-node --image=kicbase/echo-server:1.0
   ```
 
2. Verifica lo stato del deployment:
   ```bash
   kubectl get deployments
   ```

3. Verifica lo stato dei pod:
   ```bash
    kubectl get pods
    ```

4. Descrivi il deployment:
   ```bash
   kubectl describe deployment hello-node
   ```
 
### **Esercizio 6: Esposizione del Deployment tramite un Service**
1. Esponi il deployment:
   ```bash
   kubectl expose deployment hello-node --type=ClusterIP --port=8080
   ```
 
2. Verifica lo stato del service:
   ```bash
   kubectl get services
   ```

3. Ottieni l'URL del servizio:
   ```bash
     minikube service hello-node --url
    ```
    Questo comando crea un tunnel tra la shell e miniKube, permettendoti di accedere al servizio tramite l'URL fornito.

4. Prova ad aprire l'URL nel browser.

---
 
<a name="kubernetes-scaling"></a>
## **5. Scaling e Aggiornamento di un Deployment**
 
### **Esercizio 7: Scalare un Deployment**
1. Effettua un rolling update aumentando il numero di repliche del deployment:
   ```bash
   kubectl scale deployment hello-node --replicas=3
   ```
2. Verifica lo stato dei pod:
   ```bash
   kubectl get pods
   ```

---
 
### **Esercizio 8: Aggiornare un Deployment**
1. Aggiorna il deployment con una nuova immagine:
   ```bash
   kubectl set image deployment/hello-node echo-server=nginx:1.7.9
   ```
 
2. Verifica lo stato del rollout:
   ```bash
   kubectl rollout status deployment/hello-node
   ```
3. Qualcosa non va... Verifica lo stato dei pod:
   ```bash
   kubectl get pods
   ```
4. Annnulla l'aggiornamento:
   ```bash
   kubectl rollout undo deployment/hello-node
   ```

---