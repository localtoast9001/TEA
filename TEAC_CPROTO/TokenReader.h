#pragma once

#include "Token.h"

class CTokenReader
{
public:
	CTokenReader(void);
	~CTokenReader(void);

	bool Open(const std::wstring& strFileName);
	bool Read(CToken& token);
	bool Peek(CToken& token);

private:
	FILE* m_pFile;
	int m_nLineNumber;
	int m_nColumnNumber;
};

