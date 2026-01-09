# 🎮 Guía de Solución Rápida - Run For Humanity

## ✅ Problemas Resueltos

### 1. **Botones no Funcionan**
**Causa**: InputManager estaba buscando PlayerController en todas las escenas
**Solución**: InputManager ahora solo se activa en escena "Gameplay"

### 2. **Player No se Mueve**
**Causas y Soluciones**:
- ✅ Input directo con teclado agregado (A/D o flechas izq/der para carriles)
- ✅ Botones UI ahora llaman directamente a PlayerController
- ✅ PlayerController ya no depende de InputManager.Instance

### 3. **Player Aparece al Nivel del Suelo**
**Solución**: PlayerController ahora posiciona al player a 2m de altura automáticamente

---

## 🎯 Configuración del Player

### **En la Escena Gameplay:**

1. **Crear GameObject Player** (si no existe):
   ```
   GameObject → Create Empty → Nombrar "Player"
   ```

2. **Agregar Componentes**:
   - CharacterController (automático si usas PlayerSetup)
   - PlayerController (el script principal)
   - PlayerSetup (helper de configuración)

3. **Configurar PlayerSetup**:
   ```
   - Start Height: 2
   - Forward Speed: 10
   - Lane Change Speed: 10
   - Jump Force: 8
   ```

4. **Click derecho en PlayerSetup → "Configure Player"**

5. **Verificar posición**:
   - X: 0 (carril del medio)
   - Y: 2 (altura correcta)
   - Z: 0 (inicio)

---

## 🎮 Controles

### **Teclado**:
- **A / ← (Izquierda)**: Mover a carril izquierdo
- **D / → (Derecha)**: Mover a carril derecho
- **W / ↑ / SPACE**: Saltar
- **S / ↓**: Deslizar

### **Botones UI (Gameplay)**:
- **IZQUIERDA**: Mover carril izquierdo
- **DERECHA**: Mover carril derecho
- **SALTAR**: Saltar
- **DESLIZAR**: Deslizar bajo obstáculos

### **Touch (Móvil)**:
- **Swipe Izquierda/Derecha**: Cambiar carril
- **Swipe Arriba**: Saltar
- **Swipe Abajo**: Deslizar
- **Tap**: Saltar

---

## 🐛 Troubleshooting

### **Problema: Botones UI no responden**
**Solución**:
1. Verifica que hay un EventSystem en la escena
2. El MasterSetup lo crea automáticamente
3. Si no existe: `GameObject → UI → Event System`

### **Problema: Player no se mueve con teclado**
**Test Rápido**:
1. Selecciona el Player en la jerarquía
2. Click derecho en PlayerSetup
3. Selecciona "Test Move Left" o "Test Move Right"
4. Si funciona → el problema es de input
5. Si no funciona → revisa que PlayerController esté agregado

### **Problema: Player cae infinitamente**
**Solución**:
1. Verifica que CharacterController está agregado
2. Verifica que hay un suelo (Plane) en Y = 0
3. Ajusta la gravedad en PlayerController si es necesario

### **Problema: Player no salta**
**Solución**:
1. Verifica que CharacterController.isGrounded funciona
2. Puede necesitar un Collider en el suelo
3. Test: Click derecho en PlayerSetup → "Test Jump"

---

## 📋 Checklist de Configuración

### **Escena Gameplay**:
- [ ] GameObject "Player" existe
- [ ] Player tiene CharacterController
- [ ] Player tiene PlayerController
- [ ] Player tiene PlayerSetup
- [ ] Player está en posición (0, 2, 0)
- [ ] Player tiene tag "Player"
- [ ] Hay un suelo (Plane en Y=0)
- [ ] Canvas UI existe con botones
- [ ] EventSystem existe

### **Otras Escenas (MainMenu, Shop, etc)**:
- [ ] Canvas UI existe
- [ ] EventSystem existe
- [ ] Botones tienen onClick configurados
- [ ] NO hay InputManager activo (se desactiva automáticamente)

---

## 🚀 Workflow de Prueba

1. **Configurar Player**:
   ```
   - Abrir escena Gameplay
   - Crear GameObject "Player"
   - Agregar PlayerSetup
   - Click "Configure Player"
   ```

2. **Configurar UI**:
   ```
   - Crear GameObject "MasterSetup"
   - Agregar MasterSetup
   - Click "Setup This Scene"
   - Borrar MasterSetup
   ```

3. **Probar**:
   ```
   - Presionar Play
   - Probar con teclado (A/D)
   - Probar botones UI
   - Verificar que player se mueve
   ```

---

## 💡 Tips

1. **Siempre configura el Player primero** antes de la UI
2. **Usa PlayerSetup** para tests rápidos sin entrar en Play
3. **Los botones UI buscan el Player automáticamente** cuando se presionan
4. **El InputManager solo se activa en Gameplay** para no interferir con otros botones

---

## 🎯 Estado Actual

✅ **Lo que funciona**:
- Botones en todas las escenas (MainMenu, Shop, ONGSelection)
- Player se mueve con teclado
- Player se mueve con botones UI
- Player se posiciona correctamente
- Input touch en móvil (cuando se compile)

✅ **Lo que está pendiente**:
- Slide mechanic (placeholder implementado)
- Dash mechanic (placeholder implementado)
- Conectar sistemas de monedas/distancia con UI
- Animaciones del player

---

**¡Ahora todo debería funcionar correctamente!** 🎮
