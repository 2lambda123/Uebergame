#/bin/bash
# This script launches the dedicated server for Uebergame on Linux.
# For multiple servers just launch this script again, or copy, rename and modify it first.
# Dedicated servers offer the best performance, are best for running servers over long periods of time
# and the host does not have to be in the game while hosting it, since the server runs in the background.
# Dedicated servers are also necessary, if you want to run the game servers on internet servers.
LANG=C

SCRIPTLOCATION="$(readlink -f "$0")"
GAMEDIR="$(dirname "$SCRIPTLOCATION")"
# Name of the game executable, in our case the Linux 64bit dedicated build.
GAME=Uebergame-dedicated
CURRENT_DIR=`pwd`

# Level the server starts with, not that important, it changes later or can be changed ingame.
LEVEL="TG_DesertRuins_day.mis"

# "-dedicated" and "-level" flag are necessary,
# "-type" can be changed to DM, TDM, PBDM or PBTDM to create server with the specific gametype.
# For further administration join the server as client and become admin through entering the admin password.
OPTIONS="-dedicated -level $LEVEL -type DM"

# Time in seconds in which the server restarts, workaround to bring the servers back in case they crashed.
# This is only important if you plan to run the server over very long periods of time.
# 43200 /60 = 720 minutes /60 = 12 hours, you may change it, depending how stable it runs,
# or remove it alltogether and just launch it with this line:
# ./${GAME} ${OPTIONS} 2>&1 >server.log&
MAXRUNTIME=43200
# Name of the log file
LOG=server.log

cd ${GAMEDIR}

while true; do
	BeginTime="$(date +"%s")"
	echo "Start server ..."
	set -x
	./${GAME} ${OPTIONS} 2>&1 >server.log&
	set +x
	SER_PID=$!
	echo pid is ${SER_PID}
	LOOP=true
	while ${LOOP} ; do
		kill -0 "$SER_PID" ; ServerRunning=$?
		if [ ${ServerRunning} -ne 0 ] ; then
			echo "Server is not running anymore, trying restart"
			LOOP=false
		fi
		CHECKTIME="$(date +"%s")"; DIFFTIME="$(($CHECKTIME-$BeginTime))"

		if [ "$DIFFTIME" -gt ${MAXRUNTIME} ]; then
			echo "WARNING: Server is already running for $(MAXRUNTIME) seconds" >&2;
			kill "$SER_PID"
			sleep 5
			kill -0 "$SER_PID" ; ServerRunning=$?
			if [ ${ServerRunning} -eq 0 ] ; then
				# server is still running, hard kill
				kill -9 "$SER_PID"
			fi
			LOOP=false
		fi
		echo running $DIFFTIME seconds
		sleep 60;
	done
	echo "Will restart server in 10 Seconds"
	sleep 10;
done
