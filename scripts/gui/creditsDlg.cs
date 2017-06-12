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

function CreditsDlg::onWake(%this)
{
   %search = "scripts/gui/credits/*.hfl";
   CreditsFileList.entryCount = 0;
   CreditsFileList.clear();
   for(%file = findFirstFile(%search); %file !$= ""; %file = findNextFile(%search))
   {
      CreditsFileList.fileName[CreditsFileList.entryCount] = %file;
      CreditsFileList.addRow(CreditsFileList.entryCount, fileBase(%file));
      CreditsFileList.entryCount++;
   }
   CreditsFileList.sortNumerical(0);
   for(%i = 0; %i < CreditsFileList.entryCount; %i++)
   {
      %rowId = CreditsFileList.getRowId(%i);
      %text = CreditsFileList.getRowTextById(%rowId);
      %text = %i + 1 @ ". " @ restWords(%text);
      CreditsFileList.setRowById(%rowId, %text);
   }
   CreditsFileList.setSelectedRow(0);
}

function CreditsFileList::onSelect(%this, %row)
{
   %fo = new FileObject();
   %fo.openForRead(%this.fileName[%row]);
   %text = "";
   while(!%fo.isEOF())
      %text = %text @ %fo.readLine() @ "\n";

   %fo.delete();
   CreditsText.setText(%text);
}

function getCredits(%creditsName)
{
   Canvas.pushDialog(CreditsDlg);
   %index = -1;
   if(%creditsName !$= "")
   {
      for(%i = 0; %i < CreditsFileList.entryCount; %i++)
      {
         %text = CreditsFileList.getRowText(%i);
         if(%text $= %creditsName)
         {
            %index = %i;
            break;
         }
      }
      CreditsFileList.setSelectedRow(%index);
   }
}

function contextCredits()
{
   for(%i = 0; %i < Canvas.getCount(); %i++)
   {
      if(Canvas.getObject(%i).getName() $= CreditsDlg)
      {
         Canvas.popDialog(CreditsDlg);
         return;
      }
   }
   %content = Canvas.getContent();
   %creditsPage = %content.getCreditsPage();
   getCredits(%creditsPage);
}

function GuiControl::getCreditsPage(%this)
{
   return %this.creditsPage;
}
