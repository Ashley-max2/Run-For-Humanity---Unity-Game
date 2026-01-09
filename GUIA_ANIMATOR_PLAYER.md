# 🎬 CONFIGURACIÓN DEL ANIMATOR CONTROLLER DEL PLAYER

## 📋 ANIMACIONES DISPONIBLES

Tienes 4 animaciones:
1. **Running** - Corriendo (estado por defecto)
2. **Jumping** - Saltando
3. **Sliding** - Deslizándose
4. **Death** - Chocando/muriendo

---

## 🔧 PASO 1: CREAR EL ANIMATOR CONTROLLER

1. **En Unity:**
   - Click derecho en `Assets/Animations` (o crea la carpeta)
   - Create → Animator Controller
   - Nombre: `PlayerAnimatorController`

---

## 🎯 PASO 2: CONFIGURAR ESTADOS Y TRANSICIONES

### **A. Añadir los estados:**

1. **Abre el Animator Controller** (doble click)

2. **Elimina el estado "Entry"** que viene por defecto

3. **Crea los 4 estados:**
   - Click derecho → Create State → Empty
   - Nombres: `Running`, `Jumping`, `Sliding`, `Death`

4. **Asigna las animaciones:**
   - Selecciona cada estado
   - En Inspector → Motion → Arrastra tu clip de animación

5. **Marca Running como Default:**
   - Click derecho en `Running` → Set as Layer Default State
   - Debe volverse naranja

---

### **B. Crear Parámetros:**

En la pestaña "Parameters" (izquierda arriba):

1. **Añade estos parámetros:**
   ```
   [+] → Bool → "isGrounded" (default: true)
   [+] → Bool → "isSliding" (default: false)
   [+] → Trigger → "Jump"
   [+] → Trigger → "Die"
   ```

---

### **C. Configurar Transiciones:**

#### **1. Running → Jumping:**
- Click derecho en `Running` → Make Transition → `Jumping`
- En Inspector:
  ```
  Has Exit Time: ✗ (desmarcar)
  Transition Duration: 0.1
  
  Conditions:
  - Jump (Trigger)
  ```

#### **2. Jumping → Running:**
- `Jumping` → Make Transition → `Running`
- En Inspector:
  ```
  Has Exit Time: ✗
  Transition Duration: 0.2
  
  Conditions:
  - isGrounded = true
  ```

#### **3. Running → Sliding:**
- `Running` → Make Transition → `Sliding`
- En Inspector:
  ```
  Has Exit Time: ✗
  Transition Duration: 0.1
  
  Conditions:
  - isSliding = true
  ```

#### **4. Sliding → Running:**
- `Sliding` → Make Transition → `Running`
- En Inspector:
  ```
  Has Exit Time: ✗
  Transition Duration: 0.15
  
  Conditions:
  - isSliding = false
  ```

#### **5. ANY STATE → Death:**
- Click derecho en `Any State` → Make Transition → `Death`
- En Inspector:
  ```
  Has Exit Time: ✗
  Transition Duration: 0.1
  
  Conditions:
  - Die (Trigger)
  ```

**IMPORTANTE:** NO crear transición desde Death a ningún otro estado (es el final)

---

## 💻 PASO 3: AÑADIR CÓDIGO AL PLAYERCONTROLLER

Añade esto al inicio de la clase PlayerController:

```csharp
[Header("Animation")]
[SerializeField] private Animator animator;
```

Luego añade este método al PlayerController:

```csharp
void UpdateAnimations()
{
    if (animator == null) return;
    
    // Actualizar parámetros del Animator
    animator.SetBool("isGrounded", isGrounded);
    animator.SetBool("isSliding", isSliding);
}
```

Llama a `UpdateAnimations()` al final de `Update()`:

```csharp
void Update()
{
    if (isDead) return;

    HandleSlideTimer();
    HandlePowerUpTimers();
    HandleSpeedIncrease();
    HandleInput();
    MovePlayer();
    UpdateAnimations(); // ← AÑADIR ESTO
}
```

Modifica el método `Jump()` para activar el trigger:

