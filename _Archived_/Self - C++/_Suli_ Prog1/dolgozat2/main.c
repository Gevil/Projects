#include <stdio.h>
#include <stdlib.h>
#define ARRAYSIZE 10 // definiált konstans...

int main(int argc, char *argv[])
{
    int tomb[ARRAYSIZE];
    int i;
    
    printf("Adjon meg %d eletkort!\n", ARRAYSIZE);
    
    for(i = 0; i < ARRAYSIZE; i++)
    {
        printf("Kerem a %d. elemet!\n", i+1);
        scanf("%d", & tomb[i]);
    }
    int szamlalo = 0;
    for(i = 0; i < ARRAYSIZE; i++)
    {
        if(tomb[i] < 59 && tomb[i] > 30)
        {
            szamlalo++;
            printf("%d. elem: %d\n", i+1,tomb[i]);
        }
    }
    
    printf("%d eletkort adott meg 30 es 59 ev kozott.\n", szamlalo);

    getchar();
    getchar();
    return 0;
}
