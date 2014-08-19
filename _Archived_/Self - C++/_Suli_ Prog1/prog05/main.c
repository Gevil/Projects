#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    int i, j, k;
    k = 1;
    i = 1;
    printf("Faktorialis szamolasa...\n");
    printf("Irja be melyik szam faktorialisat szeretne kiszamolni!\n");
    scanf("%d", &j);
    
    for(k=1, i=1; i<=j; i++)
    {
        k*=i;
    }
    
    /*while(i<j)
    {
        i++;
        k*=i;
    }*/
    
    printf("%d", k);
    
    getchar();
    getchar();
    return 0;
}
