# 🎮 GUÍA DE CONFIGURACIÓN DEL GAMEPLAY

## ✅ PROBLEMAS SOLUCIONADOS

### 1. **Física del Jugador Mejorada**
- ✓ Saltos más controlados (ajustable)
- ✓ No atraviesa el suelo (ground check mejorado)
- ✓ Sistema de deslizamiento funcional
- ✓ Gravedad ajustada

### 2. **Sistema de Track con ObjectPooler**
- ✓ Los tracks ahora se reciclan (mejor performance)
- ✓ Sistema de dificultad progresiva
- ✓ Spawneo automático de objetos

### 3. **Nuevos Scripts Creados**
- `TrackSegment.cs` - Define puntos de spawn en cada track
- Mejoras en `PlayerController.cs` - Física y slide
- Mejoras en `TrackGenerator.cs` - Pooling y dificultad

---

## 🔧 CONFIGURACIÓN EN UNITY

### **PASO 1: Configurar el Player**

1. **Selecciona el GameObject del Player en Gameplay**
2. **En PlayerController, configura:**
   ```
   Forward Speed: 10-15
   Lane Change Speed: 15
   Jump Force: 12
   Gravity: -25
   Ground Check Distance: 0.3
   Ground Layer: Selecciona "Ground" o "Default"
   
   Slide Duration: 0.8
   Slide Speed Multiplier: 1.5
   ```

3. **CharacterController (componente):**
   ```
   Height: 2
   Radius: 0.5
   Center: (0, 1, 0)
   ```

4. **Asegúrate que el Player tenga:**
   - Tag: "Player"
   - Collider para detección

---

### **PASO 2: Configurar el Layer "Ground"**

**IMPORTANTE:** El suelo NO es un objeto estático. El suelo forma parte de cada prefab de track que se genera automáticamente.

1. **Crea el Layer "Ground":**
   - Ve a: Edit → Project Settings → Tags and Layers
   - En "Layers", crea un nuevo Layer llamado "Ground"

2. **Asigna el Layer a los prefabs de track:**
   - Abre cada prefab de track (TrackChunk_Easy, Medium, Hard, Extreme)
   - Selecciona el mesh/GameObject del suelo dentro del prefab
   - En el Inspector, cambia el Layer a "Ground"
   - Asegúrate que tenga un Collider (MeshCollider o BoxCollider)
   - Guarda el prefab

**¿Por qué?** Como el juego es infinito, el suelo se genera y recicla junto con los tracks. No necesitas un plano estático en la escena.

---

### **PASO 3: Configurar el ObjectPooler**

1. **Crea un GameObject vacío: "ObjectPooler"**

2. **Agrégale el componente ObjectPooler**

3. **Configura los pools (en el Inspector):**

   **Pool 1 - Monedas:**
   ```
   Tag: Coin
   Prefab: [Arrastra Prefabs/ObjectPooler/Coin]
   Size: 50
   ```

   **Pool 2 - Obstáculo Alto:**
   ```
   Tag: ObstacleHigh
   Prefab: [Arrastra Prefabs/ObjectPooler/ObstacleHigh]
   Size: 20
   ```

   **Pool 3 - Obstáculo Bajo:**
   ```
   Tag: ObstacleLow
   Prefab: [Arrastra Prefabs/ObjectPooler/ObstacleLow]
   Size: 20
   ```

   **Pool 4 - PowerUp Speed:**
   ```
   Tag: PowerUpSpeed
   Prefab: [Arrastra Prefabs/ObjectPooler/PowerUpSpeed]
   Size: 10
   ```

   **Pool 5 - PowerUp Shield:**
   ```
   Tag: PowerUpShield
   Prefab: [Arrastra Prefabs/ObjectPooler/PowerUpShield]
   Size: 10
   ```

   **Pool 6 - PowerUp Magnet:**
   ```
   Tag: PowerUpMagnet
   Prefab: [Arrastra Prefabs/ObjectPooler/PowerUpMagnet]
   Size: 10
   ```

---

### **PASO 4: Configurar los Track Prefabs**

**¿Qué debe contener cada prefab de track?**
- ✅ El suelo/ground con Collider y Layer "Ground"
- ✅ El script **TrackSegment.cs** (como componente en el GameObject raíz)
- ❌ NO incluir obstáculos, monedas o power-ups (se spawnean automáticamente)

**Configuración paso a paso:**

Para cada prefab de track (TrackChunk_Easy, Medium, Hard, Extreme):

1. **Abre el prefab**

2. **Verifica que tenga el suelo:**
   - Debe tener un mesh visible del suelo
   - Con Collider (MeshCollider o BoxCollider)
   - Layer asignado a "Ground"
   
