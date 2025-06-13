// BenchmarkC.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <process.h>
#include <complex>
#include <map>
#include "safequeue.h"


using namespace std;

static char* lorem = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Curabitur mollis pretium orci. Fusce porta egestas nisi. Vestibulum dolor nunc, viverra a, vestibulum eu, lacinia quis, dolor. Morbi porta. Cras erat justo, blandit vel, pharetra non, porttitor vitae, pede. Suspendisse pharetra mollis mauris. Cras non neque ac arcu porttitor elementum. Fusce vel purus. Curabitur dui. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Fusce imperdiet, elit nec sagittis consequat, tortor arcu nonummy erat, nec placerat mauris elit ut tellus. Sed metus ligula, molestie vel, nonummy quis, tincidunt vel, risus. Vivamus pharetra laoreet felis. Vivamus pede libero, aliquet molestie, laoreet sed, mattis in, tellus. Maecenas placerat risus nec lectus mattis commodo. Donec eleifend. Aenean dictum, arcu in vehicula euismod, lectus arcu tincidunt leo, quis posuere enim urna a sem. Cras cursus pretium dui. Donec quis arcu in ipsum blandit faucibus. Nullam nibh massa nunc.";
typedef pair <string, string> String_Pair;

Queue<string> q1;
Queue<string> q2;

void hashTableTest()
{
    DWORD ts = GetTickCount();

    map <string, string>::iterator m_Iter;
    map <string, string> m;
    char key[8];

    for(int i=0; i<100000; i++)
    {
        sprintf(key, "%da", i);
        m.insert( String_Pair( key, "test" ) );
    }

    int i = m.size();
    for ( m_Iter = m.begin( ); m_Iter != m.end( ); m_Iter++)
    {
        string s = m_Iter->second;
    }

    DWORD te = GetTickCount();
    printf("HASHTABLE is %d\n", te-ts);
}

void mathTest()
{
    DWORD ts = GetTickCount();

    for(float i=1.0; i<10000000.0; i=i+1.0)
    {
        float a = i / (float)sqrt((float)i);
    }

    DWORD te = GetTickCount();
    printf("MATH is %d\n", te-ts);
}

void diskWriteTest()
{
    DWORD ts = GetTickCount();
    FILE* f = fopen("testfile.txt", "w+");
    
    for (int i=0; i<1000; i++)
    {
        fwrite(lorem, sizeof(char), 100, f); 
        fflush(f);
    }

    fclose(f);
    DWORD te = GetTickCount();
    printf("DISK WRITE is %d\n", te-ts);
}

void diskReadTest()
{
    DWORD ts = GetTickCount();
    FILE* f = fopen("testfile.txt", "r+");
    char buff[100000];

    memset(&buff, 0, sizeof(buff));
    for(int i=0; i<1000; i++)
    {
        fread(buff+(i*100), sizeof(char), 100, f);
    }

    fclose(f);
    DWORD te = GetTickCount();
    printf("DISK READ is %d\n", te-ts);
}

DWORD WINAPI  thread1(void *dummy)
{
    string s;
    char key[8];
    for(int i=0; i<10000; i++)
    {
        sprintf(key, "%da", i);
        q2.Push(key);
    }
    q2.Push("quit");

    while(stricmp("quit", s.c_str()) != 0)
    {
        if (q1.IsEmpty())
            Sleep(1);
        else
        {
            s = q1.Pop();
        }
    }

    return 0;
}

DWORD WINAPI  thread2(void *dummy)
{
    string s;
    char key[8];

    while(stricmp("quit", s.c_str()) != 0)
    {
        if(q2.IsEmpty())
            Sleep(1);
        else
        {
            s = q2.Pop();
        }
    }

    // Go back the other way
    for(int i=0; i<10000; i++)
    {
        sprintf(key, "%da", i);
        q1.Push(key);
    }
    q1.Push("quit");

    return 0;
}

void threadTest(int i)
{
    HANDLE h[2];
    DWORD t1, t2;

    h[0] = CreateThread( 
        NULL,             
        0,                
        thread1,          
        0,                
        0,                
        &t1);  

    if (h[0] == 0)
        printf("Cannot creat thread 1\n");


    h[1] = CreateThread( 
        NULL,             
        0,                
        thread2,          
        0,                
        0,                
        &t2);     

    if (h[1] == 0)
        printf("Cannot creat thread 2\n");

    DWORD dwEvent = WaitForMultipleObjects( 
                                            2,         
                                            h,         
                                            TRUE,      
                                            INFINITE);  

    switch (dwEvent) 
    { 
        case WAIT_OBJECT_0 + 0: 
            break; 

        case WAIT_OBJECT_0 + 1: 
            break; 

        default: 
            printf("Wait error: %d\n", GetLastError()); 
            break;
    }

    CloseHandle(h[0]);
    CloseHandle(h[1]);
}

int _tmain(int argc, _TCHAR* argv[])
{
    hashTableTest();
    mathTest();
    diskWriteTest();
    diskReadTest();

    DWORD ts = GetTickCount();

    for (int i=0; i<100; i++)
        threadTest(i+1);

    DWORD te = GetTickCount();
    printf("THREAD TEST average is %d\n", (te-ts)/100);

	return 0;
}



