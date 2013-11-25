#pragma once

class CArguments
{
public:
	CArguments();
	~CArguments();

	bool Parse(int argc, wchar_t* argv[]);

private:
	std::wstring m_strInputFile;
	std::wstring m_strOutputListing;
};

