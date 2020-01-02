# Battlefront

This is a WIP of an AI battle simulation.
The goal is to have a team of agent fighting another team of agents while attempting to take control of the level.

### DESCRIPTION

The project is not made to be playable.
It consist of two teams of AIs that have a various amount of players using 5 classes as follow :
	assault rifle Soldier
	Demoman
	Shotgun soldier
	Rocket launcher soldier
	sniper

Every team is fighting to take control of the level, the latter is composed of command posts that have to be taken.
Every command posts controlled by a team allow it's players to respawn on it.
Every team have a set amount of tickets, one ticket allow to respawn one player.

A game ends when all the CPs are controlled by a team or when a team ran out of tickets.

	
### INSTALLATION

This repository contain all the "game rule" elements, this does not include any functionnal AI.
In order to use this repo, you will need to import Xnode (https://assetstore.unity.com/packages/tools/visual-scripting/xnode-104276) in your unity project 
as well as my behavior tree repository (https://github.com/Arghonot/My_CustomBehaviorTree).
If you just cloned the repository simply go to sample scene and press play.

### PS:
this is a work in progress, the code is not fully documented yet because it is still subject to a lot of changes.
