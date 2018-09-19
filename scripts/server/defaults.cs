//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// List of master servers to query, each one is tried in order until one responds
$pref::Net::DisplayOnMaster = 1; //list hosted games by default on master server
$pref::Server::RegionMask = 2;
$pref::Master[0] = "2:duion.com:28002";

// Information about the server
$pref::Server::Name = "Uebergame server";
$pref::Server::Info = "This is an Uebergame server.";

// Text to appear on loading screen, multiple lines possible just add more varibles
$pref::Server::Message0 = "Server Information:";
$pref::Server::Message1 = "Welcome to the Uebergame server!";
$pref::Server::Message2 = "Let the game begin.";

$pref::Server::ConnectionError =
   "You likely do not have the correct version of the game installed or "@
   "you are missing some of the related art and levels. "@
   "Please contact the server administrator and try to solve the issue.";

// The network port is also defined by the client, this value 
// overrides pref::net::port for dedicated servers
$pref::Server::Port = 28000;

// If the password is set, clients must provide it in order to connect to the server
$pref::Server::Password = "";

// Not dedicated by default of course, this is just for professionals
$pref::Server::Dedicated = 0;

// Password for admin clients, should be changed, otherwise anyone can enter with those
$pref::Server::AdminPassword = "changeme";
$pref::Server::SuperAdminPassword = "changemetoo";

// Misc server settings.
$pref::Server::BadWordFilter = 1;
$pref::Server::AiCount = 0;
$pref::Server::AiCountRandomize = 0;
$pref::Server::AiCountMinimum = 1;
$pref::Server::BaseSacking = 1;
$pref::Server::ConnectLog = 0;
$pref::Server::MissionFile = "levels/TG_DesertRuins/TG_DesertRuins_day.mis";
$pref::Server::ConnLogPath = "logs";
$pref::Server::MissionType = "DM";
$pref::Server::RandomMissions = 1; // random map cycle
$pref::Server::MaxPlayers = 64;
$pref::Server::TimeLimit = 10; // minutes
$pref::Server::KickBanTime = 300; // seconds
$pref::Server::BanTime = 1800; // seconds
$pref::Server::FloodProtectionEnabled = 1;
$pref::Server::MaxChatLen = 120;
$pref::Server::teamName[0] = "Spectator";
$pref::Server::teamName[1] = "Blue Team";
$pref::Server::teamName[2] = "Red Team";
$pref::Server::warmupTime = 0; // seconds
$pref::Server::RespawnTime = 8; // seconds
$pref::Server::EndGamePause = 8; // seconds
$pref::Server::DMScoreLimit = 100; // score
$pref::Server::TDMScoreLimit = 5; // kills multiplied by players on server
$pref::Server::RtFScoreLimit = 5; // flag capture limit
$pref::Server::MfDScoreLimit = 5; // marked kill limit
$pref::Server::FriendlyFire = 0;
$pref::Server::TournamentMode = 0;
$pref::Server::DisallowVoteMission = 0;
$pref::Server::DisallowVoteSkipMission = 0;
$pref::Server::DisallowVoteFriendlyFire = 0;
$pref::Server::DisallowVoteBaseSacking = 0;
$pref::Server::DisallowVoteServerMode = 0;
$pref::Server::DisallowVoteStartMatch = 0;
$pref::Server::DisallowVoteTimeLimit = 0;
$pref::Server::DisallowAddBots = 0;
$pref::Server::DisallowVoteKickAllBots = 0;
$pref::Server::DisallowVoteAdmin = 1;
$pref::Server::VotePassPercent = 51;
$pref::Server::VoteTime = 20; // seconds
$pref::Server::MaxWeapons = 1; // 1-4
$pref::Server::MaxSpecials = 0; // on/off
$pref::Server::MaxGrenades = 1; // on/off
$pref::Server::MaxMines = 0; // on/off
$pref::Server::Hints = 1;