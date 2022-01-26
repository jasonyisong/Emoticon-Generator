# Emoticon-Generator

- This is a hard-coding by using C#

- Coding at Maynooth University

- Coding Examination - Requirements by Dr. P. Healy, Dr. J. Timoney, Dr. J. G. Keating

- The Emoticon Generator should implement an interactive command line interface(CLI). The core commands that should be implemented as shown below. The notation "command { a | b } " indicates that the command must be followed by either a or b, for example "show mouth" adds a mouth to the face base, "style right-brow B" changes the style of the right brow to style "B", etc. Note that your application should have default styles selected that may be later override using the "style" command. For example, the command "draw" outputs the SVG representation for the emoticon to the display, "save" saves the SVG representation to the specified "<file>", "help" prints help information, and "quit" exits the application. The “reset” command resets the emoticon element to the default position and style. The default style is style “A”. The commands "undo" and "redo" undo and redo the last "show", "hide","move", "reset" or "style" SVG operation, respectively. 
 
- Command-line help:
          
```
show    { left-eye | right-eye | left-brow | right-brow | mouth }
hide    { left-eye | right-eye | left-brow | right-brow | mouth }
move    { left-eye | right-eye | left-brow | right-brow | mouth } { up | down | left | right } value
reset   { left-eye | right-eye | left-brow | right-brow | mouth }
style   { left-eye | right-eye | left-brow | right-brow | mouth } { A | B | C }
save    { <file> }
draw
undo
redo
help
 
```
 
# My Solution

Decorator Software Design Pattern for emoji decoration.
Command Software Design Pattern for Undo/Redo.
 
# PUML

 the automated PUML extensions for VS Code
 
![image](https://user-images.githubusercontent.com/33503189/151129678-4c420cae-2437-41f3-af5f-f1a167f6c2fb.png)