```csharp
public void Jump()
{
    if (isGrounded && !isSliding)
    {
        verticalVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        EventManager.TriggerPlayerJump();
        
        // AÑADIR: Activar animación
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
        
        // Animación de salto estilo Subway Surfers (squash and stretch)
        transform.DOScaleY(1.2f, 0.1f).OnComplete(() => {
            transform.DOScaleY(0.8f, 0.2f).OnComplete(() => {
                transform.DOScaleY(1f, 0.1f);
            });
        });
        
        // Partículas de salto
        if (jumpParticles != null)
        {
            jumpParticles.Play();
        }
        
        Debug.Log($"[Player] Jump! Velocity: {verticalVelocity.y}");
    }
}
```

Modifica el método `Die()` para activar el trigger:

```csharp
public void Die()
{
    if (isDead) return; // Ya está muerto
    
    // Si tiene escudo, no muere
    if (hasShield)
    {
        Debug.Log("[Player] ¡Salvado por el escudo!");
        hasShield = false; // Consumir escudo
        shieldTimer = 0f;
        return;
    }
    
    isDead = true;
    Debug.Log("[Player] ¡Jugador MUERE!");
    
    // AÑADIR: Activar animación
    if (animator != null)
    {
        animator.SetTrigger("Die");
    }
    
    // Detener partículas de movimiento
    if (runParticles != null) runParticles.Stop();
    if (slideParticles != null) slideParticles.Stop();
    
    // Crear partículas de muerte
    if (deathParticlePrefab != null)
    {
        GameObject particles = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        Destroy(particles, 3f);
    }
    
    EventManager.TriggerGameOver();
    
    // Animación de muerte
    transform.DOShakeScale(1f, 1f);
}
```

---

## 🎮 PASO 4: CONFIGURAR EN UNITY

### **¿Tu juego es 2D o 3D?**

#### **SI ES 2D (Sprites):**

1. **Verifica que el Player tenga SpriteRenderer:**
   - El Player debe tener componente `SpriteRenderer`
   - Asigna el sprite del personaje

2. **Añade el componente Animator:**
   - Add Component → Animator
   
3. **Configura el Animator:**
   ```
   Controller: [Arrastra PlayerAnimatorController]
   Avatar: NONE (déjalo en None para 2D)
   Apply Root Motion: ✗
   Update Mode: Normal
   Culling Mode: Always Animate
   ```

4. **En PlayerController:**
   - Animator: Arrastra el componente Animator del Player

5. **IMPORTANTE - Tus animaciones deben ser de tipo Sprite:**
   - Las animaciones deben animar el SpriteRenderer.sprite
   - No funcionan animaciones 3D con sprites 2D

---

#### **SI ES 3D (Modelo):**

1. **Verifica que el Player tenga los componentes correctos:**
   - El modelo debe tener `SkinnedMeshRenderer` o `MeshRenderer`
   - Debe ser visible en la Scene view

2. **Configura el modelo (si es humanoid):**
   - Selecciona tu modelo en Assets
   - Inspector → Rig → Animation Type: `Humanoid`
   - Apply
   - Se creará un Avatar automáticamente

3. **Añade el componente Animator:**
   - Add Component → Animator
   
4. **Configura el Animator:**
   ```
   Controller: [Arrastra PlayerAnimatorController]
   Avatar: [Arrastra el Avatar del modelo]
   Apply Root Motion: ✗
   Update Mode: Normal
   Culling Mode: Always Animate
   ```

5. **En PlayerController:**
   - Animator: Arrastra el componente Animator del Player

---

### **DIAGNÓSTICO: ¿Por qué no veo el personaje?**

#### **Problema: No veo el personaje en Play Mode**

**Para 2D:**
- [ ] ¿Tiene SpriteRenderer con un sprite asignado?
- [ ] ¿El sprite tiene un material? (debería ser Sprites-Default)
- [ ] ¿La cámara está en Z = -10 y el Player en Z = 0?
- [ ] ¿El Sorting Layer y Order in Layer están correctos?

**Para 3D:**
- [ ] ¿Tiene MeshRenderer o SkinnedMeshRenderer?
- [ ] ¿Los materiales están asignados?
- [ ] ¿La escala es correcta? (no demasiado pequeño)
- [ ] ¿La cámara apunta al Player?
- [ ] ¿Las capas (Layers) están configuradas correctamente?

