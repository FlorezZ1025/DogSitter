# ─── General ───────────────────────────────────────────────────────────────────
variable "project" {
  description = "Nombre del proyecto, usado como prefijo en todos los recursos"
  type        = string
  default     = "dogsitter"
}

variable "environment" {
  description = "Ambiente de despliegue (dev, staging, prod)"
  type        = string
  default     = "dev"
}

variable "location" {
  description = "Región de Azure donde se crean los recursos"
  type        = string
  default     = "eastus"
}

# ─── VNet ──────────────────────────────────────────────────────────────────────
variable "vnet_address_space" {
  description = "Espacio de direcciones de la VNet"
  type        = list(string)
  default     = ["10.0.0.0/16"]
}

variable "aks_subnet_cidr" {
  description = "CIDR para la subnet de AKS"
  type        = string
  default     = "10.0.1.0/24"
}

variable "db_subnet_cidr" {
  description = "CIDR para la subnet de la base de datos"
  type        = string
  default     = "10.0.2.0/24"
}

# ─── ACR ───────────────────────────────────────────────────────────────────────
variable "acr_sku" {
  description = "SKU del Azure Container Registry (Basic, Standard, Premium)"
  type        = string
  default     = "Basic"
}

# ─── AKS ───────────────────────────────────────────────────────────────────────
variable "kubernetes_version" {
  description = "Versión de Kubernetes para el cluster AKS"
  type        = string
  default     = "1.29"
}

variable "aks_node_count" {
  description = "Número de nodos del node pool principal"
  type        = number
  default     = 2
}

variable "aks_node_vm_size" {
  description = "Tamaño de VM para los nodos de AKS"
  type        = string
  default     = "Standard_B2s"
}

# ─── Database ──────────────────────────────────────────────────────────────────
variable "db_admin_login" {
  description = "Usuario administrador de la base de datos"
  type        = string
  default     = "dogsitteradmin"
  sensitive   = true
}

variable "db_admin_password" {
  description = "Contraseña del administrador de la base de datos"
  type        = string
  sensitive   = true
}

variable "db_sku_name" {
  description = "SKU del servidor PostgreSQL (ej: B_Standard_B1ms)"
  type        = string
  default     = "B_Standard_B1ms"
}
