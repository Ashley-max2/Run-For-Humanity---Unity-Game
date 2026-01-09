# CONFIGURACIÓN DETALLADA DE UI - RUN FOR HUMANITY

## RESOLUCIÓN BASE DEL CANVAS
**Todas las escenas usan:**
- **Reference Resolution**: 1080 x 1920 (formato vertical/móvil)
- **UI Scale Mode**: Scale With Screen Size
- **Match Width or Height**: 0.5
- **Screen Match Mode**: Match Width Or Height

---

## 1. ESCENA: PRELOADER (Pantalla de Carga)

### Canvas-LoadingScreen
- **Render Mode**: Screen Space - Overlay
- **Canvas Scaler**: Reference Resolution 1080x1920

#### BackgroundImage
- **Tipo**: Image
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (100, 100)
- **Pivot**: (0.5, 0.5)
- **Sprite**: Ninguno asignado (necesita configurar)

#### Logo
- **Tipo**: Image
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (100, 100)
- **Pivot**: (0.5, 0.5)
- **Sprite**: Ninguno asignado (necesita configurar)

#### LoadingText
- **Tipo**: TextMeshPro
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (200, 50)
- **Pivot**: (0.5, 0.5)
- **Text**: "New Text" (cambiar a "Cargando...")
- **Font Size**: 36
- **Alignment**: Center/Middle
- **Color**: Blanco (1, 1, 1, 1)

#### ProgressBar (Slider)
- **Tipo**: Slider
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 20)
- **Pivot**: (0.5, 0.5)
- **Direction**: Left to Right
- **Min Value**: 0
- **Max Value**: 1
- **Value**: 0

##### ProgressBar > Background
- **Anchors**: (0, 0.25) - (1, 0.75)
- **Size Delta**: (0, 0)
- **Sprite**: UI Sprite (Default)

##### ProgressBar > Fill Area > Fill
- **Anchors**: (0, 0) - (0, 0)
- **Size**: (10, 0)
- **Sprite**: UI Sprite (Default)
- **Color**: Blanco

##### ProgressBar > Handle Slide Area > Handle
- **Size**: (20, 20)
- **Sprite**: UI Sprite (Default)

---

## 2. ESCENA: MAIN MENU (Menú Principal)

### Canvas
- **Render Mode**: Screen Space - Overlay
- **Reference Resolution**: 1080x1920

#### Panel Principal (organizador con Layout Group)
- **Vertical Layout Group**: Activo
- **Child Force Expand**: Width & Height
- **Spacing**: 0
- **Padding**: 0

#### PlayButton
- **Tipo**: Button
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 30)
- **Pivot**: (0.5, 0.5)
- **Sprite**: UI Sprite (Default)
- **Text**: "Button" (cambiar a "JUGAR")
  - **Font Size**: 24
  - **Alignment**: Center/Middle
  - **Color**: Gris oscuro (0.196, 0.196, 0.196, 1)

#### ShopButton
- **Tipo**: Button
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 30)
- **Pivot**: (0.5, 0.5)
- **Sprite**: UI Sprite (Default)
- **Text**: "Button" (cambiar a "TIENDA")
  - **Font Size**: 24
  - **Alignment**: Center/Middle

#### SettingsButton
- **Tipo**: Button
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 30)
- **Pivot**: (0.5, 0.5)
- **Sprite**: UI Sprite (Default)
- **Text**: "Button" (cambiar a "CONFIGURACIÓN")
  - **Font Size**: 24
  - **Alignment**: Center/Middle

### SettingsPanel (Panel de Configuración)
- **Anchors**: (0, 0) - (1, 1)
- **Size Delta**: (0, 0)
- **Background Color**: Blanco semi-transparente (1, 1, 1, 0.392)

#### LanguageDropdown
- **Tipo**: Dropdown
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 30)
- **Pivot**: (0.5, 0.5)

##### LanguageDropdown > Label
- **Texto**: "Option A"
- **Font Size**: 14
- **Alignment**: Left/Middle
- **Anchors**: (0, 0) - (1, 1)
- **Offset**: Left=5, Right=-30, Top=-3, Bottom=-3

##### LanguageDropdown > Arrow
- **Size**: (20, 20)
- **Position**: Right aligned

