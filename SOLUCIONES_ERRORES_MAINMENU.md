# SOLUCIÓN DE ERRORES - MAIN MENU

## 🔴 ERRORES ENCONTRADOS:

### 1. **DontDestroyOnLoad only works for root GameObjects**
- **Afecta a**: AdManager, DonationSystem
- **Causa**: Estos GameObjects están como HIJOS de GameManager, no en la raíz

### 2. **UnassignedReferenceException: menuView not assigned**
- **Afecta a**: UIManager
- **Causa**: Las referencias de UI no están asignadas en el Inspector

---

## ✅ SOLUCIONES:

### SOLUCIÓN 1: Mover objetos a la raíz de la jerarquía

**En Unity Editor:**

1. **Abrir la escena MainMenu**
   - `Assets/Scenes/MainMenu.unity`

2. **En la ventana Hierarchy:**
   - Localizar `GameManager`
   - Expandir para ver sus hijos:
     - UIManager
     - AdManager
     - DonationSystem
     - SensorManager

3. **Arrastrar FUERA de GameManager a la raíz:**
   - Arrastra `AdManager` y suéltalo en la raíz (no dentro de ningún objeto)
   - Arrastra `DonationSystem` y suéltalo en la raíz
   - Arrastra `UIManager` y suéltalo en la raíz
   - Arrastra `SensorManager` y suéltalo en la raíz (si también usa DontDestroyOnLoad)

4. **Resultado esperado - Hierarchy debe quedar así:**
```
MainMenu (Scene)
├── Main Camera
├── Directional Light
├── Canvas (UI del menú)
├── EventSystem
├── GameManager (RAÍZ)
├── AdManager (RAÍZ)
├── DonationSystem (RAÍZ)
├── UIManager (RAÍZ)
└── SensorManager (RAÍZ)
```

---

### SOLUCIÓN 2: Asignar referencias de UIManager

**En Unity Editor:**

1. **Seleccionar UIManager** en la Hierarchy

2. **En el Inspector, buscar el componente UIManager Script**

3. **Asignar las siguientes referencias arrastrando desde la Hierarchy:**

   **Views (Paneles):**
   - **menuView** → Arrastra el GameObject que contiene el panel del menú principal
     - Busca el panel que tiene los botones Play, Shop, Settings
     - Probablemente sea un Panel dentro de Canvas
   
   - **hudView** → Arrastra el GameObject que contiene el HUD del juego
     - Es el panel que muestra monedas y distancia durante el juego
     - Si no existe, créalo (Panel vacío, márcalo como inactivo)
   
   - **gameOverView** → Arrastra el GameObject del panel de Game Over
     - Panel que se muestra cuando termina el juego
     - Si no existe, créalo (Panel vacío, márcalo como inactivo)

   **HUD Elements:**
   - **coinText** → Arrastra el TextMeshProUGUI que muestra las monedas
     - Si no existe en esta escena, déjalo en None (solo se usa en Gameplay)
   
   - **distanceText** → Arrastra el TextMeshProUGUI que muestra la distancia
     - Si no existe en esta escena, déjalo en None (solo se usa en Gameplay)

4. **Guardar la escena** (Ctrl+S)

---

### SOLUCIÓN 3: Alternativa - Modificar UIManager para MainMenu

Si MainMenu NO necesita HUD ni Game Over, modifica el script:

**Opción A: Hacer las referencias opcionales**

Edita `UIManager.cs` y cambia el método `ShowMenu()`:

```csharp
void ShowMenu()
{
    if (menuView != null) menuView.SetActive(true);
    if (hudView != null) hudView.SetActive(false);
    if (gameOverView != null) gameOverView.SetActive(false);
}
```

**Opción B: Verificar en Start si las referencias son necesarias**

```csharp
void Start()
{
    EventManager.OnGameStart += ShowHUD;
    EventManager.OnGameOver += ShowGameOver;
    EventManager.OnCoinCollected += UpdateCoinDisplay;

    // Solo mostrar menú si la referencia existe
    if (menuView != null)
    {
        ShowMenu();
    }
    else
    {
        Debug.LogWarning("UIManager: menuView not assigned. This is OK if not in MainMenu scene.");
    }
}
```

---

## 📋 JERARQUÍA RECOMENDADA PARA MAINMENU:

