# 🔍 DIAGNÓSTICO: Animator No Funciona

## Pasos de Verificación

### 1. **Estructura del GameObject** ✅
```
Player (Cápsula invisible)
├── CharacterController
├── PlayerController (campo Animator debe referenciar al hijo)
├── CapsuleCollider (opcional, deshabilitar si hay duplicado)
└── PlayerModel (Hijo - Modelo visible)
    ├── Animator ← **EL ANIMATOR DEBE ESTAR AQUÍ**
    └── MeshRenderer/SkinnedMeshRenderer
```

### 2. **Verificar Componente Animator**
- [ ] El GameObject `PlayerModel` (hijo) tiene el componente `Animator`
- [ ] En el Inspector del `Player` (padre), el campo `Animator` del `PlayerController` referencia al hijo

### 3. **Verificar Animator Controller**
- [ ] El componente `Animator` tiene asignado un `Animator Controller` (asset .controller)
- [ ] El Animator Controller tiene los 4 estados creados: **Running**, **Jumping**, **Sliding**, **Death**

### 4. **Verificar Parámetros del Animator Controller**
Abre el Animator Controller y verifica que existen estos 4 parámetros:

| Nombre | Tipo | Valor Default |
|--------|------|---------------|
| `isGrounded` | Bool | true |
| `isSliding` | Bool | false |
| `Jump` | Trigger | - |
| `Die` | Trigger | - |

⚠️ **IMPORTANTE**: Los nombres deben ser **EXACTAMENTE** iguales (mayúsculas y minúsculas importan)

### 5. **Verificar Transiciones**
En el Animator Controller, verifica que existan estas transiciones:

```
Running → Jumping: Condición: Jump (Trigger)
Jumping → Running: Condición: isGrounded (true)
Running → Sliding: Condición: isSliding (true)
Sliding → Running: Condición: isSliding (false)
Cualquier Estado → Death: Condición: Die (Trigger)
```

### 6. **Verificar Clips de Animación (Rig Settings)**
Para cada animación:
- [ ] Selecciona el clip de animación en Assets
- [ ] Ve a la pestaña `Rig`
- [ ] **Animation Type**: Generic
- [ ] **Avatar Definition**: None (o Copy From Other Avatar si tienes)
- [ ] Click `Apply`

### 7. **Asignar Animaciones a Estados**
En el Animator Controller:
- [ ] Selecciona el estado `Running` → En Inspector asigna tu clip de correr
- [ ] Selecciona el estado `Jumping` → Asigna tu clip de saltar
- [ ] Selecciona el estado `Sliding` → Asigna tu clip de deslizar
- [ ] Selecciona el estado `Death` → Asigna tu clip de muerte

### 8. **Configuración del Componente Animator**
En el componente `Animator` del `PlayerModel`:
- [ ] **Controller**: Tu Animator Controller asignado
- [ ] **Avatar**: None (debe estar en gris/vacío)
- [ ] **Apply Root Motion**: ❌ Deshabilitado (false)
- [ ] **Update Mode**: Normal
- [ ] **Culling Mode**: Always Animate

---

## 🛠️ Script de Diagnóstico

He creado el script `AnimatorDebugger.cs` para ayudarte a diagnosticar el problema:

### Cómo usar AnimatorDebugger:

1. **Arrastra** `AnimatorDebugger.cs` al GameObject `PlayerModel` (el que tiene el Animator)
2. **Dale Play** al juego
3. **Revisa la Console** - verás mensajes como:
   - ✅ Si todo está bien configurado
   - ❌ Si falta algo (Controller, parámetros, etc.)
   - 🎬 Qué animación se está reproduciendo cada 2 segundos

---

## ❌ Problemas Comunes

### Problema 1: "El Animator está en el padre"
**Solución**: El Animator debe estar en el hijo `PlayerModel`, no en el padre `Player`

### Problema 2: "No hay Animator Controller asignado"
**Solución**: 
1. Crea un Animator Controller: Click derecho en Assets → Create → Animator Controller
2. Asígnalo al componente Animator

### Problema 3: "Los parámetros no existen"
**Solución**: Abre el Animator Controller window y agrega manualmente los 4 parámetros

### Problema 4: "Las animaciones no se asignan a los estados"
**Solución**: 
1. Selecciona cada estado en el Animator window
2. En el Inspector verás "Motion"
3. Arrastra tu clip de animación ahí

### Problema 5: "Las transiciones no funcionan"
**Solución**: Verifica que:
- Las condiciones de transición usen los nombres EXACTOS de los parámetros
- Has añadido al menos una condición a cada transición
- La transición tiene "Exit Time" deshabilitado (excepto Jumping → Running)

### Problema 6: "Apply Root Motion está activado"
**Solución**: Desactívalo en el componente Animator. Esto puede causar que el personaje se mueva solo con la animación.

### Problema 7: "El modelo no se ve"
**Solución**: 
1. Verifica que el MeshRenderer del hijo esté habilitado
2. Verifica que el MeshRenderer del padre (cápsula) esté deshabilitado
3. Asegúrate de que el modelo hijo tenga un material asignado

---

## 🎯 Verificación Rápida en Unity

Ejecuta estos pasos EN ORDEN:

1. **Selecciona el Player padre** → Inspector → PlayerController → Campo "Animator" debe mostrar "PlayerModel (Animator)"
2. **Selecciona el PlayerModel hijo** → Inspector → Componente Animator debe existir
3. **En el Animator del hijo** → Controller debe tener un asset asignado (no "None")
4. **Abre la ventana Animator** (Window → Animation → Animator)
5. **Con PlayerModel seleccionado**, deberías ver tu grafo de estados y transiciones
6. **Dale Play** y observa si los estados cambian de color al reproducirse

---

## 📝 Checklist Final

Antes de probar en Play Mode:

- [ ] Animator está en el GameObject hijo (PlayerModel)
- [ ] PlayerController.animator referencia al hijo
- [ ] Animator Controller está asignado
- [ ] 4 parámetros creados con nombres correctos
- [ ] 4 estados creados (Running, Jumping, Sliding, Death)
- [ ] Animaciones asignadas a cada estado
- [ ] Transiciones creadas con condiciones correctas
- [ ] Apply Root Motion = false
- [ ] Rig de animaciones en Generic

Si todo está ✅, el Animator debería funcionar. Si no, usa `AnimatorDebugger.cs` para ver qué falta.
