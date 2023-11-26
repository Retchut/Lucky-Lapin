local genisys = require("genisys")  
local my_game = genisys.create_application("game")  
my_game.name = "Lucky Lapin"  
my_game.thumbnail = genisys.get_path("logo.png")
local launcher = my_game:create_process() 
launcher.command = { genisys.get_path("LuckyLapin.x86_64") }  
return my_game
