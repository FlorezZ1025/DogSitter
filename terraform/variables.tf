variable "environment" {
  description = "Ambiente de despliegue (prb, prd)"
  type        = string
}

variable "location" {
  description = "Región de Azure"
  type        = string
  default     = "eastus2"
}

variable "project_name" {
  description = "Nombre del proyecto"
  type        = string
}

# VNet
variable "vnet_address_space" {
  type    = list(string)
  default = ["10.0.0.0/16"]
}

variable "subnet_prefixes" {
  type    = list(string)
  default = ["10.0.1.0/24", "10.0.2.0/24"]
}

variable "subnet_names" {
  type    = list(string)
  default = ["subnet-apps", "subnet-db"]
}

# PostgreSQL
variable "pg_admin_user" {
  type      = string
  sensitive = true
}

variable "pg_admin_password" {
  type      = string
  sensitive = true
}

# Container Apps
variable "container_image" {
  type        = string
  description = "Imagen del contenedor (ej: myregistry.azurecr.io/myapp:latest)"
}

variable "container_cpu" {
  type    = number
  default = 0.5
}

variable "container_memory" {
  type    = string
  default = "1Gi"
}
# ACR
variable "acr_sku" {
  type    = string
  default = "Basic"
}

# PostgreSQL acceso público
variable "allowed_ips" {
  description = "Lista de IPs permitidas para acceder a PostgreSQL públicamente"
  type        = list(string)
  default     = []
}