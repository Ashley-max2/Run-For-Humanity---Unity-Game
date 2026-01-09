# 🏃 CONFIGURACIÓN ESTILO SUBWAY SURFERS

## ✅ CAMBIOS IMPLEMENTADOS

### **Movimiento del Jugador:**
- ✅ Cambio de carril **suave** con interpolación
- ✅ Velocidad **aumenta automáticamente** con el tiempo (10 → 30)
- ✅ Salto con animación squash & stretch
- ✅ Slide con aplastamiento visual (ScaleY 0.4, ScaleZ 1.3)
- ✅ Inclinación al cambiar de carril (±15°)

### **Cámara:**
- ✅ Cámara en tercera persona con offset fijo
- ✅ Smooth follow para movimientos suaves
- ✅ Inclinación al detectar movimiento lateral
- ✅ Look at point configurable
- ✅ **Transparencia automática** para objetos que bloquean la vista

---

## 🔧 CONFIGURACIÓN EN UNITY

### **PASO 1: Configurar el Player**

1. **Selecciona el GameObject "Player" en la escena Gameplay**

2. **En PlayerController, configura:**
   ```
   === MOVEMENT SETTINGS ===
   Forward Speed: 10
   Speed Increase Rate: 0.1 (velocidad aumenta cada segundo)
   Max Speed: 30
   Lane Change Speed: 10 (velocidad del cambio de carril suave)
   Jump Force: 2 (valores recomendados: 1.5 - 2)
   Gravity: -9.81 (gravedad realista)
   Ground Check Distance: 0.3
   Ground Layer: Ground
   
   === SLIDE SETTINGS ===
   Slide Duration: 1
   
   === LANE CHANGE SETTINGS ===
   Lane Change Duration: 0.2 (animación de inclinación)
   
   === PARTICLE EFFECTS ===
   Run Particles: [ParticleSystem]
   Jump Particles: [ParticleSystem]
   Slide Particles: [ParticleSystem]
   Death Particle Prefab: [Prefab]
   ```

3. **IMPORTANTE:** El jugador ahora cambia de carril instantáneamente (no hay movimiento lateral suave)

---

### **PASO 2: Configurar la Cámara**

1. **Selecciona el GameObject "Main Camera"**

2. **Añade el componente:**
   - Add Component → **SubwaySurfersCamera**

3. **Configura:**
   ```
   === TARGET ===
   Target: [Arrastra el Player]
   
   === CAMERA POSITION ===
   Offset: X=0, Y=3, Z=-6 (posición relativa al jugador)
   Height: 2
   Distance: 6
   
   === SMOOTHING ===
   Position Smooth Speed: 10 (suavizado de posición)
   Rotation Smooth Speed: 5 (suavizado de rotación)
   
   === LOOK AT ===
   Look At Offset: X=0, Y=1, Z=2 (punto donde mira)
   
   === TILT ===
   Max Tilt Angle: 5 (inclinación al moverse lateralmente)
   Tilt Speed: 3
   
   === TRANSPARENCY ===
   Enable Transparency: ✓ (activar sistema de transparencia)
   Transparency Alpha: 0.3 (nivel de transparencia, 0=invisible, 1=opaco)
   Obstacle Layer Mask: Everything (o selecciona capas específicas)
   ```

4. **Ajusta valores según tu preferencia:**
   - **Offset Z más negativo** = Cámara más lejana
   - **Offset Y más alto** = Cámara más elevada
   - **Look At Offset Z positivo** = Mira más adelante del jugador

---

## 🎮 DIFERENCIAS CON EL SISTEMA ANTERIOR

### **MovimieActual (Subway Surfers) |
|----------|----------------|
| Cambio de carril suave con interpolación | **Cambio de carril suave** (restaurado)
| Cambio de carril suave con interpolación | **Cambio instantáneo** |
| Velocidad constante | **Velocidad aumenta progresivamente** |
| Slide aumenta velocidad | **Slide solo cambia altura** |
| Sin animación de inclinación | **Inclinación ±15° al cambiar carril** |

### **Cámara:**
| Objetos bloquean la vista | **Transparencia automática** |
| Anterior | Subway Surfers |
|----------|----------------|
| Posición fija o básica | **Smooth follow con offset** |
| Sin inclinación dinámica | **Tilt basado en movimiento lateral** |
| Rotación estática | **Look at point adelante del jugador** |

---

## 🎯 AJUSTES RECOMENDADOS

### **Para hacer el juego más fácil:**
```
Speed Increase Rate: 0.05 (aumenta más lento)
Max Speed: 20 (velocidad máxima menor)
Jump Force: 2.5 (saltos más altos)
```

