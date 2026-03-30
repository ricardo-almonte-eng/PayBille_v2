# Inventory Module Design

## Overview

The inventory module is composed of **6 MongoDB collections** organized into two layers: a global catalog (shared across all companies) and per-company/per-branch inventory documents.

| Collection | Description |
|---|---|
| `catalogo_productos` | Global product catalog for autocomplete. No company owns it. |
| `catalogo_marcas` | Global brand list for autocomplete. |
| `catalogo_categorias` | Global categories with subcategory support. |
| `catalogo_colores` | Global color list with HEX values for UI chips. |
| `catalogo_modelos` | Global models linked to a brand for autocomplete. |
| `productos_almacen` | Company warehouse — one document per product per company. No stock here. |
| `plantillas_garantia` | Reusable warranty templates per company. |
| `inventario_sucursal` | Branch inventory — stock, prices and warranty live here. |
| `movimientos_inventario` | Append-only audit log of every stock change. |

---

## The Two Modes of `UnidadInventario`

`ItemInventarioSucursal.Unidades` is a list of `UnidadInventario` sub-documents. The behavior depends on whether the product tracks individual serial numbers or just quantities.

### Mode 1 — Unique Serial (e.g., cell phones, laptops)

- `EsUnico = true`
- Each entry = 1 physical unit with its own `CodigoSerial` (IMEI, serial number, etc.)
- `Cantidad` is always `1`
- **Available stock** = `Unidades.Count(u => !u.Vendida)`

```json
{
  "_id": "6641a1b2c3d4e5f600000001",
  "idEmpresa": "EMP001",
  "idSucursal": "SUC001",
  "idProductoAlmacen": "6641a1b2c3d4e5f600000010",
  "nombreProducto": "iPhone 15 Pro",
  "marca": "Apple",
  "modelo": "iPhone 15 Pro",
  "categoria": "Celulares",
  "manejaCodigoUnico": true,
  "unidades": [
    {
      "id": "6641a1b2c3d4e5f600000021",
      "codigoSerial": "354651230000001",
      "esUnico": true,
      "cantidad": 1,
      "costo": 850.00,
      "estadoUso": "Nuevo",
      "condicion": null,
      "grado": null,
      "vendida": false,
      "idVenta": null,
      "esCanje": false,
      "creadoEnUtc": "2024-05-12T14:00:00Z"
    },
    {
      "id": "6641a1b2c3d4e5f600000022",
      "codigoSerial": "354651230000002",
      "esUnico": true,
      "cantidad": 1,
      "costo": 850.00,
      "estadoUso": "Nuevo",
      "vendida": true,
      "idVenta": "6641a1b2c3d4e5f600000099",
      "nombreCliente": "Juan Pérez",
      "esCanje": false,
      "creadoEnUtc": "2024-05-12T14:00:00Z"
    }
  ],
  "cantidadMinimaAlerta": 2,
  "cantidadInfinita": false,
  "precios": {
    "precio1": 1099.99,
    "precio2": 1050.00
  },
  "garantia": {
    "idPlantilla": "6641a1b2c3d4e5f600000050",
    "nombrePlantilla": "Garantía Celulares 1 Año",
    "duracionDias": 365,
    "textoSnapshot": "Esta garantía cubre defectos de fábrica por 365 días..."
  },
  "permitirVenta": true,
  "creadoEnUtc": "2024-05-12T14:00:00Z"
}
```

**CantidadDisponible** = `unidades.Count(u => !u.vendida)` → **1** (only the first unit is available)

---

### Mode 2 — Quantity / Bulk (e.g., chargers, cables, accessories)

- `EsUnico = false`
- Typically one entry per batch. `CodigoSerial` is `null` or a generic batch barcode.
- `Cantidad` = units in stock for that entry.
- **Available stock** = `Unidades.Sum(u => u.Cantidad)`

