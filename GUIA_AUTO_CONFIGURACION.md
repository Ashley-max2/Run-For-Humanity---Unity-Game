# 🎮 RUN FOR HUMANITY - SISTEMA DE AUTO-CONFIGURACIÓN

## 🚀 INSTALACIÓN RÁPIDA (30 segundos)

### Paso 1: Crear GameObject Maestro
1. En Unity, abre cualquier escena (recomendado: **MainMenu**)
2. Click derecho en Hierarchy → Create Empty
3. Renombrar a: **"MasterSetup"**
4. Agregar componente: **MasterSetup** (buscar en Add Component)

### Paso 2: Ejecutar Setup
- Opción A: En el Inspector, click en **"Run Complete Setup"**
- Opción B: Presiona Play y se ejecutará automáticamente

**¡Listo! Todo está configurado automáticamente.**

---

## 📋 ¿QUÉ HACE EL SISTEMA?

### ✨ Auto-Construcción de UI
- **UIFactory**: Crea todos los componentes UI con estándares consistentes
- **UIAutoBuilder**: Detecta la escena y construye su UI automáticamente
- **Scene Builders**: Construye UI específico para cada escena:
  - PreloaderUIBuilder
  - MainMenuUIBuilder
  - GameplayUIBuilder
  - ShopUIBuilder
  - ONGSelectionUIBuilder

### 🎯 Auto-Configuración de Managers
- **EventManager**: Sistema de eventos globales
- **AudioManager**: Control de música y efectos de sonido
- **AdManager**: Integración de anuncios (Unity Ads)
- **DonationSystem**: Sistema de donaciones a ONGs
- **UIManager**: Control de vistas de interfaz
- **GameManager**: Control del estado del juego

### ✅ Validación Automática
- Verifica que todos los managers existan
- Comprueba que managers persistentes estén en root
- Valida referencias de UI
- Reporta problemas encontrados

---

## 🎨 MEJORAS DE UI IMPLEMENTADAS

### Tamaños Mejorados
- **Botones normales**: 400x80 (más grandes y fáciles de tocar)
- **Botones grandes**: 500x100 (para acciones principales)
- **Controles de juego**: 180x180 (perfectos para móvil)
- **Sliders**: 600x40 (más visibles)
- **Iconos**: 100x100