#### **Problema: Las animaciones no se ven**

- [ ] ¿El Animator está habilitado? (checkbox marcado)
- [ ] ¿El Animator Controller está asignado?
- [ ] ¿Las animaciones están asignadas en cada estado del Animator?
- [ ] ¿Las animaciones son del tipo correcto? (2D: Sprite Animation, 3D: Model Animation)
- [ ] Abre el Animator window y observa si los estados cambian en Play Mode

---

## ⚙️ CONFIGURACIONES RECOMENDADAS

### **Para animaciones más fluidas:**

En cada transición:
```
Transition Duration: 0.1 - 0.2
Has Exit Time: ✗ (desmarcado)
Fixed Duration: ✓ (marcado)
```

### **Para que la animación de muerte dure más:**

En el estado `Death`:
```
Speed: 0.5 (más lenta)
Loop Time: ✗ (no se repite)
```

### **Para que Running se mezcle suavemente:**

En el estado `Running`:
```
Speed: 1.2 (opcional, para que corra más rápido visualmente)
Loop Time: ✓ (se repite continuamente)
```

---

## 🐛 SOLUCIÓN DE PROBLEMAS

### **No veo el personaje en Play Mode:**

**SI ES 2D:**
1. Verifica que tenga `SpriteRenderer` con sprite asignado
2. Asegúrate que la cámara Main Camera tenga:
   - Projection: Orthographic
   - Size: 5 (o ajusta según tu escena)
3. El Player debe estar en Z = 0 (o cerca)
4. La cámara debe estar en Z = -10
5. Verifica el Sorting Layer y Order in Layer

**SI ES 3D:**
1. Verifica que tenga `MeshRenderer` o `SkinnedMeshRenderer`
2. Verifica que los materiales estén asignados
3. Asegúrate que la escala sea correcta (Scale: 1, 1, 1 o mayor)
4. Verifica que la cámara SubwaySurfersCamera esté configurada
5. Mira la Scene view - ¿lo ves ahí?

### **El personaje se ve pero no se anima:**

1. **Verifica el Animator:**
   - Debe estar **habilitado** (checkbox marcado)
   - Controller debe estar asignado
   - En Play Mode, abre Window → Animation → Animator
   - Observa si los estados cambian de color

2. **Verifica las animaciones:**
   - Abre el Animator Controller
   - Cada estado debe tener una animación asignada
   - Las animaciones 2D deben animar `SpriteRenderer.sprite`
   - Las animaciones 3D deben animar los bones/transforms

3. **Verifica los parámetros:**
   - En Animator window, pestaña Parameters
   - Observa si los valores cambian en Play Mode
   - Añade Debug.Log en UpdateAnimations() para verificar

### **Avatar no se puede asignar (aparece "None"):**

**Esto es NORMAL para 2D** - Los sprites no necesitan avatar

**Para 3D:**
1. Selecciona tu modelo en Assets (no en Scene)
2. Inspector → Rig tab
3. Animation Type: `Humanoid` o `Generic`
4. Apply
5. Se generará un Avatar
6. Ahora puedes asignarlo en el Animator

### **Las animaciones son del tipo incorrecto:**

**2D - Necesitas Sprite Animations:**
- Create → Animation
- Añade keyframes para SpriteRenderer.sprite
- Cambia sprites frame por frame

**3D - Necesitas Model Animations:**
- Importa animaciones de Mixamo, Asset Store, etc.
- O crea animaciones moviendo bones/transforms

### **La animación de salto no se activa:**
- Verifica que el parámetro "Jump" sea un **Trigger**, no un Bool
- Asegúrate que `animator.SetTrigger("Jump")` se esté llamando
- Revisa que la transición Running → Jumping tenga "Jump" como condición

### **El personaje se queda en animación de sliding:**
- Verifica que `isSliding` se esté actualizando correctamente
- Asegúrate que `EndSlide()` se llame cuando termine el slide
- Revisa la transición Sliding → Running

