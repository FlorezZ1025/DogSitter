# 🐾 DogSitter API — Infraestructura como Código con Terraform

Este documento describe el paso a paso para desplegar la infraestructura de la API DogSitter en Azure usando Terraform.

---

## 📋 Prerrequisitos

Asegúrate de tener instalado:

- [Terraform](https://developer.hashicorp.com/terraform/downloads) >= 1.5.0
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [.NET SDK 8](https://dotnet.microsoft.com/download)

---

## 📁 Estructura del proyecto

```
terraform/
├── root_main.tf         # Recursos principales y módulos
├── root_variables.tf    # Declaración de variables
├── root_outputs.tf      # Outputs del despliegue
├── root_locals.tf       # Valores locales y tags comunes
├── terraform.tfvars     # Valores de las variables (NO subir a git)
├── terraform.tfvars.example  # Plantilla de variables
└── modules/
    ├── acr/             # Azure Container Registry
    ├── aks/             # Azure Kubernetes Service
    ├── database/        # Azure SQL Database (MSSQL)
    └── vnet/            # Virtual Network y subnets
```

**Recursos que se crean:**

| Módulo     | Recurso Azure      | Nombre generado                          |
| ---------- | ------------------ | ---------------------------------------- |
| `vnet`     | Virtual Network    | `vnet-dogsitter-dev`                     |
| `vnet`     | Subnet AKS         | `snet-aks-dogsitter-dev`                 |
| `vnet`     | Subnet DB          | `snet-db-dogsitter-dev`                  |
| `acr`      | Container Registry | `acrdogsitterdev`                        |
| `aks`      | Kubernetes Cluster | `aks-dogsitter-dev`                      |
| `database` | SQL Server         | `sql-dogsitter-dev.database.windows.net` |
| `database` | SQL Database       | `dogsitter`                              |

---

## 🔐 Paso 1 — Autenticación con Azure

```powershell
az login
```

Se abrirá el navegador para que inicies sesión. Una vez autenticado, verifica y selecciona tu suscripción:

```powershell
az account list --output table
az account set --subscription "<SUBSCRIPTION_ID>"
```

> No se requiere Service Principal. Terraform usará tu sesión activa de Azure CLI con identidad administrada.

---

## 🔑 Paso 2 — Configurar variables

Copia la plantilla y rellena los valores:

```powershell
Copy-Item terraform\terraform.tfvars.example terraform\terraform.tfvars
```

Edita `terraform.tfvars` con tus valores reales:

```hcl
project     = "dogsitter"
environment = "dev"
location    = "westus"

# VNet
vnet_address_space = ["10.1.0.0/16"]
aks_subnet_cidr    = "10.1.1.0/24"
db_subnet_cidr     = "10.1.2.0/24"

# ACR
acr_sku = "Basic"

# AKS
kubernetes_version = "1.36.0"
aks_node_count     = 2
aks_node_vm_size   = "Standard_D2s_v3"

# Database
db_admin_login    = "dogsitteradmin"
db_admin_password = "TuPasswordSeguro!123"
db_sku_name       = "Basic"
```

> ⚠️ `terraform.tfvars` está en `.gitignore`. Nunca lo subas al repositorio.

---

## 🏗️ Paso 3 — Inicializar Terraform

```powershell
cd terraform
terraform init
```

Debe terminar con:

```
Terraform has been successfully initialized!
```

---

## 🚀 Paso 4 — Desplegar la infraestructura

```powershell
terraform apply
```

Revisa el plan, escribe `yes` cuando te lo pida. Este paso tarda entre **10 y 15 minutos**.

### 4.1 Verificar los outputs

```powershell
terraform output acr_login_server       # URL del ACR
terraform output acr_name               # Nombre del ACR
terraform output aks_cluster_name       # Nombre del cluster AKS
terraform output aks_kube_config_command  # Comando para conectarte al AKS
terraform output db_server_fqdn         # FQDN del servidor SQL
terraform output db_name                # Nombre de la base de datos
```

---

## 🐳 Paso 5 — Construir y subir las imágenes Docker

```powershell
# Login al ACR
az acr login --name acrdogsitterdev

# Stable
docker build `
  --build-arg APP_VERSION=1.0.0 `
  --build-arg APP_RELEASE=stable `
  -t acrdogsitterdev.azurecr.io/dogsitter-api:1.0.0-stable .

docker push acrdogsitterdev.azurecr.io/dogsitter-api:1.0.0-stable

# Canary
docker build `
  --build-arg APP_VERSION=2.0.0 `
  --build-arg APP_RELEASE=canary `
  -t acrdogsitterdev.azurecr.io/dogsitter-api:2.0.0-canary .

docker push acrdogsitterdev.azurecr.io/dogsitter-api:2.0.0-canary
```

---

## ☸️ Paso 6 — Conectarse al cluster AKS

```powershell
az aks get-credentials --resource-group rg-dogsitter-dev --name aks-dogsitter-dev

# Verificar conexión
kubectl get nodes
```

---

## 🔒 Paso 7 — Crear el secret de base de datos en K8s

```powershell
kubectl create secret generic dogsitter-db-secret `
  --from-literal=connection-string="Server=sql-dogsitter-dev.database.windows.net,1433;Initial Catalog=dogsitter;User ID=dogsitteradmin;Password=TuPasswordSeguro!123;Encrypt=True;TrustServerCertificate=False;"
```

---

## 📦 Paso 8 — Desplegar los manifiestos de Kubernetes

```powershell
# Instalar NGINX Ingress Controller (solo la primera vez)
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.10.1/deploy/static/provider/cloud/deploy.yaml

kubectl wait --namespace ingress-nginx `
  --for=condition=ready pod `
  --selector=app.kubernetes.io/component=controller `
  --timeout=120s

# Aplicar manifiestos de la aplicación
kubectl apply -f k8s-manifests/
```

### 8.1 Verificar el despliegue

```powershell
kubectl get pods -l app=dogsitter
kubectl get ingress
kubectl get svc -n ingress-nginx   # Obtener la IP pública (EXTERNAL-IP)
```

---

## 🔄 Paso 9 — Actualizar solo el canary (futuras versiones)

```powershell
# 1. Build y push de la nueva imagen
docker build `
  --build-arg APP_VERSION=2.0.1 `
  --build-arg APP_RELEASE=canary `
  -t acrdogsitterdev.azurecr.io/dogsitter-api:2.0.1-canary .

docker push acrdogsitterdev.azurecr.io/dogsitter-api:2.0.1-canary

# 2. Actualizar el tag en k8s-manifests/deployment-canary.yaml
#    image: acrdogsitterdev.azurecr.io/dogsitter-api:2.0.1-canary
#    ApiVersion__Version: "2.0.1"

# 3. Aplicar
kubectl apply -f k8s-manifests/deployment-canary.yaml

# 4. Verificar
kubectl get pods -l version=canary
```

---

## ⚠️ Solución de problemas comunes

| Error                            | Solución                                                                      |
| -------------------------------- | ----------------------------------------------------------------------------- |
| `az login` no abre el navegador  | Usa `az login --use-device-code`                                              |
| `Error acquiring the state lock` | Elimina el archivo `terraform.tfstate.lock.info`                              |
| `SkuNotAvailable`                | Cambia `aks_node_vm_size` en `terraform.tfvars`                               |
| `LocationIsOfferRestricted`      | Cambia `location` en `terraform.tfvars`                                       |
| `Unauthorized` en docker push    | Ejecuta `az acr login --name acrdogsitterdev` nuevamente                      |
| Pods en `CrashLoopBackOff`       | Verifica el secret de BD con `kubectl get secret dogsitter-db-secret -o yaml` |

---

## 🗑️ Eliminar toda la infraestructura

```powershell
cd terraform
terraform destroy
```

Escribe `yes` cuando te lo pida.

---

## 📝 .gitignore recomendado

```
terraform.tfvars
*.tfstate
*.tfstate.backup
**/.terraform/
.terraform.lock.hcl
```
