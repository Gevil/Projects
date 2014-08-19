#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    unsigned char i,j;
    float f;
    for(i = 0, f = 1e35; i<10; i++, f=f*10)
    {
        printf("%f \n", f);
    }
    
    
    
    getchar();
    getchar();
    return 0;
}
