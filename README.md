# Emoticon-Generator

The Emoticon Generator should implement an interactive command line interface(CLI). The core commands that should be implemented as shown below. The notation "command { a I b } " indicates that the command must be followed by either a or b, for example "add mouth" adds a mouth to the face base, " style right-brow 2" changes the style of the right brow to style "2", etc. Note that your application should have default styles selected that may be later override using the "style" command. For example, the command "draw" outputs the SVG representation for the emoticon to the display, "save" saves the SVG representation to the specified "<file>", "help" prints help information, and "quit" exits the application. The commands "undo" and "redo" undo and redo the last "add", "remove","move", "reset" or "style" SVG operation, respectively. 

  ,,,
  show    { left-eye | right-eye | left-brow | right-brow | mouth }
	hide    { left-eye | right-eye | left-brow | right-brow | mouth }
	move    { left-eye | right-eye | left-brow | right-brow | mouth } { up | down | left | right }
	reset   { left-eye | right-eye | left-brow | right-brow | mouth }
	style   { left-eye | right-eye | left-brow | right-brow | mouth } { A | B | C }
	save    { <file> }
	draw
	undo
	redo
	help

  
  ,,,

 