### **Para hacer el juego más difícil:**
```
Speed Increase Rate: 0.2 (aumenta más rápido)
Max Speed: 40 (velocidad máxima mayor)
Jump Force: 1.5 (saltos más bajos)
```

### **Para cámara más cercana (estilo móvil):**
```
Offset Z: -4
Offset Y: 2
Look At Offset Z: 1
```

### **Para cámara más lejana (mejor visibilidad):**
```
Offset Z: -8
Offset Y: 4
Look At Offset Z: 3
```

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### **El jugador no cambia de carril:**
- Verifica que LaneSystem.GetXPosition() devuelva -3, 0, 3
- Asegúrate de que currentLane esté cambiando (añade Debug.Log)
- El CharacterController debe estar habilitado
- Presiona las teclas correctamente (A/D o flechas izquierda/derecha)

### **El jugador se mueve demasiado rápido:**
- Reduce Speed Increase Rate
- Reduce Max Speed
- Verifica que Time.timeScale esté en 1

### **La cámara no sigue al jugador:**
- Asegúrate de asignar el Target en SubwaySurfersCamera
- Verifica que el Player tenga Tag "Player"
- Aumenta Position Smooth Speed si es muy lenta

### **La cámara se mueve bruscamente:**
- Aumenta Position Smooth Speed (más suave)
- Aumenta Rotation Smooth Speed (más suave)

### **Los objetos no se vuelven transparentes:**
- Verifica que Enable Transparency esté marcado
- Asegúrate que Obstacle Layer Mask incluya la capa de los objetos
- Los objetos deben tener Renderer (MeshRenderer, SkinnedMeshRenderer, etc.)
- Prueba con Transparency Alpha = 0.1 (más transparente)

### **Los objetos quedan transparentes permanentemente:**
- Esto se corrige automáticamente cuando dejan de bloquear
- Si persiste, reinicia la escena
- Verifica que el objeto tenga collider para el raycast
- Verifica que LateUpdate() se esté ejecutando
 o va muy alto:**
- **IMPORTANTE:** Jump Force debe estar entre 1.5 y 2
- Gravity debe estar en -9.81 (realista)
- Si sigue yendo muy alto, reduce Jump Force a 1.5
- Si es muy bajo, aumenta a 2.5
- Ajusta Grde carril suave con interpolación
- ✅ Velocidad progresiva
- ✅ Animaciones squash & stretch
- ✅ Inclinación al cambiar de carril
- ✅ Cámara smooth follow
- ✅ Tilt de cámara en movimiento lateral
- ✅ **Transparencia automática de obstáculos visuales**ht se esté modificando

---

## 💡 CARACTERÍSTICAS SUBWAY SURFERS

### **Implementado:**
- ✅ Cambio instantáneo de carril
- ✅ Velocidad progresiva
- ✅ Animaciones squash & stretch
- ✅ Inclinación al cambiar de carril
- ✅ Cámara smooth follow
- ✅ Tilt de cámara en movimiento lateral

### **Opcional (puedes añadir):**
- [ ] Double jump (saltar mientras está en el aire)
- [ ] Jetpack power-up
- [ ] Hoverboard que salva de obstáculos
- [ ] Combo system (multiplicador de monedas)
- [ ] Misiones diarias
- [ ] Score basado en distancia

---

## 📋 CONTROLES

**Teclado:**
- **A / ←** - Mover a carril izquierdo
- **D / →** - Mover a carril derecho
- **W / ↑ / Space** - Saltar
- **S / ↓** - Deslizarse (slide)

**Para añadir controles táctiles:**
1. Detectar swipe en Input
2. Llamar a PlayerController.MoveLeft(), MoveRight(), Jump(), Slide()

---

## 🎨 MEJORAS VISUALES OPCIONALES

### **Trail Effect (estela al moverse):**
- Añade Trail Renderer al Player
- Configura Width y Color
- Ajusta Time para la duración

### **Speed Lines:**
- Crea ParticleSystem con líneas horizontales
- Actívalo cuando forwardSpeed > 20
- Parent al Player

### **FOV Dinámico (Field of View):**
```csharp
// Añadir a SubwaySurfersCamera:
float baseFOV = 60f;
float maxFOV = 75f;
Camera.main.fieldOfView = Mathf.Lerp(baseFOV, maxFOV, playerSpeed / maxSpeed);
```

### **Motion Blur:**
- Post Processing Volume
- Motion Blur effect
- Intensity aumenta con velocidad
