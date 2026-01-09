# Guía UI Mejorada - Run For Humanity

## ✅ Cambios Realizados

### 1. **Main Menu (Menú Principal) - SIMPLIFICADO**
- **ANTES**: Logo, título con gradiente, 4 botones, panel de configuración con sliders
- **AHORA**: 
  - Título grande centrado
  - 3 botones centrados y funcionales:
    - **JUGAR** (verde) → Va a ONGSelection
    - **TIENDA** (azul) → Va a Shop
    - **SALIR** (rojo) → Cierra el juego
  - Todo visible y organizado verticalmente

### 2. **ONG Selection - REDISEÑADO**
- **ANTES**: 6 ONGs en grid, scroll view, panel de distribución con sliders
- **AHORA**:
  - Título claro "SELECCIONA TU ONG"
  - 3 botones grandes de ONGs:
    - **Cruz Roja** (rojo)
    - **UNICEF** (azul)
    - **WWF** (verde)
  - Cada botón va directamente al juego
  - Botón **VOLVER** para regresar al menú

### 3. **Gameplay - LIMPIO Y FUNCIONAL**
- **ANTES**: Múltiples paneles (HUD, pausa, game over), 4 botones de control
- **AHORA**:
  - Display de monedas (arriba izquierda)
  - Display de distancia (arriba derecha)
  - Botón de pausa (arriba derecha)
  - 2 botones de control:
    - **SALTAR** (verde, abajo derecha)
    - **DESLIZAR** (azul, junto al saltar)
  - Limpio, sin paneles superpuestos

### 4. **MasterSetup - MEJORADO**
Nuevas opciones:
- **setupOnStart**: FALSE por defecto (no auto-ejecuta)
- **clearExistingUI**: TRUE (limpia UI antes de crear)
- Nuevos comandos en Inspector:
  - **Run Complete Setup** - Configura todo
  - **Clear All UI** - Limpia solo la UI
  - **Quick Fix - UI Only** - Limpia y reconstruye UI

## 📋 Cómo Usar

### Opción 1: Desde el Inspector
1. Selecciona el GameObject con MasterSetup
2. Click derecho en el componente
3. Selecciona "Run Complete Setup"
4. Si hay problemas, usa "Clear All UI" y luego "Quick Fix - UI Only"

### Opción 2: Al Iniciar Escena
1. Marca `setupOnStart` como TRUE en el Inspector
2. Al dar Play, se configura automáticamente

## 🎮 Navegación del Juego

```
MainMenu
  ├─→ JUGAR → ONGSelection → Gameplay
  ├─→ TIENDA → Shop
  └─→ SALIR → Cierra aplicación
```

## 🔧 Solución de Problemas

### Problema: "Hay muchos botones por todos lados"
**Solución**: 
1. Selecciona MasterSetup
2. Click derecho → "Clear All UI"
3. Click derecho → "Quick Fix - UI Only"

### Problema: "Los botones no funcionan"
**Causas posibles**:
- EventSystem duplicado (el script lo limpia automáticamente)
- Canvas superpuestos (usa "Clear All UI")

### Problema: "La UI se ve rara después de varias ejecuciones"
**Solución**:
1. Detén el juego
2. Borra manualmente los Canvas en la Jerarquía
3. Ejecuta "Run Complete Setup" de nuevo

## 📝 Notas Técnicas

- **Canvas único por escena**: El sistema elimina Canvas anteriores
- **EventSystem único**: Solo uno activo a la vez
- **Botones funcionales**: Todos tienen onClick configurado
- **Colores consistentes**: 
  - Verde = Acción principal (JUGAR, SALTAR)
  - Azul = Acción secundaria (TIENDA, DESLIZAR)
  - Rojo = Peligro/Salir
  - Gris = Neutro

## ⚠️ Warnings Esperadas

Estos warnings son normales y no afectan funcionalidad:
```
Unicode \u00F3 (ó) not found - Solo afecta display
Unicode \u00E9 (é) not found - Solo afecta display
Unicode \u00D3 (Ó) not found - Solo afecta display
Unicode \u0025 (%) not found - Solo afecta display
CS0414 unused field warnings - Campos para uso futuro
```

## 🎯 Próximos Pasos Recomendados

1. **Prueba la navegación**: MainMenu → ONG → Gameplay
2. **Verifica los botones**: Cada uno debe hacer algo visible
3. **Si algo falla**: Usa "Clear All UI" y reconstruye
4. **Para ajustar posiciones**: Modifica valores en UIFactory.cs

## 🆘 Si Necesitas Ayuda

1. Ejecuta "Clear All UI" primero
2. Verifica que solo hay UN Canvas en la escena
3. Revisa que hay UN EventSystem
4. Ejecuta "Run Complete Setup"
5. Si sigue fallando, elimina manualmente todos los GameObjects de UI y vuelve a ejecutar

---

**Todo funcional y listo para probar en Unity** ✓
