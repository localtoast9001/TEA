#include "stdafx.h"
#include <string>
#include "Arguments.h"


CArguments::CArguments()
{
}


CArguments::~CArguments()
{
}

bool CArguments::Parse(int argc, wchar_t* argv[])
{
	m_strInputFile.clear();
	m_strOutputListing.clear();
	
	if (argc < 2 || argc > 3)
	{
		return false;
	}

	int i = 1;
	for (; i < argc; i++)
	{
		const wchar_t* pwszArg = argv[i];
		if (pwszArg[0] != '/')
		{
			break;
		}

		if (pwszArg[1] != 'F')
		{
			return false;
		}

		if (pwszArg[2] != 'a')
		{
			return false;
		}

		m_strOutputListing = pwszArg + 3;
	}

	if(i != argc-1)
	{
		return false;
	}

	m_strInputFile = argv[i];
	if (m_strOutputListing.length() == 0)
	{
		int extIndex = m_strInputFile.find_last_of('.', 0);
		if(extIndex > 0)
		{
			m_strOutputListing = m_strInputFile.substr(0, extIndex) + L".asm";
		}
		else
		{
			m_strOutputListing = m_strInputFile + L".asm";
		}
	}

	return true;
}
