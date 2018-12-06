Übergame v1.1.2.0
=================

Übergame is a free open source, realism like, multi-purpose,
multiplayer, casual, first-person-shooter game, based on the Torque3D MIT engine.

Official Homepage:
http://www.uebergame.com

Moddb Page for updates:
http://www.moddb.com/games/uebergame

For Binaries download the latest version or go to
http://www.duion.com/games/uebergame/playtesting
for nightly builds if available, or compile them yourself using the engine Repository.


Source Code:
=================

Game source directory: 

https://github.com/Duion/Uebergame/tree/master //latest stable release
https://github.com/Duion/Uebergame/tree/development //latest development status
https://github.com/Duion/Uebergame/releases //quick access to all release builds

Engine source directory: 

https://github.com/Duion/Torque3D/tree/ueberengine-master //latest stable release
https://github.com/Duion/Torque3D/tree/ueberengine-dev //latest development status
https://github.com/Duion/Torque3D/releases //quick access to all release builds


Source for art:
=================
The source files for the art assets have been removed from the release package 
since version 1.0.4.0, so in case you want the source files for the models, download
an earlier package or go to:
https://duion.com/art/3d-models //for the models
https://duion.com/art/textures //for the textures

I plan to rework all the assets source files and properly release them on the internet,
but I'm behind the schedule and it is not complete yet.

For derivative art assets from GarageGames based demo assets that were open sourced I created
a repository where I upload my converted and updated versions of that:
https://github.com/Duion/Pacific-remastered //For Pacific demo assets and assets that use parts of it.
The Pacific assets are shipped with the game since version 1.0.8.0 and are actively used
since version 1.1.0.0.

Some assets source files may still be missing in any of those locations, so if you really really need
them just ask me, then I can send you a messy version.

Running dedicated servers:
=================

If you want to run dedicated servers you can use the dedicated server scripts:

Uebergame-dedicated.sh for running a Linux dedicated server.
Uebergame-dedicated.bat for running a Windows dedicated server.

You may want to adjust the scripts for your needs, but basically what they do
is running the game as a dedicated server with the needed parameters and provide
a restart loop, since in the past the dedicated servers used to get lost from
the server list after a while or they crashed, so you can put a time where they
automatically restart in case you want to run them long time and not care about them.


License:
=================
Copyright (c) 2012 GarageGames, LLC
Copyright (c) 2018 Christian Femmer

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.