```json
{
  "_id": "6641a1b2c3d4e5f600000002",
  "idEmpresa": "EMP001",
  "idSucursal": "SUC001",
  "idProductoAlmacen": "6641a1b2c3d4e5f600000011",
  "nombreProducto": "Cargador USB-C 20W",
  "marca": "Apple",
  "modelo": null,
  "categoria": "Accesorios",
  "manejaCodigoUnico": false,
  "unidades": [
    {
      "id": "6641a1b2c3d4e5f600000030",
      "codigoSerial": null,
      "esUnico": false,
      "cantidad": 47,
      "costo": 8.50,
      "estadoUso": "Nuevo",
      "vendida": false,
      "esCanje": false,
      "creadoEnUtc": "2024-05-12T14:00:00Z"
    }
  ],
  "cantidadMinimaAlerta": 10,
  "cantidadInfinita": false,
  "precios": {
    "precio1": 24.99,
    "precio2": 22.00
  },
  "garantia": null,
  "permitirVenta": true,
  "creadoEnUtc": "2024-05-12T14:00:00Z"
}
```

**CantidadDisponible** = `unidades.Sum(u => u.cantidad)` → **47**

---

## How to Calculate `CantidadDisponible`

```csharp
decimal CantidadDisponible(ItemInventarioSucursal item) =>
    item.ManejaCodigoUnico
        ? item.Unidades.Count(u => !u.Vendida)
        : item.Unidades.Sum(u => u.Cantidad);
```

---

## Autocomplete Flow

```
CatalogoProducto (global)
        │
        │ user searches & selects
        ▼
ProductoAlmacen (per company)
        │  idCatalogo → CatalogoProducto._id (null if created manually)
        │
        │ assigned to branch + prices + warranty configured
        ▼
ItemInventarioSucursal (per branch)
        │  idProductoAlmacen → ProductoAlmacen._id
        │  snapshot fields: nombreProducto, marca, modelo, categoria, manejaCodigoUnico
        │
        │ units registered
        ▼
UnidadInventario[] (embedded in ItemInventarioSucursal)
```

1. User types "iPhone 15" → system searches `catalogo_productos` → returns matching entries.
2. User selects a catalog entry → `ProductoAlmacen` is created (or reused) with `IdCatalogo` filled.
3. When assigning the product to a branch, `ItemInventarioSucursal` is created. All snapshot fields are copied from `ProductoAlmacen` at that moment, including `ManejaCodigoUnico`.
4. Stock entries (`UnidadInventario`) are added as inventory arrives (purchases, trade-ins, adjustments).
5. Every stock change also writes a `MovimientoInventario` record (append-only).

The snapshot pattern ensures that if the company later renames a product or changes its warranty template, all historical sales records still show the original data at the time of the transaction.

---

## Recommended MongoDB Indexes

```javascript
// catalogo_productos
db.catalogo_productos.createIndex({ nombre: "text", marca: "text", modelo: "text" });
db.catalogo_productos.createIndex({ codigoBarra: 1 }, { sparse: true });

// productos_almacen
db.productos_almacen.createIndex({ idEmpresa: 1 });
db.productos_almacen.createIndex({ idEmpresa: 1, codigoBarra: 1 }, { sparse: true });
db.productos_almacen.createIndex({ idEmpresa: 1, nombre: "text" });

// inventario_sucursal
db.inventario_sucursal.createIndex({ idEmpresa: 1, idSucursal: 1 });
db.inventario_sucursal.createIndex({ idProductoAlmacen: 1 });
db.inventario_sucursal.createIndex({ "unidades.codigoSerial": 1 }, { sparse: true });

// movimientos_inventario
db.movimientos_inventario.createIndex({ idEmpresa: 1, idSucursal: 1, creadoEnUtc: -1 });
db.movimientos_inventario.createIndex({ idItem: 1, creadoEnUtc: -1 });
db.movimientos_inventario.createIndex({ "codigoSerialAfectado": 1 }, { sparse: true });

// plantillas_garantia
db.plantillas_garantia.createIndex({ idEmpresa: 1 });
```
