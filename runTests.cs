setLogMode(2);
$Con::LogBufferEnabled = false;
$Testing::checkMemoryLeaks = false;
runAllUnitTests();
quit();
