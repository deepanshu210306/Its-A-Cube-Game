# Its A Cube (Unity)

A simple endless runner game made in Unity.

## Features
- Score system (updates as player moves forward)
- Game Over screen with:
  - Restart button
  - Quit button
- Background music that fades out in 3 seconds on Game Over
- Clean UI with world-space score display

## Controls (PC)
- Move Left: A / Left Arrow
- Move Right: D / Right Arrow

## How It Works
- Player moves left/right based on input
- Score = distance travelled
- When the player dies, GameManager shows Game Over UI and fades out music
- Restart reloads the current scene
- Quit closes the game

## Setup
1. Add your music file to **Assets/Music**
2. Assign it to the **AudioSource** on the Music object
3. Attach **MusicFader.cs** and assign the AudioSource
4. Assign UI elements (ScoreText, GameOverPanel) in GameManager inspector

## Build Instructions
- File → Build Settings → Windows → Build & Run

