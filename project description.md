# Project description #

## Game idea / Kurzbeschreibung ## 

2D sport-spiel, bei dem zwei Charaktere jeweils das Tor des Gegners treffen und das eigene verteidigen müssen.
ähnlich zu "Kopfkicker". Es wird Gespielt bis jemand eine Bestimmte Punktzahl z.B 4 erreicht

Die Spieler können sich bewegen, springen, schießen und können Einzigartige Fertigkeiten einsetzen und lassen sich über die Tastatur steuern.


Es gibt mehrere auswählbare Charactäre, wobei jeder Charactär eine individuelle Eigenschafft hat.
    Bsp: 
    Kämpfer   - kann den Gegner ausknockken für x Sekunden
    Spiderman - kann sehr hoch springen

Weiterhinn ändert sich nach jedem Anstoß (neustart nach dem ein Tor gefallen ist) das Spielfeld und einige Funktionen etwas.
    Bsp.
    Spieler muss 2 Charaktere gleichzeitig steuern
    Bevor ein Tor fallen kann muss einen Knopf treffen

Dieses Spiel soll entweder gegen einen Bot oder einen anderen Menschlichen Spieler am gleichen Computer gespielt werden

## Genre ##

Gelegenheitsspiel, Arcade, Indie, Sportspiel

## Zielgruppen ##

Kinder und Jugendliche da: 	 -Einfaches Gameplay, leicht verständlich
                    	     -Kurze Spielrunden -> viel Action wenig Downtime
						  	 -Gut möglich mit Freunden zu spielen 
							 -möglichkeit Spiel im Internet/Handy anzubieten
		
Gelegenheitsspieler da: 	 -Kurze Runden also gut Spielbar wenn man Kurz Wartet/abschalten möchte 
		  					 -möglichkeit Spiel im Internet/Handy anzubieten

		
### Physik im Spiel ###

Die Spieler bewegen sich jeweils mit einer vorgegebeben Geschwindigkeit (Pixel je Sekunde) entlang der X Achse und können Springen entlang der y-Achse
Der Ball bewegt sich basierend auf der Schussweise durch die Charaktere entweder linear mit einer festen Geschwindigkeit oder Parabelförmig mit
     x-Richtung    Gleichförmige Bewegung
     y-Richtung:   Gleichmäßig beschleunigte Bewegung 
    



### weitere Features: ###

Todo: nach Wichtigkleit Sortieren!

Auf der Map gibt es Loot Kisten welche PowerUps geben 
    z.B 
Feuerball - Ball wird sehr schnell
Eisball   - Ball wird eingefrohren/ sehr langsam
Goal Boost - nächste Tor zählt doppelt
Freezing Effect - Der Gegner kann nicht mehr springen / nur noch springen 
großes / kleines Tor

Alle Item PowerUpssind temporär begrenzt und nur in ausgewählten Spielsituationen einsetzbar.
z.B. wer hinten liegt, kriegt mit einer höheren Wahrscheinlichkeit einen Boost

Weiteres mögliches Feature:
Es wird nach jedem Anstoß eine besondere Map ausgewählt die das Spiel interessanter machen sollen.
    Bsp.
    Kontrolle 2 Spieler 
    Superschnell
    Supergroß
    Barieren ...
    VolleyBall ( Ball darf nicht auf Boden fallen)

Jeder einzelne Charakter besitzt eine Ultimative Fähigkeit die man erst aufladen muss 
Die Ultimative Fähigkeit jedes Charakters wird durch Ballberührungen, Tore oder durch ein Item aufgeladen wird. 



## Design ##

Das Spiel soll unterschiedliche Level haben mit unterschiedlich starken Bot Gegner und eigenen Fähigkeiten, wenn ein Bot geschlagen wurde kann man selbst mit dem Charakter spielen und ihre Eigenschaften ausnutzen kann. Somit schaltet man im laufe des Spieles mehrere Charaktere frei mit denen man spielen kann. 

Nachdem man ein Level besiegt hat bekommt jeder Spieler Münzen welche dazu vewendet werden kann um Skins und Toranimationen zu kaufen 


## Controlls und Interface ##

![Interface](https://github.com/user-attachments/assets/b76ff47c-d8aa-4eb1-a891-3f4a6d41b9b5)



Bewegungen der Spieler:

 Spieler    | nach links | nach rechs | springen | schießen  | Fähigkeit | Items
----------- |  --------  | ---------- | -------- | --------- | --------- | ---------          
Spieler 1   |      a     |  d         |    w     |  	s    |	  f	 | 	e
----------  | ---------- |----------  |----------|---------- |---------- | ---------
Spieler 2   |  k         |  ö         |   o      | l         |    ä	 |    p

## Visualisierung / Grafikstil ##

Fokus liegt auf der Entwicklung einer 2D Version
Option das Spielerlebnis zu erweitern, durch die geziehlte Implementation von 3D Animationen (z.B. wenn ein Tor fällt)

Pixelgrafik? 


## Menus ##
  - Menu zur Spielerwahl
  - Menu zur Spielfeld (Hintergeund) wahl
  - Menu zum Modus auswählen (2 Spielermode, 1 Spielermode)
  - Schwierigkeitsauswahl


## Struktur des Spiels ###


![image](https://github.com/user-attachments/assets/706e053d-5354-4cd1-9732-9eba8be530cb)




## technische Implementation ## 

Implementation mit Monogame und C#


### Implemenation für die Menus ###

Das Spiel ist läuft in zwei Schleifen:
    - Wenn man das Spiel startet landed man im Startmenu (siehe oben) der ersten Schleife.
    - Wenn man in eines der Menus geht, landed man in einer zweiten, inneren Loop
    - Wenn man das Menu in dem man sich befinded, beended , determiniert die innere Schleife
    - Dadurch stellt man sicher, dass wenn man ein Menu verlässt, wieder im Hauptmenu landed.


### Implementationsideen für die Spiellogik:
    
![image](https://github.com/user-attachments/assets/f8d2b5db-fb9d-4a6f-b9b8-94121a39d1eb)
    


![image](https://github.com/user-attachments/assets/a74c3a13-ab5f-43b5-a1a9-5325fe85c676)



## Minimum Product ##

Die Spiellogik funktioniert
    - Man kann die Spieler steuern
    - Die Kollisionen mit Ball und Gegner funktionieren
    - basic Power Ups funktionieren
    - Die Physik des Balls funktioniert wie gewünscht
    
ein Bot welcher angemessen gut Spielt 
Features der Maps
ein und zwei Spieler Modus



## Zeitplan ##

1. Meilenstein funktionierendes Spiel Local mit 2 Spielern spielbar
2. Meilenstein angemessener Bot erstellen
3. Meilenstein PowerUps und Map Features
4. Grafik und Animationen
5. unterschiedliche Stärke der Bots 
  


