# ~~Army Command Simulator~~ NOW IS **Army Command Remastered**
A simulator about commanding an army in the Second World War. What more can I say? Still very much in development.


# Motivation
I wanted a game that actually involved strategical thinking. And without random luck in combat. Turns out that not many games fulfill those two very simple wishes!

It is a mix of grand strategy and rts without the millions of complicated extra stuff, making it pure skill and teamwork. 

There will only be a set number of sides, and you must work together and collaborate with each other(like your generals under one countries) able to collectivly control any unit in your amassed army against the other faction of players.

**JK** 

I just want to beat Lawrence, or get him to collab on this(Jason alr on this too)

# Tech/framework used
<b>Built with</b>
- [Unity 6000.2.0b10](https://unity.com)

# Features

- A* multi-threaded efficient pathfinding with Local Avoidence(RVO's)
- Heavily-Optimized(still room to grow) Networking able to sync hundreds if not thousands of units. With 3 different transport options(Steam, Tugboat, Photon) for different people.
- Burst and Jobs System to run certain code efficiently, natively faster, and multi-threaded
- Code is written with future in mind, somehwat commented(if i was actually thinking) and more importantly ***Designed in a way where a person with little to no knowledge of networking can do a lot since most things will be purely clientside and wont need to consider networking at all(*wink)(e.g. UI, placing buildings, stuff like that)***
- A robust selection system and controling
- Units/Army Groups are scriptable objects and designed vaguely on purpose to be able to uniquely accomadate most units with different attack types(e.g. burst dmg from artilery in intervals, high dps low burst damage from infantry) allow lots of freedom creating and customizing different types of units int he future when needed.
    -Move speed and pathfinding extreamly customizable
    -Breaking combat gives slight health disadvantage(balancing is needed)
    -Passive healing simulating reinforcements when out of combat
- A finished RTS camera with control
# ***WIP bc im lazy but may be easy to implement***
    -   GPU Instancing, GPU Resident Drawer and Upscaling, Deffered+ Renderer and Graphics Split Jobs all for better performance
    - Actual Models for Armies instead of cubes(which honestly are ok)
    -   A supply system and mechanics similar to HOI4 giving buffs and debuffs and limitatoins
    -   Dynamic Weather giving buffs and debuffs
    -   A map of some kind(maybe procedurally generated but I want a Eastern Front map ideally)
    -   Flowfield pathfinding(only if A* becomes a problem(which it shouldn't))
    -   Defense Structures, garrision, entrenchment buffs
    -   Various types of engagement(blitz through, part of your group gets hold up accordingly to the enemy)
    -   Infrastructure and Population(bc why not)
    -   Cities with value(maybe victory points of smth)
    -   Some sort of resource system(maybe not sure bc im lazy)
    -   Encirclement, or mass unit movement in formations(kinda achieved rn) or setting front lines
    -   Dynamic gains that show territory loss
    -   **Actual UI** I hate writing UI and designing it, so dont expect to much or to soon
    -   Client-Side prediction and extrapolation for smoothing visual object viewing
    -   SmoothSync for values
    -   A binary diff patch updater, only downloading changed file automaticlly, lowering bandwith usage and download time, and install time.

# ~~Installation~~  Binary Diff Self Patching Launcher/Updater/Repairer.
Coming soon...
If i can read this goddamn documentation..


# Credits:
"Groove Grove", "Sneaky Adventure"\
Kevin MacLeod (incompetech.com)\
Licensed under Creative Commons: By Attribution 3.0\
http://creativecommons.org/licenses/by/3.0/

# Ultimate Goal
- Lawrence save me bro, im dying over here