output "cluster_name"       { value = azurerm_kubernetes_cluster.main.name }
output "cluster_id"         { value = azurerm_kubernetes_cluster.main.id }
output "kube_config" {
  value     = azurerm_kubernetes_cluster.main.kube_config_raw
  sensitive = true
}
output "node_resource_group"{ value = azurerm_kubernetes_cluster.main.node_resource_group }