#### MusicSlider
- **Tipo**: Slider
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 20)
- **Pivot**: (0.5, 0.5)
- **Direction**: Left to Right
- **Min Value**: 0
- **Max Value**: 1

##### MusicSlider > Background
- **Anchors**: (0, 0.25) - (1, 0.75)
- **Sprite**: UI Sprite Default

##### MusicSlider > Fill Area > Fill
- **Color**: Blanco
- **Image Type**: Filled

##### MusicSlider > Sliding Area > Handle
- **Size**: (20, 20)
- **Sprite**: UI Sprite (circle)

#### SFXSlider
- **Configuración idéntica a MusicSlider**
- **Tipo**: Slider
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 20)

#### CloseButton (Botón cerrar configuración)
- **Tipo**: Button
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 30)
- **Text**: "Button" (cambiar a "CERRAR")

---

## 3. ESCENA: ONG SELECTION (Selección de ONG)

### Canvas
- **Render Mode**: Screen Space - Overlay
- **Reference Resolution**: 1080x1920

#### Title (Título de la pantalla)
- **Tipo**: TextMeshPro
- **Anchors**: Top Center
- **Position**: Ajustar según diseño
- **Font Size**: 48-60
- **Alignment**: Center
- **Text**: "SELECCIONA TU ONG"

#### ONGScrollView (Scroll Rect para lista de ONGs)
- **Tipo**: Scroll Rect
- **Anchors**: Stretch (0,0) - (1,1) con márgenes
- **Vertical**: True
- **Horizontal**: False
- **Movement Type**: Elastic

##### ONGScrollView > Viewport
- **Mask**: Activo
- **Show Mask Graphic**: False
- **Sprite**: UI Mask

##### ONGScrollView > Viewport > Content
- **Pivot**: (0, 1) - Top Left
- **Anchor**: (0, 1) - (1, 1)
- **Size Delta**: (0, 300)
- **Grid Layout Group**:
  - **Cell Size**: (100, 100)
  - **Spacing**: (0, 0)
  - **Constraint**: Fixed Column Count

##### ONGScrollView > Scrollbar Vertical
- **Direction**: Bottom to Top
- **Size**: (20, 0)

###### Scrollbar > Sliding Area > Handle
- **Size**: (20, 20)
- **Sprite**: UI Default

#### Panel de Distribución de Porcentajes

##### ONGSlider_1 (Primer slider ONG)
- **Tipo**: Slider
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 20)
- **Direction**: Left to Right
- **Min Value**: 0
- **Max Value**: 1
- **Value**: 0

##### ONGSlider_2 (Segundo slider ONG)
- **Configuración idéntica a ONGSlider_1**

##### ONGSlider_3 (Tercer slider ONG)
- **Configuración idéntica a ONGSlider_1**

#### PercentageDisplay (Muestra de porcentajes)
- **Tipo**: Panel con TextMeshPro
- **Contenido**: Mostrar "%"
- **Font Size**: 24

#### ConfirmButton (Botón confirmar selección)
- **Tipo**: Button
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (160, 30)
- **Pivot**: (0.5, 0.5)
- **Text**: "Button" (cambiar a "CONFIRMAR")
  - **Font Size**: 24
  - **Alignment**: Center/Middle

---

## 4. ESCENA: SHOP (Tienda)

### Canvas
- **Render Mode**: Screen Space - Overlay
- **Reference Resolution**: 1080x1920

#### CoinPanel (Panel superior con monedas)
- **Anchors**: Top Right
- **Size**: Ajustar según diseño

##### CoinIcon
- **Tipo**: Image
- **Anchors**: Center (0.5, 0.5)
- **Size**: (100, 100)
- **Sprite**: Icono de moneda (necesita asignar)

##### CoinAmount
- **Tipo**: TextMeshPro
- **Font Size**: 32
- **Alignment**: Center
- **Text**: "0"

#### ShopScrollView (Vista de scroll para items)
- **Tipo**: Scroll Rect
- **Anchors**: Stretch con márgenes
- **Vertical**: True
- **Horizontal**: False

##### ShopScrollView > Viewport > Content
- **Pivot**: (0, 1)
- **Anchor**: Top Stretch
- **Grid Layout Group**:
  - **Cell Size**: (100, 100)
  - **Spacing**: (0, 0)

