
# Xabbo Music

A Xabbo script that can be used to make music in Habbo Hotel using SFX furni and wired.

[![Xabbo Music - Game of Thrones theme](https://user-images.githubusercontent.com/34200697/203616198-720f8d33-697f-41ba-b560-44228e5b90c9.png)](https://www.youtube.com/watch?v=ltpeuaoyoNA)
## How it works

![xabbo-music-explanation-0](https://user-images.githubusercontent.com/34200697/203398655-88011d0f-5b41-4cf5-8474-83e9a5259550.png)

SFX furni in Habbo plays a different note based on the height of that furni. With that being said, you can play a song if you use this furni at the correct order and time.

![xabbo-music-explanation-1](https://user-images.githubusercontent.com/34200697/203410366-3f0cbb7f-b351-4e08-b99f-85e686e4472e.png)

The script attributes a character for every SFX furni and note, and using this characters you can create a song. You can find all characters and notes on the tables of "Supported SFX Items" section.
####
Music example (Game of Thrones Theme)
```
ne N Q W ne N Q W cm C B N cm C B N bw B M Q bw B M N nc C G H nc C G H
```

Music example (Disney opening played by ducks)
```
► ► ♫ ↓ ▬ ↨ ↕ - ¶ ¶ ↕ ► ☼ ► § - ↨ § ¶ ↕ ► ♫ ↓ - ↓ ↕ ¶ §
```
Note that every space means a 0,5s delay, if you want to recreate a specific song, beware with those spaces. You can also add longer delays using the character '-' in your song.
Character | Function
--- | ---
space | 0,5s delay
\- | 0,5s delay

## Setup

![setup-0](https://user-images.githubusercontent.com/34200697/203589110-99c0e303-39b5-4844-b5b3-cf004256ecfb.png)

You'll need to change the highlighted data on the script for it to work in your room.

#### Song

Use different notes and delays to make your songs, or copy one from example-songs.json

#### Wired space

For each note you'll need a different free tile in your room. Here you'll put the X and Y coordinates of two points that will be used to make a square. See an example in the image below.

![setup-1](https://user-images.githubusercontent.com/34200697/203592105-78efba69-8a0e-4926-93e3-bae398f69451.png)

You can get a tile location by walking to it and checking your packet logger

![setup-2](https://user-images.githubusercontent.com/34200697/203592831-0e7cc86a-9c6d-43e0-908a-d1d87fd32a73.png)

#### Effect space

Choose a tile to put all SFX furni you gonna use. You'll need to place your magic tile on this tile.

#### Magic tile id

The id of your magic tile. You can easily get it by clicking the magic tile with G-Earth connected.

![setup-3](https://user-images.githubusercontent.com/34200697/203593703-c51baa44-73bd-4d0d-a727-70c297cb1b78.png)

## Playing the music

Once all the wireds are placen down, all you need to play the music is trigger the WIRED Effect: Timer Reset

![setup-5](https://user-images.githubusercontent.com/34200697/203620731-2a3b7ad1-5214-45eb-8023-c42cbd1b4136.png)

## Supported SFX Items

#### Xylophone SFX

![sfx_xylo](https://user-images.githubusercontent.com/34200697/203387249-33279d77-2e6d-4205-ae36-c186ffc1a7e4.png) 

Character | Note
--- | ---
Q | A
2 | A#
W | B
3 | B#
E | C
R | D
5 | D#
T | E
6 | E#
Y | F
7 | F#
U | G
I | G#

#### Xylophone High SFX

![sfx_xylo_high](https://user-images.githubusercontent.com/34200697/203387252-9de6b4de-c707-4065-8f96-7bdae944179c.png)

Character | Note
--- | ---
Z | a
S | a#
X | b
D | b#
C | c
V | d
G | d#
B | e
H | e#
N | f
J | f#
M | g
, | g#

#### Pad SFX

![sfx_pad1](https://user-images.githubusercontent.com/34200697/203387247-13ebf4f7-33e1-4eec-97af-328e5be01dab.png)

Character | Note | Keys
--- | --- | ---
☺ | a | ALT + 1
☻ | a# | ALT + 2
♥ | b | ALT + 3
♦ | b# | ALT + 4
♣ | c | ALT + 5
♠ | d | ALT + 6
• | d# | ALT + 7
◘ | e | ALT + 8
○ | e# | ALT + 9
◙ | f | ALT + 10
♂ | f# | ALT + 11
♀ | g | ALT + 12
♪ | g# | ALT + 13

#### Funky Horn SFX

![sfx_funkhorn](https://user-images.githubusercontent.com/34200697/203387245-c388f8ec-eccb-437c-bdbe-48a77ddeb200.png)

Character | Note | Keys
--- | --- | ---
← | a | ALT + 27
∟ | a# | ALT + 28
↔ | b | ALT + 29
▲ | b# | ALT + 30
▼ | c | ALT + 31
! | d | ALT + 33
\# | d# | ALT + 35
$ | e | ALT + 36
% | e# | ALT + 37
& | f | ALT + 38
( | f# | ALT + 40
) | g | ALT + 41
\* | g# | ALT + 42

#### Duck SFX

![sfx_duck](https://user-images.githubusercontent.com/34200697/203387241-2b6439e3-bcb3-4c13-8d7f-d8b1167a8b2b.png)

Character | Note | Keys
--- | --- | ---
♫ | a | ALT + 14
☼ | a# | ALT + 15
► | b | ALT + 16
◄ | b# | ALT + 17
↕ | c | ALT + 18
‼ | d | ALT + 19
¶ | d# | ALT + 20
§ | e | ALT + 21
▬ | e# | ALT + 22
↨ | f | ALT + 23
↑ | f# | ALT + 24
↓ | g | ALT + 25
→ | g# | ALT + 26
