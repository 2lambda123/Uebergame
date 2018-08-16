//-----------------------------------------------------------------------------
// Bot client naming functions, used for aiConnection::onConnect()
//-----------------------------------------------------------------------------
$RandomBotNameCount = 0;
function addBotName(%name)
{
   $RandomBotName[$RandomBotNameCount] = %name;
   $RandomBotNameCount++;
}

function getRandomBotName()
{
   %index = getRandom( $RandomBotNameCount-1 );
   return($RandomBotName[%index]); 
}

// More bot names can be added below, the game will randomly pick one out of all.
//-----------------------------------------------------------------------------

// A
addBotName("A. Trappe");
addBotName("Ai Gear");
addBotName("Abort");
addBotName("Aborter");
addBotName("Automaton");
addBotName("Automat");
addBotName("Autobot");
addBotName("Ai Commander");
addBotName("Ai Fish");
addBotName("Ai Robot");
addBotName("Al Aine");
addBotName("Android");
addBotName("Androbot");
addBotName("Artificial");
addBotName("Arti");
addBotName("Artificiosus");
// B
addBotName("B Team");
addBotName("Bait");
addBotName("Ballerbot");
addBotName("Blaind");
addBotName("Ben Der");
addBotName("Blender");
addBotName("Buster");
addBotName("Borot");
addBotName("Boot");
addBotName("Billigbot");
addBotName("Blechdose");
addBotName("Bort");
addBotName("Botzkrieg");
addBotName("Botti");
addBotName("BotOMat");
addBotName("BotMaster");
addBotName("Botman");
addBotName("Boombot");
addBotName("Bot Kicker");
addBotName("Bot Omat");
addBotName("Botonaut");
addBotName("Boten Anar");
addBotName("Bot Anicus");
addBotName("Botan");
addBotName("Botaus");
addBotName("Bullseye");
addBotName("Bunny");
addBotName("Bumpkin");
// C
addBotName("Carcass");
addBotName("Copycat");
addBotName("Copydog");
addBotName("Casualty");
addBotName("Clone Krieger");
addBotName("Collateral");
addBotName("Cybok");
// D
addBotName("Default Soldier");
addBotName("Dr. Gadget");
addBotName("Das Bot");
addBotName("Databot");
addBotName("Dummy");
addBotName("Dr. Roboto");
// E
addBotName("Ey Ei");
addBotName("Endangered");
// F
addBotName("Fred");
addBotName("Fembot");
addBotName("Fluffy");
addBotName("Fodder");
addBotName("Flatline");
addBotName("Fluffy Bunny");
addBotName("Fagbot");
addBotName("Filthy Casual");
// G
addBotName("Grunt");
addBotName("Gimp");
addBotName("Golem");
// H
addBotName("Hans Wurst");
addBotName("Hadlez Tschiken");
addBotName("Husonaut");
// I
addBotName("iBot");
addBotName("I the Machine");
// J
addBotName("John Roh");
// K
addBotName("Kain Plan");
addBotName("KickMe");
addBotName("Kaputnik");
addBotName("Kork");
addBotName("Kick Botter");
// L
addBotName("Labot");
addBotName("Lebot");
// M
addBotName("Meat");
addBotName("Masochist");
addBotName("Mad Cow");
addBotName("Min Damage");
addBotName("Max Damage");
addBotName("Manufact");
addBotName("Machine Soldier");
addBotName("Master Bait");
addBotName("Motombot");
addBotName("Master Slave");
addBotName("Motorschwein");
addBotName("Motorbot");
addBotName("MegaBot");
addBotName("MiniBot");
addBotName("Mockobot");
addBotName("Mock");
addBotName("Mockai");
addBotName("Mario Net");
addBotName("Manikin");
addBotName("Mechanical Turk");
addBotName("Metabot");
// N
addBotName("Number 5");
addBotName("Nottabot");
addBotName("Nobot");
addBotName("Noboty");
addBotName("Nick Echt");
// O
addBotName("Oblivious");
addBotName("Otto");
// P
addBotName("Pro Grammar");
addBotName("Pro Gaymar");
addBotName("Pseudo Player");
addBotName("Player.7351");
addBotName("Pappkamerad");
// Q
addBotName("Quobot");
addBotName("Qwop");
// R
addBotName("Roadkill");
addBotName("Rob Oter");
addBotName("Roh Bot");
addBotName("RoBo");
addBotName("Robort");
addBotName("Rebot");
addBotName("Rebort");
addBotName("Robo Sapien");
addBotName("Replikant");
addBotName("Replikatron");
addBotName("Rie Tardo");
addBotName("Row Boat");
addBotName("Robotron");
addBotName("Run Bot");
addBotName("Run Botrun");
// S
addBotName("SkidMark");
addBotName("Spud");
addBotName("Squidloaf");
addBotName("Spastic");
addBotName("Schrotti");
addBotName("Schrottkopf");
addBotName("Scrap Man");
addBotName("Scrap Head");
addBotName("Slave");
addBotName("Sleepwalker");
addBotName("Servus ex Machina");
addBotName("Subot");
addBotName("Sodabot");
addBotName("Sudobot");
addBotName("Swagbot");
addBotName("Saibot");
// T
addBotName("Tardo");
addBotName("Terminal");
addBotName("Turing Test");
addBotName("Turing Complete");
addBotName("Turing Incomplete");
addBotName("Tin Can");
addBotName("Talbot");
addBotName("The Machinist");
// U
addBotName("UeberBot");
addBotName("UnterBot");
addBotName("Unter Soldier");
addBotName("U-Bot");
// V
addBotName("Vbot");
addBotName("V2Bot");
// W
addBotName("WormChow");
addBotName("Weltver Bot");
addBotName("Weird Ai");
addBotName("Weirdo");
// X
addBotName("Xobot");
addBotName("Xoro");
// Y
addBotName("Yobot");
addBotName("Ynot");
// Z
addBotName("Zervus");
addBotName("Zeiborg");
addBotName("Zoidbot");
addBotName("Zombot");
addBotName("Zombo");
addBotName("Zombotron");