### Colores Profesionales
- **Success**: Verde brillante (#33FF33)
- **Warning**: Naranja (#FFBB33)
- **Danger**: Rojo (#FF4444)
- **Accent**: Azul cielo (#4DCCFF)
- **Panels**: Oscuros semi-transparentes (mejor contraste)

### Efectos Visuales
- **Sombras** en todos los textos para mejor legibilidad
- **Gradientes** en títulos para look moderno
- **Hover effects** en botones (transiciones suaves)
- **Emojis** en botones para mejor UX (▶ 🛒 ⚙ ✖)

### Espaciado y Padding
- **Padding pequeño**: 10px
- **Padding medio**: 20px
- **Padding grande**: 40px
- **Spacing consistente**: 20/40/80px

### Gameplay UI Mejorado
- **Contenedores con fondo** para coins y distancia
- **Iconos visuales** al lado de los valores
- **Fuentes grandes** (48px) para fácil lectura durante el juego
- **Controles mejorados**: Botones grandes con símbolos Unicode
  - ⬆ Saltar (verde)
  - ⬇ Deslizar (naranja)
  - ⬅➡ Mover (azul)

---

## 🔧 USO AVANZADO

### Reconstruir UI de una Escena
```csharp
// Desde código
MainMenuUIBuilder.Build();
GameplayUIBuilder.Build();
// etc...
```

### Reconstruir UI desde Inspector
1. Selecciona el GameObject con **UIAutoBuilder**
2. Click derecho en el componente
3. Selecciona "Build Current Scene UI"

### Limpiar y Reconstruir
1. Click derecho en **UIAutoBuilder**
2. Selecciona "Clear All UI"
3. Luego "Build Current Scene UI"

### Arreglar Jerarquía de Managers
```csharp
// Desde código
configurator.FixManagerHierarchy();
```

O desde Inspector:
1. Selecciona GameObject con **GameAutoConfigurator**
2. Click derecho → "Fix Manager Hierarchy"

### Validar Todo el Sistema
```csharp
// Desde código
configurator.ValidateGameSetup();
```

O desde Inspector:
1. Selecciona GameObject con **GameAutoConfigurator**
2. Click derecho → "Validate Game Setup"

---

## 📁 ESTRUCTURA DE ARCHIVOS

```
Assets/Scripts/
├── UI/
│   └── Builder/
│       ├── UIFactory.cs                 // Fábrica de componentes
│       ├── UIAutoBuilder.cs             // Auto-constructor principal
│       ├── PreloaderUIBuilder.cs        // Builder de Preloader
│       ├── MainMenuUIBuilder.cs         // Builder de MainMenu
│       ├── GameplayUIBuilder.cs         // Builder de Gameplay
│       ├── ShopUIBuilder.cs             // Builder de Shop
│       └── ONGSelectionUIBuilder.cs     // Builder de ONGSelection
└── Setup/
    ├── GameAutoConfigurator.cs          // Configurador de managers
    └── MasterSetup.cs                   // Setup maestro (¡USAR ESTE!)
```

---

## 🎯 CASOS DE USO

### 1. Primera vez configurando el juego
1. Crear GameObject "MasterSetup"
2. Agregar componente MasterSetup
3. Click en "Run Complete Setup"
4. Presionar Play para probar

### 2. UI se ve mal después de cambios
1. Seleccionar MasterSetup
2. Click derecho → "Quick Fix - UI Only"

### 3. Managers tienen errores
1. Seleccionar MasterSetup
2. Click derecho → "Quick Fix - Managers Only"

### 4. Crear nueva escena
1. Agregar tu escena a Build Settings
2. Crear builder en UIAutoBuilder.BuildCurrentSceneUI()
3. Ejecutar MasterSetup

### 5. Cambiar colores/tamaños globalmente
1. Editar UIFactory.cs (sección de constantes)
2. Reconstruir UI con "Quick Fix - UI Only"
3. Todos los elementos se actualizan automáticamente

---

## ⚠️ SOLUCIÓN DE PROBLEMAS

### "DontDestroyOnLoad only works for root GameObjects"
**Solución**: Click en "Fix Manager Hierarchy" en GameAutoConfigurator

### "The variable menuView has not been assigned"
**Solución**: Click en "Run Complete Setup" - asigna referencias automáticamente

### UI se ve cortado en diferentes resoluciones
**Solución**: Canvas Scaler está configurado a 1080x1920 con match 0.5 - debe verse bien

### Botones no responden
**Solución**: Verifica que EventSystem existe (se crea automáticamente con UIFactory)

### Fuentes no se ven
**Solución**: Asegúrate que TextMesh Pro está instalado en Package Manager

---

## 🎨 PERSONALIZACIÓN

### Cambiar Colores Globales
Edita en **UIFactory.cs**:
```csharp
public static readonly Color COLOR_SUCCESS = new Color(0.2f, 0.9f, 0.3f, 1f);
public static readonly Color COLOR_WARNING = new Color(1f, 0.7f, 0.2f, 1f);
// etc...
```

### Cambiar Tamaños Globales
Edita en **UIFactory.cs**:
```csharp
public static readonly Vector2 BUTTON_SIZE = new Vector2(400, 80);
public static readonly Vector2 BUTTON_LARGE = new Vector2(500, 100);
// etc...
```

### Agregar Nuevos Elementos UI
1. Agrega método estático en **UIFactory.cs**
2. Usa en cualquier builder
3. Mantiene consistencia automáticamente

### Personalizar Builder de Escena
Edita el archivo correspondiente:
- MainMenuUIBuilder.cs
- GameplayUIBuilder.cs
- etc.

---

## 🚀 PRÓXIMOS PASOS

1. ✅ **Ejecutar MasterSetup** en tu escena principal
2. ✅ **Probar el juego** - todo debe funcionar
3. ✅ **Personalizar colores** si lo deseas
4. ✅ **Agregar sprites/iconos** a las imágenes creadas
5. ✅ **Conectar lógica** de botones a tus sistemas

---

## 💡 TIPS PROFESIONALES

- 🔄 **Rebuilding es seguro**: Puedes reconstruir UI múltiples veces sin problemas
- 🎨 **Consistencia**: Cambia valores en UIFactory para actualizar todo el juego
- 📱 **Mobile-first**: UI diseñado para 1080x1920, se adapta a otras resoluciones
- ⚡ **Performance**: UI se crea rápido, pero evita reconstruir en runtime
- 🐛 **Debug**: Usa "Validate Game Setup" para encontrar problemas rápidamente

---

## 📞 REFERENCIA RÁPIDA

### Comandos de Inspector (Click derecho en componente)

**MasterSetup**:
- Run Complete Setup
- Quick Fix - UI Only
- Quick Fix - Managers Only
- Reset Configuration Flag

**UIAutoBuilder**:
- Build Current Scene UI
- Clear All UI
- Rebuild Current Scene

**GameAutoConfigurator**:
- Setup Complete Game
- Fix Manager Hierarchy
- Validate Game Setup

---

**¡Disfruta tu juego auto-configurado!** 🎮✨
