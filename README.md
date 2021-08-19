# BomPig   
## Level generation     
The level is generated procedurally.     
The starting point is the location of the stone on the grid.    
Subsequently, the dimensions of the stone are taken to place other objects on the map.    
## Player movement through the level and placing bombs     
The player moves with the joystick and sets the bombs with the button.      
UI elements have been made responsive for different resolutions.      
Explosions are made in the style of a bomberman game and are stored in the object pool.         
## Enemy   
The enemy has three types of animations: dirty, aggressive and normal (they change during the game).       
It also has two models of behavior: moving to a random direction by a random number of steps in a certain range      
and finding the player's location using the wave algorithm and moving to this position with an increased speed.     
When receiving damage, the animation changes to dirty until the start of a new match.
## Features    
Procedural generation of destructible bushes.    
Procedural generation of speedBonus.     
Music.
