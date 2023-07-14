# Xabbo Music
A G-Earth extension that can be used to make music in Habbo Hotel using SFX furni and wired.

## How it works
SFX furni in Habbo plays a different note based on the height of that furni. With that being said, you can play a song if you use this furni at the correct order and time.

![xabbo-music-explanation-0](https://github-production-user-asset-6210df.s3.amazonaws.com/34200697/253653172-adab33c8-6619-45c5-a05b-e9d09d0c3d3e.png)

The program has mapped the notes from most SFX furni. You can drag those notes to the song timeline to create melodies, and when your song is ready, click on the Builder's Club button to build your song in your room.
![xabbo-music-explanation-1](https://github-production-user-asset-6210df.s3.amazonaws.com/34200697/253669100-efdbdbb9-8a0a-4af7-a1de-15a6ff652636.png)

### Timeline
##### Zoom
By default, the timeline speed is 50ms. This means that notes on the timeline display are 50ms apart from each other. You can increase this timespan by zooming out the timeline.
![xabbo-music-explanation-2](https://github-production-user-asset-6210df.s3.amazonaws.com/34200697/253656720-660fd924-0689-4509-a7ab-60c9951eaba6.png)

##### Blue markers
When zooming out, there is notes that you can't see depending on your timeline speed. If there's any notes between two tiles, a blue marker will appear between the tiles to indicate there's something there.
![xabbo-music-explanation-3](https://github-production-user-asset-6210df.s3.amazonaws.com/34200697/253657280-04e9dd1e-90be-43ea-9fc0-50031fa634d2.png)

##### Play
When editing your songs, if you're connected to Habbo and inside a room, you can listen to a preview of your music. Simply click the play button and hear the magic happening.

### Building a song
After your song is ready, click the Builder's Club button to build the song in your room. If everything is done right, a new window will appear:

![xabbo-music-explanation-4](https://github-production-user-asset-6210df.s3.amazonaws.com/34200697/253669416-395ae77a-733e-42a7-8512-635e68b53dae.png)

Here, you will choose where all the furni is going to be placed. Select the item and left click the tiles you want to add the item on. Right click a coloured tile to unselect it.

##### SFX and Magic tile
It doesn't really matter which is the size of your magic tile, but have in mind that it will be used to elevate the SFX furni, and at the start of the building, it will (if possible) be moved to the SFX furni location (the purple one in the room editor). If the magic tile cannot be placed at the SFX furni tile, the music building won't work.
* You can get the Magic Tile ID by clicking on it when G-Earth is connected:
* ![xabbo-music-explanation-5](https://github.com/scottstamp/RoomExfiltrator/assets/34200697/76596338-9697-4bc3-ba33-2e9e4882091e)

##### Controller
The controller is represented by a glowing ball in the room. When playing the song, the it's height will slowly increase, allowing the wireds to know exactly when to play each note. It is **very important** that the controller is placed on a tile that is not elevated (default tile height at :floor).