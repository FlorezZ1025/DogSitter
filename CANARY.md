# Estrategia de Despliegue Canary — DogSitter API

## Descripción general

La aplicación utiliza una estrategia de **Canary Deployment** sobre Kubernetes (AKS) con NGINX Ingress Controller. El tráfico se divide entre dos versiones activas de la API:

| Deployment | Imagen | Réplicas | Propósito |
|---|---|---|---|
| `dogsitter-stable` | `dogsitter-api:1.0.0-stable` | 4 | Versión productiva estable |
| `dogsitter-canary` | `dogsitter-api:2.0.0-canary` | 1 | Versión en validación |

---

## Flujo de tráfico

```
Cliente
  │
  ▼
NGINX Ingress Controller (IP pública del cluster AKS)
  │
  ├── Sin header  ──────────────────► dogsitter-service (ClusterIP)
  │                                         │
  │                              ┌──────────┴──────────┐
  │                              │ Round-robin 4:1      │
  │                         stable pods (4)       canary pods (1)
  │
  └── X-Canary: true ──────────► dogsitter-canary-service (ClusterIP)
                                        │
                                  canary pods (100%)
```

### Distribución de tráfico sin header

El Service principal `dogsitter-service` selecciona todos los pods con el label `app: dogsitter` (tanto stable como canary). NGINX distribuye en round-robin, resultando en:

- **80%** → pods stable (4 de 5 pods totales)
- **20%** → pods canary (1 de 5 pods totales)

Para cambiar el porcentaje, ajusta el valor de `replicas` en `k8s-manifests/deployment-canary.yaml`.

---

## Obtener la IP pública

```powershell
kubectl get svc -n ingress-nginx
```

La columna `EXTERNAL-IP` del servicio `ingress-nginx-controller` es la IP base para todas las peticiones.

---

## Validación con Postman

### 1. Health check — Stable (distribución normal)

| Campo | Valor |
|---|---|
| Método | `GET` |
| URL | `http://<EXTERNAL-IP>/healthz` |
| Headers | *(ninguno adicional)* |

Respuesta esperada:
```json
{
  "status": "Healthy",
  "version": "1.0.0",
  "release": "stable",
  "deployedAt": "...",
  "checks": [ ... ]
}
```

---

### 2. Health check — Canary (forzado por header)

| Campo | Valor |
|---|---|
| Método | `GET` |
| URL | `http://<EXTERNAL-IP>/healthz` |
| Header | `X-Canary: true` |

Respuesta esperada:
```json
{
  "status": "Healthy",
  "version": "2.0.0",
  "release": "canary",
  "deployedAt": "...",
  "checks": [ ... ]
}
```

> El campo `release` permite confirmar a cuál deployment llegó la petición.

---

### 3. Swagger UI

| Deployment | URL |
|---|---|
| Stable / distribución normal | `http://<EXTERNAL-IP>/` |
| Canary | `http://<EXTERNAL-IP>/` con header `X-Canary: true` |

---

### 4. Endpoints de la API

Reemplaza `<EXTERNAL-IP>` con la IP real del cluster.

#### V1

| Recurso | Método | URL |
|---|---|---|
| Razas | `GET` | `http://<EXTERNAL-IP>/api/v1/razas` |
| Perros | `GET` | `http://<EXTERNAL-IP>/api/v1/perros` |
| Cuidadores | `GET` | `http://<EXTERNAL-IP>/api/v1/cuidadores` |

#### V2 (disponible también en canary)

| Recurso | Método | URL |
|---|---|---|
| Razas | `GET` | `http://<EXTERNAL-IP>/api/v2/razas` |
| Perros | `GET` | `http://<EXTERNAL-IP>/api/v2/perros` |
| Cuidadores | `GET` | `http://<EXTERNAL-IP>/api/v2/cuidadores` |
| Mensajes | `GET` | `http://<EXTERNAL-IP>/api/v2/mensajes` |

Para forzar canary en cualquiera de estos endpoints agrega el header `X-Canary: true`.

---

## Cómo actualizar solo el canary

1. Realizar cambios en el código.

2. Build y push de la nueva imagen:
```powershell
docker build `
  --build-arg APP_VERSION=2.0.1 `
  --build-arg APP_RELEASE=canary `
  -t acrdogsitterdev.azurecr.io/dogsitter-api:2.0.1-canary .

docker push acrdogsitterdev.azurecr.io/dogsitter-api:2.0.1-canary
```

3. Actualizar en `k8s-manifests/deployment-canary.yaml`:
   - `image: acrdogsitterdev.azurecr.io/dogsitter-api:2.0.1-canary`
   - `ApiVersion__Version: "2.0.1"`

4. Aplicar solo el manifest canary:
```powershell
kubectl apply -f k8s-manifests/deployment-canary.yaml
```

5. Verificar que el pod levantó:
```powershell
kubectl get pods -l version=canary
```

6. Validar con Postman usando `X-Canary: true` que `/healthz` devuelve `"version": "2.0.1"`.

---

## Promover canary a stable

Una vez validado el canary, promoverlo a stable:

```powershell
# Re-etiquetar la imagen canary como stable
docker tag acrdogsitterdev.azurecr.io/dogsitter-api:2.0.1-canary `
           acrdogsitterdev.azurecr.io/dogsitter-api:2.0.1-stable

docker push acrdogsitterdev.azurecr.io/dogsitter-api:2.0.1-stable
```

Luego actualizar `k8s-manifests/deployment-stable.yaml` con el nuevo tag y:

```powershell
kubectl apply -f k8s-manifests/deployment-stable.yaml
```
