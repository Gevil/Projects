#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    int i, j, k;
    j = 0;
    printf("Atlag szamolasa...\n");
add:
    printf("Szam hozzaadasa.\n");
    scanf("%d", &j);
    while(j!=0)
    {
        i++;
        k+=j;
        goto add;
    }

    printf("%f", k/i);
    
    getchar();
    getchar();
    return 0;
}
