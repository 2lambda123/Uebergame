
function EndGameGui::onWake(%this)
{
   CloseMessagePopup();
   EndGameChatCtrl.attach(HudMessageVector);
   EndGameChatScroller.scrollToBottom();
}

function EndGameGui::onSleep(%this)
{
   //EndGameChatCtrl.detach(HudMessageVector);
}

function EndGameChatCtrl::addLine(%this, %text)
{
   ChatHud::addLine(%this, %text);
}