# W26 Design Week Template



## 1. Multi-Display Rendering

Each of the Viswall's monitors are identical. The Viswall is a 5x2 matrix of monitors, though each the 2x2 set at either end is used as logically as a single monitor. Below is information about the displays.

( ! ) I will confirm these details as soon as I can confirm the dimensions. Left the resources on my work laptop, but doing this at home.

- **Aspect Ratio**: (Awaiting confirmation)
  - 19 by 9
  - 16 by 10
- **Resolution**: (Awaiting confirmation)
  - 3840x by 2160y
  - 1920x by 1080y
  - 3840x by 2400y
  - 1920x by 1200y
- **Physical Dimensions**: (toughly)
  - Each individual panel: 4x2 feet
  - Each logical panel (2x2): 8x4 feet.

### 1.1 Display Setup

Unity can output up to 8 monitors simultaneously. One camera component can display to one display. This is a setting on each Camera component.

In Unity, you must create multiple Game windows and select which display output to view.

img

You must run a bit of code to activate the monitor manually when playing from a build. The project provides the `ConfigureDisplays` script and game object to do this on scene `Start`. 

img

If the rendered windows don't start where you want, you should set things up in you display settings so that Monitor 1 is the left window, and Monitor 2 is the right window. You could potentially hold down CTRL+Shift+Windows and then press arrow keys to move the game windows around, too.

## 2. Controllers

The Viswall will be set up with a tower PC connected to 6 game controllers simultaneously (XBOX and/or PS5, though any traditional controller will do). You do not need to use all controllers for your game. However, the game must remain a multiplayer game.

We encourage you to use as few buttons, triggers, and/or control stick as possible to keep button the game accessible and remove issues related to button mapping across different controllers.