#### ShopItemTemplate (Plantilla para items)
- **Estructura de cada item**:

##### ItemImage
- **Tipo**: Image
- **Size**: (80, 80)

##### ItemName
- **Tipo**: TextMeshPro
- **Font Size**: 18

##### ItemPrice
- **Tipo**: TextMeshPro
- **Font Size**: 16
- **Color**: Dorado

##### BuyButton
- **Tipo**: Button
- **Size**: (120, 40)
- **Text**: "COMPRAR"

#### BackButton (Volver al menú)
- **Tipo**: Button
- **Anchors**: Bottom Left
- **Position**: (20, 20) desde bottom-left
- **Size**: (160, 30)
- **Text**: "VOLVER"

---

## 5. ESCENA: GAMEPLAY (Juego)

### Canvas
- **Render Mode**: Screen Space - Overlay
- **Reference Resolution**: 1080x1920

#### HUDPanel (Panel del HUD superior)
- **Anchors**: Top Stretch (0, 1) - (1, 1)
- **Height**: 100-150

##### CoinText
- **Tipo**: TextMeshPro
- **Anchors**: Top Left
- **Position**: (20, -20)
- **Font Size**: 32
- **Text**: "0"
- **Alignment**: Left

##### DistanceText
- **Tipo**: TextMeshPro
- **Anchors**: Top Right
- **Position**: (-20, -20)
- **Font Size**: 32
- **Text**: "0m"
- **Alignment**: Right

##### PauseButton
- **Tipo**: Button
- **Anchors**: Top Center
- **Size**: (60, 60)
- **Sprite**: Icono de pausa

#### PausePanel (Panel de pausa - INACTIVO por defecto)
- **Anchors**: Stretch (0,0) - (1,1)
- **Background**: Semi-transparente negro (0,0,0,0.8)

##### PauseTitle
- **Tipo**: TextMeshPro
- **Text**: "PAUSA"
- **Font Size**: 60
- **Alignment**: Center
- **Position**: Top Center

##### ResumeButton
- **Tipo**: Button
- **Anchors**: Center
- **Size**: (160, 30)
- **Text**: "REANUDAR"

##### RestartButton
- **Tipo**: Button
- **Anchors**: Center
- **Size**: (160, 30)
- **Text**: "REINICIAR"

##### MainMenuButton
- **Tipo**: Button
- **Anchors**: Center
- **Size**: (160, 30)
- **Text**: "MENÚ PRINCIPAL"

#### GameOverPanel (Panel Game Over - INACTIVO por defecto)
- **Anchors**: Stretch (0,0) - (1,1)
- **Background**: Semi-transparente negro (0,0,0,0.9)

##### GameOverTitle
- **Tipo**: TextMeshPro
- **Text**: "GAME OVER"
- **Font Size**: 72
- **Alignment**: Center
- **Color**: Rojo

##### FinalDistanceText
- **Tipo**: TextMeshPro
- **Anchors**: Center (0.5, 0.5)
- **Position**: (0, 0)
- **Size**: (200, 50)
- **Font Size**: 36
- **Text**: "New Text" (cambiar a "Distancia: 0m")
- **Alignment**: Center

##### FinalCoinsText
- **Tipo**: TextMeshPro
- **Size**: (200, 50)
- **Font Size**: 32
- **Text**: "Monedas: 0"
- **Alignment**: Center

##### RestartButton (Game Over)
- **Tipo**: Button
- **Anchors**: Center
- **Size**: (160, 30)
- **Text**: "REINICIAR"

##### MainMenuButton (Game Over)
- **Tipo**: Button
- **Anchors**: Center
- **Size**: (160, 30)
- **Text**: "MENÚ PRINCIPAL"

#### ControlButtons (Controles en pantalla)

##### JumpButton
- **Tipo**: Button
- **Anchors**: Bottom Right
- **Position**: (-100, 100)
- **Size**: (100, 100)
- **Sprite**: Flecha arriba o icono saltar

##### SlideButton
- **Tipo**: Button
- **Anchors**: Bottom Right
- **Position**: (-250, 100)
- **Size**: (100, 100)
- **Sprite**: Flecha abajo o icono deslizar

##### LeftButton
- **Tipo**: Button
- **Anchors**: Bottom Left
- **Position**: (100, 100)
- **Size**: (100, 100)
- **Sprite**: Flecha izquierda

