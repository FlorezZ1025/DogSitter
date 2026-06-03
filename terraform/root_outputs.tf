# ─── Resource Group ────────────────────────────────────────────────────────────
output "resource_group_name" {
  description = "Nombre del resource group principal"
  value       = azurerm_resource_group.main.name
}

# ─── ACR ───────────────────────────────────────────────────────────────────────
output "acr_login_server" {
  description = "URL del ACR — úsala como prefijo en tus imágenes Docker"
  value       = module.acr.acr_login_server
}

output "acr_name" {
  description = "Nombre del ACR para el comando az acr login"
  value       = module.acr.acr_name
}

# ─── AKS ───────────────────────────────────────────────────────────────────────
output "aks_cluster_name" {
  description = "Nombre del cluster AKS — úsalo en az aks get-credentials"
  value       = module.aks.cluster_name
}

output "aks_cluster_id" {
  description = "ID del cluster AKS"
  value       = module.aks.cluster_id
}

output "aks_kube_config_command" {
  description = "Comando para obtener las credenciales del cluster AKS"
  value       = "az aks get-credentials --resource-group ${azurerm_resource_group.main.name} --name ${module.aks.cluster_name}"
}

# ─── Database ──────────────────────────────────────────────────────────────────
output "db_server_fqdn" {
  description = "FQDN del servidor PostgreSQL — úsalo en la connection string"
  value       = module.database.server_fqdn
}

output "db_name" {
  description = "Nombre de la base de datos"
  value       = module.database.database_name
}

output "db_connection_string" {
  description = "Connection string lista para usar en el deployment.yaml de K8s"
  value       = "Server=${module.database.server_fqdn},1433;Initial Catalog=${module.database.database_name};User ID=${var.db_admin_login};Password=${var.db_admin_password};Encrypt=True;TrustServerCertificate=False;"
  sensitive   = true
}

# ─── VNet ──────────────────────────────────────────────────────────────────────
output "vnet_id" {
  description = "ID de la VNet"
  value       = module.vnet.vnet_id
}

output "aks_subnet_id" {
  description = "ID de la subnet de AKS"
  value       = module.vnet.aks_subnet_id
}

# ─── Resumen para K8s ──────────────────────────────────────────────────────────
output "k8s_setup_commands" {
  description = "Comandos para configurar kubectl y conectar al cluster"
  value = <<-EOT
    # 1. Conectar al cluster AKS
    az aks get-credentials --resource-group ${azurerm_resource_group.main.name} --name ${module.aks.cluster_name}

    # 2. Login al ACR
    az acr login --name ${module.acr.acr_name}

    # 3. Verificar nodos
    kubectl get nodes

    # 4. Prefijo para tus imágenes Docker
    REGISTRY=${module.acr.acr_login_server}
    docker build --build-arg APP_VERSION=1.0.0 --build-arg APP_RELEASE=stable -t $REGISTRY/dogsitter-api:1.0.0-stable .
    docker push $REGISTRY/dogsitter-api:1.0.0-stable
  EOT
}
