resource "azurerm_kubernetes_cluster" "main" {
  name                = "aks-${var.project_name}-${var.environment}"
  location            = var.location
  resource_group_name = var.resource_group_name
  dns_prefix          = "${var.project_name}-${var.environment}"

  default_node_pool {
	name           = "default"
	node_count     = 2
	vm_size        = "Standard_D2_v2"
	vnet_subnet_id = var.subnet_id
  }

  identity {
	type = "SystemAssigned"
  }

  network_profile {
	network_plugin    = "azure"
	load_balancer_sku = "standard"
  }
}

resource "azurerm_role_assignment" "aks_acr_pull" {
  principal_id                     = azurerm_kubernetes_cluster.main.kubelet_identity[0].object_id
  role_definition_name             = "AcrPull"
  scope                            = var.acr_id
  skip_service_principal_aad_check = true
}
