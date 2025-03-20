# Endless Runner

## **How to Play**
- There are 2 different approaches to play. The default one is set to swipe mechanics. 
**Swipe Mechanics**
Jump     -> Swipe up
Go left  -> Swipe left
Go right -> Swipe right
**Tap Mechanics**
The screen is divided into 3 vertical parts.
Jump     -> Tap middle part
Go left  -> Tap left part
Go right -> Tap right part

## **Obstacles**
- There are two types of obstacles: **Short** and **Long**.
  - Short obstacles can be jumped over.
  - Long obstacles cannot be jumped over.
- There are three types of weapons that appear after 15 seconds of gameplay:
  - **Pickaxe**: Hanging in the air and swinging from side to side.
  - **Shuriken**: Moving horizontally from side to side.
  - **Cleaver**: Chopping from one side.
  
  These weapons function similarly to obstacles and must be avoided by the player.

## **Optimization Techniques**
- **Object Pooling** is used for efficient generation of endless platforms, obstacles, and weapons.
- Instead of loading the scene for each new play session, the positions of game objects and the game state are reset, reducing CPU overhead.

## **Features**
- The **player movement speed** increases over time to enhance the challenge.
- **Obstacles** and **platforms** are disabled once they are no longer in the player's line of sight.
- The player can walk on one of three paths. Short obstacles can occupy any of the paths, while long obstacles can occupy up to two paths. The path and obstacle placement are selected randomly.

## **Final Notes**
I thoroughly enjoyed working on this project. While there is certainly room for further enhancements, particularly in terms of visuals (such as VFX, animations for the player and other objects), and smoother, more intuitive movement animations (which could be achieved using DoTween), I am pleased with the final product given the time constraints. I hope you enjoy it as well!