```
MainMenu
├── Main Camera
│
├── Canvas (UI Root - Render Mode: Screen Space Overlay)
│   ├── MenuPanel (menuView - GameObject)
│   │   ├── TitleText
│   │   ├── PlayButton
│   │   ├── ShopButton
│   │   └── SettingsButton
│   │
│   └── SettingsPanel (Opcional, si existe)
│       ├── MusicSlider
│       ├── SFXSlider
│       ├── LanguageDropdown
│       └── CloseButton
│
├── EventSystem
│
├── --- MANAGERS (TODOS EN RAÍZ) ---
│
├── GameManager (RAÍZ - NO tiene hijos managers)
│
├── AdManager (RAÍZ - usa DontDestroyOnLoad)
│
├── DonationSystem (RAÍZ - usa DontDestroyOnLoad)
│
├── UIManager (RAÍZ)
│
└── SensorManager (RAÍZ - si usa DontDestroyOnLoad)
```

---

## 🎯 PASOS RÁPIDOS (RESUMEN):

### Paso 1: Reorganizar Hierarchy
```
1. Abrir MainMenu.unity
2. En Hierarchy, expandir GameManager
3. Arrastrar AdManager fuera de GameManager → a la raíz
4. Arrastrar DonationSystem fuera de GameManager → a la raíz
5. Arrastrar UIManager fuera de GameManager → a la raíz
6. Guardar escena (Ctrl+S)
```

### Paso 2: Configurar UIManager
```
1. Seleccionar UIManager en Hierarchy
2. En Inspector, buscar sección "Views"
3. Arrastrar el Panel del menú → campo "menuView"
4. Si no hay HUD/GameOver en MainMenu, dejar en None
5. Guardar escena (Ctrl+S)
```

### Paso 3: Probar
```
1. Play en Unity
2. Verificar que no aparezcan errores de DontDestroyOnLoad
3. Verificar que no aparezca error de menuView unassigned
```

---

## ⚠️ NOTAS IMPORTANTES:

### Sobre DontDestroyOnLoad:
- **SOLO funciona en GameObjects de la RAÍZ** de la jerarquía
- Si un GameObject es hijo de otro, `DontDestroyOnLoad()` lanza warning
- Todos los Managers con Singleton deben estar en la raíz

### Sobre UIManager en diferentes escenas:
- **MainMenu**: Solo necesita `menuView`
- **Gameplay**: Necesita `hudView`, `gameOverView`, `coinText`, `distanceText`
- **Shop**: Puede necesitar sus propias referencias

### Recomendación:
- Crear un **UIManager diferente por escena**
- O hacer las referencias **opcionales** con null checks
- O usar un **Prefab de UIManager** configurado por escena

---

## 🔧 CÓDIGO MEJORADO (OPCIONAL):

Si quieres hacer UIManager más robusto:

```csharp
void ShowMenu()
{
    SetViewActive(menuView, true);
    SetViewActive(hudView, false);
    SetViewActive(gameOverView, false);
}

void ShowHUD()
{
    SetViewActive(menuView, false);
    SetViewActive(hudView, true);
    
    if (hudView != null)
    {
        hudView.transform.localScale = Vector3.zero;
        hudView.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
    }
}

void ShowGameOver()
{
    SetViewActive(gameOverView, true);
}

// Helper method para evitar NullReferenceException
private void SetViewActive(GameObject view, bool active)
{
    if (view != null)
    {
        view.SetActive(active);
    }
}
```

---

## ✔️ VERIFICACIÓN FINAL:

Después de aplicar los cambios, deberías ver:

✅ **NO más errores** de "DontDestroyOnLoad only works for root GameObjects"
✅ **NO más errores** de "menuView has not been assigned"
✅ **Menú funcional** al darle Play

---

## 📞 Si persisten los errores:

1. **Verificar que los GameObjects estén en la RAÍZ**
   - En Hierarchy, los managers NO deben estar indentados bajo otros objetos

2. **Verificar las referencias en el Inspector**
   - UIManager debe tener menuView asignado
   - Si ves "Missing (GameObject)", reasignar

3. **Limpiar y recompilar**
   - Assets > Reimport All
   - Edit > Clear All PlayerPrefs
   - File > Save Project

---

¡Aplica estos cambios y el juego debería funcionar correctamente!
