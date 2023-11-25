local genisys = require("genisys")  
local my_game = genisys.create_application("game")  
my_game.name = "Club Penguin"  
my_game.thumbnail = genisys.get_path("icon.jpg")
local launcher = my_game:create_process() 
launcher.command = { genisys.get_path("ClubPenguin.x86_64") }  
return my_game