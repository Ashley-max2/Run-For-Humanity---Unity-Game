# 🪙 GUÍA DEL NUEVO SISTEMA DE MONEDAS Y OBJETOS

## ✅ CAMBIOS REALIZADOS

### **Sistema Simplificado:**
- ✅ **TrackGenerator** - Solo genera los 4 tipos de tracks dinámicamente
- ✅ **Sistema de monedas con JSON** - Guarda/carga automáticamente
- ✅ **Objetos manuales** - Monedas, obstáculos y power-ups se colocan manualmente en cada prefab de track
- ❌ **NO hay spawneo dinámico de objetos** - Los objetos son parte del prefab

### **Scripts del Sistema:**

1. **CoinDataManager.cs** - Sistema de guardado en JSON
2. **Coin.cs** - Moneda con partículas y guardado automático
3. **CoinUICounter.cs** - Contador visual en UI
4. **Obstacle.cs** - Obstáculo que causa muerte del jugador
5. **SpeedPowerUp.cs** - Power-up de velocidad
6. **ShieldPowerUp.cs** - Power-up de escudo (invencibilidad temporal)
7. **MagnetPowerUp.cs** - Power-up de imán (atrae monedas)
8. **TrackGenerator.cs** - Generador de tracks (sin spawneo de objetos)

---

## 🔧 CONFIGURACIÓN

### **PASO 1: Crear Prefab de Moneda**

1. **Crea un GameObject "Coin":**
   - GameObject → 3D Object → Sphere (o Cylinder)
   - Scale: (0.5, 0.5, 0.5) o el tamaño que quieras

2. **Añade componentes:**
   - Add Component → **Coin** (el script nuevo)
   - El Collider ya viene con el objeto (asegúrate que sea Trigger)

3. **Configura el script Coin:**
   ```
   Coin Value: 1
   Rotation Speed: 100
   Collect Particle Prefab: (opcional - arrastra prefab de partículas)
   Collect Sound: (opcional - arrastra clip de audio)
   Move Towards Player: ✗ (desmarcado, o ✓ para efecto imán)
   ```

4. **Asigna Tag "Player"** al jugador si no lo tiene

5. **Guarda como prefab:** Arrastra el Coin a `Assets/Prefabs/`

---

### **PASO 2: Crear Prefabs de Obstáculos**

**IMPORTANTE:** Los obstáculos MATAN al jugador al tocarlos, y tienen partículas.

1. **Crea GameObjects para obstáculos:**
   - **Obstáculo Alto:** Cube con Position Y = 2, Scale (1, 2, 1)
   - **Obstáculo Bajo:** Cube con Position Y = 0.5, Scale (1, 1, 1)

2. **Añade componentes:**
   - Add Component → **Obstacle**
   - Add Component → Box Collider (is Trigger: ✓)

3. **Configura:**
   ```
   Destroy On Hit: ✗ (se queda para ver)
   Hit Particle Prefab: (arrastra prefab de partículas - se genera en el obstáculo)
   Hit Sound: (opcional)
   ```

4. **IMPORTANTE:** El obstáculo tiene partículas cuando el jugador choca

5. **Guarda como prefabs**

**Solo hay 3 tipos:** Speed, Shield y Magnet (NO DoubleCoins).

1. **Crea GameObjects** (usa diferentes formas/colores):
   - **Speed:** Esfera roja (Position Y = 1)
   - **Shield:** Cubo azul (Position Y = 1)
   - **Magnet:** Cilindro amarillo (Position Y = 1)

2. **Para Speed Power-Up:**
   - Add Component → **SpeedPowerUp**
   - Add Component → Sphere Collider (is Trigger: ✓)
   - Configurar:
     ```
     Speed Boost: 5 (velocidad adicional)
     Duration: 5 (segundos)
     Rotation Speed: 100
     Collect Particle Prefab: (opcional)
     Collect Sound: (opcional)
     ```

3. **Para Shield Power-Up:**
   - Add Component → **ShieldPowerUp**
   - Add Component → Box Collider (is Trigger: ✓)
   - Configurar:
     ```
     Duration: 5 (segundos de invencibilidad)
     Rotation Speed: 100
     Collect Particle Prefab: (opcional)
     Collect Sound: (opcional)
     ```

4. **Para Magnet Power-Up:**
   - Add Component → **MagnetPowerUp**
   - Add Component → Capsule Collider (is Trigger: ✓)
   - Configurar:
     ```
     Duration: 5 (segundos)
     Magnet Range: 10 (rango de atracción)
     Rotation Speed: 100
     Collect Particle Prefab: (opcional)
     Collect Sound: (opcional)
     ```

