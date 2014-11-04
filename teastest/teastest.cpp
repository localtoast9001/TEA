// teastest.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

namespace Samples
{
    class FooBar
    {
        public:
            int __cdecl Func1() { return 0; }
            int __cdecl Func2() const { return 1; }

    };

    class Program
    {
       public:
           static int Main(int argc, _TCHAR* argv[])
           {
               return Main2(argc, argv);
           }
       protected:
           static int Main2(int argc, _TCHAR* argv[])
           {
               return Main3(argc, argv);
           }
       private:
           static int Main3(int argc, _TCHAR* argv[])
           {
               return Main4(argc);
           }

           static int Main4(int argc)
           {
               FooBar foo;
               return Main5(&foo);
           }

           static int Main5(FooBar* pFoo)
           {
               return pFoo->Func1();
           }
    };
};

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
    Samples::FooBar foo;
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

    foo.Func1();
    foo.Func2();
    return Samples::Program::Main(argc, argv);
}

