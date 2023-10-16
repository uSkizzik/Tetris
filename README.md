## uSkizzik's Tetris

This is a Tetris clone played entirely in the CLI.  
It's a quick side project done out of boredom that tries following the Tetris guidelines.

One Thursday (10/12/2023) I was bored in programming class and decided to make a Tetris clone. 
Initially, I wanted to work on it only during school in programming classes, however I ended up finishing it over the weekend.

This is a quick and dirty side-project. It's not meant to be performant, it's not meant to be easily maintainable.
I didn't spend a lot of time cleaning up code because it won't be developed further. 
I also tried using a lot of different concepts to exercise myself and learn how exactly they work in C# (because I'm primarily a webdev).

I used the [Tetris Guideline](https://tetris.wiki/Tetris_Guideline) as a reference while making this clone.
I didn't do everything the way Tetris recommends, however I did my best to recreate the rules set by the guideline.

### Controls
| Key         | Action                   |
|-------------|--------------------------|
| Up Arrow    | Rotate Clockwise         |
| Down Arrow  | Move Down                | 
| Left Arrow  | Move Left                | 
| Right Arrow | Move Right               | 
| Space Bar   | Hard Drop                | 
| C           | Hold Tetromino           | 
| Z           | Rotate Counter-Clockwise | 
| M           | Toggle Audio             | 
| Esc         | Return to main menu      | 
| F5          | Redraw Frame             | 

### Features

- ✔️ Moving, Rotating
- ✔️ Soft Drops, Hard Drops
- ✔️ Wall Kicks
- ✔️ Starting Positions at 21st and 22nd rows
- ✔️ Lock Down (Extended Placement Lock Down)
- ✔️ Piece Preview / Next Queue
- ✔️ Holding
- ✔️ Random Bag / 7 Bag
- ❌ Ghost Piece
- ✔️ Scoring via Line Clears
- ✔️ Scoring via Perfect Clears
- ✔️ Scoring Combos
- ✔️ Scoring Back to Back
- ❌ Scoring T-Spins
- ✔️ Levels
- ✔️ High Score (Persists between restarts)
- ✔️ Sound Effects
- ✔️ Music (using Console.Beep())