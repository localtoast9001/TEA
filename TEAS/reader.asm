; Copyright (C) 2013 Jon Rowlett. All rights reserved
option unicode
model flat,stdcall

includelib "win32.lib"
procdesc CreateFileW :dword,:dword,:dword,:dword,:dword,:dword,:dword
procdesc WriteFileW :dword,:dword,:dword,:dword,:dword

