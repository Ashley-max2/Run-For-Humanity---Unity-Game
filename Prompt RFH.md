::: {#page0 style="width:596.0pt;height:842.0pt"}
**[PROMPT COMPLETO PARA IA: DESARROLLO DE \"RUN
]**

**[FOR HUMANITY\" - JUEGO MÓVIL SOLIDARIO
]**

[🎮]**[
CONTEXTO Y VISIÓN GENERAL
]**

[Título del Proyecto: \"Run For Humanity\" - Endless Runner
Solidario​]

[Plataforma: Móvil (iOS/Android) - Unity 2022
LTS​]

[Género: Endless Runner Vertical 3D con mecánicas sociales y propósito
solidario​]

[Audiencia: Jugadores casuales 13-45 años interesados en causas
sociales​]

[Modelo de Negocio: Gratuito con anuncios no intrusivos +
microtransacciones
]

[éticas + suscripción
mensual​]

[Propósito Único: Cada jugador personaliza a qué ONGs van las ganancias
que ]

[genera
]

[ ]

[📱]**[
CARACTERÍSTICAS PRINCIPALES
]**

**[1. GAMEPLAY CORE
]**

[●​][
Movimiento: Carril automático hacia arriba, deslizar izquierda/derecha
para ]

[cambiar carriles
]

[●​][
Controles: Tap para saltar, mantener para deslizar, swipe rápido para
dash ]

[lateral
]

[●​][
Progresión: Distancia infinita, velocidad incremental, generación
procedural
]

[de escenarios
]

[●​][
Power-ups: Multiplicadores, imanes de monedas, escudos, sprints
]

[●​][
Obstáculos: Dinámicos que varían según la causa activa
]

**[2. SISTEMA SOLIDARIO PERSONALIZADO
]**

[●​][
Cada jugador ELIGE el % de distribución entre ONGs verificadas
]

[●​][
Pantalla de selección tipo \"donut chart\" con arrastre para ajustar
porcentajes
]

[●​][
Transparencia total: ver dinero generado, distribuido y certificados
]

[descargables
]

[●​][
Cambio de distribución permitido mensualmente
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
**[3. COMPONENTES SOCIALES
]**

[●​][
Ver \"fantasmas\" de otros jugadores corriendo a tu misma distancia
]

[●​][
Sistema de grupos/clanes (hasta 20 personas)
]

[●​][
Eventos en tiempo real: Olas de solidaridad, retos comunitarios
]

[●​][
Competición cooperativa: Suma de kilómetros grupales
]

**[4. PROGRESIÓN Y RETENCIÓN
]**

[●​][
Sistema de niveles (Corredor
][→][
Voluntario
][→][
Activista
][→][
Héroe
][→][
Leyenda)
]

[●​][
Insignias por logros de donación y distancia
]

[●​][
Eventos temporales temáticos (Día del Agua, Maratón contra el Hambre)
]

[●​][
Misiones diarias/semanales vinculadas a causas sociales
]

**[5. MONETIZACIÓN ÉTICA
]**

[●​][
Banner inferior no intrusivo
]

[●​][
Rewarded videos opcionales (revivir, doblar impacto, power-ups
temporales)
]

[●​][
Moneda premium: \"Doblones Solidarios\" (compra skins, eventos
especiales)
]

[●​][
Suscripción mensual \"Kit del Corredor\" (\$2.99): sin ads, +10%
impacto, icono
]

[especial
]

[●​][
80% de ingresos va a las ONGs elegidas por los jugadores
]

[ ]

[🎯]**[
REQUERIMIENTOS TÉCNICOS DETALLADOS
]**

**[SISTEMA DE CARRERA (GAMEPLAY)
]**

[csharp
]

[// Se necesita
implementar:][
]

[1][.][
CharacterController con físicas responsivas
]

[2][.][
Sistema
][de][
carriles
][(][3][-][5][
líneas][)][
con cambio suave
]

[3][.][
Generador procedural de pistas con secciones temáticas
]

[4][.][
Pooling de
obstáculos][,][
power][-][ups
y decoración
]

[5][.][
Sistema de colisiones y recolección de monedas
]

[6][.][
Gestor de
][dificultad][
progresiva
][(][velocidad
aumenta
][con][
distancia][)][
]

[7][.][
Animaciones][:][
correr][,][
saltar][,][
deslizar][,][
dash][,][
muerte
]

[8][.][
Partículas y efectos VFX para impacto visual
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[9][.][
Sistema de cámaras dinámicas
][(][shake][,][
zoom en momentos
épicos][)][
]

[10][.][
][Audio][
adaptativo
][(][música][,][
SFX][,][
][sonidos][
ambientales][)
]

**[SISTEMA DE ONGs Y DONACIONES
]**

[csharp
]

[// Arquitectura de
datos:][
]

[\[][System][.][Serializable][\]][
]

[public][
][class][
][ONGData][
][{][
]

[
][public][
][string][
id][;][
]

[
][public][
][string][
nombre][;][
]

[
][public][
][string][
categoria][;][
][//
\"Agua\", \"Educación\", \"Salud\",
etc.][
]

[
][public][
][string][
descripcion][;][
]

[
][public][
][string][
impacto][;][
][//
\"1\$ = 20 litros de
agua\"][
]

[
][public][
][string][
logoURL][;][
]

[
][public][
][string][
sitioWeb][;][
]

[
][public][
][float][
porcentajeTransparencia][;][
][//
0-100%][
]

[
][public][
][float][
ratingUsuarios][;][
]

[
][public][
][bool][
verificada][;][
]

[}][
]

[ ]

[public][
][class][
][UserDonationPreferences][
][{][
]

[
][public][
][string][
userId][;][
]

[
][public][
][List][\<][ONGDistribution][\>][
distribucion][;][
]

[
][public][
][float][
totalAcumuladoMes][;][
]

[
][public][
][float][
totalAcumuladoHistorico][;][
]

[
][public][
][DateTime][
proximaTransferencia][;][
]

[
][public][
][List][\<][Certificado][\>][
certificados][;][
]

[}][
]

[ ]

[public][
][class][
][RevenueTracking][
][{][
]

[
][public][
][enum][
][Fuente][
][{][
AdRewarded][,][
CompraInApp][,][
Suscripcion
][}][
]

[
][public][
][void][
][RegistrarIngreso][(][float][
monto][,][
][Fuente][
fuente][)][
][{][
]

[
][//
Distribuir según preferencias
usuario][
]

[
][//
Actualizar UI en tiempo
real][
]

[
][//
Guardar en
historial][
]

[
][}][
]

[}
]

**[INTERFACES DE USUARIO REQUERIDAS
]**
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[text
]

[1. PANTALLA INICIO:
]

[ - Botón jugar (grande)
]

[ - Panel \"Mi Impacto\" (destacado)
]

[ - Menú: Tienda, Grupos, Eventos, Configuración
]

[ - Banner inferior pequeño
]

[ ]

[2. PANTALLA DE CARRERA (HUD):
]

[ - Distancia actual (km)
]

[ - Monedas recogidas
]

[ - \"Impacto en tiempo real\" (litros/árboles/comidas)
]

[ - Power-ups activos
]

[ - Indicador de otros jugadores cercanos
]

[ ]

[3. PANTALLA POST-CARRERA:
]

[ - Resumen de stats (distancia, monedas, tiempo)
]

[ - Impacto generado (\$ y equivalente en ayuda)
]

[ - Comparativa con mejor personal
]

[ - Botones: Continuar, Cambiar ONGs, Compartir
]

[ ]

[4. PANTALLA SELECCIÓN ONGs:
]

[ - Lista scrollable de ONGs verificadas
]

[ - Gráfico circular interactivo (drag para ajustar %)
]

[ - Total debe sumar 100%
]

[ - Previsualización: \"Así se distribuirán tus próximas donaciones\"
]

[ - Filtros: Categoría, Rating, Transparencia
]

[ ]

[5. PANEL DE TRANSPARENCIA:
]

[ - Total donado (personal y global)
]

[ - Desglose por ONG
]

[ - Certificados descargables
]

[ - Próxima fecha de transferencia
]

[ - Verificador blockchain (opcional)
]

[ ]

[6. MENÚ SOCIAL:
]

[ - Lista de amigos/grupos
]

[ - Estado de actividad de cada uno
]

[ - Retos activos grupales
]

[ - Chat motivacional básico
]

[ - Invitar amigos
]

[ ]

[7. TIENDA:
]

[ - Skins de personaje (solo cosméticos)
]

[ - Power-ups temporales
]

[ - Doblones Solidarios (moneda premium)
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[ - Suscripción mensual
]

[ - Paquetes solidarios especiales
]

**[SISTEMA BACKEND Y MULTIJUGADOR
]**

[csharp
]

[// Servicios
necesarios:][
]

[1][.][
AUTENTICACIÓN][:][
]

[
][-][
Login
][con][
email][,][
Google][,][
Apple][,][
Facebook
]

[
][-][
Perfil de usuario con stats persistentes
]

[ ]

[2][.][
BASE DE DATOS EN
][TIEMPO][
REAL][:][
]

[
][-][
Posiciones de
][otros][
jugadores
][(][fantasmas][)][
]

[
][-][
Leaderboards por causa
]

[
][-][
Stats globales de impacto
]

[
][-][
Estado de eventos en tiempo real
]

[ ]

[3][.][
SERVICIOS
][DE][
PAGO][:][
]

[
][-][
Integración con Unity IAP
]

[
][-][
Google
][Play][
Billing
][(][Android][)][
]

[
][-][
Apple
][App][
Store
][(][iOS][)][
]

[
][-][
Sistema de recibos y verificación
]

[ ]

[4][.][
SERVIDOR
][DE][
EVENTOS][:][
]

[
][-][
Programación de eventos especiales
]

[
][-][
Matchmaking para carreras simultáneas
]

[
][-][
Sistema de notificaciones push
]

[ ]

[5][.][
ANALYTICS][:][
]

[
][-][
Seguimiento de métricas de donación
]

[
][-][
Comportamiento de usuarios
]

[
][-][
Optimización de anuncios
]

[
][-][
Detección de fraudes
]

[ ]

[// Solución
recomendada:][
]

[-][
Firebase][:][
Auth][,][
][Realtime][
DB][,][
Cloud Functions
]

[-][
Unity
][Gaming][
Services][:][
Analytics][,][
Cloud Save
]

[-][
Servidor
Node][.][js
][en][
Render][.][com
][(][gratis][
inicialmente][)][
]

[-][
Supabase para
][base][
de datos PostgreSQL
]

**[SISTEMA DE ANUNCIOS
]**
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[csharp
]

[// Implementación
ética:][
]

[public][
][class][
][AdManager][
][:][
][MonoBehaviour][
]

[{][
]

[
][//
Banner no intrusivo (solo en
menús)][
]

[
][public][
][void][
][ShowBanner][()][
][{][
][/\*
320x50 bottom
\*/][
][}][
]

[ ]

[
][//
Rewarded videos
OPTATIVOS][
]

[
][public][
][void][
][ShowRewardedAd][(][string][
placement][)][
][{][
]

[
][//
Placement
types:][
]

[
][//
\"revive\" - Revivir después de
morir][
]

[
][//
\"double\_impact\" - Doblar impacto de la
partida][
]

[
][//
\"free\_powerup\" - Power-up aleatorio
gratis][
]

[
][//
\"bonus\_coins\" - Monedas
extra][
]

[ ]

[
][//
Siempre mostrar tiempo
restante][
]

[
][//
Nunca obligatorio para
progresar][
]

[
][}][
]

[ ]

[
][//
Interstitial entre partidas (máximo 1 cada 2
partidas)][
]

[
][public][
][void][
][ShowInterstitial][()][
][{][
]

[
][//
Solo mostrar después de 2-3 carreras
completadas][
]

[
][//
Nunca durante gameplay
activo][
]

[
][}][
]

[}
]

**[SISTEMA DE NOTIFICACIONES
]**

[csharp
]

[// Notificaciones inteligentes (máx
3/día):][
]

[public][
][class][
][SmartNotifications][
][:][
][MonoBehaviour][
]

[{][
]

[
][public][
][void][
][ScheduleNotifications][()][
][{][
]

[
][//
Tipos:][
]

[
][//
1. Motivacional: \"Tu grupo necesita 5km más para la
meta\"][
]

[
][//
2. Social: \"Ana superó tu récord en Agua
Limpia\"][
]

[
][//
3. Impacto: \"Tus \$2.34 serán donados en 2
días\"][
]

[
][//
4. Eventos: \"Ola de Solidaridad empieza en 1
hora\"][
]

[
][//
5. Logros: \"Desbloqueaste la skin
\'Filántropo\'\"][
]

[ ]

[
][//
Personalizar
según:][
]

[
][//
- Hábitos de
juego][
]

[
][//
- ONGs
favoritas][
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[
][//
- Horario
local][
]

[
][//
- Participación en
eventos][
]

[
][}][
]

[}
]

[ ]

[🎨]**[
ASPECTOS ARTÍSTICOS Y DE DISEÑO
]**

**[ESTILO VISUAL:
]**

[●​][
Arte: Low-poly 3D vibrante y optimizado para móvil
]

[●​][
Paleta: Colores inspiradores (azules para agua, verdes para
reforestación)
]

[●​][
Personajes: Diversos, estilizados, con opciones de personalización
]

[●​][
Ambientes: Cambian según causa activa (desiertos
][→][
agua, ciudades
]

[contaminadas
][→][
limpias)
]

[●​][
UI: Moderna, minimalista, con énfasis en datos de impacto
]

**[ANIMACIONES REQUERIDAS:
]**

[1.​ Personaje: Run, Jump, Slide, Dash, Celebration, Fail
]

[2.​ ONGs: Logos animados en pantalla de selección
]

[3.​ UI: Transiciones suaves, feedback táctil
]

[4.​ Efectos: Partículas para monedas, power-ups, impacto social
]

[5.​ Transiciones entre zonas temáticas
]

**[AUDIO Y MÚSICA:
]**

[●​][
Música: Inspiradora, variable según causa (percusión africana para agua,
etc.) ]

[●​][
SFX: Satisfactorios para recolección, fluidos para movimiento
]

[●​][
Voces: Narración ocasional de historias de impacto
]

[●​][
Audio dinámico: Intensidad musical aumenta con velocidad
]

[ ]

[📊]**[
ESTRUCTURA DE ARCHIVOS UNITY
]**

[text
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[Assets/
]

`├──`[ Scripts/
]

`│`[
]`├──`[
Gameplay/
]

`│`[
]`│`[
]`├──`[
PlayerController.cs
]

`│`[
]`│`[
]`├──`[
LaneSystem.cs
]

`│`[
]`│`[
]`├──`[
ObjectPooler.cs
]

`│`[
]`│`[
]`├──`[
TrackGenerator.cs
]

`│`[
]`│`[
]`└──`[
PowerUpManager.cs
]

`│`[
]`├──`[
Systems/
]

`│`[
]`│`[
]`├──`[
DonationSystem.cs
]

`│`[
]`│`[
]`├──`[
ONGManager.cs
]

`│`[
]`│`[
]`├──`[
AdManager.cs
]

`│`[
]`│`[
]`├──`[
IAPManager.cs
]

`│`[
]`│`[
]`└──`[
AnalyticsManager.cs
]

`│`[
]`├──`[
UI/
]

`│`[
]`│`[
]`├──`[
UIManager.cs
]

`│`[
]`│`[
]`├──`[
ONGSelectionUI.cs
]

`│`[
]`│`[
]`├──`[
ShopUI.cs
]

`│`[
]`│`[
]`└──`[
SocialUI.cs
]

`│`[
]`└──`[
Network/
]

`│`[
]`├──`[
FirebaseManager.cs
]

`│`[
]`├──`[
MultiplayerGhosts.cs
]

`│`[
]`└──`[
EventManager.cs
]

`├──`[ Scenes/
]

`│`[
]`├──`[
MainMenu.unity
]

`│`[
]`├──`[
Gameplay.unity
]

`│`[
]`├──`[
ONGSelection.unity
]

`│`[
]`└──`[
Shop.unity
]

`├──`[ Prefabs/
]

`│`[
]`├──`[
Player/
]

`│`[
]`├──`[
Obstacles/
]

`│`[
]`├──`[
PowerUps/
]

`│`[
]`└──`[
UI/
]

`├──`[ Art/
]

`│`[
]`├──`[
Models/
]

`│`[
]`├──`[
Textures/
]

`│`[
]`├──`[
Animations/
]

`│`[
]`└──`[
Materials/
]

`├──`[ Audio/
]

`│`[
]`├──`[
Music/
]

`│`[
]`└──`[
SFX/
]

`└──`[ Settings/
]

[
]`├──`[
InputSettings.inputactions
]

[
]`├──`[
QualitySettings.asset
]

[
]`└──`[
ProjectSettings/
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[ ]

[🔧]**[
CONFIGURACIÓN TÉCNICA ESPECÍFICA
]**

**[Unity Settings:
]**

[yaml
]

[Versión][:][
Unity 2022.3 LTS
]

[Render
Pipeline][:][
URP (Universal Render Pipeline)
]

[Target
Framerate][:][
60 FPS (ajustable a 30 en dispositivos bajos)
]

[Resolución][:][
Soporte para notch y diferentes aspect ratios
]

[Input][:][
Touch con feedback háptico opcional
]

[Compresión de
Texturas][:][
ASTC para
Android][,][
PVRTC para iOS
]

[Optimización][:][
LOD
groups][,][
occlusion
culling][,][
GPU instancing
]

[ ]

[Paquetes
Essentiales][:][
]

[-][
Unity Gaming Services
]

[-][
Firebase SDK
]

[-][
Unity IAP
]

[-][
Unity Ads
]

[-][
TextMeshPro
]

[-][
Cinemachine
]

[-][
Input System
]

[-][
Addressables (para contenido descargable)
]

**[Configuración Firebase:
]**

[json
]

[{][
]

[
][\"config\"][:][
][{][
]

[
][\"auth\_domains\"][:][
][\[][\"runforhumanity.firebaseapp.com\"][\],][
]

[
][\"database\_url\"][:][
][\"https://runforhumanity.firebaseio.com\"][,][
]

[
][\"project\_id\"][:][
][\"runforhumanity\"][,][
]

[
][\"storage\_bucket\"][:][
][\"runforhumanity.appspot.com\"][
]

[
][},][
]

[
][\"database\_structure\"][:][
][{][
]

[
][\"users\"][:][
][{][
]

[
][\"\$userId\"][:][
][{][
]

[
][\"profile\"][:][
][{},][
]

[
][\"donation\_preferences\"][:][
][{},][
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[
][\"stats\"][:][
][{},][
]

[
][\"friends\"][:][
][{}][
]

[
][}][
]

[
][},][
]

[
][\"ongs\"][:][
][{][
]

[
][\"\$ongId\"][:][
][{][
]

[
][\"info\"][:][
][{},][
]

[
][\"total\_donations\"][:][
][{},][
]

[
][\"live\_counter\"][:][
][{}][
]

[
][}][
]

[
][},][
]

[
][\"global\_stats\"][:][
][{][
]

[
][\"total\_km\"][:][
][{},][
]

[
][\"total\_donations\"][:][
][{},][
]

[
][\"active\_events\"][:][
][{}][
]

[
][}][
]

[
][}][
]

[}
]

[ ]

[🚀]**[
ROADMAP DE IMPLEMENTACIÓN
]**

**[FASE 1: PROTOTIPO (Semanas 1-4)
]**

[markdown
]

[Semana 1: Core gameplay básico
]

[
][-][
Movimiento del personaje
]

[
][-][
Sistema de carriles
]

[
][-][
Obstáculos simples
]

[
][-][
Generación básica de pistas
]

[ ]

[Semana 2: Sistemas básicos
]

[
][-][
Recolección de monedas
]

[
][-][
Power-ups simples
]

[
][-][
UI básica (HUD, menú principal)
]

[
][-][
Sistema de puntuación
]

[ ]

[Semana 3: Integración backend
]

[
][-][
Firebase Auth
]

[
][-][
Guardado de progreso
]

[
][-][
Leaderboards básicos
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[
][-][
Analytics
]

[ ]

[Semana 4: Monetización básica
]

[
][-][
Banner ads
]

[
][-][
Rewarded videos
]

[
][-][
Tienda simple (skins)
]

[
][-][
Unity IAP configurado
]

**[FASE 2: SISTEMA SOLIDARIO (Semanas 5-8)
]**

[markdown
]

[Semana 5: Catálogo de ONGs
]

[
][-][
Base de datos de ONGs verificadas
]

[
][-][
Pantalla de selección con gráfico circular
]

[
][-][
Sistema de distribución porcentual
]

[
][-][
UI/UX para gestión de donaciones
]

[ ]

[Semana 6: Tracking de ingresos
]

[
][-][
Sistema que rastrea \$ generado por jugador
]

[
][-][
Distribución automática según preferencias
]

[
][-][
Panel de transparencia personal
]

[
][-][
Certificados digitales básicos
]

[ ]

[Semana 7: Impacto visualizado
]

[
][-][
Conversión \$
]`→`[
impacto real (litros, árboles, etc.)
]

[
][-][
Feedback durante carrera
]

[
][-][
Pantalla post-carrera con desglose
]

[
][-][
Historial de donaciones
]

[ ]

[Semana 8: Optimización móvil
]

[
][-][
Performance en dispositivos bajos
]

[
][-][
Gestión de memoria
]

[
][-][
Load times optimizados
]

[
][-][
Battery consumption reducido
]

**[FASE 3: COMPONENTES SOCIALES (Semanas 9-12)
]**

[markdown
]

[Semana 9: Fantasmas multijugador
]

[
][-][
Ver otros jugadores en tiempo real
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[
][-][
Sistema de matchmaking por distancia
]

[
][-][
Indicadores de actividad social
]

[
][-][
Animaciones para interacción
]

[ ]

[Semana 10: Grupos y eventos
]

[
][-][
Creación/unión a grupos
]

[
][-][
Retos grupales semanales
]

[
][-][
Eventos en tiempo real programados
]

[
][-][
Notificaciones push inteligentes
]

[ ]

[Semana 11: Compartición viral
]

[
][-][
Generador de imágenes para redes
]

[
][-][
Sistema de invitación a amigos
]

[
][-][
Logros shareables
]

[
][-][
Integración con redes sociales
]

[ ]

[Semana 12: Pulido y testeo
]

[
][-][
Balance de dificultad
]

[
][-][
User testing interno
]

[
][-][
Fix de bugs críticos
]

[
][-][
Preparación para beta cerrada
]

**[FASE 4: LANZAMIENTO (Semanas 13-16)
]**

[markdown
]

[Semana 13: Beta cerrada
]

[
][-][
100-1000 usuarios de prueba
]

[
][-][
Feedback collection system
]

[
][-][
A/B testing de mecánicas
]

[
][-][
Optimización basada en datos
]

[ ]

[Semana 14: Localización y accesibilidad
]

[
][-][
Traducción a 5 idiomas
]

[
][-][
Opciones de accesibilidad
]

[
][-][
Ajustes para diferentes culturas
]

[
][-][
Testeo internacional
]

[ ]

[Semana 15: Preparación store
]

[
][-][
Assets para App Store/Play Store
]

[
][-][
Página web de transparencia
]

[
][-][
Políticas de privacidad
]

[
][-][
Soporte técnico configurado
]

[ ]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[Semana 16: Lanzamiento oficial
]

[
][-][
Deploy a stores
]

[
][-][
Campaña de lanzamiento
]

[
][-][
Monitorización en tiempo real
]

[
][-][
Plan de soporte post-lanzamiento
]

[ ]

[🎯]**[
MÉTRICAS DE ÉXITO Y ANALYTICS
]**

**[KPIs a Seguir:
]**

[csharp
]

[// Implementar tracking
para:][
]

[public][
][class][
][AnalyticsKPIs][
][:][
][MonoBehaviour][
]

[{][
]

[
][//
Engagement][
]

[
][public][
][float][
DailyActiveUsers][;][
]

[
][public][
][float][
SessionLength][;][
][//
Objetivo: 8-12
minutos][
]

[
][public][
][float][
SessionsPerDay][;][
][//
Objetivo:
2.5-3.5][
]

[ ]

[
][//
Retención][
]

[
][public][
][float][
Day1Retention][;][
][//
Objetivo:
\>45%][
]

[
][public][
][float][
Day7Retention][;][
][//
Objetivo:
\>25%][
]

[
][public][
][float][
Day30Retention][;][
][//
Objetivo:
\>12%][
]

[ ]

[
][//
Monetización][
]

[
][public][
][float][
AverageRevenuePerUser][;][
][//
Objetivo:
\$0.15][
]

[
][public][
][float][
ConversionRate][;][
][//
Objetivo:
3-5%][
]

[
][public][
][float][
AdARPDAU][;][
][//
Ingreso por anuncios por usuario activo
]

[diario][
]

[ ]

[
][//
Impacto
Social][
]

[
][public][
][float][
TotalDonations][;][
]

[
][public][
][float][
AverageDonationPerUser][;][
]

[
][public][
][float][
ONGDistributionVariance][;][
]

[
][public][
][float][
SocialShares][;][
]

[ ]

[
][//
Viralidad][
]

[
][public][
][float][
KFactor][;][
][//
Invitations per
user][
]

[
][public][
][float][
OrganicInstalls][;][
]

[
][public][
][float][
RatingAppStore][;][
][//
Objetivo:
4.5+][
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[}
]

**[Eventos Personalizados a Trackear:
]**

[csharp
]

[// Firebase Analytics Custom
Events][
]

[Analytics][.][LogEvent][(][\"donation\_preferences\_changed\"][,][
][new][
][Dictionary][\<][string][,][
]

[object][\>][
][{][
]

[
][{][
][\"number\_of\_ongs\"][,][
selectedONGs][.][Count
][},][
]

[
][{][
][\"distribution\_variance\"][,][
][CalculateVariance][()][
][}][
]

[});][
]

[ ]

[Analytics][.][LogEvent][(][\"social\_interaction\"][,][
][new][
][Dictionary][\<][string][,][
][object][\>][
][{][
]

[
][{][
][\"type\"][,][
interactionType
][},][
]

[
][{][
][\"group\_size\"][,][
currentGroupMembers
][},][
]

[
][{][
][\"event\_id\"][,][
activeEventId
][}][
]

[});][
]

[ ]

[Analytics][.][LogEvent][(][\"impact\_achievement\"][,][
][new][
][Dictionary][\<][string][,][
][object][\>][
][{][
]

[
][{][
][\"amount\_donated\"][,][
totalDonated
][},][
]

[
][{][
][\"equivalent\_impact\"][,][
impactEquivalent
][},][
]

[
][{][
][\"time\_to\_achieve\"][,][
daysToAchieve
][}][
]

[});
]

[ ]

[⚠️]**[
CONSIDERACIONES LEGALES Y ÉTICAS
]**

**[Requerimientos:
]**

[1.​ Transparencia total: Publicar mensualmente desglose de donaciones
]

[2.​ Privacidad: GDPR, CCPA compliant. No vender datos de usuarios
]

[3.​ Certificación ONGs: Verificar cada organización antes de incluirla
]

[4.​ Edad mínima: 13+ o consentimiento parental
]

[5.​ Accesibilidad: Cumplir WCAG 2.1 para discapacitados
]

[6.​ Términos claros: Explicar exactamente cómo se distribuye el dinero
]

**[Documentación Necesaria:
]**
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[●​][
Términos de Servicio
]

[●​][
Política de Privacidad
]

[●​][
Explicación del modelo de donaciones
]

[●​][
Certificados de ONGs asociadas
]

[●​][
Informes financieros trimestrales
]

[ ]

[🎮]**[
VALORES DE DISEÑO DEL JUEGO
]**

**[Principios Fundamentales:
]**

[1.​ El gameplay siempre es primero: Debe ser divertido incluso sin
componente
]

[social
]

[2.​ Transparencia absoluta: Los jugadores deben ver exactamente dónde
va su ]

[dinero
]

[3.​ Respeto al jugador: Anuncios opcionales, no pay-to-win, progreso
justo ]

[4.​ Comunidad sobre competencia: Cooperar es más recompensado que
]

[competir
]

[5.​ Impacto medible: Cada acción debe tener equivalente en ayuda real
]

[6.​ Inclusividad: Diseño para todas las edades, géneros y habilidades
]

[7.​ Optimismo: Mensaje positivo sobre capacidad de cambiar el mundo
]

**[Tonos de Comunicación:
]**

[●​][
Inspirador, no culpabilizador
]

[●​][
Empoderador, no paternalista
]

[●​][
Transparente, no técnico
]

[●​][
Social, no solitario
]

[●​][
Celebratorio, no competitivo
]

[ ]

[📱]**[
INTEGRACIONES EXTERNAS REQUERIDAS
]**

**[APIs y SDKs:
]**

[yaml
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[Essential][:][
]

[-][
][Firebase
SDK][:][
Auth][,][
Realtime
DB][,][
Analytics][,][
Cloud Messaging
]

[-][
][Unity
Ads
SDK][:][
Monetización
]

[-][
][Unity
IAP
SDK][:][
Compras
in][-][app
]

[-][
][Unity
Gaming
Services][:][
Cloud
Save][,][
Leaderboards
]

[-][
][Facebook
SDK][:][
Login][,][
sharing (opcional)
]

[-][
][Google
Sign-In][:][
Para Android
]

[-][
][Apple
Sign-In][:][
Para iOS 13+
]

[ ]

[Optional but
Recommended][:][
]

[-][
][AppsFlyer][:][
Attribution tracking
]

[-][
][Adjust][:][
Analytics avanzado
]

[-][
][Sentry][:][
Crash reporting
]

[-][
][OneSignal][:][
Push notifications avanzadas
]

[-][
][Branch][:][
Deep linking
]

**[Servicios de Pago:
]**

[●​][
Stripe Connect para transferencias a ONGs
]

[●​][
PayPal Payouts para transferencias internacionales
]

[●​][
TransferWise para mejores tipos de cambio
]

[ ]

[🔍]**[
TESTING Y CALIDAD
]**

**[Estrategia de Testing:
]**

[1.​ Unit Testing: Sistemas críticos (donaciones, IAP)
]

[2.​ Integration Testing: Flujos completos de usuario
]

[3.​ Play Testing: Con usuarios reales cada 2 semanas
]

[4.​ Device Testing: 20+ dispositivos Android/iOS
]

[5.​ Load Testing: Servidores para 10k usuarios concurrentes
]

[6.​ Security Testing: Penetration test antes de lanzamiento
]

**[Checklist Pre-Lanzamiento:
]**

[markdown
]

[\[ \] Performance: 60 FPS en dispositivos de gama media
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[\[ \] Memory: \<300MB RAM uso, \<200MB instalación
]

[\[ \] Battery: \<10% por hora de juego
]

[\[ \] Data: \<50MB/mes uso de datos
]

[\[ \] Load Times: \<5 segundos carga inicial
]

[\[ \] Crashes: \<1% de sesiones con crash
]

[\[ \] ANR: 0% de Application Not Responding
]

[\[ \] Store Compliance: Cumple todas políticas
]

[\[ \] Legal: Documentación completa
]

[\[ \] Backup: Sistema de backup y recovery
]

[ ]

[🎪]**[
CONTENIDO POST-LANZAMIENTO
]**

**[Roadmap de 12 Meses:
]**

[markdown
]

[Mes 1-3: Estabilización y feedback
]

[
][-][
Arreglar bugs críticos
]

[
][-][
Añadir 10 ONGs nuevas
]

[
][-][
Mejorar tutorial
]

[
][-][
Primera temporada de eventos
]

[ ]

[Mes 4-6: Expansión de contenido
]

[
][-][
Nuevos personajes y skins
]

[
][-][
2 nuevos biomas temáticos
]

[
][-][
Sistema de \"alianzas\" entre grupos
]

[
][-][
Integración con wearables (Apple Watch, Fitbit)
]

[ ]

[Mes 7-9: Funcionalidades avanzadas
]

[
][-][
Modo cooperativo en tiempo real
]

[
][-][
Sistema de \"patrocinios\" empresariales
]

[
][-][
NFT no especulativos para certificados
]

[
][-][
Realidad aumentada (ver impacto en tu ciudad)
]

[ ]

[Mes 10-12: Internacionalización
]

[
][-][
10 nuevos idiomas
]

[
][-][
ONGs locales por país
]

[
][-][
Eventos culturalmente relevantes
]

[
][-][
Versión para tablets y web
]
:::

::: {#page0 style="width:596.0pt;height:842.0pt"}
[ ]

[💡]**[
INSTRUCCIONES FINALES PARA LA IA
]**

[Objetivo Principal: Crear un juego que no solo entretenga, sino que
empodere a los
]

[jugadores para ayudar al mundo mientras juegan.
]

[Enfoque de Desarrollo:
]

[1.​ Primero la diversión: Si el juego no es divertido, nadie jugará
para donar
]

[2.​ Transparencia como característica: No es algo \"extra\", es central
al diseño
]

[3.​ Comunidad sobre competencia: Crear conexiones, no rivalidades
tóxicas
]

[4.​ Impacto medible: Cada jugador debe ver exactamente su contribución
]

[5.​ Ética sobre ganancias: Monetización que los jugadores se sientan
bien ]

[apoyando
]

[Mensaje Final para los Desarrolladores
(IA):​]

[\"Estás creando más que un juego. Estás creando una plataforma que
transforma
]

[tiempo de ocio en impacto social real. Cada línea de código, cada
asset, cada
]

[decisión de diseño debe reflejar esta misión. No es solo un endless
runner; es un
]

[movimiento social disfrazado de juego casual.\"
]

[Resultado Esperado: Un juego móvil Unity completamente funcional con
todas las
]

[características descritas, listo para beta testing, con especial
atención en:
]

[1.​ Optimización para móvil
]

[2.​ Sistema de donaciones transparente
]

[3.​ Gameplay adictivo pero ético
]

[4.​ Comunidad integrada
]

[5.​ Monetización que no compromete los valores del juego
]

[ ]
:::
