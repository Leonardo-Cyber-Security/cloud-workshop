# **Helm Exercises: Step-by-Step Guide**

This `README.md` provides a comprehensive guide to practicing Helm based on the concepts we've discussed, with step-by-step exercises and detailed explanations.

---

## **Prerequisites**
1. **Install Helm**:  
   Follow [Helm Installation Guide](https://helm.sh/docs/intro/install/) to install Helm on your system.
2. **Access to a Kubernetes Cluster**:  
   Ensure you have `kubectl` installed and configured to access your cluster.
3. **Namespace Permissions**:  
   Verify you have permissions to create namespaces, ConfigMaps, and other Kubernetes resources.
4. **(Optional) A Container Registry**:  
   Set up access to a container registry for packaging and pushing Helm charts in OCI format.

---

## **Exercise 1: Installing Your First Helm Chart**

### **Objective**  
Install a sample Helm chart to understand the basics of `helm install`.

### **Steps**
1. **Add a Helm Repository**:  
   ```bash
   helm repo add bitnami  
   helm repo update
   ```
   > This adds the Bitnami repository, which hosts a variety of pre-built charts.

2. **Search for Charts**:  
   ```bash
   helm search repo bitnami
   ```
   > Explore available charts.

3. **Install a Chart**:  
   Install WordPress using the Bitnami chart:  
   ```bash
   helm install my-wordpress bitnami/wordpress --namespace demo --create-namespace
   ```
   - `my-wordpress`: Name of the release.  
   - `--namespace demo`: Deploys into the `demo` namespace.  
   - `--create-namespace`: Creates the namespace if it doesn’t exist.

4. **Verify Installation**:  
   ```bash
   helm list -n demo
   kubectl get all -n demo
   ```
5. **Check release status**:  
   ```bash
   helm status my-wordpress -n demo
   ```
   > Check the release status and deployed resources.

6. **View Rendered Templates**:  
   ```bash
   helm get manifest my-wordpress -n demo
   ```
   > View the rendered Kubernetes resources.

---

## **Exercise 2: Customizing Deployments with `values.yaml`**

### **Objective**  
Modify chart configurations using a custom `values.yaml` file.

### **Steps**
1. **Download Default `values.yaml`**:  
   ```bash
   helm show values bitnami/wordpress > custom-values.yaml
   ```

2. **Edit `custom-values.yaml`**:  
   Update parameters such as `wordpressUsername`, `wordpressPassword`, and `service.type`.

   Example changes:
   ```yaml
   wordpressUsername: admin
   wordpressPassword: password123
   service:
     type: NodePort
   ```

3. **Install with Custom Values**:  
   ```bash
   helm install my-custom-wordpress bitnami/wordpress -f custom-values.yaml --namespace demo
   ```

4. **Verify Changes**:  
   ```bash
   kubectl get svc -n demo
   ```
   Check for the `NodePort` service type.

5. **View Release Configuration**:  
   ```bash
   helm get values my-custom-wordpress -n demo
   ```
   > View the applied configuration.

---

## **Exercise 3: Creating and Packaging Your Own Helm Chart**

### **Objective**  
Create a custom Helm chart and package it in OCI format.

### **Steps**
1. **Create a New Chart**:  
   ```bash
   helm create my-chart
   cd my-chart
   ```

2. **Edit Chart Files**:  
   Modify `values.yaml`, `templates/deployment.yaml`, and `Chart.yaml` to define your application’s configuration.

3. **Package the Chart**:  
   ```bash
   helm package .
   ```

4. **Push to OCI Registry**:  
   If you have an OCI registry configured, push the chart to the registry:
   ```bash
   helm registry login <registry_url>
   helm push my-chart-0.1.0.tgz oci://<registry_url>/my-charts
   ```

5. **Install from OCI Registry**:
   If you have access to the OCI registry, install the chart directly from the registry:
   ```bash
   helm install my-oci-chart oci://<registry_url>/my-charts/my-chart --version 0.1.0
   ```

---

## **Exercise 4: Managing Dependencies with Subcharts**

### **Objective**  
Add a dependency to your chart and manage it effectively.

### **Steps**
1. **Define a Dependency**:  
   Add the dependency to `Chart.yaml`:  
   ```yaml
   dependencies:
     - name: wordpress
       version: 24.1.2
       repository: https://charts.bitnami.com/bitnami
   ```

2. **Update Dependencies**:  
   ```bash
   helm dependency update
   ```

3. **Install Chart with Dependencies**:  
   ```bash
   helm install my-app ./my-chart
   ```

4. **Verify Redis Deployment**:  
   ```bash
   kubectl get pods
   ```

---

## **Exercise 5: Using Helm Libraries**

### **Objective**  
Leverage a Helm library to reuse common templates.

### **Steps**
1. **Create a Library Chart**:  
   ```bash
   helm create my-library
   ```

2. **Add a Helper Template**:  
   Edit `templates/_helpers.tpl` in the library chart:  
   ```yaml
   {{- define "common.labels" -}}
   app: {{ .Chart.Name }}
   release: {{ .Release.Name }}
   {{- end -}}
   ```

3. **Use the Library in Another Chart**:  
   Add the library as a dependency:  
   ```yaml
   dependencies:
     - name: my-library
       version: 0.1.0
       repository: "file://../my-library"
   ```

4. **Reference the Template**:  
   In `templates/deployment.yaml`:  
   ```yaml
   metadata:
     labels:
       {{ include "common.labels" . | nindent 6 }}
   ```

---

## **Exercise 6: Debugging Helm Charts**

### **Objective**  
Practice troubleshooting Helm deployments.

### **Steps**
1. **Simulate an Error**:  
   Edit a chart to introduce an invalid syntax, such as a missing `:` in `templates/deployment.yaml`.

2. **Run `helm lint`**:  
   ```bash
   helm lint ./my-chart
   ```
   > Linter detects errors in your chart.

3. **Use `--dry-run` to Debug**:  
   ```bash
   helm install my-debug-chart ./my-chart --dry-run --debug
   ```

4. **Check Logs and Events**:  
   ```bash
   kubectl describe pods -n <namespace>
   kubectl logs <pod_name>
   ```

---

## **Exercise 7: Upgrades and Rollbacks**

### **Objective**  
Perform and manage upgrades and rollbacks of releases.

### **Steps**
1. **Upgrade a Release**:  
   Edit `values.yaml` to change the replica count or other parameter:
   ```yaml
   replicaCount: 3
   ```
   Apply the upgrade:  
   ```bash
   helm upgrade my-release ./my-chart
   ```

2. **Rollback to Previous Version**:  
   ```bash
   helm rollback my-release 1
   ```

---

## **Exercise 8: Automating with CI/CD**

### **Objective**  
Integrate Helm with a CI/CD pipeline for automated deployments.

### **Steps**
1. **Script the Deployment**:  
   Create a script to automate installation:
   ```bash
   # deploy.sh
   helm upgrade --install my-app ./my-chart -f values-prod.yaml --namespace prod
   ```

2. **Integrate with CI/CD Tool**:  
   Add the script to your CI/CD pipeline (e.g., GitHub Actions, Jenkins).  
   Example GitHub Action step:
   ```yaml
   - name: Deploy Helm Chart
     run: ./deploy.sh
   ```

---

These exercises cover a range of Helm functionalities, from basics to advanced features, ensuring you gain hands-on experience with real-world scenarios. Would you like to dive deeper into any specific topic?