### **La animación de muerte no funciona:**
- Verifica que "Die" sea un **Trigger**
- Asegúrate que NO haya transición desde Death a otros estados
- La animación Death debe tener Loop Time desactivado

### **Las transiciones son muy bruscas:**
- Aumenta el Transition Duration (0.2 - 0.3)
- Activa "Fixed Duration"
- En estados de loop, activa "Foot IK" si tienes IK configurado

---

## 📊 DIAGRAMA DE ESTADOS

```
┌──────────┐      Jump      ┌──────────┐
│          │───────────────→│          │
│ Running  │                │ Jumping  │
│ (START)  │←───────────────│          │
│          │   isGrounded   └──────────┘
└────┬─────┘
     │
     │ isSliding=true
     ↓
┌──────────┐
│          │
│ Sliding  │
│          │
└────┬─────┘
     │
### **Para 2D:**
```
=== PLAYER COMPONENTS ===
- SpriteRenderer (con sprite asignado)
- Animator
- CharacterController (para movimiento)
- PlayerController (tu script)

=== ANIMATOR SETTINGS ===
Controller: PlayerAnimatorController
Avatar: None (no necesario para 2D)
Apply Root Motion: ✗
Update Mode: Normal
Culling Mode: Always Animate

=== ANIMACIONES 2D ===
- Deben animar: SpriteRenderer.sprite
- Frame rate: 12-24 fps típicamente
- Crear con: Create → Animation
```

### **Para 3D:**
```
=== PLAYER COMPONENTS ===
- SkinnedMeshRenderer o MeshRenderer
- Animator
- CharacterController (para movimiento)  
- PlayerController (tu script)

=== ANIMATOR SETTINGS ===
Controller: PlayerAnimatorController
Avatar: [Tu Avatar del modelo]
Apply Root Motion: ✗
Update Mode: Normal
Culling Mode: Always Animate

=== ANIMACIONES 3D ===
- Importadas de: Mixamo, Asset Store, etc.
- Animation Type: Humanoid o Generic
- Rig configurado en el modelo
```

### **Transiciones (ambos casos):**
```
Has Exit Time: ✗ (la mayoría)
Transition Duration: 0.1 - 0.2
Fixed Duration: ✓
```

### **Estados (ambos casos):**
```
---

## ✨ MEJORAS OPCIONALES

### **1. Blend Tree para Running (velocidad variable):**
- En vez de un estado Running simple, usa un Blend Tree
- Parámetro: Speed (float)
- Mezcla entre animación idle (speed=0) y running (speed=1)

### **2. Animación de cambio de carril:**
- Añade animaciones de "MoveLeft" y "MoveRight"
- Triggers: "LaneLeft", "LaneRight"
- Transiciones desde Running

### **3. Diferentes animaciones de salto:**
- Jump Up (subiendo)
- Jump Peak (en el aire)
- Jump Down (cayendo)
- Usa parámetro float "VerticalVelocity"

### **4. Animaciones de power-ups:**
- Shield Activate
- Speed Boost
- Magnet Active

---

## 📝 CHECKLIST FINAL

- [ ] Animator Controller creado
- [ ] 4 estados creados (Running, Jumping, Sliding, Death)
- [ ] 4 parámetros añadidos (isGrounded, isSliding, Jump, Die)
- [ ] Todas las transiciones configuradas
- [ ] Running es el estado por defecto (naranja)
- [ ] Código añadido a PlayerController
- [ ] Animator asignado en Inspector
- [ ] Animaciones asignadas a cada estado
- [ ] Probado en Play Mode

---

## 🎯 VALORES RECOMENDADOS

```
=== ANIMATOR SETTINGS ===
Apply Root Motion: ✗ (desactivado, usamos CharacterController)
Update Mode: Normal
Culling Mode: Always Animate

=== TRANSICIONES ===
Has Exit Time: ✗ (la mayoría)
Transition Duration: 0.1 - 0.2
Fixed Duration: ✓

=== ESTADOS ===
Running: Loop ✓, Speed 1.0
Jumping: Loop ✗, Speed 1.0
Sliding: Loop ✗, Speed 1.0
Death: Loop ✗, Speed 0.5-0.7
```
