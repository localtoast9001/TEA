// teastest.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

class A
{
    public:
       virtual ~A() {}
       virtual int F1() { return 0; }
       virtual int F2() { return 0; }
       virtual int F3() { return 0; }
};

int derp(short a, int b, short c, bool d)
{
    int length = 0;
    return length;
}

int _tmain(int argc, _TCHAR* argv[])
{
    double dblA = 10.0f;
    double dblB = 100.0f;
    bool fDblResult = dblA == dblB;
    A* pA = new A();
    pA->F1();
    derp(0, 1, 2, false);
	puts("Hello World!");
    bool fResult = argc > 0;

	int fh2 = _wopen( L"\\tmp\\CRT_OPEN.BMP", _O_WRONLY | _O_CREAT | _O_BINARY, _S_IREAD | 
                            _S_IWRITE ); // C4996
   if( fh2 == -1 )
      perror( "Open failed on output file" );
   else
   {
      printf( "Open succeeded on output file\n" );
      _close( fh2 );
   }
	return 0;
}

