# Accesso al Pod PostgreSQL e Esecuzione di Query

Puoi verificare che lo script di inizializzazione sia stato lanciato effettivamente tramite la seguente procedura.
Per accedere al pod PostgreSQL e lanciare query, segui questi passaggi:

1. **Trova il nome del pod PostgreSQL:**
    ```sh
    kubectl get pods -n <namespace>
    ```

2. **Accedi al pod PostgreSQL:**
    ```sh
    kubectl exec -it <nome-pod> -n <namespace> -- /bin/bash
    ```

3. **Accedi a PostgreSQL:**
    ```sh
    psql -U <nome-utente> -d <nome-database> -n <namespace>
    ```

4. **Esegui le tue query:**
    ```sql
    SELECT * FROM <nome-tabella>;
    ```

5. **Verifica tutte le tabelle:**
    ```sql
    \dt
    ```

6. **Esci da PostgreSQL:**
    ```sh
    \q
    ```

7. **Esci dal pod:**
    ```sh
    exit
    ```

Sostituisci `<nome-pod>`, `<nome-utente>`, `<nome-database>`, `<nome-tabella>` e `<namespace>` con i valori appropriati per il tuo ambiente.

# StatefulSet vs Deployment

## Differenze principali

### Deployment
- **Scopo**: Utilizzato per applicazioni stateless, dove ogni pod è intercambiabile.
- **Replica**: I pod sono creati e distrutti senza mantenere l'ordine o l'identità.
- **Aggiornamenti**: Aggiornamenti rolling, dove i pod vengono aggiornati uno alla volta.
- **Persistenza**: Non garantisce la persistenza dei dati tra i pod.

### StatefulSet
- **Scopo**: Utilizzato per applicazioni stateful, dove l'identità e l'ordine dei pod sono importanti.
- **Replica**: I pod sono creati e distrutti in un ordine specifico e mantengono un'identità stabile (nome e storage).
- **Aggiornamenti**: Aggiornamenti rolling con garanzia di ordine e stabilità dei pod.
- **Persistenza**: Garantisce la persistenza dei dati associando volumi persistenti a ciascun pod.

## Perché usare uno StatefulSet per un database?

1. **Identità Stabile**: Ogni pod in uno StatefulSet ha un nome stabile e un'identità che non cambia, il che è cruciale per i database che richiedono una connessione stabile e identificabile.
2. **Persistenza dei Dati**: I volumi persistenti associati ai pod garantiscono che i dati non vengano persi quando i pod vengono riavviati o ricreati.
3. **Ordine di Creazione e Distruzione**: I pod vengono creati e distrutti in un ordine specifico, il che è importante per i database che richiedono un'inizializzazione sequenziale o una chiusura ordinata.
4. **Aggiornamenti Sicuri**: Gli aggiornamenti rolling garantiscono che i pod vengano aggiornati uno alla volta, mantenendo la disponibilità del servizio e la consistenza dei dati.