##### RightButton
- **Tipo**: Button
- **Anchors**: Bottom Left
- **Position**: (250, 100)
- **Size**: (100, 100)
- **Sprite**: Flecha derecha

---

## NOTAS IMPORTANTES PARA CONFIGURACIÓN:

### 1. ANCHORS COMUNES:
- **Center**: (0.5, 0.5) - (0.5, 0.5)
- **Top Left**: (0, 1) - (0, 1)
- **Top Right**: (1, 1) - (1, 1)
- **Bottom Left**: (0, 0) - (0, 0)
- **Bottom Right**: (1, 0) - (1, 0)
- **Stretch Horizontal**: (0, 0.5) - (1, 0.5)
- **Stretch Vertical**: (0.5, 0) - (0.5, 1)
- **Stretch Full**: (0, 0) - (1, 1)

### 2. BOTONES ESTÁNDAR:
- **Size**: (160, 30) para botones normales
- **Font Size**: 24
- **Color Normal**: Blanco (1, 1, 1, 1)
- **Color Highlighted**: (0.96, 0.96, 0.96, 1)
- **Color Pressed**: (0.78, 0.78, 0.78, 1)
- **Transition**: Color Tint
- **Fade Duration**: 0.1

### 3. SLIDERS ESTÁNDAR:
- **Size**: (160, 20)
- **Direction**: Left to Right
- **Min Value**: 0
- **Max Value**: 1
- **Whole Numbers**: No
- **Background Anchors**: (0, 0.25) - (1, 0.75)
- **Fill Area Anchors**: (0, 0.25) - (1, 0.75)
- **Handle Size**: (20, 20)

### 4. TEXT MESH PRO:
- **Font Asset**: LiberationSans SDF (default)
- **Material**: LiberationSans SDF Material
- **Vertex Color**: Blanco por defecto
- **Wrapping**: Habilitado
- **Overflow**: Overflow
- **Alignment**: Según necesidad

### 5. SCROLL RECTS:
- **Movement Type**: Elastic
- **Elasticity**: 0.1
- **Inertia**: True
- **Scroll Sensitivity**: 1
- **Viewport**: Con Mask component
- **Content**: Con Layout Group (Vertical o Grid)

### 6. COLORES IMPORTANTES:
- **Fondo UI Panel**: (1, 1, 1, 0.392) - Blanco semi-transparente
- **Fondo Overlay**: (0, 0, 0, 0.8) - Negro semi-transparente
- **Texto Primario**: (1, 1, 1, 1) - Blanco
- **Texto Secundario**: (0.196, 0.196, 0.196, 1) - Gris oscuro

### 7. TAMAÑOS RECOMENDADOS:
- **Botones grandes**: 200 x 60
- **Botones medianos**: 160 x 40
- **Botones pequeños**: 100 x 30
- **Iconos**: 64 x 64 o 100 x 100
- **Títulos**: Font Size 48-72
- **Texto normal**: Font Size 24-32
- **Texto pequeño**: Font Size 16-20

---

## ORDEN DE CONFIGURACIÓN RECOMENDADO:

1. **Configurar Canvas Scaler** en todas las escenas (1080x1920)
2. **Crear jerarquía de paneles** (HUD, Menú, Pausa, etc.)
3. **Configurar anchors** de todos los elementos
4. **Ajustar posiciones y tamaños**
5. **Asignar sprites e iconos**
6. **Configurar textos y fuentes**
7. **Conectar botones con scripts**
8. **Probar en diferentes resoluciones**

---

## SPRITES Y ASSETS NECESARIOS:

### ICONOS:
- Icono de moneda
- Icono de pausa
- Icono de play/resume
- Iconos de control (flechas)
- Logo del juego
- Iconos de ONGs

### SPRITES UI:
- Fondo de botón
- Fondo de panel
- Barra de progreso (fill)
- Handle de slider
- Sprites de dropdown
- Bordes y decoraciones

### FUENTES:
- TextMesh Pro - Liberation Sans SDF (por defecto)
- Fuente personalizada si es necesario

---

**IMPORTANTE**: Todos los valores de Position están en relación a los Anchors configurados. Ajusta según el diseño visual específico de tu juego.
