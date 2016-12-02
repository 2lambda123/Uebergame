@echo off  
REM This script launches the dedicated server for Uebergame on Windows.
REM For multiple servers just launch this script again, or copy, rename and modify it first.
REM Dedicated servers offer the best performance, are best for running servers over long periods of time
REM and the host does not have to be in the game while hosting it, since the server runs in the background.
REM Dedicated servers are also necessary, if you want to run the game servers on internet servers.
                         
:loop    

REM "-dedicated" and "-level" flag are necessary,
REM "-type" can be changed to DM, TDM, PBDM or PBTDM to create server with the specific gametype.
REM For further administration join the server as client and become admin through entering the admin password.
start "Uebergame-dedicated" "Uebergame.exe" -dedicated -level TG_DesertRuins_day.mis -type DM

REM This part starts a timer in seconds until the server is killed and restarted.
REM 43200 /60 = 720 minutes /60 = 12 hours
REM This is a workaround in case the server crashes or disappears from the server list.
REM Can be changed or removed depending how stable the server runs.
timeout /T 43200            
taskkill /f /im "Uebergame.exe" >nul   
timeout /T 7
                
REM after timeout go to the beginning and start it all over again.
goto loop                           