# 🐾 DogSitter API — Infraestructura como Código con Terraform

Este README describe el paso a paso para desplegar la infraestructura de la API DogSitter en Azure usando Terraform.

---

## 📋 Prerrequisitos

Asegúrate de tener instalado:

- [Terraform](https://developer.hashicorp.com/terraform/downloads) >= 1.5.0
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET SDK](https://dotnet.microsoft.com/download) (para las migraciones)
- [Git Bash](https://git-scm.com/downloads) (en Windows)

---

## 📁 Estructura del proyecto

```
terraform/
├── main.tf
├── variables.tf
├── outputs.tf
├── provider.tf
├── environments/
│   ├── prb.tfvars       # Variables no sensibles de PRB
│   ├── prd.tfvars       # Variables no sensibles de PRD
│   ├── prb.env          # Variables sensibles de PRB (NO subir a git)
│   └── prd.env          # Variables sensibles de PRD (NO subir a git)
└── modules/
    ├── AzureVNet/
    ├── AzureSQLDatabase/
    ├── AzureContainerApps/
    ├── AzureContainerRegistry/
    └── AzureJumpHost/
```

---

## 🔐 Paso 1 — Autenticación con Azure

### 1.1 Inicia sesión

```bash
az login --tenant <TENANT_ID>
```

### 1.2 Verifica y selecciona tu suscripción

```bash
az account list --output table
az account set --subscription "<SUBSCRIPTION_ID>"
```

### 1.3 Registra los providers necesarios

```bash
az provider register --namespace Microsoft.App
az provider register --namespace Microsoft.OperationalInsights

# Verifica que quedaron registrados
az provider show --namespace Microsoft.App --query registrationState
az provider show --namespace Microsoft.OperationalInsights --query registrationState
```

### 1.4 Crea el Service Principal

```bash
az ad sp create-for-rbac --name "terraform-sp" --role="Contributor" \
  --scopes="/subscriptions/$(az account show --query id -o tsv)"
```

Guarda el JSON que te devuelve:
```json
{
  "appId":       "...",
  "displayName": "terraform-sp",
  "password":    "...",
  "tenant":      "..."
}
```

---

## 🔑 Paso 2 — Configurar variables de entorno

### 2.1 Crea tu llave SSH (requerida para el Jump Host)

```bash
ssh-keygen -t rsa -b 4096 -f ~/.ssh/id_rsa_azure
```

### 2.2 Obtén tu IP pública

```bash
curl https://ifconfig.me
```

### 2.3 Crea el archivo `environments/prb.env`

> ⚠️ Este archivo nunca debe subirse a git.

```bash
# ── Conexión a Azure ──────────────────────────────
export ARM_CLIENT_ID="<appId del JSON>"
export ARM_CLIENT_SECRET="<password del JSON>"
export ARM_TENANT_ID="<tenant del JSON>"
export ARM_SUBSCRIPTION_ID="<id de az account show>"

# ── Variables sensibles de tu infra ──────────────
export TF_VAR_pg_admin_user="dogsitteradmin"
export TF_VAR_pg_admin_password='<TuPasswordSeguro!123>'
```

> 💡 Si tu password contiene `$` o `!`, usa comillas simples `'` en lugar de dobles `"`.

### 2.4 Carga las variables en tu sesión

```bash
source environments/prb.env
export MSYS_NO_PATHCONV=1   # Necesario en Git Bash para Windows
```

---

## 🏗️ Paso 3 — Inicializar Terraform

```bash
cd terraform
terraform init
```

Debe terminar con:
```
Terraform has been successfully initialized!
```

---

## 🐳 Paso 4 — Desplegar el ACR primero

El ACR debe crearse antes del resto de la infraestructura para poder subir la imagen del contenedor.

```bash
terraform apply -var-file="environments/prb.tfvars" -target=module.acr -lock=false
```

Escribe `yes` cuando te lo pida.

### 4.1 Obtén el login server del ACR

```bash
terraform output acr_login_server
# → acrdogsitterapiprb.azurecr.io
```

---

## 🖼️ Paso 5 — Construir y subir la imagen Docker

Desde la carpeta raíz de tu proyecto .NET (donde está el Dockerfile):

```bash
# Login al ACR
az acr login --name acrdogsitterapiprb

# Construir la imagen
docker build -t acrdogsitterapiprb.azurecr.io/dogsitter-api:latest .

# Subir la imagen
docker push acrdogsitterapiprb.azurecr.io/dogsitter-api:latest

# Verificar que quedó en el ACR
az acr repository list --name acrdogsitterapiprb --output table
```

---

## 🚀 Paso 6 — Desplegar el resto de la infraestructura

```bash
terraform apply -var-file="environments/prb.tfvars" -lock=false
```

Escribe `yes` cuando te lo pida. Este paso tarda entre **10 y 15 minutos**.

### 6.1 Verifica los outputs

```bash
terraform output container_app_url
terraform output db_name
terraform output jump_host_ip
MSYS_NO_PATHCONV=1 terraform output db_host
```

---

## 🗄️ Paso 7 — Aplicar migraciones de Entity Framework

Con los datos del output anterior:

```bash
dotnet ef database update --connection "Host=<db_host>;Database=<db_name>;Username=<pg_admin_user>;Password=<pg_admin_password>;SSL Mode=Require;"
```

> ⚠️ Asegúrate de que tu IP esté en `allowed_ips` dentro del `prb.tfvars` antes de correr este comando.

---

## 🔄 Paso 8 — Redesplegar la API (actualizaciones futuras)

Cada vez que hagas cambios en tu API:

```bash
# 1. Reconstruir la imagen
docker build -t acrdogsitterapiprb.azurecr.io/dogsitter-api:latest .

# 2. Subir la imagen
az acr login --name acrdogsitterapiprb
docker push acrdogsitterapiprb.azurecr.io/dogsitter-api:latest

# 3. Forzar el redespliegue
az containerapp update \
  --name ca-dogsitter-api-prb \
  --resource-group rg-dogsitter-api-prb \
  --image acrdogsitterapiprb.azurecr.io/dogsitter-api:latest
```

---

## 🔒 Paso 9 — Conectarse a la BD via SSH (DBeaver / pgAdmin)

### 9.1 Verifica la conexión SSH

```bash
ssh -i ~/.ssh/id_rsa_azure azureuser@<jump_host_ip>
```

### 9.2 Configura DBeaver

**Pestaña SSH:**
```
☑ Use SSH Tunnel
Host:        <jump_host_ip>
Port:        22
Username:    azureuser
Auth method: Public key
Private key: C:\Users\<TuUsuario>\.ssh\id_rsa_azure
```

**Pestaña Main (conexión PostgreSQL):**
```
Host:     <db_host>
Port:     5432
Database: <db_name>
Username: <pg_admin_user>
Password: <pg_admin_password>
SSL:      Require
```

---

## 🌍 Despliegue en PRD

El proceso es idéntico al de PRB, solo cambia el archivo de variables:

```bash
source environments/prd.env
terraform apply -var-file="environments/prd.tfvars" -lock=false
```

---

## ⚠️ Solución de problemas comunes

| Error | Solución |
|---|---|
| `source: command not found` | Usa Git Bash, no CMD ni PowerShell |
| `MSYS_NO_PATHCONV` path issues | Ejecuta `export MSYS_NO_PATHCONV=1` antes de terraform |
| `Error acquiring the state lock` | Ejecuta `rm -f .terraform.tfstate.lock.info` |
| `var.pg_admin_password: Enter a value` | Recarga con `source environments/prb.env` |
| `SkuNotAvailable` | Cambia el SKU de la VM en `modules/AzureJumpHost/main.tf` |
| `LocationIsOfferRestricted` | Cambia la `location` en el `.tfvars` |

---

## 🗑️ Script para eliminar toda la infraestructura

Crea el archivo `destroy.sh` en la raíz de tu carpeta `terraform/`:

```bash
#!/bin/bash

echo "⚠️  ADVERTENCIA: Esto eliminará TODA la infraestructura de $1"
echo "¿Estás seguro? Escribe 'yes' para continuar:"
read confirmation

if [ "$confirmation" != "yes" ]; then
  echo "❌ Operación cancelada."
  exit 1
fi

# Cargar variables del ambiente
ENVIRONMENT=$1
source environments/$ENVIRONMENT.env
export MSYS_NO_PATHCONV=1

echo "🔥 Eliminando infraestructura de $ENVIRONMENT..."

terraform destroy \
  -var-file="environments/$ENVIRONMENT.tfvars" \
  -lock=false \
  -auto-approve

echo "✅ Infraestructura de $ENVIRONMENT eliminada exitosamente."
```

### Uso del script

```bash
# Dale permisos de ejecución
chmod +x destroy.sh

# Eliminar PRB
./destroy.sh prb

# Eliminar PRD
./destroy.sh prd
```

---

## 📝 .gitignore recomendado

Asegúrate de tener esto en tu `.gitignore`:

```
environments/*.env
*.tfstate
*.tfstate.backup
**/.terraform/
.terraform.lock.hcl
```
