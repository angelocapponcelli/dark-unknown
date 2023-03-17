# DarkUnknown

Single player roguelike game developed for the Videogame Design and Programming (VGD) class at Politecnico di Milano.

## Game introduction

The adventure takes place in a foreign fantasy kingdom ruled by a mysterious entity that usurped the throne of the previous legitimate king. His reign makes the life of the subjects a living hell, taxing them and requiring people to work more and more. In addition, many children were taken from their homes and families to be trained in his army.

A dark and mysterious entity, the Dark Unknown, has taken control of the Kingdom and has set his powerful minions to scare the populous... all except for a young boy, named Legolas, who is willing to restore peace and order in the kingdom. Along his path, our hero will face and defeat powerful enemies to reach his goal.

This demo includes 3 levels, each of 5 rooms, where Legolas will face several undead created by the black magic of the Dark Unknown. 
In each room, all the enemies must be defeated to unlock the 3 doors allowing the player to continue his adventure. Above each door, an icon gives a hint on which reward can be obtained in the room behind that door, so the player has to choose carefully. But remember: the reward will spawn only when all enemies are defeated! 

### Current version changelog

  - NOW WITH CONTROLLER SUPPORT;
  - Added 1 new weapon: the throwable axe;
  - Added 2 new levels of 5 randomly selected rooms, 1 initial and 1 final, each with a new environment;
  - Added a new type of trap;
  - Improved tutorial area;
  - Added a difficulty progression system, based on the number of enemies in the levels, with the first being easy, the second being harder and the third being very hard. Could be changed and balanced in the future;
  - Fixed Skeleton Overlord boss fight mechanic and moved it to the initial level;
  - Added 2 new bosses with different mechanics, 1 for each new level;
  - Added 5 new enemy types;
  - Added ability and mana system, with 5 new abilities to be found as room reward that will help you in your run. These can be used by spending a specific amount of mana, different per ability. Mana can be recharged by killing enemies;
  - Added a key bind system, so you can play with whatever button you'd like;
  - Added graphics menu;
  - Added checkpoint system and hub rooms for each level, where you will respawn after death. A new checkpoint is unlocked once the boss of the level is killed. Currently limited to 2 lives per level;
  - Added gems currency as a reward for killing enemies. These can be currently used to buy health-increase power-ups in hub rooms. Could be used in the future to buy various weapon and ability upgrades at a merchant in the hub rooms;
  - Revamped healing system:
    - Removed health as a room reward;
    - Added healing potion consumables found in rooms at an interval;
    - Added health-increase power-up in hub rooms;
  - Increased speed and strength rewards to make them more valuable and noticeable;
  - Added an initial lore cutscene;
  - Fixed probelm with speed and strength having no limit, resulting in the player being way too fast after acquiring many of these rewards.

### Know issues

  - Reanimated worms in the Skeleton Overlord boss fight might not attack the player;
  - The bat companion ability may stay activated indefinitely, even though it should only last 20 seconds;
  - Poison status persists even after death;
  - Controller UI navigation not implemented yet.

### Future prospects

  - Bug fixes;
  - Adding movement to worms;
  - Adding merchant in hub rooms that sells weapon and ability upgrades for gems;
  - An improved weapon, enemies, and difficulty balancing;
  - Maybe new weapons;
  - Dynamic lights through the Universal Rendering Pipeline;
  - Random selection of boss when reaching a boss room;
  - Random spawning position of potions;
  - Final cutscene;
  - Procedurally generated maps at runtime.

## Development team

 - Angelo Capponcelli - Project leader, programmer
 - Francesco Scandale - Programmer
 - Luca Fornasari - Programmer, AI designer, improvised 2D artist
 - Matteo Santi - Programmer, level designer
 - Mattia Carini - Programmer, level designer

## Download

You can download the game at this link:
https://polimi-game-collective.itch.io/dark-unknown
