// TEAC.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Arguments.h"
#include "TokenReader.h"

const wchar_t* g_wszUsage = L"TEA Compiler v1.0 for IA32\r\nCopyright (C) 2013 Jon Rowlett. All rights reserved.\r\nUsage:\r\nTEAC.exe [Options] <input file.tea>\r\n/Fa<file.asm> - Output listing\r\nInput File - TEA source code to compile.";

int wmain(int argc, wchar_t* argv[])
{
	CArguments args;
	if(!args.Parse(argc, argv))
	{
		_putws(g_wszUsage);
		return 1;
	}

	CTokenReader tokenReader;

	return 0;
}