5. **Guarda los 3cle Prefab: (opcional)
   Collect Sound: (opcional)
   ```

4. **Guarda como prefabs**

---

### **PASO 4: Crear los Prefabs de Track con Objetos**

**IMPORTANTE:** Los objetos (monedas, obstáculos, power-ups) NO se generan dinámicamente. Se colocan manualmente en cada prefab de track.

**Para cada prefab de track (Easy, Medium, Hard, Extreme):**

1. **Abre el prefab** en el editor

2. **El prefab debe contener:**
   - ✅ Suelo (Plane con Collider y Layer "Ground")
   - ✅ Objetos colocados manualmente como hijos:
     - Monedas (con script Coin)
     - Obstáculos (con script Obstacle)
     - Power-ups (con script PowerUp)

3. **Ejemplo de estructura:**
   ```
   TrackChunk_Easy
   ├── Ground (Plane)
   ├── Coin_1 (con script Coin)
   ├── Coin_2
   ├── Coin_3
   ├── Obstacle_Low (con script Obstacle)
   ├── PowerUp_Speed (con script SpeedPowerUp)
   └── (etc...)
   ```

4. **Coloca los objetos donde quieras:**
   - Usa las 3 lanes: X = -3, 0, 3
   - Distribuye a lo largo del track (Z = 0 a 20)
   - Altura Y según el objeto:
     - Monedas: Y = 1
     - Obstáculos bajos: Y = 0.5
     - Obstáculos altos: Y = 2
     - Power-ups: Y = 1

5. **Guarda el prefab**

**Diseña cada nivel con diferente dificultad:**
- **Easy:** Pocas monedas, obstáculos simples y muy separados, ningún power-up
- **Medium:** Más monedas, obstáculos más frecuentes, algún power-up de Speed
- **Hard:** Muchas monedas, obstáculos complejos, power-ups de Shield y Magnet
- **Extreme:** Máxima dificultad, patrones desafiantes, mezcla de power-ups

**IMPORTANTE: El player tiene partículas!**
- Necesitas asignarle 4 sistemas de partículas al Player en Unity:
  - **Run Particles** - Partículas mientras corre (siempre activo)
  - **Jump Particles** - Partículas al saltar
  - **Slide Particles** - Partículas al deslizarse
  - **Death Particle Prefab** - Prefab de partículas al morir
### **PASO 5: Configurar TrackGenerator**

1. **Selecciona el GameObject "TrackGenerator" en Gameplay**

2. **Configura:**
   ```
   Track Prefabs (array de 4):
     Element 0: TrackChunk_Easy
     Element 1: TrackChunk_Medium
     Element 2: TrackChunk_Hard
     Element 3: TrackChunk_Extreme
   
   Track Length: 20 (debe coincidir con la longitud de tus prefabs)
   Initial Segments: 5
   Player Transform: [Arrastra el Player]
   
   Difficulty Progress: 0
   Auto Increase Difficulty: ✓
   Difficulty Increase Rate: 0.01
   
   Safe Segments: 2 (primeros segmentos siempre Easy)
   ```

3. **Listo!** Los tracks se generarán dinámicamente con los objetos que colocaste

---

### **PASO 6: Añadir Contador de Monedas en UI
---

### **PASO 5: Configurar TrackGenerator**

**En la escena Gameplay:**

1. **Crea un Text (TMP):**
   - Click derecho en Canvas → UI → Text - TextMeshPro

2. **Posiciónalo** (ejemplo: esquina superior derecha):
   - Anchor: Top Right
   - Position: (-100, -50)

3. **Añade el componente CoinUICounter:**
   - Add Component → CoinUICounter

4. **Configura:**
   ```
   Coin Text: [Arrastra el TextMeshPro]
   Prefix: "" (vacío) o "💰 "
   Suffix: " Coins"
   Use Thousands Separator: ✓
   Animate On Change: ✓
   Animation Duration: 0.3
   ```

**En MainMenu/Shop (opcional):**
- Repite el mismo proceso para mostrar monedas totales

---

## 🎮 CÓMO FUNCIONA

### **Sistema de Obstáculos:**
1. **El jugador toca un obstáculo** → Se activa Obstacle.OnTriggerEnter()
2. **El obstáculo genera partículas** en el punto de impacto
3. **El obstáculo llama** a PlayerController.Die()
4. **Si el jugador tiene Shield activo** → Se consume el escudo y NO muere
5. **Si NO tiene escudo** → El jugador MUERE con partículas de muerte
6. **Se dispara** EventManager.TriggerGameOver()

### **Sistema de Power-Ups:**

**Speed Power-Up:**
- Aumenta la velocidad del jugador temporalmente
- Se suma a la velocidad base (forwardSpeed += speedBoost)
- Al terminar, se resta automáticamente
- Múltiples power-ups se acumulan

**Shield Power-Up:**
- Otorga invencibilidad temporal
- El jugador NO muere al chocar con obstáculos
- Al recibir un golpe, el escudo se consume (1 uso)
- Visual feedback: TODO (material, partículas, etc.)

**Magnet Power-Up:**
- Las monedas se mueven automáticamente hacia el jugador
- Configurar en Coin.cs: MoveTowardsPlayer = true
- Se activa cuando PlayerController.HasMagnet es true
- Rango configurable (MagnetRange)

### **Sistema de Partículas del Jugador:**
- **Run:** Siempre activo mientras está vivo
- **Jump:** Se activa al saltar (Play())
- **Slide:** Se activa al deslizarse, se detiene al terminar
- **Death:** Prefab instanciado al morir (dura 3 segundos)

### **Sistema de Monedas:**
1. **TrackGenerator spawnea tracks** basados en la dificultad actual
2. **Los tracks contienen objetos** ya colocados manualmente
3. **Cuando el jugador avanza**, los tracks de atrás se reciclan (pooling)
4. **Los objetos se reciclan** junto con el track (se desactivan/activan)

**Diferencia con sistema anterior:**
- ❌ Antes: Spawneo dinámico de objetos desde ObjectPooler
- ✅ Ahora: Objetos son parte del prefab del track, más control manual

### **Flujo de recogida de moneda:**
```
Player toca Coin → 
Partícula aparece → 
Sonido se reproduce → 
+1 moneda en JSON → 
UI se actualiza → 
Moneda desaparece
```

3. **Consultar Monedas desde Código:**
   ```csharp
   int total = CoinDataManager.GetTotalCoins();
   CoinDataManager.AddCoins(10);
   bool success = CoinDataManager.SpendCoins(50);
   ```

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### **No se recogen las monedas:**
- Verifica que el jugador tenga Tag "Player"
- Asegúrate que la moneda tenga Collider con is Trigger activado
- Revisa la consola para ver los logs "[Coin] ¡Moneda recogida!"

### **El contador no se actualiza:**
- Verifica que CoinUICounter tenga la referencia al TextMeshPro
- Llama a `UpdateDisplay()` manualmente si es necesario
- Revisa que CoinDataManager esté cargando los datos

### **No aparecen objetos en los tracks:**
- Los objetos deben estar colocados **manualmente** en cada prefab de track
- Abre el prefab y verifica que los objetos estén ahí
- Asegúrate de guardar el prefab después de añadir objetos

### **El jugador no muere al chocar:**
- Verifica que el obstáculo tenga el script **Obstacle** asignado
- Verifica que el collider del obstáculo sea **is Trigger = true**
- Asegúrate que el jugador tenga Tag "Player"
- Revisa la consola para ver "[Obstacle] ¡Jugador chocó con obstáculo! - MUERTE"

### **El escudo no funciona:**
- El Shield se consume en el primer golpe (1 uso)
- Verifica que el power-up sea de tipo ShieldPowerUp
- Revisa la consola: "[Player] Shield applied for Xs"
- Cuando se usa: "[Player] ¡Salvado por el escudo!"

### **El speed boost no se aplica:**
- Verifica que SpeedPowerUp tenga Speed Boost > 0
- Revisa la consola: "[Player] Speed boost applied: +X"
- Cuando termina: "[Player] Speed boost ended"

### **Las partículas no aparecen:**
- Asegúrate de asignar los ParticleSystems en el Inspector del Player
- Para Death Particles, arrastra un **prefab** (no un ParticleSystem en escena)
- Las partículas de Run deben estar en **Play On Awake = false**
### **El juego crashea al guardar:**
- Verifica que Application.persistentDataPath sea accesible
- Revisa permisos de escritura en tu sistema

---

## 📝 USAR MONEDAS EN LA TIENDA

Para usar las monedas en tu shop:

```csharp
// En ShopUIBuilder o tu script de tienda:
public void BuyItem(int price)
{
    if (CoinDataManager.SpendCoins(price))
    {
        Debug.Log("¡Compra exitosa!");
        // Desbloquear item
    }
    else
    {
        Debug.Log("No tienes suficientes monedas");
        // Mostrar mensaje de error
    }
    
    // Actualizar UI
    FindObjectOfType<CoinUICounter>()?.UpdateDisplay();
}
```

---

## 🎯 TESTING

### **Resetear Monedas (para pruebas):**
```csharp
// Añade esto a un botón de debug o en la consola de Unity:
CoinDataManager.ResetCoins();
```

### **Añadir Monedas Manualmente:**
```csharp
// Para testing:
CoinDataManager.AddCoins(1000);
```

### **Ver dónde se guarda:**
```csharp
Debug.Log(Application.persistentDataPath);
// Windows: C:/Users/USER/AppData/LocalLow/CompanyName/GameName/
// El archivo será: coinDa, obstáculos y power-ups
- [ ] Partículas más elaboradas para cada acción
- [ ] Efectos visuales para Shield activo (material brillante, aura)
- [ ] Efectos visuales para Magnet activo (campo magnético)
- [ ] Añadir animaciones a las monedas (bounce, wobble)
- [ ] Sistema de combo/multiplicador
- [ ] Achievements relacionados con monedas
- [ ] Diferentes valores de monedas (bronce=1, plata=5, oro=10)
- [ ] Sistema de revivals (usar monedas para revivir
- [ ] Sonidos para monedas y power-ups
- [ ] Partículas más elaboradas
- [ ] Implementar completamente los power-ups (Shield, DoubleCoins)
- [ ] Añadir animaciones a las monedas
- [ ] Sistema de combo/multiplicador
- [ ] Achievements relacionados con monedas
- [ ] Diferentes valores de monedas (bronce, plata, oro)
