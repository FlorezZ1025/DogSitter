resource "azurerm_kubernetes_cluster" "main" {
  name                = "aks-${var.project}-${var.environment}"
  resource_group_name = var.resource_group_name
  location            = var.location
  dns_prefix          = "${var.project}-${var.environment}"
  kubernetes_version  = var.kubernetes_version

  default_node_pool {
    name           = "system"
    node_count     = var.node_count
    vm_size        = var.node_vm_size
    vnet_subnet_id = var.vnet_subnet_id

    # Habilita el auto-scaler para producción
    # enable_auto_scaling = true
    # min_count           = 2
    # max_count           = 5
  }

  # Identidad gestionada — AKS se autentica con Azure sin service principals manuales
  identity {
    type = "SystemAssigned"
  }

  network_profile {
    network_plugin    = "azure"
    load_balancer_sku = "standard"
    # Necesario para el Ingress Controller NGINX
    outbound_type     = "loadBalancer"
  }

  tags = var.tags
}

# Permite que AKS jale imágenes del ACR sin credenciales manuales
resource "azurerm_role_assignment" "aks_acr_pull" {
  principal_id                     = azurerm_kubernetes_cluster.main.kubelet_identity[0].object_id
  role_definition_name             = "AcrPull"
  scope                            = var.acr_id
  skip_service_principal_aad_check = true
}
