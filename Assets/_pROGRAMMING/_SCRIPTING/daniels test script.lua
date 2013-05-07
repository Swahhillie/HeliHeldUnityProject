helispeed=50
flightheight=100
buttontime=2
rainstrength=10

MainMenu{
	button;label=1Speler;pos=3240,160,3750;rot=0,285,0;scale=30,5,1;action=level,Level1
	button;label=2Spelers;pos=3240,150,3750;rot=0,285,0;scale=30,5,1;action=interface,MainMenu
	button;label=Editor;pos=3240,140,3750;rot=0,285,0;scale=30,5,1;action=editor
}

Level1{
	mission;1;3
	nextLevel;Level2
	
	trigger;name=testTrigger;pos=4955.16,0,4982.92;eventname=Test;type=2;time=5;radius=20
	
	ship;pos=3360,24.5,2895;rot=0,0,0;spawn=1;action=1,Destroyed;action=0,Spawned;lifetime=10;
	ship;name=navyboatvrmvrm;pos=2760,33.5,3215;rot=0,0,0;spawn=1
	ship;pos=3910,54.5,3847;rot=0,0,0;spawn=1
	ship;pos=1968,53.5,3835;rot=0,0,0;event=Test,0;spawn=0
	ship;pos=1968,53.5,2929;rot=0,0,0;event=Test,0;spawn=0
	
	castaway;name=dennis;pos=2686.5,25.5,3255;rot=90,138,0;action=3,Drowned;lifetime=5
	castaway;pos=2827,25.5,3232.5;rot=48,287,299
	castaway;pos=2778,22.5,3163;rot=8,220,130
	castaway;pos=2692,22.5,3163;rot=90,138,0
}

Level2{
	trigger;name=lvl2Trigger;pos=1000,0,400;eventname=Test;type=2;time=5;radius=20
	
	ship;pos=1968,53.5,3835;rot=0,0,0;event=Test,0;spawn=0
	ship;pos=1968,53.5,2929;rot=0,0,0;event=Test,0;spawn=0
	
	castaway;name=dennis2;pos=2686.5,25.5,3255;rot=90,138,0
	castaway;pos=2778,22.5,3163;rot=8,220,130
	castaway;pos=2692,22.5,3163;rot=90,138,0
}

event Test{
message=There_is_a_ship_on_fire!
mission=-1,-2
}

event Destroyed{
message=Ship_Destroyed!
}

event Drowned{
message=Castaway_drowned!
}

event Spawned{
message=Ship_spawned!
mission=1,2
}

function foo{
System.String[]
}
