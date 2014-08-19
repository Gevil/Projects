#include <stdio.h>
#include <stdlib.h>
#define YES 1
#define NO 0

int main(int argc, char *argv[])
{
    char sz(80);
    char c, nc, ns, nsz, szo;
    nc, ns, nsz = 0; szo = NO;
    //scanf("%c",&c);
    //printf("%c - %d\n", c, c);
    //c = "\n"
    
    while((c = getchar()) != EOF)
    {
        putchar(c);
        nc++;
        if(c=='\n') ns++;
        if(c==' ' || c=='\n' || c=='\t')
        {szo = NO;}
        else if(szo == NO)
        {nsz++; szo = YES;}
    }
    printf("Sorok szama: %d\nKarakterek szama: %d\nSzavak szama: %d\n", ns, nc, nsz);
    
    scanf("%s", sz);
    printf("%s\n", sz);
    // '\0' bináris nulla karakter tömb lezárására
    getchar();
    getchar();
    return 0;
}
