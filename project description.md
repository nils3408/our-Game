# Project description #

## Game idea ## 

2D sport-spiel, bei dem zwei Charaktere jeweils das Tor des Gegners mit einem Ball treffen und das eigene verteidigen müssen.
ähnlich zu "Kopfkicker"

Die Spieler können sich bewegen, springen und schießen und lassen sich über die Tastatur steuern und Fähigkeiten Aktivieren


Es gibt mehrere auswählbare Charactäre, wobei jeder Charactär eine individuelle Eigenschafft hat.
    Bsp: 
    Kämpfer   - kann den Gegner ausknockken für x Sekunden
    Spiderman - kann sehr hoch springen

Weiterhinn ändert sich nach jedem Anstoß (neustart nach dem ein Tor gefallen ist) das Spielfeld und einige Funktionen etwas.
    Bsp.
    Spieler muss 2 Charaktere gleichzeitig steuern

Dieses Spiel soll entweder gegen einen Bot oder einen anderen Menschlichen Spieler gespielt werden

### Physik im Spiel ###
Die Spieler bewegen sich jeweils mit einer vorgegebeben Geschwindigkeit (Pixel je Sekunde) entlang der X Achse
Der Ball bewegt sich basierend auf der Schussweise  entweder linear mit einer festen Geschwindigkeit oder Parabelförmig mit
     x-Richtung    Gleichförmige Bewegung
     y-Richtung:    Gleichmäßig beschleunigte Bewegung (Fallbewegung)
    
Die Charaktere untereinander und mit dem Ball sollen sich gegenseitig Blocken und beim Ball die Geschwindigkeit ändern.


### weitere Features: ###
Feuerball - Ball wird sehr schnell
Eisball   - Ball wird eingefrohren/ sehr langsam
Goal Boost - nächste Tor zählt doppelt
Freezing Effect - Der Gegner kann nicht mehr springen / nur noch springen 
großßes / kleines Tor

Alle Features sind temporär begrenzt und nur in ausgewählten Spielsituationen einsetzbar.
z.B. wer hinten liegt, kriegt mit einer höheren Wahrscheinlichkeit einen Boost

Nach jedem Anstoß kleine Variationen die das Spiel etwas verändern sollen.
    -2 Charaktere gleichzeitig Bewegen
    -Bariere vor Tor -> Knopf treffen oder mehrmals drauf schießen
    -neues Minigame z.B Pong 
    -unterschiedliche Erdanziehung
    - Volleyball -> Ball darf nicht auf dem Boden fallen

## Visualisierung ##

Fokus liegt auf der Entwicklung einer 2D Version
Option das Spielerlebnis zu erweitern, durch die geziehlte Implementation von 3D Animationen (z.B. wenn ein Tor fällt)


## Menus ##
  - Menu zur Spielerwahl
  - Menu zur Spielfeld (Hintergeund) wahl
  - Menu zum Modus auswählen (2 Spielermode, 1 Spielermode)
  -einstellungen
  -credits


## Struktur des Spiels ###


![image](https://github.com/user-attachments/assets/706e053d-5354-4cd1-9732-9eba8be530cb)




## technische Implementation ## 

Implementation mit Monogame und C#


## Minimum Product ##
todo 


## Zeitplan ##
todo
  
1. Meilenstein funktionierendes Spiel
2. Meilenstein Spielfunktionen, Powerups Charaktere
3. Animationen, Grafik

