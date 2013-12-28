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
    A* pA = new A();
    pA->F1();
    derp(0, 1, 2, false);
	puts("Hello World!");
    bool fResult = argc > 0;
	return 0;
}