3. **Selecciona el GameObject raíz del prefab**

4. **Add Component → Busca "TrackSegment"**
   - Este es el script que gestiona el spawneo de objetos

5. **Configura el componente TrackSegment:**
   ```
   Length: 20 (debe coincidir con trackLength del TrackGenerator)
   Auto Generate Spawns: ✓ (marca esta casilla)
   Coins Per Lane: 5
   Obstacles Per Segment: 2
   Power Ups Per Segment: 1
   ```
   
6. **Guarda el prefab (Ctrl+S)**

**RESUMEN:** Cada prefab de track = Suelo + Script TrackSegment
- El **suelo** es la geometría visible con collider
- El **script TrackSegment** calcula dónde spawnear los objetos del ObjectPooler

---

### **PASO 5: Configurar el TrackGenerator**

1. **Selecciona el GameObject "TrackGenerator" en Gameplay**

2. **Configura:**
   ```
   Track Prefabs (array de 4):
     Element 0: TrackChunk_Easy
     Element 1: TrackChunk_Medium
     Element 2: TrackChunk_Hard
     Element 3: TrackChunk_Extreme
   
   Track Length: 20
   Initial Segments: 5
   Player Transform: [Arrastra el Player]
   
   Difficulty Progress: 0
   Auto Increase Difficulty: ✓
   Difficulty Increase Rate: 0.01
   
   Safe Segments: 2
   ```

---

## 🎮 CÓMO FUNCIONA

### **Sistema de Tracks:**
1. El TrackGenerator spawnea tracks basados en la dificultad actual
2. Cuando el jugador avanza, los tracks de atrás se reciclan (pooling)
3. Cada track tiene un componente TrackSegment

### **Sistema de Spawns:**
1. Cada TrackSegment genera puntos de spawn automáticamente
2. Cuando el track se activa, spawnea objetos desde el ObjectPooler
3. Cuando el track se recicla, devuelve los objetos al pool

### **Dificultad Progresiva:**
- Empieza en 0 (fácil)
- Aumenta automáticamente con el tiempo
- A mayor dificultad, más tracks difíciles aparecen
- Los primeros 2 segmentos siempre son fáciles (safe zone)

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### **El jugador salta muy alto:**
- Reduce `Jump Force` (prueba con 10-12)
- Aumenta `Gravity` (prueba con -30)

### **El jugador atraviesa el suelo:**
- Verifica que el suelo tenga un Collider
- Asigna correctamente el `Ground Layer` en el PlayerController
- Asegúrate que el CharacterController tenga Skin Width > 0.08

### **El slide no funciona:**
- Verifica que DOTween esté instalado
- Revisa que `Slide Duration` sea > 0
- Asegúrate de presionar S o Flecha Abajo

### **No aparecen objetos en los tracks:**
- Verifica que ObjectPooler esté en la escena
- Revisa que los tags en ObjectPooler coincidan exactamente
- Asegúrate que los prefabs de track tengan el componente TrackSegment
- Revisa la consola para errores

### **Los tracks no se generan:**
- Asigna los prefabs de track en TrackGenerator
- Verifica que Player Transform esté asignado
- Asegúrate que trackLength coincida en TrackGenerator y TrackSegment

---

## 🎯 AJUSTES RECOMENDADOS

### **Para Gameplay Más Fácil:**
```
Player:
  Forward Speed: 8
  Jump Force: 10
  
TrackGenerator:
  Difficulty Increase Rate: 0.005
  Safe Segments: 3
```

### **Para Gameplay Más Difícil:**
```
Player:
  Forward Speed: 15
  Jump Force: 14
  
TrackGenerator:
  Difficulty Increase Rate: 0.02
  Safe Segments: 1
```

---

## 📝 NOTAS IMPORTANTES

1. **Los prefabs del ObjectPooler NO se colocan manualmente en los tracks**
   - Se spawnean automáticamente desde el pool
   - Los tracks solo definen DÓNDE spawnear

2. **El sistema de lanes:**
   - Lane -1 = Izquierda
   - Lane 0 = Centro
   - Lane 1 = Derecha
   - Distancia entre lanes: 3 unidades

3. **Los objetos spawneados se reciclan automáticamente**
   - Cuando un track se desactiva, sus objetos vuelven al pool
   - Esto mejora el performance significativamente

4. **Para probar:**
   - Play Mode
   - Usa WASD o Flechas para moverte
   - Space para saltar
   - S o Flecha Abajo para deslizarse

---

## ✨ PRÓXIMOS PASOS OPCIONALES

- Añadir animaciones al jugador
- Crear más tipos de obstáculos
- Implementar sistema de vidas
- Añadir efectos visuales (partículas)
- Implementar sistema de puntuación
- Añadir sonidos
